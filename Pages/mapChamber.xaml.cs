using Firebase.Database;
using Firebase.Database.Query;
using Jobbara.Models;

namespace Jobbara.Pages;


public partial class mapChamber : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    public mapChamber()
    {

        InitializeComponent();
        LoadDataNotification();
    }

    public async void LoadDataNotification()
    {
        var notificaciones = await client
        .Child("WorkAlert")
        .OnceAsync<notificationWorkModel>();


        var noti = notificaciones.FirstOrDefault(n => n.Key == notificationWorkModel.key);


        if (noti != null)
        {
            lblName.Text = noti.Object.userRequesting;
            jobLbl.Text = noti.Object.officeRequired.Name;
            addressLbl.Text = noti.Object.address;
            paymentLbl.Text = noti.Object.payment;
        }
    }
    
    private async void OnAceptarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Aceptado", "Has aceptado esta solicitud", "OK");

        await client
                    .Child("WorkAlert")
                    .Child(notificationWorkModel.key)
                    .Child("isAccepted")
                    .PutAsync(true);
        await client
            .Child("WorkAlert")
            .Child(notificationWorkModel.key)
            .PatchAsync(new { whoAccepted = UserSessionData.username_usd });

        await Shell.Current.GoToAsync("//homePage");
    }
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Rechazado", "Has rechazado esta solicitud", "OK");
        notificationRejects.keyRejects.Add(notificationWorkModel.key);
        await Shell.Current.GoToAsync("//homePage");
    }
}