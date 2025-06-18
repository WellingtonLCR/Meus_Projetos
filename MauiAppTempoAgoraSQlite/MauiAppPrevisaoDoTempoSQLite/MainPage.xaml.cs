using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MauiAppPrevisaodoTempo.Data;
using MauiAppPrevisaodoTempo.Services;
using PrevisaoDoTempoApp.Models;
using Microsoft.Maui.Controls;
using System.Data;

namespace MauiAppPrevisaodoTempo
{
    public partial class MainPage : ContentPage
    {
        private readonly ServicoPrevisaoTempo _servicoPrevisaoTempo;
        public ObservableCollection<PesquisaPrevisaoTempo> Consultas { get; set; }

        public MainPage()
        {
            InitializeComponent();
            _servicoPrevisaoTempo = new ServicoPrevisaoTempo(App.Database);
            Consultas = new ObservableCollection<PesquisaPrevisaoTempo>();
            BindingContext = this;
            CarregarConsultas();
        }

        private async void CarregarConsultas()
        {
            var consultas = await App.Database.GetConsultasAsync();
            Consultas.Clear();
            foreach (var consulta in consultas.OrderByDescending(c => c.DataPesquisa))
            {
                Consultas.Add(consulta);
            }
        }

        private async void OnPesquisarClicked(object sender, EventArgs e)
        {
            string cidade = cidadeEntry.Text; // Obtém o valor do campo de entrada
            var previsao = await _servicoPrevisaoTempo.ObterPrevisao(cidade);
            if (previsao != null)
            {
                cidadeLabel.Text = previsao.Cidade;
                temperaturaLabel.Text = $"{previsao.Temperatura}°C";
                dataLabel.Text = previsao.Data.ToString("dd/MM/yyyy HH:mm");

                CarregarConsultas();
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível obter a previsão do tempo. Verifique o nome da cidade e tente novamente.", "OK");
            }
        }
    }
}