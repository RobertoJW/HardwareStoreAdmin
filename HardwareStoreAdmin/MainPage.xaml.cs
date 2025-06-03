using System.Collections.ObjectModel;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using HardwareStoreAdmin.AppClass;
using System.Threading.Tasks;

namespace HardwareStoreAdmin
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Producto> Productos { get; set; } = new();

        private readonly ProductoService _productoService = new ProductoService();

        List<Button> botonesFiltro;

        private Dictionary<string, string> filtros = new()
        {
            { "Todo", "Todo" },
            { "Nombre del producto", "NombreProducto" },
            { "Marca", "NombreEmpresa" }
        };

        private string categoriaSeleccionada = "Todo";

        public MainPage()
        {
            InitializeComponent();
            myLabel.Text = $"¡Bienvenid@, {App.UsuarioActual?.userName ?? "Invitado"}!";
            BindingContext = this;
            PickerProducto.ItemsSource = filtros.Keys.ToList();
            PickerProducto.SelectedItem = "Todo";
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

        private async void BtnFiltro(object sender, EventArgs e)
        {
            var boton = sender as Button;
            string categoria = boton?.Text;
            categoriaSeleccionada = categoria;
            List<Producto> productosFiltrados;

            FiltroTodos.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];
            FiltroPortatil.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];
            FiltroSobremesa.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];
            FiltroMovil.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];

            var button = sender as Button;
            categoriaSeleccionada = boton?.Text ?? "Todo";
            button.BackgroundColor = (Color)Application.Current.Resources["Naranja"];

            if (categoria == "Todo")
            {
                productosFiltrados = await _productoService.GetProductosAsync();
            }
            else
            {
                productosFiltrados = await _productoService.GetProductoFiltrado(categoria);
            }

            Productos.Clear();
            foreach (var producto in productosFiltrados)
            {
                Productos.Add(producto);
            }
        }

        private async void BuscarProductoBtn(object sender, EventArgs e)
        {
            // recoge lo que está escrito en el buscador y lo que se ha elegido en el picker. 
            var textoBusqueda = busquedaProductoTxt.Text?.Trim() ?? "";
            var filtroSeleccionado = (PickerProducto.SelectedItem as string) ?? "Todo";

            filtros.TryGetValue(filtroSeleccionado, out var datoSeleccionado);
            var resultado = await _productoService.GetProductoFiltradoSearchBar(textoBusqueda, datoSeleccionado, categoriaSeleccionada);

            Productos.Clear();
            foreach (var producto in resultado)
            {
                Productos.Add(producto); 
            }
        }
    }
}
