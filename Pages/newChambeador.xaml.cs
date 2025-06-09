using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui;
using Firebase.Database;
using System.Text.RegularExpressions;
using Jobbara.Models;
namespace Jobbara.Pages;

public partial class newChambeador : ContentPage
{
    private bool areTermsVisible = false;
    FirebaseClient client = new FirebaseClient("https://jobbara-default-rtdb.firebaseio.com/");
    public List<officeModel> officesList { get; set; }

    public newChambeador()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public void LoadOffices() // Cargar los oficios en el picker
    {
        var offices = client.Child("Offices").OnceAsync<officeModel>();
        officesList = offices.Result.Select(x => x.Object).ToList();
    }
    // Mostrar/Ocultar términos
    private void OnToggleTermsClicked(object sender, EventArgs e)
    {
        areTermsVisible = !areTermsVisible;
        TermsText.IsVisible = areTermsVisible;
        ToggleTermsButton.Text = areTermsVisible ? "Ocultar Términos ↑" : "Ver Términos y Condiciones ↓";
    }

    // Tap manual al texto de términos (opcional)
    private void OnTermsTapped(object sender, EventArgs e)
    {
        TermsCheck.IsChecked = !TermsCheck.IsChecked;
    }

    
    private bool GuardarDatosLocal() // Función para guardar datos en Preferences
    {
        string curp = CurpEntry.Text?.Trim().ToUpper() ?? "";
        string rfc = RfcEntry.Text?.Trim().ToUpper() ?? "";
        string ine = IneEntry.Text?.Trim().ToUpper() ?? "";
        //string job = JobEntry.Text?.Trim() ?? "";
        string address = AddressEntry.Text?.Trim() ?? "";

        // Expresiones regulares
        string curpPattern = @"^[A-Z]{4}\d{6}[HM][A-Z]{5}[0-9A-Z]\d$";
        string rfcPattern = @"^[A-ZÑ&]{3,4}\d{6}[A-Z0-9]{3}$";
        string inePattern = @"^[A-Z0-9]{18}$";

        // Validación general
        if (string.IsNullOrWhiteSpace(curp) ||
            string.IsNullOrWhiteSpace(address) ||
            !Regex.IsMatch(curp, curpPattern) ||
            !Regex.IsMatch(rfc, rfcPattern) ||
            !Regex.IsMatch(ine, inePattern))
        {

            return false; // No guardar
        }

        Preferences.Set("IsWorker", true);
        //Preferences.Set("Oficio", JobEntry.Text);
        Preferences.Set("INE", IneEntry.Text);
        Preferences.Set("CURP", CurpEntry.Text);
        Preferences.Set("Domicilio", AddressEntry.Text);
        Preferences.Set("RFC", RfcEntry.Text);
        return true;
    }
    // Solo guardar datos (quedarse en la misma página)


    private async void OnSaveClicked(object sender, EventArgs e)
    {
        bool datosValidos = GuardarDatosLocal();

        if (!datosValidos)
        {
            await DisplayAlert("Error", "Revisa que todos los campos estén completos y correctamente escritos (CURP, RFC, INE, etc).", "OK");
            return;
        }

        await DisplayAlert("Guardado", "Tus datos se guardaron correctamente, vaquero 🐎", "OK");
    }

    // Guardar + regresar al perfil


    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        if (!TermsCheck.IsChecked)
        {
            await DisplayAlert("Falta aceptar", "Debes aceptar los términos y condiciones para continuar.", "OK");
            return;
        }

        bool datosValidos = GuardarDatosLocal();

        if (!datosValidos)
        {
            await DisplayAlert("Error", "Revisa que todos los campos estén completos y correctamente escritos (CURP, RFC, INE, etc).", "OK");
            return;
        }
        GuardarDatosLocal();

        await DisplayAlert("¡Listo!", "Ya eres un chambeador certificado, amigazo ????", "OK");
        await Shell.Current.GoToAsync("//userProfile");
        // Regresa al perfil
    }

}