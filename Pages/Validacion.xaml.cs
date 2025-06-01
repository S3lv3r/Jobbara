namespace Jobbara.Pages;

public partial class Validacion : ContentPage
{
	public Validacion()
	{
		InitializeComponent();
	}
    private void OnCodeEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && e.NewTextValue.Length == 1)
        {
            if (entry == Code1) Code2.Focus();
            else if (entry == Code2) Code3.Focus();
            else if (entry == Code3) Code4.Focus();
            else if (entry == Code4) Code5.Focus();
            else if (entry == Code5) Code6.Focus();
            else if (entry == Code6) entry.Unfocus();
        }
    }

}