using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin.Servicios
{
    public class CarritoCompraService
    {
        private readonly HttpClient _httpClient;

        public CarritoCompraService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<CarritoCompra>> GetCarritoCompraAsync()
        {
            var response = await _httpClient.GetAsync("https://hardwarestore-8071e.oa.r.appspot.com/api/carritocompra");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CarritoCompra>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<CarritoCompra>();
        }
    }
}
