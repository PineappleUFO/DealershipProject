using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Documents;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Models.PersonModel;
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
        public DelegateCommand DeleteCar { get; set; }

        public DelegateCommand AddCarInCart { get; set; }

        readonly CarRepository carRepository = new CarRepository();


        public Visibility ManagerVisible { get; set; }
        private IPerson CurrentUser;

        public CatalogViewModel(EnumSelectedPerson person, IPerson currentUser)
        {

            CurrentUser = currentUser;
            AllCarsCollection = carRepository.GetAllCars();

            //Commands
            OpenEditCarView = new DelegateCommand(OpenEditCarAction);
            OpenAddCarView = new DelegateCommand(OpenAddCarAction);
            DeleteCar = new DelegateCommand(DeleteCarAction);
            AddCarInCart = new DelegateCommand(AddCarInCartAction);

            ManagerVisible = person == EnumSelectedPerson.Client ? Visibility.Collapsed : Visibility.Visible;

        }

        private void AddCarInCartAction(object obj)
        {
            if (obj is ICar currentCar)
            {
                if (currentCar.Count == 0)
                {
                    MessageBox.Show($"Авто {currentCar.Name} в данный момент не в наличии, позвоните менеджеру что бы узнать альтернативные предложения");
                    return;
                }
                if (carRepository.AddCarInCart(currentCar, CurrentUser))
                {
                    MessageBox.Show("Авто успешно добавлено в корзину и зарезервированно для заказа");
                    AllCarsCollection = carRepository.GetAllCars();
                }
            }
        }

        private void DeleteCarAction(object obj)
        {
            if (SelectedCar != null)
            {
                var mBoxResult = MessageBox.Show($"Вы действительно хотите удалить машину :{SelectedCar.Name}","Внимание",MessageBoxButton.YesNo,MessageBoxImage.Warning);
                if (mBoxResult == MessageBoxResult.Yes)
                {
                    carRepository.DeleteCarFromBd(SelectedCar);
                    AllCarsCollection = carRepository.GetAllCars();
                }
            }
        }

        private void OpenAddCarAction(object obj)
        {
            CatalogEditOrAddView catalogEditOrAddView = new CatalogEditOrAddView();
            catalogEditOrAddView.DataContext = new CatalogEditOrAddViewModel();
            catalogEditOrAddView.Show();
            catalogEditOrAddView.Closed += (sender, args) =>
            {
                AllCarsCollection = carRepository.GetAllCars();
            };
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
