<<<<<<< HEAD
=======
using System.Text.RegularExpressions;
>>>>>>> 50b5b32 (Primer commit bro)
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
<<<<<<< HEAD
        var emailInTextBox = emailTxt.Text;
        var passwordInTextBox = passwordTxt.Text;

=======
        string emailInTextBox = emailTxt.Text?.Trim();
        string passwordInTextBox = passwordTxt.Text;
        if (string.IsNullOrWhiteSpace(emailInTextBox) || string.IsNullOrWhiteSpace(passwordInTextBox))
        {
        await DisplayAlert("Error", "Por favor, completa ambos campos.", "OK");
        return;
        }
        if (passwordInTextBox.Length < 6)
        {
            await DisplayAlert("Error", "La contraseña debe tener al menos 6 caracteres.", "OK");
            return;
        }
        if (!IsValidEmail(emailInTextBox))
        {
            await DisplayAlert("Error", "El correo electrónico no es válido.", "OK");
            return;
        }
>>>>>>> 50b5b32 (Primer commit bro)
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
<<<<<<< HEAD
        await Shell.Current.GoToAsync("//HomePage");
=======
        await Shell.Current.GoToAsync("//userProfile");
    }
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
>>>>>>> 50b5b32 (Primer commit bro)
    }
}