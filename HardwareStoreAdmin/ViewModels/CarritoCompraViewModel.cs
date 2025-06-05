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
            if (ProductosEnCarrito == null || ProductosEnCarrito.Count == 0)
            {
                await Shell.Current.DisplayAlert(
                    "Carrito vacío",
                    "No puedes realizar una compra si no hay productos en el carrito.",
                    "OK");
                return;
            }

            var confirmacion = await Shell.Current.DisplayAlert(
                "Confirmar compra",
                $"Vas a realizar tu compra por un total de {PrecioTotal:C}.\n¿Deseas continuar?",
                "Sí", "No");

            if (!confirmacion) return;

            var result = await _usuarioService.VaciarCarritoUsuarioAsync(App.UsuarioActual.userId);

            if (result)
            {
                // Vaciar productos localmente
                ProductosEnCarrito.Clear();
                PrecioTotal = 0;
                OnPropertyChanged(nameof(PrecioTotal));

                // Generar número de orden simulado
                var numeroOrden = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                // Mostrar mensaje personalizado
                await Shell.Current.DisplayAlert(
                    "🎉 ¡Gracias por tu compra!",
                    $"Tu pedido ha sido recibido con éxito.\n\nN.º de orden: {numeroOrden}",
                    "Aceptar");
            }
            else
            {
                await Shell.Current.DisplayAlert(
                    "Error",
                    "No se pudo completar la compra. Intenta de nuevo.",
                    "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
