namespace Jobbara.Pages;

public partial class homePage : ContentPage
{
	public homePage()
	{
		InitializeComponent();
	}
<<<<<<< HEAD
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
=======
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
}