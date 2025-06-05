using Firebase.Database;
using Firebase.Database.Query;
using Jobbara.Models;
namespace Jobbara.Pages;

public partial class inicioSesion : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    public inicioSesion()
	{
		InitializeComponent();
	}
    private async void OnLogin(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//signPage");
    }

    private void LoginUserDB_Cliked(object sender, EventArgs e)
    {
        VerifyUser();
    }

    private async void VerifyUser()
    {
        var emailInTextBox = emailTxt.Text;
        var passwordInTextBox = passwordTxt.Text;

        var usersDB = await client
            .Child("Users")
            .OnceAsync<usersModel>();

        var matchingEmail = usersDB.FirstOrDefault(u => u.Object.email == emailInTextBox);

        if (matchingEmail == null)
        {
            await DisplayAlert("Error", "No hay una cuanta asociada a ese correo electronico.", "OK");
            return;
        }

        if (matchingEmail.Object.password != passwordInTextBox)
        {
            await DisplayAlert("Error", "Contraseña incorrecta.", "OK");
            return;
        }

        await DisplayAlert("Éxito", "Inicio de sesion correctamente.", "OK");
        await Shell.Current.GoToAsync("//HomePage");
    }
}