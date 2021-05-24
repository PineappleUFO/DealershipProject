using System;
using System.Windows.Input;
using Dealership.SQL.Models.PersonModel;
using Dealership.UI.View;
using Dealership.UI.View.CatalogView;
using Dealership.UI.View.HomeView;
using Dealership.UI.View.OrderView;
using Dealership.UI.View.UserView;
using Dealership.UI.ViewModel;

namespace Dealership.UI.MVVM.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;
        private EnumSelectedPerson _person;
        private IPerson currentUser;

        public UpdateViewCommand(MainViewModel viewModel,EnumSelectedPerson person,IPerson user)
        {
            currentUser = user;
            _person = person;
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
                    viewModel.SelectedViewModel = new OrderViewModel(_person,currentUser);
                    break;
                case "Catalog":
                    viewModel.SelectedViewModel = new CatalogViewModel(_person,currentUser);
                    break;
                case "User":
                    viewModel.SelectedViewModel = new UserViewModel();
                    break;
            }
        }
    }
}
