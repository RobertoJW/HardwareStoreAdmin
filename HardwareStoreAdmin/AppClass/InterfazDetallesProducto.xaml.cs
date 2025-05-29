using Firebase.Auth;
using HardwareStoreAdmin;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using System.Diagnostics;

namespace HardwareStoreAdmin.AppClass;

public partial class InterfazDetallesProducto : ContentPage
{
    bool esClicado = false;
    private Producto producto;

    private readonly ListaFavoritoService favoritoService = new ListaFavoritoService();

    public InterfazDetallesProducto(Producto producto)
    {
        InitializeComponent();
        this.producto = producto;
        BindingContext = producto;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InicializarEstadoFavoritoAsync();
    }

    private async Task InicializarEstadoFavoritoAsync()
    {
        if (App.UsuarioActual == null)
        {
            esClicado = false;
            BtnFavourite.TextColor = (Color)Application.Current.Resources["GrisClaro"];
            return;
        }

        var favoritos = await favoritoService.GetListaFavoritoServiceAsync();

        // Obtener la lista del usuario actual
        var listaUsuario = favoritos.FirstOrDefault(f => f.userId == App.UsuarioActual.userId);

        // Verificar si su lista contiene el producto actual
        esClicado = listaUsuario?.Productos.Any(p => p.IdProducto == producto.IdProducto) ?? false;

        BtnFavourite.TextColor = esClicado ? Colors.Red : (Color)Application.Current.Resources["GrisClaro"];
    }


    private async void BtnProductoAListaDeFavoritos(object sender, EventArgs e)
    {
        if (App.UsuarioActual == null)
        {
            await DisplayAlert("Atención", "Debes iniciar sesión para usar favoritos.", "OK");
            return;
        }

        var button = (Button)sender;
        if (button == null) return;

        if (!esClicado)
        {
            bool agregado = await favoritoService.AgregarAFavoritos(App.UsuarioActual.userId, producto.IdProducto);
            if (agregado)
            {
                button.TextColor = Colors.Red;
                esClicado = true;
            }
            else
            {
                await DisplayAlert("Error", "No se pudo agregar a favoritos", "OK");
            }
        }
        else
        {
            bool eliminado = await favoritoService.QuitarDeFavoritos(App.UsuarioActual.userId, producto.IdProducto);
            if (eliminado)
            {
                button.TextColor = (Color)Application.Current.Resources["GrisClaro"];
                esClicado = false;
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar de favoritos", "OK");
            }
        }
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
