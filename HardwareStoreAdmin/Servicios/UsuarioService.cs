using System.Text.Json;
using HardwareStoreAdmin.Modelo;

namespace HardwareStoreAdmin.Servicios
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync("https://hardwarestore-8071e.oa.r.appspot.com/api/usuarios");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Usuario>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Usuario>();
        }
    }
}
