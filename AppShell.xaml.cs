namespace Jobbara
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("userProfile", typeof(Jobbara.Pages.userProfile));
            Routing.RegisterRoute("loginPage", typeof(Jobbara.Pages.inicioSesion));
            Routing.RegisterRoute("signPage", typeof(Jobbara.Pages.Registro));
            Routing.RegisterRoute("newChambeador", typeof(Jobbara.Pages.newChambeador));
            Routing.RegisterRoute("homePage", typeof(Jobbara.Pages.homePage));
            Routing.RegisterRoute("ajustes", typeof(Jobbara.Pages.Ajustes));
        }
    }
}
