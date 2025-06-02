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
        private readonly CarritoCompraService _carritoService = new();
        public ObservableCollection<ProductoDetalleViewModel> Productos { get; } = 
            new ObservableCollection<ProductoDetalleViewModel>();

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
            if (isCargando) return;
            isCargando = true;

            try
            {
                Productos.Clear();

                // Obtener productos favoritos
                var usuario = await _usuarioService.GetUsuarioByIdAsync(userId);
                var favoritos = usuario?.ListaFavoritos?.Productos ?? Enumerable.Empty<Producto>();

                // Obtener productos en carrito
                var usuarioConCarrito = await _usuarioService.GetUsuarioConCarritoAsync(userId);
                var productosCarrito = usuarioConCarrito?.CarritoCompra?.Productos ?? new List<Producto>();
                var idsCarrito = new HashSet<int>(productosCarrito.Select(p => p.IdProducto));

                foreach (var producto in favoritos)
                {
                    bool estaEnCarrito = idsCarrito.Contains(producto.IdProducto);

                    Productos.Add(new ProductoDetalleViewModel(
                        producto,
                        esFavorito: true,
                        estaEnCarrito: estaEnCarrito,
                        userId,
                        _favService,
                        _carritoService));
                }

                Debug.WriteLine($"Productos cargados: {Productos.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CargarListaFavoritos] Error: {ex.Message}");
            }

            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
