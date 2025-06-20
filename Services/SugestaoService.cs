using PamTcc.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PamTcc.Services
{
    public class SugestaoService
    {
        private readonly HttpClient _http;

        public SugestaoService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5125/") // Certifique-se que isso corresponde ao seu backend real
            };
        }

        public async Task<List<Sugestao>?> GetAllSugestoesAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<Sugestao>>("api/sugestao");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao buscar sugestões: {ex.Message}");
                return null;
            }
        }

        public async Task<Sugestao?> GetSugestaoByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<Sugestao>($"api/sugestao/{id}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao buscar sugestão por ID: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateSugestaoAsync(Sugestao sugestao)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/sugestao", sugestao);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao criar sugestão: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSugestaoAsync(int id, Sugestao sugestao)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/sugestao/{id}", sugestao);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar sugestão: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteSugestaoAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/sugestao/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao deletar sugestão: {ex.Message}");
                return false;
            }
        }
    }
}
