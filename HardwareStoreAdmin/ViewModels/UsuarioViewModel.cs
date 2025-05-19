using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.ViewModels
{
    public class UsuarioViewModel
    {
        private readonly UsuarioService _usuarioService = new();
        public ObservableCollection<Usuario> Usuario { get; set; } = new();

        // si es true, la app está cargando los productos, si es false, todos los productos ya se han cargado
        private bool estaCargando;
        public bool isCargando
        {
            get => estaCargando;
            set
            {
                estaCargando = value;
                OnPropertyChanged();
            }
        }
        public UsuarioViewModel()
        {
            CargarUsuariosComando = new Command(async () => await CargarUsuario());
            _ = CargarUsuario(); // carga automatica de productos al abrir
        }

        public Command CargarUsuariosComando { get; }

        public async Task CargarUsuario()
        {
            if (isCargando) return;
            isCargando = true;

            var lista = await _usuarioService.GetUsuariosAsync();
            Usuario.Clear();

            foreach (var usuario in lista)
            {
                Usuario.Add(usuario);
            }
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));

    }
}
