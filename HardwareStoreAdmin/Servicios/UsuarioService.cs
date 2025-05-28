using System.Buffers.Text;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using HardwareStoreAdmin.Modelo;

namespace HardwareStoreAdmin.Servicios
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://hardwarestore-8071e.oa.r.appspot.com/api/usuarios";

        public UsuarioService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Usuario>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<Usuario>();
        }

        public async Task<Usuario?> VerificarCredencialesAsync(string email, string password)
        {
            var usuarioLogin = new Usuario
            {
                email = email,
                password = password
            };

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/login", usuarioLogin);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Usuario>();
        }

        public async Task<Usuario?> RegistrarUsuarioAsync(string nombre, string email, string password)
        {
            var nuevoUsuario = new Usuario
            {
                userName = nombre,
                email = email,
                password = password
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(baseUrl, nuevoUsuario);
                var contenido = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[ERROR HTTP] Código: {response.StatusCode}");
                    Debug.WriteLine($"[ERROR BODY] Respuesta: {contenido}");
                    return null;
                }

                return JsonSerializer.Deserialize<Usuario>(contenido, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[EXCEPCIÓN] Error en RegistrarUsuarioAsync: {ex.Message}");
                return null;
            }
        }

    }
}
