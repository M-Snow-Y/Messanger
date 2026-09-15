namespace Messanger.Shared.Module;
public class CustomThemeItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Новая тема";
    public bool IsDark { get; set; } = true;
    public bool IsBuiltIn { get; set; } = false;

    public string BgPrimary { get; set; } = "#0e1621";
    public string BgSecondary { get; set; } = "#17212b";
    public string TextPrimary { get; set; } = "#f5f5f5";
    public string TextSecondary { get; set; } = "#708499";
    public string BubbleMy { get; set; } = "#2b5278";
    public string BubbleOther { get; set; } = "#182533";
    public string Accent { get; set; } = "#5288c1";
    public string Border { get; set; } = "rgba(0,0,0,0.2)";

    public string ToCssString() =>
        $"--tg-bg-primary: {BgPrimary}; " +
        $"--tg-bg-secondary: {BgSecondary}; " +
        $"--tg-bg-header: {BgSecondary}; " +
        $"--tg-bg-input: {BgSecondary}; " +
        $"--tg-text-primary: {TextPrimary}; " +
        $"--tg-text-secondary: {TextSecondary}; " +
        $"--tg-bubble-my: {BubbleMy}; " +
        $"--tg-bubble-other: {BubbleOther}; " +
        $"--tg-accent: {Accent}; " +
        $"--tg-border: {Border};";
}