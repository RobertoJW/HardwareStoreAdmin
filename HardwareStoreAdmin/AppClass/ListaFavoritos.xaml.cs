using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HardwareStoreAdmin.AppClass;

public partial class ListaFavoritos : ContentPage
{
    private readonly UsuarioService _usuarioService = new();
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
    }

    private async void OnProductoSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Producto productoSeleccionado)
        {
            // Quitamos la seleccion para evitar que quede seleccionado visualmente. 
            ((CollectionView)sender).SelectedItem = null;

            var productoCompleto = await _productoService.GetProductoPorIdAsync(productoSeleccionado.IdProducto);
            // navegamos a la página con el producto que hemos clicado. 
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
    }
}
