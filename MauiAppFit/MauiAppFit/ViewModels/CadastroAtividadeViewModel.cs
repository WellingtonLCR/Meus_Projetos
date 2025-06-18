using MauiAppFit.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiAppFit.ViewModels
{
    [QueryProperty("PegarIdDaNavegacao", "parametro_id")]

    public class CadastroAtividadeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string descricao, observacoes;
        int id;
        DateTime data;
        double? peso;

        public string PegarIdDaNavegacao
        {
            set
            {
                int id_parametro = Convert.ToInt32(Uri.EscapeDataString(value));

                VerAtividade.Execute(id_parametro);
            }
        }
        public string Descricao
        {
            get => descricao;
            set
            {
                descricao = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Descricao"));
            }
        }
        public string Observacoes
        {
            get => observacoes;
            set
            {
                observacoes = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Observacoes"));
            }
        }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }
        public DateTime Data
        {
            get => data;
            set
            {
                data = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Data"));
            }
        }
        public double? Peso
        {
            get => peso;
            set
            {
                peso = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Peso"));
            }
        }
        public ICommand NovaAtividade
        {
            get
            {
                return new Command(() =>
                {
                    Id = 0;
                    Descricao = string.Empty;
                    Data = DateTime.Now;
                    Peso = null;
                    Observacoes = string.Empty;
                });
            }
        }
        public ICommand SalvarAtividade
        { 
            get => new Command(async() =>
            {
                try
                {
                    Atividade model = new()
                    {
                        Descricao = this.Descricao,
                        Data = this.Data,
                        Peso = this.Peso,
                        Observacoes = this.Observacoes
                    };

                    if (this.Id == 0)
                    {
                        await App.Database.Insert(model);
                    }
                    else
                    {
                        model.Id = this.Id;
                        await App.Database.Update(model);
                    }
                    await Shell.Current.DisplayAlert("Sucesso", "Atividade salva com sucesso!", "OK");

                    await Shell.Current.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Erro", "Erro ao salvar atividade: " + ex.Message, "OK");
                }      
            });
        } // Fecha SalvarAtividade
        public ICommand VerAtividade
        {
            get => new Command<int>(async (id) =>
            {
                try
                {
                    Atividade model = await App.Database.GetById(id);
                    this.Id = model.Id;
                    this.Descricao = model.Descricao;
                    this.Data = model.Data;
                    this.Peso = model.Peso;
                    this.Observacoes = model.Observacoes;
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Erro", "Erro ao carregar atividade: " + ex.Message, "OK");
                }
            });
        } // Fecha VerAtividade
    } // Fecha classe CadastroAtividadeViewModel
} // Fecha namespace MauiAppFit.ViewModels
