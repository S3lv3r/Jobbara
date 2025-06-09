namespace Jobbara.Pages;

public partial class newChamba : ContentPage
{
	public newChamba()
	{
		InitializeComponent();
	}
    private async void OnAceptarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//mapa");
        await DisplayAlert("Chamba publicada", "Tu solicitud fue enviada exitosamente. ¡Espérate a que te llamen, vaquero!", "OK");
        
    }
}