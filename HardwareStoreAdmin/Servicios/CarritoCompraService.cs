using System.Diagnostics;
using System.Text;
using System.Text.Json;
using HardwareStoreAdmin.Modelo;

namespace HardwareStoreAdmin.Servicios
{
    public class CarritoCompraService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://hardwarestore-8071e.oa.r.appspot.com/api/carritocompra";

        public CarritoCompraService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<CarritoCompra>> GetCarritoCompraAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/usuario/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CarritoCompra>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<CarritoCompra>();
        }
        public async Task<bool> AgregarAlCarrito(int userId, int productoId)
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
            Debug.WriteLine($"[AgregarAlCarrito] Respuesta: {response.StatusCode} - {contenido}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> QuitarDelCarrito(int userId, int productoId)
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
