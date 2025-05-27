namespace HardwareStoreAdmin.AppClass;

public partial class ImagenCompleta : ContentPage
{
	public ImagenCompleta(string imagenUrl)
	{
		InitializeComponent();
        ImagenGrande.Source = imagenUrl;
    }

	private async void CerrarImagenCompleta_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}