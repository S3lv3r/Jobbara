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

        // 👈 Cargar imagen guardada previamente (si existe)
        string userKey = UserSessionData.email_usd.Replace(".", "_").ToLower();
        string storedImagePath = Preferences.Get($"user_profile_image_{userKey}", string.Empty);

        if (!string.IsNullOrEmpty(storedImagePath) && File.Exists(storedImagePath))
        {
            selectedImagePath = storedImagePath;
            SelectedImage.Source = ImageSource.FromFile(storedImagePath);
        }
        else
        {
            SelectedImage.Source = "default_profile.png"; // 👈 Imagen predeterminada en recursos
        }
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
                // Guardar ruta en preferencias locales (clave única por usuario)
                string clave = UserSessionData.email_usd.Replace(".", "_").ToLower();
                Preferences.Set($"user_profile_image_{clave}", selectedImagePath);

                SelectedImage.Source = ImageSource.FromFile(selectedImagePath);

                // 👈 Guardar ruta local por usuario
                string userKey = UserSessionData.email_usd.Replace(".", "_").ToLower();
                Preferences.Set($"user_profile_image_{userKey}", selectedImagePath);
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

        // 👈 Pasar la imagen al perfil visualmente
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

            if (!nuevoUsuario.Equals(UserSessionData.username_usd, StringComparison.OrdinalIgnoreCase))
            {
                bool usernameRepetido = usersDB.Any(u =>
                    u.Object.username.Trim().Equals(nuevoUsuario, StringComparison.OrdinalIgnoreCase) &&
                    u.Key != usuarioExistente.Key);

                if (usernameRepetido)
                {
                    await DisplayAlert("Error", "El nombre de usuario ya está en uso.", "OK");
                    return false;
                }
            }

            if (!nuevoEmail.Equals(UserSessionData.email_usd.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                bool emailRepetido = usersDB.Any(u =>
                    u.Object.email.Trim().ToLower() == nuevoEmail &&
                    u.Key != usuarioExistente.Key);

                if (emailRepetido)
                {
                    await DisplayAlert("Error", "El correo electrónico ya está registrado.", "OK");
                    return false;
                }
            }

            bool huboCambios = !nuevoUsuario.Equals(UserSessionData.username_usd) ||
                               !nuevoEmail.Equals(UserSessionData.email_usd.Trim().ToLower());

            if (!huboCambios)
            {
                await DisplayAlert("Sin cambios", "No hiciste ningún cambio en tus datos.", "OK");
                return true;
            }

            string emailAnterior = UserSessionData.email_usd.Trim().ToLower();
            string claveAnterior = emailAnterior.Replace(".", "_");
            string claveNueva = nuevoEmail.Replace(".", "_");

            var usuarioActualizado = new usersModel
            {
                username = nuevoUsuario,
                email = nuevoEmail,
                password = usuarioExistente.Object.password,
                isWorking = usuarioExistente.Object.isWorking,
                office = usuarioExistente.Object.office,
                alertWork = usuarioExistente.Object.alertWork
            };

            await client.Child("Users").Child(usuarioExistente.Key).PutAsync(usuarioActualizado);

            if (claveNueva != claveAnterior)
            {
                var chambeadorAnterior = await client
                    .Child("Chambeadores")
                    .Child(claveAnterior)
                    .OnceSingleAsync<ChambeadorModel>();

                if (chambeadorAnterior != null)
                {
                    await client
                        .Child("Chambeadores")
                        .Child(claveNueva)
                        .PutAsync(chambeadorAnterior);

                    await client
                        .Child("Chambeadores")
                        .Child(claveAnterior)
                        .DeleteAsync();

                    UserSessionData.ine_usd = chambeadorAnterior.ine;
                    UserSessionData.curp_usd = chambeadorAnterior.curp;
                    UserSessionData.address_usd = chambeadorAnterior.domicilio;
                    UserSessionData.rfc_usd = chambeadorAnterior.rfc;
                    UserSessionData.office_usd = chambeadorAnterior.oficio;
                }
            }

            // 👈 Si el email cambió, migramos la imagen también
            if (claveNueva != claveAnterior)
            {
                string oldPath = Preferences.Get($"user_profile_image_{claveAnterior}", string.Empty);
                if (!string.IsNullOrEmpty(oldPath))
                {
                    Preferences.Set($"user_profile_image_{claveNueva}", oldPath);
                    Preferences.Remove($"user_profile_image_{claveAnterior}");
                }
            }

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
