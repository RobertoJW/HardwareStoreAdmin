using System.Diagnostics;
using System.Text.RegularExpressions;
using HardwareStoreAdmin.Servicios;
using HardwareStoreAdmin.Modelo;

namespace HardwareStoreAdmin;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private int seleccion = 0;
    private readonly UsuarioService _usuarioService = new UsuarioService();

    private async void OnLoginSelected(object sender, EventArgs e)
    {
        if (seleccion == 0) return;
        seleccion = 0;

        await SliderBackground.TranslateTo(-SelectorGrid.Width / 2, 0, 200, Easing.CubicOut);
        Grid.SetColumn(SliderBackground, 0);
        SliderBackground.TranslationX = 0;

        LoginTab.TextColor = Colors.Black;
        RegisterTab.TextColor = Colors.Gray;

        // Cambiar visibilidad y animar
        RegisterForm.IsVisible = false;
        LoginForm.Opacity = 0;
        LoginForm.IsVisible = true;
        await LoginForm.FadeTo(1, 250, Easing.CubicIn);
    }

    private async void OnRegisterSelected(object sender, EventArgs e)
    {
        if (seleccion == 1) return;
        seleccion = 1;

        await SliderBackground.TranslateTo(SelectorGrid.Width / 2, 0, 200, Easing.CubicOut);
        Grid.SetColumn(SliderBackground, 1);
        SliderBackground.TranslationX = 0;

        LoginTab.TextColor = Colors.Gray;
        RegisterTab.TextColor = Colors.Black;

        // Cambiar visibilidad y animar
        LoginForm.IsVisible = false;
        RegisterForm.Opacity = 0;
        RegisterForm.IsVisible = true;
        await RegisterForm.FadeTo(1, 250, Easing.CubicIn);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (MantenerSesion.IsChecked)
            MantenerSesion.IsChecked = false;
        else
            MantenerSesion.IsChecked = true;
    }
    bool EsCorreoValido(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        CorreoErrorLabel.IsVisible = false;
        CorreoErrorLabel.Opacity = 0;
        EmailLoginBorder.Stroke = Colors.Gray;
        passwordInicioSesion.EntryBorderPublic.Stroke = Colors.Gray;
        passwordInicioSesion.LabelPublic.IsVisible = false;

        var email = EmailLoginEntry.Text;
        var contrase�a = passwordInicioSesion.Password;

        bool hayError = false;

        if (string.IsNullOrWhiteSpace(email) || !EsCorreoValido(email))
        {
            // Error visual
            EmailLoginBorder.Stroke = Colors.Red;
            CorreoErrorLabel.IsVisible = true;

            // Animaci�n sutil
            await CorreoErrorLabel.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (string.IsNullOrWhiteSpace(contrase�a))
        {
            // Error visual
            passwordInicioSesion.EntryBorderPublic.Stroke = Colors.Red;
            passwordInicioSesion.LabelPublic.IsVisible = true;

            // Animaci�n sutil
            await passwordInicioSesion.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }
        if (hayError) return;
        var usuario = await VerificarCredenciales(email, contrase�a);

        if (usuario == null)
        {
            await DisplayAlert("Error", "Correo o contrase�a incorrectos", "Aceptar");
            return;
        } else
        {
            if (MantenerSesion.IsChecked)
            {
                Preferences.Set("usuario_logueado_id", usuario.userId);
            }
            App.UsuarioActual = usuario;
            OnLoginSuccess();
            await DisplayAlert("Bienvenido", "Inicio de sesi�n exitoso", "Continuar");
        }

    }
    private async Task<Usuario?> VerificarCredenciales(string email, string password)
    {
        return await _usuarioService.VerificarCredencialesAsync(email, password);
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        UserErrorLabel.IsVisible = false;
        UserRegisterBorder.Stroke = Colors.Gray;
        EmailErrorLabel.IsVisible = false;
        EmailRegisterBorder.Stroke = Colors.Gray;
        passwordRegistro.EntryBorderPublic.Stroke = Colors.Gray;
        passwordRegistro.LabelPublic.IsVisible = false;
        passwordRegistro2.EntryBorderPublic.Stroke = Colors.Gray;
        passwordRegistro2.LabelPublic.IsVisible = false;

        var nombre = UserRegisterEntry.Text;
        var email = EmailRegisterEntry.Text;
        var contrase�a1 = passwordRegistro.Password;
        var contrase�a2 = passwordRegistro2.Password;

        bool hayError = false;

        if (string.IsNullOrEmpty(nombre))
        {
            UserRegisterBorder.Stroke = Colors.Red;
            UserErrorLabel.IsVisible = true;

            await UserErrorLabel.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (string.IsNullOrWhiteSpace(email) || !EsCorreoValido(email))
        {
            // Error visual
            EmailRegisterBorder.Stroke = Colors.Red;
            EmailErrorLabel.IsVisible = true;

            // Animaci�n sutil
            await EmailErrorLabel.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (string.IsNullOrWhiteSpace(contrase�a1))
        {
            // Error visual
            passwordRegistro.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro2.LabelPublic.Text = "Es obligatorio confirmar su contrase�a.";
            passwordRegistro.LabelPublic.IsVisible = true;

            // Animaci�n sutil
            await passwordRegistro.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (string.IsNullOrWhiteSpace(contrase�a2))
        {
            // Error visual
            passwordRegistro2.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro2.LabelPublic.IsVisible = true;

            // Animaci�n sutil
            await passwordRegistro2.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (contrase�a1 != contrase�a2)
        {
            // Error visual
            passwordRegistro2.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro2.LabelPublic.Text = "Las contrase�as no coinciden.";
            passwordRegistro2.LabelPublic.IsVisible = true;

            // Animaci�n sutil
            await passwordRegistro2.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }
        if (hayError) return;

        // Intentamos registrar
        var usuario = await RegistrarUsuario(nombre, email, contrase�a1);

        if (usuario == null)
        {
            await DisplayAlert("Error", "El usuario ya existe o el correo est� en uso.", "Aceptar");
            return;
        }

        // Usuario creado correctamente
        App.UsuarioActual = usuario;

        await DisplayAlert("Bienvenido", $"Cuenta creada exitosamente, {usuario.userName}.", "Continuar");

        OnLoginSuccess(); // Ir al men� principal o donde necesites
    }
    private async Task<Usuario> RegistrarUsuario(string user, string email, string password)
    {
        try
        {
            var usuario = await _usuarioService.RegistrarUsuarioAsync(user, email, password);
            if (usuario == null)
            {
                Debug.WriteLine("[FALLO] No se pudo crear el usuario.");
                return null;
            }

            Debug.WriteLine("[�XITO] Usuario creado con ID: " + usuario.userId);
            return usuario;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("[EXCEPCI�N] " + ex.Message);
            return null;
        }
    }

    private async void OnLoginSuccess()
    {
        LimpiarCampos();
        // Navegar a la p�gina principal
        await Shell.Current.GoToAsync("//MainPage");

        // Mostrar el TabBar
        if (Shell.Current is AppShell appShell)
        {
            appShell.FindByName<TabBar>("MainTabBar").IsVisible = true;
        }
    }

    private void OnCorreoRegisterTextChanged(object sender, TextChangedEventArgs e)
    {
        EmailRegisterBorder.Stroke = Colors.Gray;
        EmailErrorLabel.IsVisible = false;
    }
    private void OnUserRegisterTextChanged(object sender, TextChangedEventArgs e)
    {
        UserRegisterBorder.Stroke = Colors.Gray;
        UserErrorLabel.IsVisible = false;
    }
    private void OnCorreoTextChanged(object sender, TextChangedEventArgs e)
    {
        EmailLoginBorder.Stroke = Colors.Gray;
        CorreoErrorLabel.IsVisible = false;
    }
    public void LimpiarCampos()
    {
        // Campos de login
        EmailLoginEntry.Text = string.Empty;
        passwordInicioSesion.Clear(); // Asumiendo que PasswordEntryView tiene este m�todo o propiedad

        // Campos de registro
        UserRegisterEntry.Text = string.Empty;
        EmailRegisterEntry.Text = string.Empty;
        passwordRegistro.Clear();
        passwordRegistro2.Clear();

        // Resetear errores
        CorreoErrorLabel.IsVisible = false;
        CorreoErrorLabel.Opacity = 0;
        UserErrorLabel.IsVisible = false;
        UserErrorLabel.Opacity = 0;
        EmailErrorLabel.IsVisible = false;
        EmailErrorLabel.Opacity = 0;

        // Checkbox
        MantenerSesion.IsChecked = false;
    }

}