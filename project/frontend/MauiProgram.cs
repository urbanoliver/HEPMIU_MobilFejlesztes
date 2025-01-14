using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using WeaponMobileApp.Services;

namespace WeaponMobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Register WeaponService with HttpClient
            builder.Services.AddHttpClient<WeaponService>(client =>
            {
                client.BaseAddress = new Uri("http://192.168.1.100:44331/"); // Use machine's IP instead of localhost
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
