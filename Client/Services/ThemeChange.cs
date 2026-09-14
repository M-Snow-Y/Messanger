namespace Client.Services
{
    public class ThemeService
    {
        public bool IsDarkTheme{ get; private set; } = true;
        public event Action? OnChange;

    public void Initialize()
    {
        IsDarkTheme = Microsoft.Maui.Storage.Preferences.Default.Get("AppTheme_IsDark", true);
        NotifyStateChanged();
    }

    public void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        Microsoft.Maui.Storage.Preferences.Default.Set("AppTheme_IsDark", IsDarkTheme);
        NotifyStateChanged();
    }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
