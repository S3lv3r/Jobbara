namespace Jobbara.Pages;


public partial class mapChamber : ContentPage
{
    public mapChamber()
    {

        InitializeComponent();
    }
    
    private async void OnAceptarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Aceptado", "Has aceptado esta solicitud", "OK");
        // Aqu� puedes agregar navegaci�n u otra l�gica
    }
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Rechazado", "Has rechazado esta solicitud", "OK");
        // Aqu� puedes agregar navegaci�n u otra l�gica
    }
}