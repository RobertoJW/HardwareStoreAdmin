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
    public class CarritoCompraViewModel
    {
        private readonly CarritoCompraService _carritoCompraService = new();
        public ObservableCollection<CarritoCompra> CarritoCompra { get; set; } = new();

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
        public CarritoCompraViewModel()
        {
            CargarCarritoCompraComando = new Command(async () => await CargarCarritoCompra());
            _ = CargarCarritoCompra(); // carga automatica de productos al abrir
        }

        public Command CargarCarritoCompraComando { get; }

        public async Task CargarCarritoCompra()
        {
            if (isCargando) return;
            isCargando = true;

            var lista = await _carritoCompraService.GetCarritoCompraAsync();
            CarritoCompra.Clear();

            foreach (var cc in lista)
            {
                CarritoCompra.Add(cc);
            }
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
