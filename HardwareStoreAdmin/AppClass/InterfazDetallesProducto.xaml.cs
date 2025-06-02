using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using System.Diagnostics;

namespace HardwareStoreAdmin.AppClass;

public partial class InterfazDetallesProducto : ContentPage
{
    private Producto producto;
    private ProductoDetalleViewModel viewModel;
    private readonly ListaFavoritoService favoritoService = new ListaFavoritoService();
    private readonly UsuarioService usuarioService = new UsuarioService();
    private readonly CarritoCompraService carritoService = new CarritoCompraService();
    public InterfazDetallesProducto(Producto producto)
    {
        InitializeComponent();
        this.producto = producto;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (App.UsuarioActual == null)
        {
            await DisplayAlert("Atención", "Debes iniciar sesión para continuar.", "OK");
            return;
        }

        var userId = App.UsuarioActual.userId;

        var usuario = await usuarioService.GetUsuarioConFavoritosAsync(userId);
        var usuarioConCarrito = await usuarioService.GetUsuarioConCarritoAsync(userId); // <-- Este método lo necesitas

        bool esFavorito = usuario?.ListaFavoritos?.Productos?.Any(p => p.IdProducto == producto.IdProducto) ?? false;
        bool estaEnCarrito = usuarioConCarrito?.CarritoCompra?.Productos?.Any(p => p.IdProducto == producto.IdProducto) ?? false;

        var viewModel = new ProductoDetalleViewModel(
            producto,
            esFavorito,
            estaEnCarrito,
            userId,
            favoritoService,
            new CarritoCompraService()
        );

        BindingContext = viewModel;
    }
    public async void BtnVueltaPaginaPrincipal(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void Imagen_Tapped(object sender, EventArgs e)
    {
        var imagenUrl = (BindingContext as Producto)?.ImagenUrl;

        if (!string.IsNullOrEmpty(imagenUrl))
        {
            await Navigation.PushModalAsync(new ImagenCompleta(imagenUrl));
        }
    }
}