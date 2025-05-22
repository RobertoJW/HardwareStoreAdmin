using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using HardwareStoreAdmin.ViewModels;
using System.Diagnostics;

namespace HardwareStoreAdmin.AppClass;

public partial class ListaFavoritos : ContentPage
{
    private bool esClicado = false;
    private ListaFavoritosViewModel _listaFavoritoViewModel;
    public ListaFavoritos()
    {
        InitializeComponent();
        _listaFavoritoViewModel = new ListaFavoritosViewModel();
        BindingContext = _listaFavoritoViewModel;
        CargarDatosAsync();
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

    private async void CargarDatosAsync()
    {
        try
        {
            await _listaFavoritoViewModel.CargarListaFavoritos();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error cargando productos: {ex.Message}");
            // Aquí puedes mostrar un alert si quieres
            await DisplayAlert("Error", $"No se pudieron cargar los productos: {ex.Message}", "OK");
        }
    }
}