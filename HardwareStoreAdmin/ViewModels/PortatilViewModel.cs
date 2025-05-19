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
    public class PortatilViewModel
    {
        private readonly PortatilService _portatilService = new();
        public ObservableCollection<Portatil> Portatil { get; set; } = new();

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
        public PortatilViewModel()
        {
            CargarPortatilComando = new Command(async () => await CargarPortatil());
            _ = CargarPortatil(); // carga automatica de productos al abrir
        }

        public Command CargarPortatilComando { get; }

        public async Task CargarPortatil()
        {
            if (isCargando) return;
            isCargando = true;

            var lista = await _portatilService.GetPortatilAsync();
            Portatil.Clear();

            foreach (var portatil in lista)
            {
                Portatil.Add(portatil);
            }
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
