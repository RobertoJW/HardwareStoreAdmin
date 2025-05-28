using System.Diagnostics;
using System.Text.RegularExpressions;
using HardwareStoreAdmin.Servicios;

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
        var contraseña = passwordInicioSesion.Password;

        bool hayError = false;

        if (string.IsNullOrWhiteSpace(email) || !EsCorreoValido(email))
        {
            // Error visual
            EmailLoginBorder.Stroke = Colors.Red;
            CorreoErrorLabel.IsVisible = true;

            // Animación sutil
            await CorreoErrorLabel.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (string.IsNullOrWhiteSpace(contraseña))
        {
            // Error visual
            passwordInicioSesion.EntryBorderPublic.Stroke = Colors.Red;
            passwordInicioSesion.LabelPublic.IsVisible = true;

            // Animación sutil
            await passwordInicioSesion.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }
        if (hayError) return;
        bool credencialesCorrectas = await VerificarCredenciales(email, contraseña);

        if (!credencialesCorrectas)
        {
            await DisplayAlert("Error", "Correo o contraseña incorrectos", "Aceptar");
            return;
        } else
        {
            OnLoginSuccess();
        }
        await DisplayAlert("Bienvenido", "Inicio de sesión exitoso", "Continuar");

    }
    private async Task<bool> VerificarCredenciales(string email, string password)
    {
        var usuario = await _usuarioService.VerificarCredencialesAsync(email, password);
        return usuario != null;
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
        var contraseña1 = passwordRegistro.Password;
        var contraseña2 = passwordRegistro2.Password;

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

            // Animación sutil
            await EmailErrorLabel.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (string.IsNullOrWhiteSpace(contraseña1))
        {
            // Error visual
            passwordRegistro.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro2.LabelPublic.Text = "Es obligatorio confirmar su contraseña.";
            passwordRegistro.LabelPublic.IsVisible = true;

            // Animación sutil
            await passwordRegistro.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (string.IsNullOrWhiteSpace(contraseña2))
        {
            // Error visual
            passwordRegistro2.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro2.LabelPublic.IsVisible = true;

            // Animación sutil
            await passwordRegistro2.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }

        if (contraseña1 != contraseña2)
        {
            // Error visual
            passwordRegistro2.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro.EntryBorderPublic.Stroke = Colors.Red;
            passwordRegistro2.LabelPublic.Text = "Las contraseñas no coinciden.";
            passwordRegistro2.LabelPublic.IsVisible = true;

            // Animación sutil
            await passwordRegistro2.LabelPublic.FadeTo(1, 250, Easing.CubicIn);
            hayError = true;
        }
        if (hayError) return;

        bool registrado = await RegistrarUsuario(nombre, email, contraseña1);

        if (!registrado)
        {
            await DisplayAlert("Error", "El usuario ya existe o el correo está en uso", "Aceptar");
            return;
        }
        else
        {
            OnLoginSuccess();
            await DisplayAlert("Éxito", "Usuario registrado correctamente", "Aceptar");
        }
    }
    private async Task<bool> RegistrarUsuario(string user, string email, string password)
    {
        try
        {
            var usuario = await _usuarioService.RegistrarUsuarioAsync(user, email, password);
            if (usuario == null)
            {
                Debug.WriteLine("[FALLO] No se pudo crear el usuario.");
                return false;
            }

            Debug.WriteLine("[ÉXITO] Usuario creado con ID: " + usuario.userId);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("[EXCEPCIÓN] " + ex.Message);
            return false;
        }
    }


    private async void OnLoginSuccess()
    {
        // Navegar a la página principal
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
}