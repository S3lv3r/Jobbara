using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Jobbara.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Jobbara.Pages;

public partial class EditarPerfil : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    string selectedImagePath;

    public EditarPerfil()
    {
        InitializeComponent();
        // Cargar datos actuales en los Entry
        UsuarioEntry.Text = UserSessionData.username_usd;
        emailEntry.Text = UserSessionData.email_usd;
    }

    private async void OnSelectImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Selecciona una imagen"
            });

            if (result != null)
            {
                selectedImagePath = result.FullPath;
                SelectedImage.Source = ImageSource.FromFile(selectedImagePath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo seleccionar la imagen: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text?.Trim();
        string email = emailEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(usuario))
        {
            await DisplayAlert("Campo requerido", "El campo 'Usuario' no puede estar vacío.", "OK");
            return;
        }

        if (!IsValidEmail(email))
        {
            await DisplayAlert("Correo inválido", "Por favor, introduce un correo electrónico válido.", "OK");
            return;
        }

        await DisplayAlert("Guardado", "Tus datos se guardaron correctamente, vaquero 🐎", "OK");
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        bool datosValidos = await GuardarDatosEnFirebase();

        if (!datosValidos)
            return;

        await DisplayAlert("¡Listo!", "Ya eres un chambeador certificado, amigazo 🐎🐾", "OK");
        await Shell.Current.GoToAsync($"//userProfile?imagePath={Uri.EscapeDataString(selectedImagePath ?? string.Empty)}");
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    private async Task<bool> GuardarDatosEnFirebase()
    {
        string nuevoUsuario = UsuarioEntry.Text?.Trim() ?? "";
        string nuevoEmail = emailEntry.Text?.Trim().ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(nuevoUsuario) || string.IsNullOrWhiteSpace(nuevoEmail))
        {
            await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
            return false;
        }

        if (!IsValidEmail(nuevoEmail))
        {
            await DisplayAlert("Error", "El correo electrónico no es válido.", "OK");
            return false;
        }

        try
        {
            var usersDB = await client.Child("Users").OnceAsync<usersModel>();

            var usuarioExistente = usersDB.FirstOrDefault(u =>
                u.Object.email.Trim().ToLower() == UserSessionData.email_usd.Trim().ToLower() &&
                u.Object.username.Trim() == UserSessionData.username_usd.Trim());

            if (usuarioExistente == null)
            {
                await DisplayAlert("Error", "No se encontró el usuario actual en la base de datos.", "OK");
                return false;
            }

            bool usernameRepetido = usersDB.Any(u =>
                u.Object.username.Trim().Equals(nuevoUsuario, StringComparison.OrdinalIgnoreCase) &&
                u.Key != usuarioExistente.Key);

            if (usernameRepetido)
            {
                await DisplayAlert("Error", "El nombre de usuario ya está en uso.", "OK");
                return false;
            }

            bool emailRepetido = usersDB.Any(u =>
                u.Object.email.Trim().ToLower() == nuevoEmail &&
                u.Key != usuarioExistente.Key);

            if (emailRepetido)
            {
                await DisplayAlert("Error", "El correo electrónico ya está registrado.", "OK");
                return false;
            }

            var usuarioActualizado = new usersModel
            {
                username = nuevoUsuario,
                email = nuevoEmail,
                password = usuarioExistente.Object.password,
                isWorker = usuarioExistente.Object.isWorker,
                office = usuarioExistente.Object.office,
                alertWork = usuarioExistente.Object.alertWork
            };

            await client.Child("Users").Child(usuarioExistente.Key).PutAsync(usuarioActualizado);

            // Actualizar datos en sesión
            UserSessionData.username_usd = nuevoUsuario;
            UserSessionData.email_usd = nuevoEmail;

            await DisplayAlert("Éxito", "Datos actualizados correctamente.", "OK");
            return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al guardar: {ex.Message}", "OK");
            return false;
        }
    }
}
