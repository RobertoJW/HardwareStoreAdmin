using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using HardwareStoreAdmin.ViewModels;

namespace HardwareStoreAdmin.ViewModels
{
    public class ListaFavoritosViewModel : INotifyPropertyChanged
    {
        private readonly UsuarioService _usuarioService = new();
        private readonly ListaFavoritoService _favService = new();
        public ObservableCollection<ProductoFavoritoViewModel> Productos { get; } = 
            new ObservableCollection<ProductoFavoritoViewModel>();

        public Command<int> CargarListaFavoritosComando { get; }

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
        public ListaFavoritosViewModel()
        {
            CargarListaFavoritosComando = new Command<int>(
            async userId => await CargarListaFavoritos(userId)
            );
        }

        public async Task CargarListaFavoritos(int userId)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(userId);
            Productos.Clear();

            var favoritos = usuario?.ListaFavoritos?.Productos ?? Enumerable.Empty<Producto>();
            var productosFavoritos = new HashSet<int>(favoritos.Select(p => p.IdProducto));

            if (isCargando) return;
            isCargando = true;

            // cargamos los datos del usuario
            foreach (var producto in favoritos)
            {
                Productos.Add(new ProductoFavoritoViewModel(
                    producto,
                    esFavorito: true,
                    userId,
                    _favService));
            }

            Debug.WriteLine($"Productos cargados: {Productos.Count}");
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
