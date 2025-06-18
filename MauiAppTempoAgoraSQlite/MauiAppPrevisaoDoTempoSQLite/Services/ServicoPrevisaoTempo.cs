using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrevisaoDoTempoApp.Models;
using MauiAppPrevisaodoTempo.Data;

namespace MauiAppPrevisaodoTempo.Services
{
    public class ServicoPrevisaoTempo
    {
        private readonly HttpClient _httpClient;
        private readonly string appId = "6135072afe7f6cec1537d5cb08a5a1a2";
        private readonly string baseUrl = "https://api.openweathermap.org/data/2.5/weather";
        private readonly PrevisaoTempoDatabase _database;

        public ServicoPrevisaoTempo(PrevisaoTempoDatabase database)
        {
            _httpClient = new HttpClient();
            _database = database;
        }

        public async Task<PrevisaoTempo> ObterPrevisao(string cidade)
        {
            var url = $"{baseUrl}?q={cidade}&units=metric&appid={appId}";
            var response = await _httpClient.GetStringAsync(url);

            if (!string.IsNullOrEmpty(response))
            {
                var previsao = JsonConvert.DeserializeObject<PrevisaoTempo>(response);
                if (previsao != null)
                {
                    await _database.SaveConsultaAsync(new PesquisaPrevisaoTempo
                    {
                        Cidade = cidade,
                        DataPesquisa = DateTime.Now
                    });

                    previsao.Cidade = cidade;
                    previsao.Data = DateTime.Now;
                }
                return previsao;
            }

            return null;
        }
    }
}