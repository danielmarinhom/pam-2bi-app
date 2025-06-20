using PamTcc.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PamTcc.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _http;

        public UsuarioService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5125/") //local
            };
        }

        public async Task<List<Usuario>?> GetAllUsuariosAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<Usuario>>("api/usuario");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<Usuario>($"api/usuario/{id}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateUsuarioAsync(Usuario usuario)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/usuario", usuario);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUsuarioAsync(int id, Usuario usuario)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/usuario/{id}", usuario);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/usuario/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
                return false;
            }
        }
    }
}
