using System.Text.RegularExpressions;
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
        var emailInTextBox = emailTxt.Text?.Trim();
        var passwordInTextBox = passwordTxt.Text?.Trim();

        if (string.IsNullOrWhiteSpace(emailInTextBox) || string.IsNullOrWhiteSpace(passwordInTextBox))
        {
            await DisplayAlert("Error", "Por favor, completa ambos campos.", "OK");
            return;
        }

        List<FirebaseObject<usersModel>> usersDB = null;

        try
        {
            usersDB = (await client
                .Child("Users")
                .OnceAsync<usersModel>()).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo acceder a la base de datos: {ex.Message}", "OK");
            return;
        }

        var matchingEmail = usersDB.FirstOrDefault(u => u.Object.email == emailInTextBox);

        if (matchingEmail == null)
        {
            await DisplayAlert("Error", "No hay una cuenta asociada a ese correo electr�nico.", "OK");
            return;
        }

        if (matchingEmail.Object.password != passwordInTextBox)
        {
            await DisplayAlert("Error", "Contrase�a incorrecta.", "OK");
            return;
        }

        UserSessionData.username_usd = matchingEmail.Object.username;
        UserSessionData.email_usd = matchingEmail.Object.email;
        UserSessionData.password_usd = matchingEmail.Object.password;

        await DisplayAlert("�xito", "Inicio de sesi�n correctamente.", "OK");
        await Shell.Current.GoToAsync("//homePage");
    }

}