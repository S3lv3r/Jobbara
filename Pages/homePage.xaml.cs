using Jobbara.Models;
using Firebase.Database;

namespace Jobbara.Pages;

public partial class homePage : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    public homePage()
    {
        InitializeComponent();
        LoadDataUser();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = OnListeningAlert();
    }
    public async Task OnListeningAlert()
    {
        while (true)
        {
            var users = await client
                .Child("WorkAlert")
                .OnceAsync<dynamic>();

            var userCurrent = users.FirstOrDefault(u =>
            u.Object.officeRequired.Name == UserSessionData.office_usd &&
            u.Object.isAccepted == false &&
            !notificationRejects.keyRejects.Contains(u.Key) 
        );

            if (userCurrent != null)
            {
                notificationWorkModel.key = userCurrent.Key;
                await Shell.Current.GoToAsync("//mapa");
            }

            await Task.Delay(1000);
        }
    }
    private void LoadDataUser()
    {
        usernameLbl.Text = UserSessionData.username_usd;
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
        await Shell.Current.GoToAsync("chamba");
    }
}