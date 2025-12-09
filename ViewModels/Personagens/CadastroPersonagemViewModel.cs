using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    [QueryProperty(nameof(PersonagemSelecionadoId), "pId")]
    public class CadastroPersonagemViewModel : BaseViewModel
    {
        private readonly PersonagemService pService;

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; }

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);

            _ = ObterClasses();

            SalvarCommand = new Command(
                async () => await SalvarPersonagem(),
                () => ValidarCampos()
            );

            CancelarCommand = new Command(async () => await CancelarCadastro());
        }

        private async Task CancelarCadastro() =>
            await Shell.Current.GoToAsync("..");

        // ============================
        // PROPRIEDADES
        // ============================

        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        private string nome;
        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        private int pontosVida;
        public int PontosVida
        {
            get => pontosVida;
            set
            {
                pontosVida = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CadastroHabilitado));
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        private int forca;
        public int Forca
        {
            get => forca;
            set
            {
                forca = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        private int defesa;
        public int Defesa
        {
            get => defesa;
            set
            {
                defesa = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        private int inteligencia;
        public int Inteligencia
        {
            get => inteligencia;
            set { inteligencia = value; OnPropertyChanged(); }
        }

        private int disputas;
        public int Disputas
        {
            get => disputas;
            set { disputas = value; OnPropertyChanged(); }
        }

        private int vitorias;
        public int Vitorias
        {
            get => vitorias;
            set { vitorias = value; OnPropertyChanged(); }
        }

        private int derrotas;
        public int Derrotas
        {
            get => derrotas;
            set { derrotas = value; OnPropertyChanged(); }
        }

        // ============================
        // CLASSES DO PERSONAGEM
        // ============================

        private ObservableCollection<TipoClasse> listaTiposClasse;
        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get => listaTiposClasse;
            set { listaTiposClasse = value; OnPropertyChanged(); }
        }

        public async Task ObterClasses()
        {
            ListaTiposClasse = new ObservableCollection<TipoClasse>()
            {
                new TipoClasse() { Id = 1, Descricao = "Cavaleiro" },
                new TipoClasse() { Id = 2, Descricao = "Mago" },
                new TipoClasse() { Id = 3, Descricao = "Clérigo" }
            };
        }

        private TipoClasse tipoClasseSelecionado;
        public TipoClasse TipoClasseSelecionado
        {
            get => tipoClasseSelecionado;
            set
            {
                tipoClasseSelecionado = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        // ============================
        // SALVAR
        // ============================

        public async Task SalvarPersonagem()
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Id = Id,
                    Nome = Nome,
                    PontosVida = PontosVida,
                    Forca = Forca,
                    Defesa = Defesa,
                    Inteligencia = Inteligencia,
                    Disputas = Disputas,
                    Derrotas = Derrotas,
                    Vitorias = Vitorias,
                    Classe = (ClasseEnum)TipoClasseSelecionado.Id
                };

                if (Id == 0)
                    await pService.PostPersonagemAsync(model);
                else
                    await pService.PutPersonagemAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        // ============================
        // CARREGAR PERSONAGEM
        // ============================

        private string personagemSelecionadoId;
        public string PersonagemSelecionadoId
        {
            set
            {
                personagemSelecionadoId = Uri.UnescapeDataString(value);
                CarregarPersonagem();
            }
        }

        public async void CarregarPersonagem()
        {
            if (string.IsNullOrEmpty(personagemSelecionadoId))
                return;

            try
            {
                Personagem p = await pService.GetPersonagemAsync(int.Parse(personagemSelecionadoId));

                Id = p.Id;
                Nome = p.Nome;
                PontosVida = p.PontosVida;
                Forca = p.Forca;
                Defesa = p.Defesa;
                Inteligencia = p.Inteligencia;
                Disputas = p.Disputas;
                Vitorias = p.Vitorias;
                Derrotas = p.Derrotas;

                TipoClasseSelecionado = ListaTiposClasse.FirstOrDefault(c => c.Id == (int)p.Classe);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        // ============================
        // VALIDAÇÃO
        // ============================

        public bool CadastroHabilitado => PontosVida > 0;

        public bool ValidarCampos()
        {
            return !string.IsNullOrWhiteSpace(Nome)
                && Forca > 0
                && Defesa > 0
                && TipoClasseSelecionado != null
                && CadastroHabilitado;
        }
    }
}
