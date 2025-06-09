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
        // Aquí puedes agregar navegación u otra lógica
    }
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Rechazado", "Has rechazado esta solicitud", "OK");
        // Aquí puedes agregar navegación u otra lógica
    }
}