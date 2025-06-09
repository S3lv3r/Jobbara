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
        try
        {
            var emailInTextBox = emailTxt.Text;
            var passwordInTextBox = passwordTxt.Text;

            if (string.IsNullOrWhiteSpace(emailInTextBox) || string.IsNullOrWhiteSpace(passwordInTextBox))
            {
                await DisplayAlert("Error", "Por favor, completa ambos campos.", "OK");
                return;
            }

            var usersDB = await client
                .Child("Users")
                .OnceAsync<usersModel>();

            var matchingEmail = usersDB.FirstOrDefault(u => u.Object.email == emailInTextBox);

            if (matchingEmail == null)
            {
                await DisplayAlert("Error", "No hay una cuenta asociada a ese correo electrónico.", "OK");
                return;
            }

            if (matchingEmail.Object.password != passwordInTextBox)
            {
                await DisplayAlert("Error", "Contraseña incorrecta.", "OK");
                return;
            }

            UserSessionData.username_usd = matchingEmail.Object.username;
            UserSessionData.email_usd = matchingEmail.Object.email;
            UserSessionData.password_usd = matchingEmail.Object.password;
            UserSessionData.userKey_usd = matchingEmail.Key;
            UserSessionData.office_usd = matchingEmail.Object.office?.Name ?? "No especificado";

            await DisplayAlert("Éxito", "Inicio de sesión correcto.", "OK");
            await Shell.Current.GoToAsync("//ajustes");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error inesperado", ex.Message, "OK");
        }
    }

}