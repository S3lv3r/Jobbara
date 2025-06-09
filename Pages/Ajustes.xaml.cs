namespace Jobbara.Pages;

public partial class Ajustes : ContentPage
{
    public Ajustes()
    {
        InitializeComponent();
    }
    private async void OnGoToHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homePage");
    }
    private async void OnGoToNot(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//notificaciones");
    }
    private async void GoToAjustes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ajustes");
    }

    private async void GoToPagos(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//pagos");
    }
    private async void OnCerrarSesion(object sender, EventArgs e)
    {
        // Aquí podrías limpiar preferencias, tokens, etc.
        await DisplayAlert("Cerrando sesión", "Has cerrado sesión, vaquero.", "OK");
        // Navegar al login u otra página
    }

    private async void OnBorrarCuenta(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("¿Estás seguro?", "Esta acción eliminará tu cuenta para siempre.", "Sí, borrar", "Cancelar");
        if (confirm)
        {
            // Aquí podrías llamar a un servicio que borre la cuenta
            await DisplayAlert("Cuenta borrada", "Tu cuenta ha sido eliminada. Adiós, cowboy ??", "OK");
            // Navegar al login u otra página
        }
    }

    private async void OnFreeMoiney(object sender, EventArgs e)
    {
        await DisplayAlert("Free Moiney", "Pues vuélvete chambeador y gana dinero ??", "OK");
    }
    private async void GoToChamba(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//chambaclient");
    }
}