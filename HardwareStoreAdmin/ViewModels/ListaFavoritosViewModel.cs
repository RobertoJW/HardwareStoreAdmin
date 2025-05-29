using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin.ViewModels
{
    public class ListaFavoritosViewModel
    {
        private readonly ListaFavoritoService _listaFavoritoService = new();
        //public ObservableCollection<ListaFavoritos> ListaFavoritos { get; set; } = new();

        // Estoy usando la colección de productos porque lo que yo quiero cargar son los Productos. 
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
        public ListaFavoritosViewModel()
        {
            CargarListaFavoritosComando = new Command(async () => await CargarListaFavoritos());
            _ = CargarListaFavoritos(); // carga automatica de productos al abrir
        }

        public Command CargarListaFavoritosComando { get; }

        public async Task CargarListaFavoritos()
        {
            if (isCargando) return;
            isCargando = true;

            var lista = await _listaFavoritoService.GetListaFavoritoServiceAsync();
            Productos.Clear();

            foreach (var favoritos in lista)
            {
                foreach (var productos in favoritos.Productos)
                {
                    Productos.Add(productos);
                }
                
            }
            Debug.WriteLine($"Productos cargados: {Productos.Count}");
            isCargando = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
