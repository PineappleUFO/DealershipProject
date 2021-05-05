using Dealership.UI.View.HomeView;
using Dealership.UI.ViewModel;
using Dealership.UI.ViewModel.Commands;
using System.Windows.Input;

namespace Dealership.UI.View.MenuView
{

    /// <summary>
    /// Вью модель для навигации 
    /// </summary>
    public class MenuViewModel : BaseViewModel
    {

        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        public ICommand UpdateViewCommand { get; set; }


        public MenuViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            //View по умолчанию
            SelectedViewModel = new HomeViewModel();
        }
    }
}
