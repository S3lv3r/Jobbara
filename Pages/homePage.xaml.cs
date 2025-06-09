namespace Jobbara.Pages;

public partial class homePage : ContentPage
{
    public homePage()
    {
        InitializeComponent();
    }
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
    private async void GoToChamba(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//chamba");
    }
}