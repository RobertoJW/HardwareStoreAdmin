namespace HardwareStoreAdmin.AppClass;

public partial class MenuUsuario : ContentPage
{
    public string UsuarioNombre { get; set; }
    public string UsuarioEmail { get; set; }

    public MenuUsuario()
    {
        InitializeComponent();
        BindingContext = this;

        if (App.UsuarioActual != null)
        {
            UsuarioNombre = App.UsuarioActual.userName;
            UsuarioEmail = App.UsuarioActual.email;
        }
    }

    private async void CerrarSesion_Clicked(object sender, EventArgs e)
    {
        // 1) Limpiamos App.UsuarioActual y Preferences
        App.UsuarioActual = null;
        Preferences.Remove("usuario_logueado_id");

        // 2) Navegar de vuelta al LoginPage (ruta absoluta)
        await Shell.Current.GoToAsync("//LoginPage");

        await DisplayAlert("Sesión cerrada", "Se ha cerrado la sesión correctamente.", "OK");
    }

    private void CambiarModoOscuro_Clicked(object sender, EventArgs e)
    {
        App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
            ? AppTheme.Light
            : AppTheme.Dark;
    }

    private async void MostrarInfo_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Acerca de", "HardwareStore v1.0\nDesarrollado por Roberto y Daniel.", "OK");
    }
}
