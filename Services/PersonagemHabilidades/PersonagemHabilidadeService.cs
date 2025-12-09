using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRpgEtec.Models;
using AppRpgEtec.Services;

namespace AppRpgEtec.Services.PersonagemHabilidades
{
    public class PersonagemHabilidadeService : Request
    {
        private readonly Request _request = null;
        private readonly string _token;

        private const string _apiUrlBase = "https://rpgraphael-dvejarfmgyfycwba.brazilsouth-01.azurewebsites.net/PersonagemHabilidades/"; //meu azure nao ta funcionando, peguei o do colega

        public PersonagemHabilidadeService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<PersonagemHabilidade>>
            GetPersonagemHabilidadesAsync(int personagemId)
        {
            string url = $"{_apiUrlBase}{personagemId}";

            return await _request.GetAsync<ObservableCollection<PersonagemHabilidade>>
                (url, _token);
        }

        public async Task<ObservableCollection<Habilidade>>
            GetHabilidadesAsync()
        {
            string url = $"{_apiUrlBase}GetHabilidades";

            return await _request.GetAsync<ObservableCollection<Habilidade>>
                (url, _token);
        }
    }
}
