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
    public class SobremesaService
    {
        private readonly HttpClient _httpClient;

        public async Task<List<Sobremesa>> GetSobremesaAsync()
        {
            var response = await _httpClient.GetAsync("https://hardwarestore-8071e.oa.r.appspot.com/api/controladorusuarios");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Sobremesa>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Sobremesa>();
        }
    }
}
