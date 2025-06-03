using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Converter;

namespace HardwareStoreAdmin.Servicios
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            var response = await _httpClient.GetAsync("https://hardwarestore-8071e.oa.r.appspot.com/api/productos");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new ProductoConverter());
                return JsonSerializer.Deserialize<List<Producto>>(json, options);
            }
            return new List<Producto>();
        }

        public async Task<Producto> GetProductoPorIdAsync(int idProducto)
        {
            // cargamos todos los productos
            var todosLosProductos = await GetProductosAsync();

            // buscamos el producto por su id
            var producto = todosLosProductos.FirstOrDefault(p => p.IdProducto == idProducto);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {idProducto} no encontrado.");

            return producto;
        }

        // METODOS PARA FILTROS Y PARA LA BARRA DE BÚSQUEDA.
        public async Task<List<Producto>> GetProductoFiltrado(string categoria)
        {
            // cargamos todos los productos
            var todosLosProductos = await GetProductosAsync();

            // buscamos el producto por su id
            var producto = todosLosProductos.Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (producto.Count == null)
                throw new KeyNotFoundException($"Productos con categoria {categoria} no encontrados.");

            return producto;
        }

        public async Task<List<Producto>> GetProductoFiltradoSearchBar(string textoBusqueda, string datoSeleccionado, string categoria)
        {
            // cargamos todos los productos
            var todosLosProductos = await GetProductosAsync();

            if (categoria != "Todo")
            {
                todosLosProductos = todosLosProductos
                    .Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (datoSeleccionado == "Todo")
            {
                return todosLosProductos.Where(p => p.NombreEmpresa.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase) 
                || p.NombreProducto.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                var productoFiltrado = todosLosProductos
                    .Where(p =>
                    {
                        // obtenemos el tipo de objeto (Producto), luego un atributo del objeto (datoSeleccionado) y finalmente su valor como texto.
                        var valorTextoBusqueda = p.GetType()?.GetProperty(datoSeleccionado)?.GetValue(p)?.ToString();
                        return valorTextoBusqueda != null && valorTextoBusqueda.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase);
                    }).ToList();

                return productoFiltrado;
            }
        }
    }
}
