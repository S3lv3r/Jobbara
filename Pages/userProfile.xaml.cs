using System;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Jobbara.Models;
using Microsoft.Maui.Controls;

namespace Jobbara.Pages;

[QueryProperty(nameof(ImagePath), "imagePath")]
public partial class userProfile : ContentPage
{
    private string imagePath;

    public string ImagePath
    {
        get => imagePath;
        set
        {
            imagePath = value;
            OnPropertyChanged();
            UpdateProfileImage(imagePath);
        }
    }

    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");

    public userProfile()
    {
        InitializeComponent();
        LoadDataUser();
    }

    private void UpdateProfileImage(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            ProfileImage.Source = ImageSource.FromFile(path);
        }
        else
        {
            ProfileImage.Source = "profile_default.png";
        }
    }

    private async void OnGoToHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homePage");
    }

    private async void OnGoToNot(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//notificaciones");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateProfileImage(ImagePath); // Asegura que la imagen se actualice si se recibió por parámetro
        MostrarDatosChambeador();
        _ = OnListeningAlert();
    }

    private void LoadDataUser()
    {
        usernameLbl.Text = UserSessionData.username_usd;
        lblUsuario.Text = "Usuario: " + UserSessionData.username_usd;
        lblCorreo.Text = "Correo: " + UserSessionData.email_usd;
    }

    private async void CreateAWorkNotificationClicked(object sender, EventArgs e)
    {
        var users = await client
            .Child("Users")
            .OnceAsync<usersModel>();

        foreach (var user in users)
        {
            if (user.Object.isWorker && user.Object.office == "carpintero")
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

    private async void MostrarDatosChambeador()
    {
        try
        {
            var user = await client
                .Child("Users")
                .Child(UserSessionData.username_usd)
                .OnceSingleAsync<ChambeadorModel>();

            if (user != null && user.IsWorker)
            {
                BecomeWorkerButton.IsVisible = false;
                WorkerDataSection.IsVisible = true;

                OficioLabel.Text = user.Oficio;
                IneLabel.Text = user.INE;
                CurpLabel.Text = user.CURP;
                DomicilioLabel.Text = user.Domicilio;
                RfcLabel.Text = user.RFC;
            }
            else
            {
                BecomeWorkerButton.IsVisible = true;
                WorkerDataSection.IsVisible = false;
            }
        }
        catch
        {
            BecomeWorkerButton.IsVisible = true;
            WorkerDataSection.IsVisible = false;
        }
    }

    private async void OnBecomeWorkerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//newChambeador");
    }

    private async Task BorrarDatosDeFirebase()
    {
        try
        {
            var userRef = client.Child("Users").Child(UserSessionData.username_usd);

            var user = await userRef.OnceSingleAsync<ChambeadorModel>();

            if (user != null)
            {
                user.INE = string.Empty;
                user.CURP = string.Empty;
                user.RFC = string.Empty;
                user.Oficio = string.Empty;
                user.Domicilio = string.Empty;
                user.IsWorker = false;

                await userRef.PutAsync(user);

                await DisplayAlert("Datos eliminados", "Tus datos como chambeador han sido eliminados.", "OK");

                MostrarDatosChambeador();
            }
            else
            {
                await DisplayAlert("Error", "No se encontraron datos para borrar.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    private async void GoToAjustes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ajustes");
    }

    private async void GoToPagos(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//pagos");
    }

    private async void OnDeleteChambeadorData(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmar", "¿Seguro que deseas borrar tus datos como chambeador?", "Sí", "Cancelar");

        if (confirm)
        {
            await BorrarDatosDeFirebase();
        }
    }

    private async void GoToChamba(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//chamba");
    }

    private async void OnEditPerfilClicked(object sender, EventArgs e)
    {
        string inputUsername = await DisplayPromptAsync(
            "Verificación",
            "Ingresa tu nombre de usuario:",
            placeholder: "Usuario",
            maxLength: 100,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(inputUsername))
        {
            await DisplayAlert("Cancelado", "No ingresaste un nombre de usuario.", "OK");
            return;
        }

        string inputPassword = await DisplayPromptAsync(
            "Verificación",
            "Ingresa tu contraseña:",
            placeholder: "Contraseña",
            maxLength: 100,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(inputPassword))
        {
            await DisplayAlert("Cancelado", "No ingresaste una contraseña.", "OK");
            return;
        }

        try
        {
            var users = await client
                .Child("Users")
                .OnceAsync<usersModel>();

            var matchedUser = users.FirstOrDefault(u =>
                u.Object.username == inputUsername && u.Object.password == inputPassword);

            if (matchedUser != null)
            {
                await DisplayAlert("Éxito", "Autenticación correcta.", "OK");

                // Simulamos que ya tienes la imagen seleccionada
                string rutaImagen = imagePath ?? string.Empty;

                await Shell.Current.GoToAsync($"//editarPerfil?imagePath={Uri.EscapeDataString(rutaImagen)}");
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}
