using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.ViewModels;

namespace HardwareStoreAdmin.AppClass
{
    public partial class CarritoCompra : ContentPage
    {
        private readonly CarritoCompraViewModel _viewModel;

        public CarritoCompra()
        {
            InitializeComponent();
            _viewModel = new CarritoCompraViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarCarrito();
        }

        private async void OnProductoSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Producto p)
            {
                ((CollectionView)sender).SelectedItem = null;
                var detalle = await _viewModel.CargarProductoDetalle(p.IdProducto);
                await Navigation.PushAsync(new InterfazDetallesProducto(detalle));
            }
        }
    }
}
