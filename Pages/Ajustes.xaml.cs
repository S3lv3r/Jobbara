using Jobbara.Models;
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
        // Aqu� podr�as limpiar preferencias, tokens, etc.
        await DisplayAlert("Cerrando sesi�n", "Has cerrado sesi�n, vaquero.", "OK");
        // Navegar al login u otra p�gina
    }

    private async void OnBorrarCuenta(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("�Est�s seguro?", "Esta acci�n eliminar� tu cuenta para siempre.", "S�, borrar", "Cancelar");
        if (confirm)
        {
            // Aqu� podr�as llamar a un servicio que borre la cuenta
            await DisplayAlert("Cuenta borrada", "Tu cuenta ha sido eliminada. Adi�s, cowboy ??", "OK");
            // Navegar al login u otra p�gina
        }
    }

    private async void OnFreeMoiney(object sender, EventArgs e)
    {

        await DisplayAlert("Free Moiney", "Pues vu�lvete chambeador y gana dinero ??", "OK");
    }
    private async void GoToChamba(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//chamba");
        
    }
}
