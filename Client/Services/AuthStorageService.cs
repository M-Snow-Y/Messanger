using Messanger.Shared.Module;
using Microsoft.Maui.Storage;

namespace Client.Services;

public class AuthStorageService
{
    private const string UserIdKey = "SavedUserId";
    private const string NameKey = "SavedName";
    private const string UserNameKey = "SavedUserName";
    private const string EmailKey = "SavedEmail";

    public void SaveUserSession(User user)
    {
        Preferences.Default.Set(UserIdKey, user.Id);
        Preferences.Default.Set(NameKey, user.Name ?? "");
        Preferences.Default.Set(UserNameKey, user.UserName ?? "");
        Preferences.Default.Set(EmailKey, user.Email ?? "");
    }

    public int GetUserId() => Preferences.Default.Get(UserIdKey, 0);

    public string GetName() => Preferences.Default.Get(NameKey, "Гость");

    public string GetUserName() => Preferences.Default.Get(UserNameKey, "");

    public string GetEmail() => Preferences.Default.Get(EmailKey, "");

    public Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            int userId = GetUserId();
            return Task.FromResult(userId > 0);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public void Logout()
    {
        Preferences.Default.Remove(UserIdKey);
        Preferences.Default.Remove(NameKey);
        Preferences.Default.Remove(UserNameKey);
        Preferences.Default.Remove(EmailKey);
    }
}