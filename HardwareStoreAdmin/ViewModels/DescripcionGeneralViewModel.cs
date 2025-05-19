using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;
using Microsoft.Maui.Controls; // O Xamarin.Forms si usas eso

namespace HardwareStoreAdmin.ViewModels
{
    public class DescripcionGeneralViewModel : INotifyPropertyChanged
    {
        private readonly DescripcionGeneralService _dgService;

        public ObservableCollection<DescripcionGeneral> DescripcionGeneral { get; set; } = new();

        private bool estaCargando;
        public bool IsCargando
        {
            get => estaCargando;
            set
            {
                estaCargando = value;
                OnPropertyChanged();
            }
        }

        public Command CargarDescripcionGeneralComando { get; }

        public DescripcionGeneralViewModel()
        {
            _dgService = new DescripcionGeneralService();
            CargarDescripcionGeneralComando = new Command(async () => await CargarDescripcionGeneral());
            _ = CargarDescripcionGeneral(); // carga automática al abrir
        }

        public async Task CargarDescripcionGeneral()
        {
            if (IsCargando) return;
            IsCargando = true;

            var lista = await _dgService.GetDescripcionGeneralAsync();

            DescripcionGeneral.Clear();
            foreach (var item in lista)
            {
                DescripcionGeneral.Add(item);
            }

            IsCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
