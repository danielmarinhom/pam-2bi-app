using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Models;

namespace AppRpgEtec.Services
{
    public class DisputaService
    {
        private readonly Request _request;
        private string _token;

        private const string _apiUrlBase = "https://xyz/Disputa";

        public DisputaService(string token)
        {
            _request = new Request();
            _token = token;
        }
        public async Task<Disputa> PostDisputaComArmaAsync(Disputa d)
        {
            string urlComplementar = "/Arma";
            return await _request.PostAsync(_apiUrlBase + urlComplementar, d, _token);
        }public async Task<Disputa> PostDisputaComHabilidadesAsync(Disputa d)
        {
            string urlComplementar = "/Habilidade";            return await _request.PostAsync(_apiUrlBase + urlComplementar, d, _token);
        }
        public async Task<Disputa> PostDisputaGeralAsync(Disputa d)
        {
            string urlComplementar = "/DisputaEmGrupo";
            return await _request.PostAsync(_apiUrlBase + urlComplementar, d, _token);
        }

    }

}
