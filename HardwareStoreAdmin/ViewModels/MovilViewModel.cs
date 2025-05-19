using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin.ViewModels
{
    public class MovilViewModel
    {
        private readonly MovilService _movilService = new();
        public ObservableCollection<Movil> Movil { get; set; } = new();

        // si es true, la app está cargando los productos, si es false, todos los productos ya se han cargado
        private bool estaCargando;
        public bool isCargando
        {
            get => estaCargando;
            set
            {
                estaCargando = value;
                OnPropertyChanged();
            }
        }
        public MovilViewModel()
        {
            CargarMovilComando = new Command(async () => await CargarMovil());
            _ = CargarMovil(); // carga automatica de productos al abrir
        }

        public Command CargarMovilComando { get; }

        public async Task CargarMovil()
        {
            if (isCargando) return;
            isCargando = true;

            var lista = await _movilService.GetMovilAsync();
            Movil.Clear();

            foreach (var movil in lista)
            {
                Movil.Add(movil);
            }
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
