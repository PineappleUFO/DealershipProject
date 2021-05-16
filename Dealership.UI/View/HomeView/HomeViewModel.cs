using Dealership.UI.ViewModel;
using System.Windows.Input;
using Dealership.UI.MVVM;

namespace Dealership.UI.View.HomeView
{
    public class HomeViewModel : BaseViewModel
    {
        public string Title => "Home";

        public ICommand NavigateCatalogCommand { get; }
    }
}
