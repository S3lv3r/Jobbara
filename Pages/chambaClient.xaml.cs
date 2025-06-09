namespace Jobbara.Pages;

public partial class chambaClient : ContentPage
{
	public chambaClient()
	{
		InitializeComponent();
	}
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Cancelado", "Has cancelado esta solicitud", "OK");
        // Aquí puedes agregar navegación u otra lógica
    }
}