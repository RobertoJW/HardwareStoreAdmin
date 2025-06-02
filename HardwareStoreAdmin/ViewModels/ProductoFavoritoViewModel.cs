using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

public class ProductoFavoritoViewModel : INotifyPropertyChanged
{
    private readonly ListaFavoritoService _favService;
    private readonly int _userId;

    public string ImagenUrl => Model.ImagenUrl;
    public string NombreProducto => Model.NombreProducto;
    public decimal Precio => Model.Precio;
    public string NombreEmpresa => Model.NombreEmpresa; 
    public DescripcionGeneral DescripcionGeneral => Model.DescripcionGeneral;


    public Producto Model { get; }
    public ICommand ToggleFavoritoCommand { get; }

    private bool _isFavorito;
    public bool IsFavorito
    {
        get => _isFavorito;
        set
        {
            if (_isFavorito == value) return;
            _isFavorito = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ColorFavorito));
        }
    }

    // Color / icono según estado
    public Color ColorFavorito => IsFavorito ? Colors.Red : Colors.Gray;

    public ProductoFavoritoViewModel(Producto producto, bool esFavorito, int userId, ListaFavoritoService favService)
    {
        Model = producto;
        IsFavorito = esFavorito;
        _userId = userId;
        _favService = favService;
        ToggleFavoritoCommand = new Command(async () =>
        {
            if (IsFavorito)
                await _favService.QuitarDeFavoritos(_userId, Model.IdProducto);
            else
                await _favService.AgregarAFavoritos(_userId, Model.IdProducto);

            // cambiamos localmente
            IsFavorito = !IsFavorito;
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}