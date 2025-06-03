using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using Microsoft.Maui.Storage;

namespace HardwareStoreAdmin
{
    public partial class App : Application
    {
        public static Usuario? UsuarioActual { get; set; }

        public App()
        {
            InitializeComponent();

            // Asignamos provisionalmente AppShell; luego, en InitializeAsync, navegaremos a la página adecuada.
            MainPage = new AppShell();

            // Iniciar la comprobación de sesión de forma asíncrona
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await Task.Delay(100); // Espera breve para asegurar que Shell.Current se inicialice

            // 1) Intentamos leer el ID guardado
            int userIdGuardado = Preferences.Get("usuario_logueado_id", -1);

            if (userIdGuardado != -1)
            {
                // 2) Si había un ID, lo cargamos desde la API
                var usuarioService = new UsuarioService();
                var usuario = await usuarioService.GetProductoPorIdUsuarioAsync(userIdGuardado);

                if (usuario != null)
                {
                    // 3) Si existe, asignamos y navegamos a la página principal
                    UsuarioActual = usuario;
                    await Shell.Current.GoToAsync("//MainPage");
                    return;
                }
            }   

            // 4) Si no hay ID o no se pudo recuperar, nos quedamos en LoginPage
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}