using AppRpgEtec.ViewModels.Disputas;

namespace AppRpgEtec.Views.Disputas;

public partial class ListagemView : ContentPage
{
    DisputaViewModel _viewModel;
    public ListagemView()
    {
        InitializeComponent();

        _viewModel = new DisputaViewModel();
        BindingContext = _viewModel;
    }

}
