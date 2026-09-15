using Client.Services;
using Microsoft.Extensions.Logging;

namespace Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        //Единый базовый адрес: для Android эмулятора 10.0.2.2, для Windows - localhost
        string baseUrl = DeviceInfo.Platform == DevicePlatform.Android 
            ? "http://10.0.2.2:5050/" 
            : "http://localhost:5050/";

        builder.Services.AddScoped(sp => new HttpClient 
        { 
            BaseAddress = new Uri(baseUrl) // Твой реальный IPv4!
        });

        builder.Services.AddHttpClient<IChatService, ChatService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        builder.Services.AddSingleton<AuthStorageService>();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });


        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddSingleton<AuthStorageService>();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }




}