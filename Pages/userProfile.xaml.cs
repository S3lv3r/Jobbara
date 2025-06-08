namespace Jobbara.Pages;

public partial class userProfile : ContentPage
{
    public userProfile()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MostrarDatosChambeador();
    }
    private void MostrarDatosChambeador()
    {
        bool isWorker = Preferences.Get("IsWorker", false);

        if (isWorker)
        {
            // Si ya es chambeador, mostramos la sección con sus datos
            BecomeWorkerButton.IsVisible = false;
            WorkerDataSection.IsVisible = true;

            OficioLabel.Text = Preferences.Get("Oficio", "No especificado");
            IneLabel.Text = Preferences.Get("INE", "No especificado");
            CurpLabel.Text = Preferences.Get("CURP", "No especificado");
            DomicilioLabel.Text = Preferences.Get("Domicilio", "No especificado");
            RfcLabel.Text = Preferences.Get("RFC", "No especificado");
        }
        else
        {
            // Si no es chambeador, solo muestra el botón
            BecomeWorkerButton.IsVisible = true;
            WorkerDataSection.IsVisible = false;
        }
    }

    private async void OnBecomeWorkerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//newChambeador");
    }
    private void OnDeleteChambeadorData(object sender, EventArgs e)
    {
        Preferences.Remove("IsWorker");
        Preferences.Remove("Oficio");
        Preferences.Remove("INE");
        Preferences.Remove("CURP");
        Preferences.Remove("Domicilio");
        Preferences.Remove("RFC");
        DisplayAlert("Datos borrados", "Ya no eres un chambeador, vaquero ??", "OK");
        MostrarDatosChambeador();
    }

}
