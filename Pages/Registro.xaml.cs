<<<<<<< HEAD
=======
using System.Text.RegularExpressions;
>>>>>>> 50b5b32 (Primer commit bro)
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
<<<<<<< HEAD
=======
        string username = userNameTxt.Text?.Trim();
        string email = emailTxt.Text?.Trim();
        string password = passwordTxt.Text;

        // 1. Validaci�n b�sica
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
            return;
        }

        // 2. Validaci�n de correo electr�nico
        if (!IsValidEmail(email))
        {
            await DisplayAlert("Error", "El correo electr�nico no es v�lido.", "OK");
            return;
        }

        // 3. Validaci�n de contrase�a
        if (password.Length < 6)
        {
            await DisplayAlert("Error", "La contrase�a debe tener al menos 6 caracteres.", "OK");
            return;
        }

>>>>>>> 50b5b32 (Primer commit bro)
        var usernameInTextBox = userNameTxt.Text;

        var usersDB = await client
            .Child("Users")
            .OnceAsync<usersModel>();

        bool exist = usersDB.Any(u => u.Object.username == usernameInTextBox);

        if (exist)
        {
            await DisplayAlert("Error", "El nombre de usuario ya est� en uso.", "OK");
            return;
        }

        await client.Child("Users").PostAsync(new usersModel
        {
            username = usernameInTextBox,
            email = emailTxt.Text,
            password = passwordTxt.Text,
        });

        await DisplayAlert("�xito", "Usuario registrado correctamente.", "OK");
        await Shell.Current.GoToAsync("//HomePage");
    }
<<<<<<< HEAD

}
=======
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}
>>>>>>> 50b5b32 (Primer commit bro)
