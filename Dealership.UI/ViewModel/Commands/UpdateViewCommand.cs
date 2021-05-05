using Dealership.UI.View.CatalogView;
using Dealership.UI.View.HomeView;
using Dealership.UI.View.OrderView;
using System;
using System.Windows.Input;
using Dealership.UI.View.UserView;

namespace Dealership.UI.ViewModel.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Home":
                    viewModel.SelectedViewModel = new HomeViewModel();
                    break;
                case "Order":
                    viewModel.SelectedViewModel = new OrderViewModel();
                    break;
                case "Catalog":
                    viewModel.SelectedViewModel = new CatalogViewModel();
                    break;
                case "User":
                    viewModel.SelectedViewModel = new UserViewModel();
                    break;
            }
        }
    }
}
