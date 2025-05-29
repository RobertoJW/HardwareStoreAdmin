using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin.Servicios
{
    public class ListaFavoritoService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://hardwarestore-8071e.oa.r.appspot.com/api/listafavoritos";

        public ListaFavoritoService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<ListaFavoritos>> GetListaFavoritoServiceAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ListaFavoritos>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<ListaFavoritos>();
        }
        public async Task<bool> AgregarAFavoritos(int userId, int productoId)
        {
            var data = new
            {
                UserId = userId,
                ProductoId = productoId
            };

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/agregar", content);

            var contenido = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"[AgregarAFavoritos] Respuesta: {response.StatusCode} - {contenido}");

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> QuitarDeFavoritos(int userId, int productoId)
        {
            var data = new
            {
                UserId = userId,
                ProductoId = productoId
            };

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/quitar", content);
            return response.IsSuccessStatusCode;
        }

    }
}
