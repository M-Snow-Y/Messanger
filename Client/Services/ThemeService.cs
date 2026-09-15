using System.Text.Json;
using Client.Models;

namespace Client.Services;

public class ThemeService
{
    private const string ActiveThemeKey = "active_theme_id";
    private readonly string _themesDir;

    public List<CustomThemeItem> Themes { get; private set; } = new();
    public CustomThemeItem ActiveTheme { get; private set; } = CustomThemeFactory.GetDark();

    public event Action? OnChange;

    public ThemeService()
    {
        _themesDir = Path.Combine(FileSystem.AppDataDirectory, "themes");
        if (!Directory.Exists(_themesDir))
        {
            Directory.CreateDirectory(_themesDir);
        }
    }

    public async Task InitializeAsync()
    {
        Themes.Clear();
        await EnsureDefaultsAsync();

        var files = Directory.GetFiles(_themesDir, "*.json");
        foreach (var file in files)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var theme = JsonSerializer.Deserialize<CustomThemeItem>(json);
                if (theme != null && !Themes.Any(t => t.Id == theme.Id))
                {
                    Themes.Add(theme);
                }
            }
            catch { }
        }

        var savedId = Preferences.Default.Get(ActiveThemeKey, "dark_default");
        ActiveTheme = Themes.FirstOrDefault(t => t.Id == savedId) ?? Themes.First();
        OnChange?.Invoke();
    }

    public void SetActiveTheme(string id)
    {
        var t = Themes.FirstOrDefault(x => x.Id == id);
        if (t != null)
        {
            ActiveTheme = t;
            Preferences.Default.Set(ActiveThemeKey, t.Id);
            OnChange?.Invoke();
        }
    }

    // Циклическое переключение тем одной кнопкой
    public void CycleTheme()
    {
        if (Themes.Count == 0) return;
        var idx = Themes.FindIndex(t => t.Id == ActiveTheme.Id);
        var nextIdx = (idx + 1) % Themes.Count;
        SetActiveTheme(Themes[nextIdx].Id);
    }

    public void ToggleTheme() => CycleTheme();

    public string ThemeIcon => ActiveTheme.IsDark ? "🌙" : "☀️";

    public async Task SaveThemeAsync(CustomThemeItem theme)
    {
        var path = Path.Combine(_themesDir, $"{theme.Id}.json");
        var json = JsonSerializer.Serialize(theme, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(path, json);

        var existing = Themes.FirstOrDefault(t => t.Id == theme.Id);
        if (existing != null)
        {
            var idx = Themes.IndexOf(existing);
            Themes[idx] = theme;
        }
        else
        {
            Themes.Add(theme);
        }

        if (ActiveTheme?.Id == theme.Id) ActiveTheme = theme;
        OnChange?.Invoke();
    }

    public async Task<bool> ImportThemeAsync(Stream stream, string fileName)
    {
        try
        {
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var theme = JsonSerializer.Deserialize<CustomThemeItem>(json);
            if (theme == null) return false;

            theme.Id = Guid.NewGuid().ToString();
            theme.IsBuiltIn = false;
            if (string.IsNullOrWhiteSpace(theme.Name))
                theme.Name = Path.GetFileNameWithoutExtension(fileName);

            await SaveThemeAsync(theme);
            SetActiveTheme(theme.Id);
            return true;
        }
        catch { return false; }
    }

    public void DeleteTheme(CustomThemeItem theme)
    {
        if (theme.IsBuiltIn) return;
        var path = Path.Combine(_themesDir, $"{theme.Id}.json");
        if (File.Exists(path)) File.Delete(path);
        Themes.Remove(theme);
        if (ActiveTheme.Id == theme.Id) SetActiveTheme(Themes.First().Id);
        else OnChange?.Invoke();
    }

    private async Task EnsureDefaultsAsync()
    {
        var dark = CustomThemeFactory.GetDark();
        var darkPath = Path.Combine(_themesDir, $"{dark.Id}.json");
        if (!File.Exists(darkPath))
        {
            await File.WriteAllTextAsync(darkPath, JsonSerializer.Serialize(dark, new JsonSerializerOptions { WriteIndented = true }));
        }

        var light = CustomThemeFactory.GetLight();
        var lightPath = Path.Combine(_themesDir, $"{light.Id}.json");
        if (!File.Exists(lightPath))
        {
            await File.WriteAllTextAsync(lightPath, JsonSerializer.Serialize(light, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}

public static class CustomThemeFactory
{
    public static CustomThemeItem GetDark() => new()
    {
        Id = "dark_default",
        Name = "Классическая тёмная",
        IsDark = true,
        IsBuiltIn = true,
        BgPrimary = "#0e1621",
        BgSecondary = "#17212b",
        TextPrimary = "#f5f5f5",
        TextSecondary = "#708499",
        BubbleMy = "#2b5278",
        BubbleOther = "#182533",
        Accent = "#5288c1",
        Border = "rgba(0,0,0,0.25)"
    };

    public static CustomThemeItem GetLight() => new()
    {
        Id = "light_default",
        Name = "Классическая светлая",
        IsDark = false,
        IsBuiltIn = true,
        BgPrimary = "#f0f2f5",
        BgSecondary = "#ffffff",
        TextPrimary = "#111827",
        TextSecondary = "#6b7280",
        BubbleMy = "#4fa5e3",
        BubbleOther = "#ffffff",
        Accent = "#3b82f6",
        Border = "rgba(0,0,0,0.1)"
    };
}