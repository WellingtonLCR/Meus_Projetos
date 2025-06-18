using MauiAppFit.ViewModels;
using System.Runtime.Intrinsics.X86;

namespace MauiAppFit.Views;

public partial class CadastroAtividade : ContentPage
{
	public CadastroAtividade()
	{
		InitializeComponent();

		BindingContext = new CadastroAtividadeViewModel();
	}
	protected override async void OnAppearing()
	{
		var vm = (CadastroAtividadeViewModel)BindingContext;
		if (vm.Id == 0)
		{
			vm.NovaAtividade.Execute(null);
		}
	}
}