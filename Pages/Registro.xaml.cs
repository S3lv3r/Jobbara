using Firebase.Database;
using Firebase.Database.Query;
using Jobbara.Models;
namespace Jobbara.Pages;

public partial class Registro : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    public Registro()
	{
		InitializeComponent();
	}
    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//loginPage");
    }

    private void RegisterUserDB_Cliked(object sender, EventArgs e)
    {
        saveUserData();
    }
    private async void saveUserData()
    {
        var usernameInTextBox = userNameTxt.Text;

        var usersDB = await client
            .Child("Users")
            .OnceAsync<usersModel>();

        bool exist = usersDB.Any(u => u.Object.username == usernameInTextBox);

        if (exist)
        {
            await DisplayAlert("Error", "El nombre de usuario ya está en uso.", "OK");
            return;
        }

        await client.Child("Users").PostAsync(new usersModel
        {
            username = usernameInTextBox,
            email = emailTxt.Text,
            password = passwordTxt.Text,
        });

        await DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
        await Shell.Current.GoToAsync("//HomePage");
    }

}