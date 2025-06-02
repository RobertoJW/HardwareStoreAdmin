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
            Productos.Clear();

            var usuario = await _usuarioService.GetUsuarioByIdAsync(userId);
            var favoritos = usuario?.ListaFavoritos?.Productos ?? Enumerable.Empty<Producto>();

            // ✅ Obtener los productos que están ya en el carrito
            var carrito = await _carritoService.GetCarritoCompraAsync(userId);
            var idsEnCarrito = carrito
                .SelectMany(c => c.Productos ?? Enumerable.Empty<Producto>())
                .Select(p => p.IdProducto)
                .ToHashSet();

            // Crear los ViewModels con el estado correcto
            foreach (var producto in favoritos)
            {
                Productos.Add(new ProductoDetalleViewModel(
                    producto,
                    esFavorito: true,
                    estaEnCarrito: idsEnCarrito.Contains(producto.IdProducto),
                    userId,
                    _favService,
                    _carritoService));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
