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
    public class SobremesaViewModel
    {
        private readonly SobremesaService _sobremesaService = new();
        public ObservableCollection<Sobremesa> Sobremesa { get; set; } = new();

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
        public SobremesaViewModel()
        {
            CargarSobremesaComando = new Command(async () => await CargarSobremesa());
            _ = CargarSobremesa(); // carga automatica de productos al abrir
        }

        public Command CargarSobremesaComando { get; }

        public async Task CargarSobremesa()
        {
            if (isCargando) return;
            isCargando = true;

            var lista = await _sobremesaService.GetSobremesaAsync();
            Sobremesa.Clear();

            foreach (var sb in lista)
            {
                Sobremesa.Add(sb);
            }
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
