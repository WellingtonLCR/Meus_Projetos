using MauiAppFit.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiAppFit.ViewModels
{
    internal class ListaAtividadesViewModel : INotifyPropertyChanged
    {
        ObservableCollection<Atividade> listaAtividades = new ObservableCollection<Atividade>();
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public string ParametrosBusca { get; set; }

        bool estaAtualizando = false;

        

        public bool EstaAtualizando
        {
            get => EstaAtualizando;
            set
            {
                estaAtualizando = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EstaAtualizado"));
            }
        }
       

        public ObservableCollection<Atividade> ListaAtividades
        {
            get => listaAtividades;
            set => listaAtividades = value;
        }
        public ICommand AtualizarLista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;
                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.GetAllRows();

                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
        public ICommand Buscar
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;
                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.Search(ParametrosBusca);

                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
        public ICommand AbrirDetalhesAtividade
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    await Shell.Current.GoToAsync($"//CadastroAtividade?Atividade?parametro_id={id}");
                });
            }
        }
        public ICommand Remover
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    try
                    {
                        bool conf = await Shell.Current.DisplayAlert("Confirmação", "Deseja remover a atividade selecionada?", "Sim", "Não");
                        if (conf)
                        {
                            await App.Database.Delete(id);
                            AtualizarLista.Execute(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
    }
}



    
