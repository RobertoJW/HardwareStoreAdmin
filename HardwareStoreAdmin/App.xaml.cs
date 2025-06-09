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
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = new Window(new AppShell());

            // Ejecuta la inicialización asincrónica después que la UI cargue
            Task.Run(async () => await InitializeAsync());

            return window;
        }

        private async Task InitializeAsync()
        {
            await Task.Delay(100); // Por seguridad

            int userIdGuardado = Preferences.Get("usuario_logueado_id", -1);

            if (userIdGuardado != -1)
            {
                var usuarioService = new UsuarioService();
                var usuario = await usuarioService.GetProductoPorIdUsuarioAsync(userIdGuardado);

                if (usuario != null)
                {
                    UsuarioActual = usuario;

                    if (Shell.Current != null)
                        await MainThread.InvokeOnMainThreadAsync(() =>
                            Shell.Current.GoToAsync("//MainPage"));
                    return;
                }
            }

            if (Shell.Current != null)
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Shell.Current.GoToAsync("//LoginPage"));
        }
    }
}