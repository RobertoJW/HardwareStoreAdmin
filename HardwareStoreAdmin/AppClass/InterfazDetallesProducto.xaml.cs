using Firebase.Auth;
using HardwareStoreAdmin;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using System.Diagnostics;

namespace HardwareStoreAdmin.AppClass;

public partial class InterfazDetallesProducto : ContentPage
{
    private Producto producto;
    private ProductoFavoritoViewModel viewModel;
    private readonly ListaFavoritoService favoritoService = new ListaFavoritoService();
    private readonly UsuarioService usuarioService = new UsuarioService();

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
            await DisplayAlert("Atención", "Debes iniciar sesión para usar favoritos.", "OK");
            return;
        }

        var usuario = await usuarioService.GetUsuarioConFavoritosAsync(App.UsuarioActual.userId);

        bool esFavorito = usuario?.ListaFavoritos?.Productos?.Any(p => p.IdProducto == producto.IdProducto) ?? false;

        viewModel = new ProductoFavoritoViewModel(producto, esFavorito, App.UsuarioActual.userId, favoritoService);
        BindingContext = viewModel;
        Debug.WriteLine($"Producto {producto.NombreProducto} es favorito: {esFavorito}");
    }

    public async void BtnVueltaPaginaPrincipal(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
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
