using System.Net.Http.Json;
using Messanger.Shared.Module;

namespace Client.Services;

public interface IChatService
{
    List<Chat> Chats { get; }
    List<Message> CurrentMessages { get; }
    int? ActiveChatId { get; }

    event Action? OnChange;

    Task LoadChatsAsync(int userId);
    Task SelectChatAsync(int chatId);
    Task RefreshActiveMessagesAsync();
    Task SendMessageAsync(int chatId, int senderId, string text, string? imageUrl = null);
    Task<string?> UploadPhotoAsync(Stream fileStream, string fileName);
    void StartPeriodicPolling(int userId, TimeSpan interval);
    void StopPeriodicPolling();
}

public class ChatService : IChatService, IDisposable
{
    private readonly HttpClient _http;
    private PeriodicTimer? _timer;
    private CancellationTokenSource? _cts;
    private int _currentUserId;

    public List<Chat> Chats { get; private set; } = new();
    public List<Message> CurrentMessages { get; private set; } = new();
    public int? ActiveChatId { get; private set; }

    public event Action? OnChange;

    public ChatService(HttpClient http)
    {
        _http = http;
    }

    public async Task LoadChatsAsync(int userId)
    {
        _currentUserId = userId;
        try
        {
            var response = await _http.GetFromJsonAsync<List<Chat>>($"api/chat/user/{userId}");
            if (response != null)
            {
                Chats = response;
                NotifyStateChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatService] Ошибка загрузки чатов: {ex.Message}");
        }
    }

    public async Task SelectChatAsync(int chatId)
    {
        ActiveChatId = chatId;
        await RefreshActiveMessagesAsync();
    }

    public async Task RefreshActiveMessagesAsync()
    {
        if (ActiveChatId == null) return;

        try
        {
            var response = await _http.GetFromJsonAsync<List<Message>>($"api/messages/chat/{ActiveChatId.Value}");
            if (response != null)
            {
                CurrentMessages = response;
                NotifyStateChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatService] Ошибка обновления сообщений: {ex.Message}");
        }
    }

    public async Task SendMessageAsync(int chatId, int senderId, string text, string? imageUrl = null)
    {
        var msg = new Message
        {
            ChatId = chatId,
            SenderId = senderId,
            Text = text,
            ImageUrl = imageUrl ?? "",
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            var response = await _http.PostAsJsonAsync("api/messages/send", msg);
            if (response.IsSuccessStatusCode)
            {
                var created = await response.Content.ReadFromJsonAsync<Message>();
                if (created != null && ActiveChatId == chatId)
                {
                    CurrentMessages.Add(created);
                    NotifyStateChanged();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatService] Ошибка отправки: {ex.Message}");
        }
    }

    public async Task<string?> UploadPhotoAsync(Stream fileStream, string fileName)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);
            content.Add(streamContent, "file", fileName);

            var response = await _http.PostAsync("api/messages/upload-photo", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UploadResult>();
                return result?.url;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatService] Ошибка загрузки фото: {ex.Message}");
        }
        return null;
    }

    public void StartPeriodicPolling(int userId, TimeSpan interval)
    {
        StopPeriodicPolling();
        _currentUserId = userId;
        _cts = new CancellationTokenSource();
        _timer = new PeriodicTimer(interval);

        _ = Task.Run(async () =>
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    await LoadChatsAsync(_currentUserId);
                    if (ActiveChatId != null)
                    {
                        await RefreshActiveMessagesAsync();
                    }
                }
            }
            catch (OperationCanceledException) { }
        });
    }

    public void StopPeriodicPolling()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _timer?.Dispose();
        _cts = null;
        _timer = null;
    }

    private void NotifyStateChanged()
    {
        MainThread.BeginInvokeOnMainThread(() => OnChange?.Invoke());
    }

    public void Dispose()
    {
        StopPeriodicPolling();
    }

    private record UploadResult(string url);
}