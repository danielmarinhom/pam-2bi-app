using PamTcc.Models;
using PamTcc.ViewModels;

namespace PamTcc
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            LoadData(); //get na inicializacao da tela
            this.BindingContext = _viewModel;
        }
        private async void OnAdicionarSugestaoClicked(object sender, EventArgs e)
        {
            var texto = NovaSugestaoEntry.Text;
            var usuarioSelecionado = UsuarioPicker.SelectedItem as Usuario;

            if (string.IsNullOrWhiteSpace(texto) || usuarioSelecionado == null)
            {
                await DisplayAlert("Erro", "Nulo ou Espaco em Branco", "OK");
                return;
            }

            var novaSugestao = new Sugestao
            {
                Text = texto,
                UsuarioId = usuarioSelecionado.Id,
                Usuario = null //a api aceita null
            };

            bool sucesso = await _viewModel.AddSugestaoAsync(novaSugestao);
            if (sucesso)
            {
                NovaSugestaoEntry.Text = string.Empty;
                UsuarioPicker.SelectedItem = null;
                await DisplayAlert("Sucesso", "Sugestao adicionada", "OK");
                LoadData(); //atualiza a tela
            }
            else
            {
                await DisplayAlert("Erro", "Erro ao adicionar sugestao", "OK");
            }
        }

        private async void OnDeletarSugestaoClicked(object sender, EventArgs e)
        {
            if (int.TryParse(SugestaoIdParaDeletarEntry.Text, out int id))
            {
                bool sucesso = await _viewModel.DeleteSugestaoAsync(id);
                if (sucesso)
                {
                    SugestaoIdParaDeletarEntry.Text = string.Empty;
                    await DisplayAlert("Sucesso", "Sugestao deletada", "OK");
                    LoadData();
                }
                else
                {
                    await DisplayAlert("Erro", "Sugestao invalida", "OK");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Id invalido", "OK");
            }
        }

        private async void LoadData()
        {
            try
            {
                await _viewModel.LoadUsuariosComSugestoesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
                await DisplayAlert("Erro", $"{ex.Message}", "OK");
            }
        }
        private async void OnAtualizarSugestaoClicked(object sender, EventArgs e)
        {
            if (!int.TryParse(SugestaoIdParaAtualizarEntry.Text, out int id))
            {
                await DisplayAlert("Erro", "id invalido", "OK");
                return;
            }

            var novoTexto = TextoSugestaoAtualizadaEntry.Text;
            var usuarioSelecionado = UsuarioParaAtualizarPicker.SelectedItem as Usuario;

            if (string.IsNullOrWhiteSpace(novoTexto) || usuarioSelecionado == null)
            {
                await DisplayAlert("Erro", "texto vazio ou invalido", "OK");
                return;
            }

            var sugestaoAtualizada = new Sugestao
            {
                Id = id,
                Text = novoTexto,
                UsuarioId = usuarioSelecionado.Id,
                Usuario = null
            };

            bool sucesso = await _viewModel.UpdateSugestaoAsync(sugestaoAtualizada);
            if (sucesso)
            {
                SugestaoIdParaAtualizarEntry.Text = string.Empty;
                TextoSugestaoAtualizadaEntry.Text = string.Empty;
                UsuarioParaAtualizarPicker.SelectedItem = null;
                await DisplayAlert("Sucesso", "Sugestao atualizada", "OK");
                LoadData();
            }
            else
            {
                await DisplayAlert("Erro", "Erro", "OK");
            }
        }

        private async void OnAtualizarClicked(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
