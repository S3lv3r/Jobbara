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
        string username = userNameTxt.Text?.Trim() ?? "";
        string email = emailTxt.Text?.Trim().ToLower() ?? "";  // Normalizamos a minúsculas
        string password = passwordTxt.Text ?? "";

        // Validación básica
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
            return;
        }

        // Validación de correo electrónico
        if (!IsValidEmail(email))
        {
            await DisplayAlert("Error", "El correo electrónico no es válido.", "OK");
            return;
        }

        // Validación de contraseña
        if (password.Length < 6)
        {
            await DisplayAlert("Error", "La contraseña debe tener al menos 6 caracteres.", "OK");
            return;
        }

        // Obtener lista de usuarios ya registrados
        var usersDB = await client
            .Child("Users")
            .OnceAsync<usersModel>();

        // Validar si ya existe el usuario (case insensitive)
        bool usernameExists = usersDB.Any(u =>
            !string.IsNullOrEmpty(u.Object.username) &&
            u.Object.username.Trim().Equals(username, StringComparison.OrdinalIgnoreCase));

        if (usernameExists)
        {
            await DisplayAlert("Error", "El nombre de usuario ya está en uso.", "OK");
            return;
        }

        // Validar si ya existe el correo (case insensitive)
        bool emailExists = usersDB.Any(u =>
            !string.IsNullOrEmpty(u.Object.email) &&
            u.Object.email.Trim().ToLower() == email);

        if (emailExists)
        {
            await DisplayAlert("Error", "El correo electrónico ya está registrado.", "OK");
            return;
        }

        // Guardar nuevo usuario en Firebase
        await client.Child("Users").PostAsync(new usersModel
        {
            username = username,
            email = email,
            password = password,
            isWorker = false,
            office = string.Empty,
            alertWork = false
        });

        await DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");

        await Shell.Current.GoToAsync("//loginPage");
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}
