using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;

namespace HardwareStoreAdmin.Servicios
{
    public class DescripcionGeneralService
    {
        private readonly HttpClient _httpClient;

        public DescripcionGeneralService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<DescripcionGeneral>> GetDescripcionGeneralAsync()
        {
            var response = await _httpClient.GetAsync("https://hardwarestore-8071e.oa.r.appspot.com/api/controladorusuarios");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<DescripcionGeneral>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<DescripcionGeneral>();
        }
    }
}
