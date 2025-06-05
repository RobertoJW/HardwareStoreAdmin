using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin.ViewModels
{
    public class CarritoCompraViewModel : INotifyPropertyChanged
    {
        private readonly UsuarioService _usuarioService = new();
        private readonly CarritoCompraService _carritoCompraService = new();

        public ObservableCollection<Producto> ProductosEnCarrito { get; } = new ObservableCollection<Producto>();

        private decimal _precioTotal;
        public decimal PrecioTotal
        {
            get => _precioTotal;
            set { _precioTotal = value; OnPropertyChanged(); }
        }

        private bool _isCargando;
        public bool IsCargando
        {
            get => _isCargando;
            set { _isCargando = value; OnPropertyChanged(); }
        }

        public ICommand CargarCarritoCommand { get; }
        public ICommand PagarCommand { get; }

        public CarritoCompraViewModel()
        {
            CargarCarritoCommand = new Command(async () => await CargarCarrito());
            PagarCommand = new Command(async () => await Pagar());

            _ = CargarCarrito(); // carga inicial
        }

        public async Task CargarCarrito()
        {
            if (IsCargando) return;
            IsCargando = true;

            ProductosEnCarrito.Clear();
            PrecioTotal = 0;

            // Usamos el método que deserializa con ProductoConverter
            var usuario = await _usuarioService.GetProductoPorIdUsuarioAsync(App.UsuarioActual.userId);
            var carrito = usuario?.CarritoCompra;

            if (carrito?.Productos != null && carrito.Productos.Count > 0)
            {
                foreach (var p in carrito.Productos)
                {
                    ProductosEnCarrito.Add(p);
                    PrecioTotal += p.Precio;
                }
            }

            // Notificar que se actualizó el total
            OnPropertyChanged(nameof(PrecioTotal));

            IsCargando = false;
        }

        public async Task<Producto> CargarProductoDetalle(int idProducto)
        {
            // Reusa tu servicio de productos
            var svc = new ProductoService();
            return await svc.GetProductoPorIdAsync(idProducto);
        }

        private async Task Pagar()
        {
            var result = await _usuarioService.VaciarCarritoUsuarioAsync(App.UsuarioActual.userId);
            if (result)
            {
                ProductosEnCarrito.Clear();
                PrecioTotal = 0;
                OnPropertyChanged(nameof(PrecioTotal));
                await Shell.Current.DisplayAlert("Compra realizada", "Gracias por tu compra.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo completar la compra.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
