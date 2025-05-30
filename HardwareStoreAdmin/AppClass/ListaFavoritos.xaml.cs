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
        int userId = Preferences.Get("userId", 0);
        _listaFavoritoViewModel.CargarListaFavoritos(userId);
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
}