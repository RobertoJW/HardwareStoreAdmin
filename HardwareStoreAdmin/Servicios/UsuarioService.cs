﻿using System.Buffers.Text;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using HardwareStoreAdmin.Modelo;
using HardwareStoreAdmin.Converter;


namespace HardwareStoreAdmin.Servicios
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;
        private ProductoService _productoService = new ProductoService(); 
        private readonly string baseUrl = "https://hardwarestore-8071e.oa.r.appspot.com/api/usuarios";

        public UsuarioService()
        {
            _httpClient = new HttpClient();
        }

        // metodo para obtener todos los usuarios de la base de datos.
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

        // metodo para obtener un usuario por su id.
        public async Task<Usuario?> GetUsuarioByIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{userId}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Usuario>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // metodo para verificar si un usuario existe en la base de datos
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

        // Metodo para registrar un nuevo usuario.
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
                    Debug.WriteLine($"[HTTP ERROR] StatusCode: {response.StatusCode}");
                    Debug.WriteLine($"[BODY] {contenido}");
                    return null;
                }

                try
                {
                    var usuarioCreado = JsonSerializer.Deserialize<Usuario>(contenido, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (usuarioCreado == null)
                    {
                        Debug.WriteLine("[DESERIALIZACIÓN] El objeto deserializado es null");
                    }

                    return usuarioCreado;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[EXCEPCIÓN DESERIALIZACIÓN] {ex.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[EXCEPCIÓN HTTP] {ex.Message}");
                return null;
            }
        }

        // método para mostrar toda la informacion de UN usuario en concreto. 
        public async Task<Usuario> GetProductoPorIdUsuarioAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new ProductoConverter() }
                };
                return JsonSerializer.Deserialize<Usuario>(json, options);
            }
            return null;
        }

        // Vaciar el carrito de un usuario
        public async Task<bool> VaciarCarritoUsuarioAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{userId}/carrito");
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"[VaciarCarritoUsuario] Error: {response.StatusCode}");
            }
            return response.IsSuccessStatusCode;
        }

        // METODOS PARA FILTROS Y BARRA DE BUSQUEDA
        // metodo para mostrar TODOS los productos de la lista de favoritos.
        public async Task<List<Producto>> GetTodosLosProductosListaFavoritos(string categoria, int userId)
        {
            // cargamos todos los productos
            var todosLosProductos = await _productoService.GetProductosAsync();

            // obtener usuario con su lista de favoritos
            var usuarioFavorito = await GetProductoPorIdUsuarioAsync(userId);

            if (usuarioFavorito?.ListaFavoritos?.Productos == null)
                return new List<Producto>(); // si no hay productos en la lista de favoritos, nos devolverá una lista vacía. 

            // Sacamos los productos favoritos del usuario
            var productosFavoritos = usuarioFavorito.ListaFavoritos.Productos.Select(p => p.IdProducto).ToHashSet();

            // filtramos los productos que están en favoritos y en la categoria seleccionada
            var favoritos = todosLosProductos.Where(p => productosFavoritos.Contains(p.IdProducto)).ToList();

            return favoritos;
        }

        // metodo para ayudarnos a filtrar los productos de la lista de favoritos de un usuario.
        public async Task<List<Producto>> GetProductoFiltradoFavorito(string categoria, int userId)
        {
            // cargamos todos los productos
            var todosLosProductos = await _productoService.GetProductosAsync();

            // obtener usuario con su lista de favoritos
            var usuarioFavorito = await GetProductoPorIdUsuarioAsync(userId);

            if (usuarioFavorito?.ListaFavoritos?.Productos == null)
                return new List<Producto>(); // si no hay productos en la lista de favoritos, nos devolverá una lista vacía. 

            // Sacamos los productos favoritos del usuario
            var productosFavoritos = usuarioFavorito.ListaFavoritos.Productos.Select(p => p.IdProducto).ToHashSet();

            // filtramos los productos que están en favoritos y en la categoria seleccionada
            var producto = todosLosProductos
                .Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase) &&
                            productosFavoritos.Contains(p.IdProducto))
                .ToList();

            return producto;
        }
    }
}
