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
    }
    private async void OnBecomeWorkerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//newChambeador");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        LoadDataUser();
        UpdateProfileImage(ImagePath);
        await MostrarDatosChambeador();

        // Lanzar el listener de alertas (sin bloquear UI)
        _ = OnListeningAlert();
    }

    private void LoadDataUser()
    {
        usernameLbl.Text = UserSessionData.username_usd;
        lblUsuario.Text = "Usuario: " + UserSessionData.username_usd;
        lblCorreo.Text = "Correo: " + UserSessionData.email_usd;
    }

    private void UpdateProfileImage(string path = null)
    {
        string clave = UserSessionData.email_usd.Replace(".", "_").ToLower();

        // Usar el parámetro recibido (si existe), o buscar la ruta guardada local
        string rutaImagen = !string.IsNullOrEmpty(path)
            ? path
            : Preferences.Get($"user_profile_image_{clave}", string.Empty);

        if (!string.IsNullOrEmpty(rutaImagen) && File.Exists(rutaImagen))
        {
            ProfileImage.Source = ImageSource.FromFile(rutaImagen);
        }
        else
        {
            ProfileImage.Source = "profile_default.png"; // Imagen por defecto en Resources
        }

        // Si se recibió una ruta nueva, guardar en Preferences
        if (!string.IsNullOrEmpty(path))
        {
            Preferences.Set($"user_profile_image_{clave}", path);
        }
    }


    private async Task MostrarDatosChambeador()
    {
        try
        {
            string safeKey = UserSessionData.email_usd.Replace(".", "_");

            var chambeador = await client
                .Child("Chambeadores")
                .Child(safeKey)
                .OnceSingleAsync<ChambeadorModel>();

            if (chambeador != null && chambeador.isWorker)
            {
                BecomeWorkerButton.IsVisible = false;
                WorkerDataSection.IsVisible = true;

                OficioLabel.Text = chambeador.oficio ?? "No especificado";
                IneLabel.Text = chambeador.ine ?? "No especificado";
                CurpLabel.Text = chambeador.curp ?? "No especificado";
                DomicilioLabel.Text = chambeador.domicilio ?? "No especificado";
                RfcLabel.Text = chambeador.rfc ?? "No especificado";
            }
            else
            {
                BecomeWorkerButton.IsVisible = true;
                WorkerDataSection.IsVisible = false;

                OficioLabel.Text = "";
                IneLabel.Text = "";
                CurpLabel.Text = "";
                DomicilioLabel.Text = "";
                RfcLabel.Text = "";
            }
        }
        catch
        {
            BecomeWorkerButton.IsVisible = true;
            WorkerDataSection.IsVisible = false;
        }
    }

    private async Task BorrarDatosDeFirebase()
    {
        try
        {
            string safeKey = UserSessionData.email_usd.Replace(".", "_");
            var chambeadorRef = client.Child("Chambeadores").Child(safeKey);
            await chambeadorRef.DeleteAsync();

            await DisplayAlert("Datos eliminados", "Tus datos como chambeador han sido eliminados.", "OK");
            await MostrarDatosChambeador();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteChambeadorData(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmar", "¿Seguro que deseas borrar tus datos como chambeador?", "Sí", "Cancelar");

        if (confirm)
        {
            await BorrarDatosDeFirebase();

            UserSessionData.office_usd = string.Empty;
            UserSessionData.ine_usd = string.Empty;
            UserSessionData.curp_usd = string.Empty;
            UserSessionData.address_usd = string.Empty;
            UserSessionData.rfc_usd = string.Empty;

            await MostrarDatosChambeador();
        }
    }

    private async void OnEditPerfilClicked(object sender, EventArgs e)
    {
        string inputUsername = await DisplayPromptAsync("Verificación", "Ingresa tu nombre de usuario:");
        if (string.IsNullOrWhiteSpace(inputUsername)) return;

        string inputPassword = await DisplayPromptAsync("Verificación", "Ingresa tu contraseña:");
        if (string.IsNullOrWhiteSpace(inputPassword)) return;

        try
        {
            var users = await client.Child("Users").OnceAsync<usersModel>();
            var matched = users.FirstOrDefault(u =>
                u.Object.username == inputUsername && u.Object.password == inputPassword);

            if (matched != null)
            {
                string rutaImagen = imagePath ?? string.Empty;

                // Solo verificación, sin cambio de correo aquí
                await Shell.Current.GoToAsync($"//editarPerfil?imagePath={Uri.EscapeDataString(rutaImagen)}");
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


    // Método para migrar nodo de chambeador si cambió el correo
    private async Task ActualizarCorreoChambeadorAsync(string correoAnterior, string correoNuevo)
    {
        if (string.IsNullOrWhiteSpace(correoAnterior) || string.IsNullOrWhiteSpace(correoNuevo))
            return;

        string claveAnterior = correoAnterior.Replace(".", "_");
        string claveNueva = correoNuevo.Replace(".", "_");

        if (claveAnterior == claveNueva)
            return; // No cambió, no hacer nada

        var chambeadorAnterior = await client
            .Child("Chambeadores")
            .Child(claveAnterior)
            .OnceSingleAsync<ChambeadorModel>();

        if (chambeadorAnterior != null)
        {
            await client.Child("Chambeadores").Child(claveNueva).PutAsync(chambeadorAnterior);
            await client.Child("Chambeadores").Child(claveAnterior).DeleteAsync();

            // Actualizar datos en sesión
            UserSessionData.ine_usd = chambeadorAnterior.ine;
            UserSessionData.curp_usd = chambeadorAnterior.curp;
            UserSessionData.address_usd = chambeadorAnterior.domicilio;
            UserSessionData.rfc_usd = chambeadorAnterior.rfc;
            UserSessionData.office_usd = chambeadorAnterior.oficio;

            await DisplayAlert("Actualización", "Tu correo y datos se han actualizado correctamente.", "OK");
        }
    }

    // Prompt para obtener nuevo correo
    private Task<string> ObtenerNuevoCorreoAsync()
    {
        return DisplayPromptAsync("Nuevo correo", "Ingresa tu nuevo correo:");
    }

    private async void GoToAjustes(object sender, EventArgs e) => await Shell.Current.GoToAsync("//ajustes");
    private async void GoToPagos(object sender, EventArgs e) => await Shell.Current.GoToAsync("//pagos");
    private async void GoToChamba(object sender, EventArgs e) => await Shell.Current.GoToAsync("//chambaclient");

    // Alert listener
    public async Task OnListeningAlert()
    {
        while (true)
        {
            var users = await client.Child("Users").OnceAsync<usersModel>();
            var current = users.FirstOrDefault(u => u.Object.username == UserSessionData.username_usd);

            if (current?.Object.alertWork == true)
            {
                await DisplayAlert("Alerta de Trabajo", "Hay un nuevo trabajo disponible para ti.", "OK");
            }

            await Task.Delay(1000);
        }
    }

    private async void CreateAWorkNotificationClicked(object sender, EventArgs e)
    {
        var users = await client.Child("Users").OnceAsync<usersModel>();
        foreach (var user in users)
        {
            if (user.Object.office != null && user.Object.office.Name == "carpintero")
            {
                await client.Child("Users").Child(user.Key).Child("alertWork").PutAsync(true);
            }
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
}
