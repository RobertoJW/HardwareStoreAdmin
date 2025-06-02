using System.Collections.ObjectModel;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin.AppClass;

public partial class CarritoCompra : ContentPage
{
    private readonly UsuarioService _usuarioService = new();
    private readonly ProductoService _productoService = new();
    public int ProductosEnCarritoNumero;

    public ObservableCollection<Producto> ProductosEnCarrito { get; set; } = new();

    public CarritoCompra()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarCarrito();
    }

    private async Task CargarCarrito()
    {
        var usuario = await _usuarioService.GetProductoPorIdUsuarioAsync(App.UsuarioActual.userId);
        ProductosEnCarrito.Clear();

        if (usuario?.CarritoCompra?.Productos != null)
        {
            foreach (var producto in usuario.CarritoCompra.Productos)
            {
                ProductosEnCarrito.Add(producto);
            }
        }
        ProductosEnCarritoNumero = ProductosEnCarrito.Count;
    }

    private async void OnProductoSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Producto productoSeleccionado)
        {
            ((CollectionView)sender).SelectedItem = null;

            var productoCompleto = await _productoService.GetProductoPorIdAsync(productoSeleccionado.IdProducto);
            await Navigation.PushAsync(new InterfazDetallesProducto(productoCompleto));
        }
    }
}