using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HardwareStoreAdmin.AppClass;

public partial class ListaFavoritos : ContentPage
{
    private readonly UsuarioService _usuarioService = new();
    private int _totalFavoritosInicial = 0;
    public ObservableCollection<Producto> Productos { get; set; } = new();
    private readonly ProductoService _productoService = new ProductoService();

    public ListaFavoritos()
    {
        InitializeComponent();
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarFavoritos();
    }

    private async Task CargarFavoritos()
    {
        var usuario = await _usuarioService.GetProductoPorIdUsuarioAsync(App.UsuarioActual.userId);
        Productos.Clear();

        if (usuario?.ListaFavoritos?.Productos != null)
        {
            foreach (var producto in usuario.ListaFavoritos.Productos)
            {
                Productos.Add(producto);
            }
        }

        _totalFavoritosInicial = Productos.Count;
        ActualizarMensajeSinResultados("Todos");
    }

    private async void OnProductoSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Producto productoSeleccionado)
        {
            // Quitamos la seleccion para evitar que quede seleccionado visualmente. 
            ((CollectionView)sender).SelectedItem = null;

            var productoCompleto = await _productoService.GetProductoPorIdAsync(productoSeleccionado.IdProducto);
            // navegamos a la p�gina con el producto que hemos clicado. 
            await Navigation.PushAsync(new InterfazDetallesProducto(productoCompleto));
        }
    }

    private async void BtnFiltro(object sender, EventArgs e)
    {
        var boton = sender as Button;
        string categoria = boton?.Text;
        List<Producto> productosFiltrados;

        FiltroTodos.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];
        FiltroPortatil.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];
        FiltroSobremesa.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];
        FiltroMovil.BackgroundColor = (Color)Application.Current.Resources["NaranjaClaro"];

        var button = sender as Button;
        button.BackgroundColor = (Color)Application.Current.Resources["Naranja"];

        if (categoria == "Todos")
        {
            productosFiltrados = await _usuarioService.GetTodosLosProductosListaFavoritos(categoria, App.UsuarioActual.userId);
        }
        else
        {
            productosFiltrados = await _usuarioService.GetProductoFiltradoFavorito(categoria, App.UsuarioActual.userId);
        }

        Productos.Clear();
        foreach (var producto in productosFiltrados)
        {
            Productos.Add(producto);
        }
        ActualizarMensajeSinResultados(categoria);
    }
    void ActualizarMensajeSinResultados(string categoria)
    {
        if (Productos.Count == 0)
        {
            // Si originalmente no hab�a ning�n favorito, siempre mensaje gen�rico
            if (_totalFavoritosInicial == 0)
            {
                lblNoFavoritos.Text = "Todav�a no has a�adido ning�n producto a favoritos. �Prueba a a�adir alguno para gestionar tus productos preferidos de forma sencilla!";
            }
            else
            {
                // Si s� hab�a favoritos originalmente pero el filtro actual no trajo nada...
                if (string.IsNullOrWhiteSpace(categoria) || categoria == "Todos")
                    lblNoFavoritos.Text = "Todav�a no has a�adido ning�n producto a favoritos. �Prueba a a�adir alguno para gestionar tus productos preferidos de forma sencilla!";
                else
                    lblNoFavoritos.Text = $"A�n no has a�adido a favoritos ning�n producto de la categor�a �{categoria}�.";
            }
            lblNoFavoritos.IsVisible = true;
        }
        else
        {
            lblNoFavoritos.IsVisible = false;
        }
    }

}
