using Jobbara.Models;
using System.Text.RegularExpressions;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace Jobbara.Pages;

public partial class userProfile : ContentPage
{
    private async void OnGoToHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homePage");
    }
    private async void OnGoToNot(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//notificaciones");
    }
    public userProfile()
    {
        InitializeComponent();
        LoadDataUser();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MostrarDatosChambeador();
        _ = OnListeningAlert();
    }

    private void LoadDataUser() // Esto carga los datos del usuario guardados en UserSessionData.cs, y los pone en pantalla
    {

        usernameLbl.Text = UserSessionData.username_usd;
    }
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/"); // Referencia a la base de datos


    private async void CreateAWorkNotificationClicked(object sender, EventArgs e)
    {
        var users = await client
            .Child("Users")
            .OnceAsync<usersModel>();

        foreach (var user in users)
        {

            if (user.Object.isWorking)
            {
                await client
                    .Child("Users")
                    .Child(user.Key)

                    .Child("alertWork")
                    .PutAsync(true);
            }
        }
    }

    public async Task OnListeningAlert()
    {

        while (true)

        {
            var users = await client
                .Child("Users")
                .OnceAsync<dynamic>();

            var userCurrent = users.FirstOrDefault(u => u.Object.username == UserSessionData.username_usd);

            if (userCurrent != null)

            {
                var alertWork = userCurrent.Object.alertWork;
                if (alertWork == true)
                {
                    await DisplayAlert("Alerta de Trabajo", "Hay un nuevo trabajo disponible para ti.", "OK");
                }
            }

            await Task.Delay(1000);
        }
    }
    private void MostrarDatosChambeador()
    {
        bool isWorker = Preferences.Get("IsWorker", false);

        if (isWorker)
        {
            // Si ya es chambeador, mostramos la sección con sus datos
            BecomeWorkerButton.IsVisible = false;
            WorkerDataSection.IsVisible = true;

            OficioLabel.Text = Preferences.Get("Oficio", "No especificado");
            IneLabel.Text = Preferences.Get("INE", "No especificado");
            CurpLabel.Text = Preferences.Get("CURP", "No especificado");
            DomicilioLabel.Text = Preferences.Get("Domicilio", "No especificado");
            RfcLabel.Text = Preferences.Get("RFC", "No especificado");
        }
        else
        {
            // Si no es chambeador, solo muestra el botón
            BecomeWorkerButton.IsVisible = true;
            WorkerDataSection.IsVisible = false;
        }
    }

    private async void OnBecomeWorkerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("newChambeador");
    }
    private void OnDeleteChambeadorData(object sender, EventArgs e)
    {
        Preferences.Remove("IsWorker");
        Preferences.Remove("Oficio");
        Preferences.Remove("INE");
        Preferences.Remove("CURP");
        Preferences.Remove("Domicilio");
        Preferences.Remove("RFC");
        DisplayAlert("Datos borrados", "Ya no eres un chambeador, vaquero ??", "OK");
        MostrarDatosChambeador();
    }

    private async void GoToAjustes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ajustes");
    }

    private async void GoToPagos(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//pagos");
    }
}
