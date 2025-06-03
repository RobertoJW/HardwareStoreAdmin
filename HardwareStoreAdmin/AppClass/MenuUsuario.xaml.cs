using System.ComponentModel;

namespace HardwareStoreAdmin.AppClass;

public partial class MenuUsuario : ContentPage, INotifyPropertyChanged
{
    private string _usuarioNombre;
    public string UsuarioNombre
    {
        get => _usuarioNombre;
        set
        {
            if (_usuarioNombre != value)
            {
                _usuarioNombre = value;
                OnPropertyChanged(nameof(UsuarioNombre));
            }
        }
    }

    private string _usuarioEmail;
    public string UsuarioEmail
    {
        get => _usuarioEmail;
        set
        {
            if (_usuarioEmail != value)
            {
                _usuarioEmail = value;
                OnPropertyChanged(nameof(UsuarioEmail));
            }
        }
    }
    public MenuUsuario()
    {
        InitializeComponent();

        if (App.UsuarioActual != null)
        {
            UsuarioNombre = App.UsuarioActual.userName;
            UsuarioEmail = App.UsuarioActual.email;
        }
        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

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
        await DisplayAlert("Acerca de", "HardwareStore v1.0\nDesarrollado por Roberto Jiang y Daniel Pajarón.", "OK");
    }
    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}
