//Daniel Marinho
using AppRpgEtec.Models;
using AppRpgEtec.Services;
using AppRpgEtec.Services.Personagens;
using AppRpgEtec.Services.PersonagemHabilidades;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Disputas
{
    public class DisputaViewModel : BaseViewModel
    {
        private readonly PersonagemService pService;
        private readonly DisputaService dService;
        private readonly PersonagemHabilidadeService phService;

        public ObservableCollection<Personagem> PersonagensEncontrados { get; set; }
        public ObservableCollection<PersonagemHabilidade> Habilidades { get; set; }

        public Personagem Atacante { get; set; }
        public Personagem Oponente { get; set; }
        public Disputa DisputaPersonagens { get; set; }

        public ICommand PesquisarPersonagensCommand { get; set; }
        public ICommand DisputaComArmaCommand { get; set; }
        public ICommand DisputaComHabilidadeCommand { get; set; }
        public ICommand DisputaGeralCommand { get; set; }

        private Personagem personagemSelecionado;
        private PersonagemHabilidade habilidadeSelecionada;
        private string textoBuscaDigitado = string.Empty;

        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);

            pService = new PersonagemService(token);
            dService = new DisputaService(token);
            phService = new PersonagemHabilidadeService(token);

            Atacante = new Personagem();
            Oponente = new Personagem();
            DisputaPersonagens = new Disputa();

            PersonagensEncontrados = new ObservableCollection<Personagem>();

            PesquisarPersonagensCommand = new Command<string>(async s => await PesquisarPersonagens(s));
            DisputaComArmaCommand = new Command(async () => await ExecutarDisputaArmada());
            DisputaComHabilidadeCommand = new Command(async () => await ExecutarDisputaHabilidades());
            DisputaGeralCommand = new Command(async () => await ExecutarDisputaGeral());
        }

        public PersonagemHabilidade HabilidadeSelecionada
        {
            get => habilidadeSelecionada;
            set
            {
                if (value == null) return;

                habilidadeSelecionada = value;
                OnPropertyChanged();
            }
        }

        public Personagem PersonagemSelecionado
        {
            set
            {
                if (value == null) return;

                personagemSelecionado = value;
                SelecionarPersonagem(personagemSelecionado);
                OnPropertyChanged();
                PersonagensEncontrados.Clear();
            }
        }

        public string TextoBuscaDigitado
        {
            get => textoBuscaDigitado;
            set
            {
                textoBuscaDigitado = value;
                OnPropertyChanged();

                if (!string.IsNullOrWhiteSpace(textoBuscaDigitado))
                    _ = PesquisarPersonagens(textoBuscaDigitado);
                else
                    PersonagensEncontrados.Clear();
            }
        }

        public string DescricaoPersonagemAtacante => Atacante.Nome;
        public string DescricaoPersonagemOponente => Oponente.Nome;

        public async Task ObterHabilidadesAsync(int personagemId)
        {
            try
            {
                Habilidades = await phService.GetPersonagemHabilidadesAsync(personagemId);
                OnPropertyChanged(nameof(Habilidades));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private async Task ExecutarDisputaHabilidades()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;
                DisputaPersonagens.HabilidadeId = habilidadeSelecionada.HabilidadeId;

                DisputaPersonagens = await dService.PostDisputaComHabilidadesAsync(DisputaPersonagens);

                await Application.Current.MainPage.DisplayAlert("Resultado", DisputaPersonagens.Narracao, "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private async Task ExecutarDisputaArmada()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;

                DisputaPersonagens = await dService.PostDisputaComArmaAsync(DisputaPersonagens);

                await Application.Current.MainPage.DisplayAlert("Resultado", DisputaPersonagens.Narracao, "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private async Task ExecutarDisputaGeral()
        {
            try
            {
                var lista = await pService.GetPersonagensAsync();
                DisputaPersonagens.ListaIdPersonagens = lista.Select(x => x.Id).ToList();

                DisputaPersonagens = await dService.PostDisputaGeralAsync(DisputaPersonagens);

                string resultados = string.Join(" | ", DisputaPersonagens.Resultados);

                await Application.Current.MainPage.DisplayAlert("Resultado", resultados, "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task PesquisarPersonagens(string texto)
        {
            try
            {
                PersonagensEncontrados = await pService.GetByNomeAproximadoAsync(texto);
                OnPropertyChanged(nameof(PersonagensEncontrados));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async void SelecionarPersonagem(Personagem p)
        {
            try
            {
                string tipoCombatente = await Application.Current.MainPage.DisplayActionSheet(
                    "Atacante ou Oponente?", "Cancelar", "", "Atacante", "Oponente");

                if (tipoCombatente == "Atacante")
                {
                    Atacante = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemAtacante));
                    await ObterHabilidadesAsync(p.Id);
                }
                else if (tipoCombatente == "Oponente")
                {
                    Oponente = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemOponente));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}