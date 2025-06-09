namespace Jobbara.Pages;


public partial class mapChamber : ContentPage
{
	public mapChamber()
	{

        InitializeComponent();
	}
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Cancelado", "Has cancelado esta solicitud", "OK");
        // Aquí puedes agregar navegación u otra lógica
    }

    private async void OnReportarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Reporte enviado", "Gracias por tu reporte, lo revisaremos pronto.", "OK");
        // Aquí podrías enviar datos a la BD o levantar un flag
    }

}