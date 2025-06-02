using System.Collections.ObjectModel;
using System.ComponentModel;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin.AppClass;

public partial class CarritoCompra : ContentPage, INotifyPropertyChanged
{
    private readonly UsuarioService _usuarioService = new();
    private readonly ProductoService _productoService = new();

    public ObservableCollection<Producto> ProductosEnCarrito { get; set; } = new();

    private int _productosEnCarritoNumero;
    public int ProductosEnCarritoNumero
    {
        get => _productosEnCarritoNumero;
        set
        {
            _productosEnCarritoNumero = value;
            OnPropertyChanged();
        }
    }

    private decimal _precioTotal;
    public decimal PrecioTotal
    {
        get => _precioTotal;
        set
        {
            _precioTotal = value;
            OnPropertyChanged();
        }
    }

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
        PrecioTotal = ProductosEnCarrito.Sum(p => p.Precio);
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