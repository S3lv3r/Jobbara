namespace Jobbara.Pages;

public partial class Registro : ContentPage
{
	public Registro()
	{
		InitializeComponent();
	}
    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NavigationPage(new Pages.inicioSesion()));
    }

}