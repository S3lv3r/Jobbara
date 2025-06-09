using Microsoft.Extensions.Logging;
using Firebase.Database;
using Jobbara.Models;
using Firebase.Database.Query;

namespace Jobbara
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            RegisterOffice();
            return builder.Build();
        }

        public static void RegisterOffice()
        {
            FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
            var offices = client.Child("Offices").OnceAsync<officeModel>().Result;
            if(offices.Count == 0)
            {
                client.Child("Offices").PostAsync(new officeModel{ Name = "Carpintero", } );
                client.Child("Offices").PostAsync(new officeModel { Name = "Electricista", });
                client.Child("Offices").PostAsync(new officeModel { Name = "Plomero", });
                client.Child("Offices").PostAsync(new officeModel { Name = "Pintor", });
                client.Child("Offices").PostAsync(new officeModel { Name = "Jardinero", });
                client.Child("Offices").PostAsync(new officeModel { Name = "Técnico en aire acondicionado", });
                client.Child("Offices").PostAsync(new officeModel { Name = "Soldador", });
                client.Child("Offices").PostAsync(new officeModel { Name = "Servicio doméstico", });
            }
        }
    }
}
