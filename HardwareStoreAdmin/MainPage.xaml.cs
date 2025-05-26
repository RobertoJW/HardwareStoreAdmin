using System.Collections.ObjectModel;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using HardwareStoreAdmin.AppClass; 

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

        private async void OnProductoSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Producto productoSeleccionado)
            {
                // Quitamos la seleccion para evitar que quede seleccionado visualmente. 
                ((CollectionView)sender).SelectedItem = null;

                // navegamos a la página con el producto que hemos clicado. 
                await Navigation.PushAsync(new InterfazDetallesProducto(productoSeleccionado));
            }
        }
    }
}
