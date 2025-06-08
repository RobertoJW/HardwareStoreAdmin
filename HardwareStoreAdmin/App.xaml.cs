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
            MainPage = new AppShell(); // No navegamos aún
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await InitializeAsync(); // Aquí ya la UI debería estar lista
        }

        private async Task InitializeAsync()
        {
            await Task.Delay(100); // Por si acaso

            int userIdGuardado = Preferences.Get("usuario_logueado_id", -1);

            if (userIdGuardado != -1)
            {
                var usuarioService = new UsuarioService();
                var usuario = await usuarioService.GetProductoPorIdUsuarioAsync(userIdGuardado);

                if (usuario != null)
                {
                    UsuarioActual = usuario;

                    // Verifica que Shell.Current no sea null antes de navegar
                    if (Shell.Current != null)
                        await Shell.Current.GoToAsync("//MainPage");
                    return;
                }
            }

            if (Shell.Current != null)
                await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}