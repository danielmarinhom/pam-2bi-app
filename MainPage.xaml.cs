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
            this.BindingContext = _viewModel;
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

        private async void OnAtualizarClicked(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
