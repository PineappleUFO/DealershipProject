﻿using System.Windows.Input;
using Dealership.UI.MVVM;
using Dealership.UI.View.HomeView;
using Dealership.UI.ViewModel.Commands;

namespace Dealership.UI.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set => SetProperty(ref _selectedViewModel, value);
        }
        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel()
        {
            SelectedViewModel = new HomeViewModel();
            UpdateViewCommand = new UpdateViewCommand(this);
        }
    }
}
