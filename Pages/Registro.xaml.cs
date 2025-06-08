using System.Text.RegularExpressions;
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
            await DisplayAlert("Error", "El correo electronico no es valido.", "OK");
            return;
        }

        // 3. Validaci�n de contrase�a
        if (password.Length < 6)
        {
            await DisplayAlert("Error", "La contrasena debe tener al menos 6 caracteres.", "OK");
            return;
        }

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
            isWorker = false,   
            office = string.Empty,
            alertWork = false
        });

        await DisplayAlert("�xito", "Usuario registrado correctamente.", "OK");
<<<<<<< HEAD
        await Shell.Current.GoToAsync("//loginPage");
=======
        await Shell.Current.GoToAsync("//HomePage");
>>>>>>> 142eef0d389d5f4b6554b01b49634745e9e07cc3
    }
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}
