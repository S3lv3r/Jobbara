using Firebase.Database;
using Firebase.Database.Query;
using Jobbara.Models;
namespace Jobbara.Pages;

public partial class chambaClient : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    private CancellationTokenSource _cts;
    private string notificationKey = string.Empty;
    public chambaClient()
	{
		InitializeComponent();
        LoadNotifications();
	}
    private async void LoadNotifications()
    {
        var notificaciones = await client
        .Child("WorkAlert")
        .OnceAsync<notificationWorkModel>();


        var noti = notificaciones
            .FirstOrDefault(n => n.Key == notificationWorkModel.key);

        if (noti != null)
        {
            nameLbl.Text = noti.Object.userRequesting;
            jobTypeLbl.Text = noti.Object.officeRequired?.Name;
            var firebaseKey = noti.Key;

            if (notificationKey != firebaseKey)
            {
                notificationKey = firebaseKey;
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                _ = OnListeningAlert(_cts.Token);
            }
        }
    }

    
    public async Task OnListeningAlert(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var users = await client
                .Child("WorkAlert")
                .OnceAsync<dynamic>();

            var userCurrent = users.FirstOrDefault(u =>
                u.Key == notificationKey &&
                u.Object.isAccepted == true);

            if (userCurrent != null)
            {
                string whoAccepted = userCurrent.Object.whoAccepted;
                await DisplayAlert("Solicitud Aceptada", $"Tu solicitud fue aceptada por {whoAccepted}.", "OK");
                await Shell.Current.GoToAsync("//homePage");

                _cts.Cancel();
                break;
            }

            await Task.Delay(1000);
        }
    }



    
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(notificationKey))
        {
            await client
                .Child("WorkAlert")
                .Child(notificationKey)
                .DeleteAsync();
        }

        _cts?.Cancel(); 
        await DisplayAlert("Cancelado", "Has cancelado esta solicitud", "OK");
        await Shell.Current.GoToAsync("//homePage");
    }
}