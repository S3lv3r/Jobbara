namespace Jobbara.Pages;

public partial class Pagos : ContentPage
{
	public Pagos()
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
}