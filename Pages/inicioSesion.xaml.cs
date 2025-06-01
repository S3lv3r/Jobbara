namespace Jobbara.Pages;

public partial class inicioSesion : ContentPage
{
	public inicioSesion()
	{
		InitializeComponent();
	}
    private async void OnLogin(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NavigationPage(new Pages.Registro()));
    }
}