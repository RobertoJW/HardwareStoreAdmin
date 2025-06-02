using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

public class ProductoDetalleViewModel : INotifyPropertyChanged
{
    private readonly ListaFavoritoService _favService;
    private readonly CarritoCompraService _carritoService;
    private readonly int _userId;

    public Producto Model { get; }

    public string ImagenUrl => Model.ImagenUrl;
    public string NombreProducto => Model.NombreProducto;
    public decimal Precio => Model.Precio;
    public string NombreEmpresa => Model.NombreEmpresa;
    public DescripcionGeneral DescripcionGeneral => Model.DescripcionGeneral;

    // FAVORITO
    public ICommand ToggleFavoritoCommand { get; }

    private bool _esFavorito;
    public bool EsFavorito
    {
        get => _esFavorito;
        set
        {
            _esFavorito = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ColorFavorito));
        }
    }

    public Color ColorFavorito => EsFavorito ? Colors.Red : Colors.Gray;

    // CARRITO
    public ICommand ToggleCarritoCommand { get; }

    private bool _estaEnCarrito;
    public bool EstaEnCarrito
    {
        get => _estaEnCarrito;
        set
        {
            _estaEnCarrito = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TextoBotonCarrito));
        }
    }

    public string TextoBotonCarrito => EstaEnCarrito ? "Quitar del carrito" : "Agregar al carrito";

    public ProductoDetalleViewModel(
        Producto producto,
        bool esFavorito,
        bool estaEnCarrito,
        int userId,
        ListaFavoritoService favService,
        CarritoCompraService carritoService)
    {
        Model = producto;
        EsFavorito = esFavorito;
        EstaEnCarrito = estaEnCarrito;
        _userId = userId;
        _favService = favService;
        _carritoService = carritoService;

        ToggleFavoritoCommand = new Command(async () =>
        {
            if (EsFavorito)
                await _favService.QuitarDeFavoritos(_userId, Model.IdProducto);
            else
                await _favService.AgregarAFavoritos(_userId, Model.IdProducto);

            EsFavorito = !EsFavorito;
        });

        ToggleCarritoCommand = new Command(async () =>
        {
            if (EstaEnCarrito)
                await _carritoService.QuitarDelCarrito(_userId, Model.IdProducto);
            else
                await _carritoService.AgregarAlCarrito(_userId, Model.IdProducto);

            EstaEnCarrito = !EstaEnCarrito;
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}