namespace Client.Services;

public class AuthStorageService
{
    private const string TokenKey = "auth_user_token";
    private const string UserNameKey = "auth_user_name";

    // Сохраняем после успешного входа
    public async Task SaveLoginAsync(string token, string userName)
    {
        await SecureStorage.Default.SetAsync(TokenKey, token);
        Preferences.Default.Set(UserNameKey, userName);
    }

    // Получаем сохраненный токен при запуске
    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.Default.GetAsync(TokenKey);
    }

    public string? GetUserName()
    {
        return Preferences.Default.Get(UserNameKey, null);
    }

    // Выход из аккаунта
    public void Logout()
    {
        SecureStorage.Default.Remove(TokenKey);
        Preferences.Default.Remove(UserNameKey);
    }

    // Проверка: залогинен ли пользователь
    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}