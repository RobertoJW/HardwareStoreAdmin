using HardwareStoreAdmin;
using HardwareStoreAdmin.Modelo;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.AppClass;

public partial class InterfazDetallesProducto : ContentPage
{
    bool esClicado = false;
    private Producto producto; 
    public InterfazDetallesProducto(Producto producto)
	{
		InitializeComponent();
        this.producto = producto; 
        BindingContext = producto;
    }

	public async void BtnVueltaPaginaPrincipal(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//MainPage");
    }

    private void BtnProductoAListaDeFavoritos(object sender, EventArgs e)
    {
        var button = (Button)sender;
        if (button == null) return;

        var colorOriginalBtn = (Color)Application.Current.Resources["GrisClaro"];

        button.TextColor = esClicado ? colorOriginalBtn : Colors.Red;

        // al clicar de vuelta el boton, cambiará de color
        esClicado = !esClicado;
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