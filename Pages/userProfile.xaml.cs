using Jobbara.Models;
using System.Text.RegularExpressions;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace Jobbara.Pages;

public partial class userProfile : ContentPage
{
<<<<<<< HEAD
    private async void OnGoToHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homePage");
    }
    private async void OnGoToNot(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//notificaciones");
    }
=======
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
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
<<<<<<< HEAD

=======
    
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/"); // Referencia a la base de datos
    private async void CreateAWorkNotificationClicked(object sender, EventArgs e)
    {
        var users = await client
            .Child("Users")
            .OnceAsync<usersModel>();

        foreach (var user in users)
        {
<<<<<<< HEAD
            if (user.Object.isWorker && user.Object.office == "carpintero")
=======
            if(user.Object.isWorker && user.Object.office == "carpintero")
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
            {
                await client
                    .Child("Users")
                    .Child(user.Key)
<<<<<<< HEAD
                    .Child("alertWork")
=======
                    .Child("alertWork") 
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
                    .PutAsync(true);
            }
        }
    }

    public async Task OnListeningAlert()
    {
<<<<<<< HEAD
        while (true)
=======
        while(true)
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
        {
            var users = await client
                .Child("Users")
                .OnceAsync<dynamic>();

            var userCurrent = users.FirstOrDefault(u => u.Object.username == UserSessionData.username_usd);

<<<<<<< HEAD
            if (userCurrent != null)
=======
            if(userCurrent != null)
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
            {
                var alertWork = userCurrent.Object.alertWork;
                if (alertWork == true)
                {
                    await DisplayAlert("Alerta de Trabajo", "Hay un nuevo trabajo disponible para ti.", "OK");
                }
            }

<<<<<<< HEAD
            await Task.Delay(1000);
=======
            await Task.Delay(1000); 
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
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
<<<<<<< HEAD
        await Shell.Current.GoToAsync("//newChambeador");
=======
        await Shell.Current.GoToAsync("newChambeador");
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
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

<<<<<<< HEAD
    private async void GoToAjustes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ajustes");
    }

    private async void GoToPagos(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//pagos");
    }
}
=======
}
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
