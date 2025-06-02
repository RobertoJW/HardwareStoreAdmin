using System.Collections.ObjectModel;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using System.Diagnostics;

namespace HardwareStoreAdmin.AppClass;

public partial class CarritoCompra : ContentPage
{
    private readonly UsuarioService _usuarioService = new();
    private readonly ProductoService _productoService = new();
    private readonly CarritoCompraService _carritoCompraService = new();

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
        try
        {
            // Obtener usuario con carrito
            var usuario = await _usuarioService.GetUsuarioConCarritoAsync(App.UsuarioActual.userId);
            ProductosEnCarrito.Clear();

            if (usuario?.CarritoCompra?.Productos != null)
            {
                foreach (var producto in usuario.CarritoCompra.Productos)
                {
                    ProductosEnCarrito.Add(producto);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[CargarCarrito] Error: {ex.Message}");
            await DisplayAlert("Error", "No se pudo cargar el carrito de compras.", "OK");
        }
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