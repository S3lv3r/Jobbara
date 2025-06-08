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
        var emailInTextBox = emailTxt.Text;
        var passwordInTextBox = passwordTxt.Text;

        string emailInTextBox2 = emailTxt.Text?.Trim();
        string passwordInTextBox2 = passwordTxt.Text;
        if (string.IsNullOrWhiteSpace(emailInTextBox) || string.IsNullOrWhiteSpace(passwordInTextBox))
        {
        await DisplayAlert("Error", "Por favor, completa ambos campos.", "OK");
        return;
        }
        if (passwordInTextBox.Length < 6)
        {
            await DisplayAlert("Error", "La contrase�a debe tener al menos 6 caracteres.", "OK");
            return;
        }
        if (!IsValidEmail(emailInTextBox))
        {
            await DisplayAlert("Error", "El correo electr�nico no es v�lido.", "OK");
            return;
        }
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
            await DisplayAlert("Error", "Contrase�a incorrecta.", "OK");
            return;
        }

        await DisplayAlert("�xito", "Inicio de sesion correctamente.", "OK");
        await Shell.Current.GoToAsync("//userProfile");
    }
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}