using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Documents;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Repository;
using Dealership.UI.MVVM;
using Dealership.UI.MVVM.Commands;
using Dealership.UI.View.CatalogView.CatalogEditOrAdd;
using Dealership.UI.ViewModel;

namespace Dealership.UI.View.CatalogView
{
    public class CatalogViewModel : BaseViewModel
    {
        private  List<ICar> _allCarsCollection;
        private ICar _selectedCar;

        public List<ICar> AllCarsCollection
        {
            get => _allCarsCollection;
            set => SetProperty(ref _allCarsCollection, value);
        }

        public ICar SelectedCar
        {
            get => _selectedCar;
            set => SetProperty(ref _selectedCar, value);
        }

        public DelegateCommand OpenEditCarView { get; set; }
        public DelegateCommand OpenAddCarView { get; set; }
        public CatalogViewModel()
        {
            CarRepository carRepository = new CarRepository();
            AllCarsCollection = carRepository.GetAllCars();

            //Commands
            OpenEditCarView = new DelegateCommand(OpenEditCarAction);
            OpenAddCarView = new DelegateCommand(OpenAddCarAction);
        }

        private void OpenAddCarAction(object obj)
        {
            CatalogEditOrAddView catalogEditOrAddView = new CatalogEditOrAddView();
            catalogEditOrAddView.DataContext = new CatalogEditOrAddViewModel();
            catalogEditOrAddView.Show();
        }

        private void OpenEditCarAction(object obj)
        {
            if (SelectedCar == null)
            {
               MessageBox.Show("Выберите автомобиль для редактирования");
                return;
            }
            else
            {
                CatalogEditOrAddView catalogEditOrAddView = new CatalogEditOrAddView();
                catalogEditOrAddView.DataContext = new CatalogEditOrAddViewModel(SelectedCar);
                catalogEditOrAddView.Show();
            }
        }
    }
}
