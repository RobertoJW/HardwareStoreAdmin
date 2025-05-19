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
    public class MovilService
    {
        private readonly HttpClient _httpClient;

        public async Task<List<Movil>> GetMovilAsync()
        {
            var response = await _httpClient.GetAsync("https://hardwarestore-8071e.oa.r.appspot.com/api/controladorusuarios");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Movil>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Movil>();
        }
    }
}
