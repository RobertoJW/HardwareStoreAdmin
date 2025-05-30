using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;

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
    }
}
