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
    public class ProductoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService _productoService = new();
        public ObservableCollection<Producto> Productos { get; set; } = new();

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
        public ProductoViewModel()
        {
            CargarProductosComando = new Command(async () => await CargarProductos());
            _ = CargarProductos(); // carga automatica de productos al abrir
        }

        public Command CargarProductosComando { get; }

        public async Task CargarProductos()
        {
            if(isCargando) return;
            isCargando = true;

            var lista = await _productoService.GetProductosAsync();
            Productos.Clear();

            foreach (var producto in lista)
            {
                Productos.Add(producto);
            }
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));

    }
}
