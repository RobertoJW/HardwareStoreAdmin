using System.Collections.ObjectModel;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Producto> Productos { get; set; } = new();

        private readonly ProductoService _productoService = new ProductoService();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadProductos();
        }

        private async void LoadProductos()
        {
            var productos = await _productoService.GetProductosAsync();
            Productos.Clear();
            foreach (var producto in productos)
            {
                Productos.Add(producto);
            }
        }
    }
}
