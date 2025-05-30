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
        var usuario = await _usuarioService.GetUsuarioConFavoritosAsync(App.UsuarioActual.userId);
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

            // navegamos a la página con el producto que hemos clicado. 
            await Navigation.PushAsync(new InterfazDetallesProducto(productoSeleccionado));
        }
    }
    private void BtnFilterDesktop(object sender, EventArgs e)
    {
        
    }

    private void BtnFilterLaptop(object sender, EventArgs e)
    {

    }

    private void BtnFilterPhone(object sender, EventArgs e)
    {

    }

    private void BtnFilterAll_Clicked(object sender, EventArgs e)
    {

    }
}
