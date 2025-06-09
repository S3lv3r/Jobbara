using Firebase.Database;
using Firebase.Database.Query;
using Jobbara.Models;
namespace Jobbara.Pages;

public partial class newChamba : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    public List<officeModel> officesList { get; set; }
    public newChamba()
    {
        InitializeComponent();
        BindingContext = this;
        _ = LoadOffices();
    }
    public async Task LoadOffices() 
    {
        var offices = await client.Child("Offices").OnceAsync<officeModel>();
        officesList = offices.Select(x => x.Object).ToList();
        officePicker.ItemsSource = officesList;
    }
    private async void OnAceptarClicked(object sender, EventArgs e)
    {

        officeModel office = officePicker.SelectedItem as officeModel;
        var usersDB = await client
            .Child("WorkAlert")
            .OnceAsync<usersModel>();
        var key = await client.Child("WorkAlert").PostAsync(new notificationWorkModel
        {
            userRequesting = UserSessionData.username_usd,
            officeRequired = office,
            workDescription = descNotiLbl.Text?.Trim(),
            payment = payNotiLbl.Text?.Trim(),
            address = adressNotiLbl.Text?.Trim(),
            isAccepted = false,
            whoAccepted = string.Empty

        });

        notificationWorkModel.key = key.Key;
        await DisplayAlert("Chamba publicada", "Tu solicitud fue enviada exitosamente. ¡Espérate a que te llamen", "OK");
        await Shell.Current.GoToAsync("//chambaclient");
    }
}