namespace Jobbara.Pages;

public partial class Notificaciones : ContentPage
{
    public Notificaciones()
    {
        InitializeComponent();

        var lista = new List<Notificacion>
        {
            new Notificacion { Titulo = "Trabajo aceptado", Mensaje = "Tu solicitud fue aceptada", Fecha = "07/06/2025" },
            new Notificacion { Titulo = "Pago recibido", Mensaje = "Has recibido un pago de $250", Fecha = "06/06/2025" },
            new Notificacion { Titulo = "Nueva oportunidad", Mensaje = "Hay un nuevo trabajo cerca de ti", Fecha = "05/06/2025" }
        };

        NotificacionesCollection.ItemsSource = lista;
    }
    private async void OnGoToHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homePage");
    }
    private async void GoToAjustes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ajustes");
    }
    private async void GoToPagos(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//pagos");
    }
    public class Notificacion
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Fecha { get; set; }
    }
}