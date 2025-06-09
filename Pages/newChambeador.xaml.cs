using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui;
using Firebase.Database;
using System.Text.RegularExpressions;
using Jobbara.Models;
using System.Threading.Tasks;
using Firebase.Database.Query;
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
        _ = LoadOffices();
    }

    public async Task LoadOffices() // Cargar los oficios en el picker
    {
        var offices = await client.Child("Offices").OnceAsync<officeModel>();
        officesList = offices.Select(x => x.Object).ToList();
        officePicker.ItemsSource = officesList;
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

    private async Task<bool> SaveDateLocalAndDB()
    {
        string curp = CurpEntry.Text?.Trim().ToUpper() ?? "";
        string rfc = RfcEntry.Text?.Trim().ToUpper() ?? "";
        string ine = IneEntry.Text?.Trim().ToUpper() ?? "";
        string address = AddressEntry.Text?.Trim() ?? "";

        string curpPattern = @"^[A-Z]{4}\d{6}[HM][A-Z]{5}[0-9A-Z]\d$";
        string rfcPattern = @"^[A-ZÑ&]{3,4}\d{6}[A-Z0-9]{3}$";
        string inePattern = @"^[A-Z0-9]{18}$";

        if (officePicker.SelectedItem == null ||
            string.IsNullOrWhiteSpace(address) ||
            !Regex.IsMatch(curp, curpPattern) ||
            !Regex.IsMatch(rfc, rfcPattern) ||
            !Regex.IsMatch(ine, inePattern))
        {
            return false;
        }

        officeModel office = officePicker.SelectedItem as officeModel;

        await client
            .Child("Users")
            .Child(UserSessionData.userKey_usd)
            .Child("office")
            .PutAsync(office);

        Preferences.Set("IsWorker", true);
        UserSessionData.ine_usd = ine;
        UserSessionData.curp_usd = curp;
        UserSessionData.address_usd = address;
        UserSessionData.rfc_usd = rfc;
        UserSessionData.office_usd = office.Name;

        return true;
    }

    private async void OnGuardarYConfirmarClicked(object sender, EventArgs e)
    {
        if (!TermsCheck.IsChecked)
        {
            await DisplayAlert("Falta aceptar", "Debes aceptar los términos y condiciones.", "OK");
            return;
        }

        bool datosValidos = await SaveDateLocalAndDB();

        if (!datosValidos)
        {
            await DisplayAlert("Error", "Revisa que todos los campos estén completos y correctamente escritos.", "OK");
            return;
        }

        await DisplayAlert("¡Listo!", "Ya eres un chambeador certificado, amigazo 🐎", "OK");
        await Shell.Current.GoToAsync("//userProfile");
    }

}