using PamTcc.Models;
using PamTcc.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PamTcc.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly SugestaoService _sugestaoService = new SugestaoService();

        public ObservableCollection<Usuario> Usuarios { get; } = new ObservableCollection<Usuario>();
        public ObservableCollection<Sugestao> Sugestoes { get; } = new ObservableCollection<Sugestao>();

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public async Task<bool> AddUsuarioAsync(Usuario usuario)
        {
            var result = await _usuarioService.CreateUsuarioAsync(usuario);
            if (result)
            {
                StatusMessage = "Usuário adicionado!";
            }
            return result;
        }

        public async Task<bool> UpdateUsuarioAsync(Usuario usuario)
        {
            var result = await _usuarioService.UpdateUsuarioAsync(usuario.Id, usuario);
            if (result)
            {
                StatusMessage = "Usuário atualizado!";
            }
            return result;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var result = await _usuarioService.DeleteUsuarioAsync(id);
            if (result)
            {
                StatusMessage = "Usuário deletado!";
            }
            return result;
        }

        public async Task LoadUsuariosAsync()
        {
            try
            {
                var lista = await _usuarioService.GetAllUsuariosAsync();
                Usuarios.Clear();

                if (lista != null)
                {
                    foreach (var usuario in lista)
                        Usuarios.Add(usuario);

                    StatusMessage = $"Carregado {Usuarios.Count} usuários.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao carregar usuários: {ex.Message}";
            }
        }


        public async Task<bool> AddSugestaoAsync(Sugestao sugestao)
        {
            var result = await _sugestaoService.CreateSugestaoAsync(sugestao);
            if (result)
            {
                StatusMessage = "Sugestão adicionada!";
            }
            return result;
        }

        public async Task<bool> UpdateSugestaoAsync(Sugestao sugestao)
        {
            var result = await _sugestaoService.UpdateSugestaoAsync(sugestao.Id, sugestao);
            if (result)
            {
                StatusMessage = "Sugestão atualizada!";
            }
            return result;
        }

        public async Task<bool> DeleteSugestaoAsync(int id)
        {
            var result = await _sugestaoService.DeleteSugestaoAsync(id);
            if (result)
            {
                StatusMessage = "Sugestão deletada!";
            }
            return result;
        }

        public async Task LoadSugestoesAsync()
        {
            try
            {
                var lista = await _sugestaoService.GetAllSugestoesAsync();
                Sugestoes.Clear();

                if (lista != null)
                {
                    foreach (var sugestao in lista)
                        Sugestoes.Add(sugestao);

                    StatusMessage = $"Carregado {Sugestoes.Count} sugestões.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao carregar sugestões: {ex.Message}";
            }
        }
    }
}
