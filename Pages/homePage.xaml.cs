using Jobbara.Models;

namespace Jobbara.Pages;

public partial class homePage : ContentPage
{
    public homePage()
    {
        InitializeComponent();
        LoadDataUser();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarImagenPerfil();
    }

    private void CargarImagenPerfil()
    {
        try
        {
            string clave = UserSessionData.email_usd.Replace(".", "_").ToLower();
            string rutaImagen = Preferences.Get($"user_profile_image_{clave}", string.Empty);

            if (!string.IsNullOrEmpty(rutaImagen) && File.Exists(rutaImagen))
            {
                ImgPerfil.Source = ImageSource.FromFile(rutaImagen);
            }
            else
            {
                ImgPerfil.Source = "usuario.png"; // Imagen por defecto si no hay imagen guardada
            }
        }
        catch
        {
            ImgPerfil.Source = "usuario.png"; // Por si falla algo
        }
    }
    private void LoadDataUser()
    {
        usernameLbl.Text = UserSessionData.username_usd;
    }

    private async void OnGoToProfile(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//userProfile");
    }

    private async void GoToAjustes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ajustes");
    }

    private async void GoToPagos(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//pagos");
    }
    private async void OnGoToNot(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//notificaciones");
    }
    private async void GoToChamba(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//chambaclient");
    }
}