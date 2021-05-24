using System.Collections.Generic;
using System.Windows;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Models.OrdersModels;
using Dealership.SQL.Models.PersonModel;
using Dealership.SQL.Repository;
using Dealership.UI.MVVM;
using Dealership.UI.MVVM.Commands;
using Dealership.UI.ViewModel;

namespace Dealership.UI.View.OrderView
{
    public class OrderViewModel : BaseViewModel
    {
        private List<ICar> _cartCars;
 

        public List<ICar> CartCars
        {
            get => _cartCars;
            set => SetProperty(ref _cartCars , value);
        }

        public List<IOrderModel> ListOrders
        {
            get => _listOrders;
            set => SetProperty(ref _listOrders , value);
        }


        readonly PersonRepository personRepository = new PersonRepository();

        private IPerson _currentUser;
        private List<IOrderModel> _listOrders;
        public DelegateCommand DeleteFromCart { get; set; }
        public DelegateCommand AddCarInOrder { get; set; }

        public Visibility MangerVisibility { get; set; }
        public OrderViewModel(EnumSelectedPerson person, IPerson currentUser)
        {
            _currentUser = currentUser;
            if (person == EnumSelectedPerson.Client)
            {
                CartCars = personRepository.GetCarCartPerson(currentUser);
                ListOrders = personRepository.GetAllOrders(currentUser);
            }
            else
            {
                MangerVisibility = Visibility.Collapsed;
            }

            DeleteFromCart = new DelegateCommand(DeleteFromCartAction);
            AddCarInOrder = new DelegateCommand(AddCarInOrderAction);
           
        }

        private void AddCarInOrderAction(object obj)
        {
            if (obj is CarModel car)
            {
                var resultDialog = MessageBox.Show($"Оформить заказ на {car.Name} ?", "Оформление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultDialog == MessageBoxResult.Yes)
                {
                    if(personRepository.AddCarInOrder(_currentUser, car))
                    {
                       MessageBox.Show($"Заказ на {car.Name} успешно оформлен");
                       CartCars = personRepository.GetCarCartPerson(_currentUser);
                       ListOrders = personRepository.GetAllOrders(_currentUser);
                    }

                }
            }
        }

        private void DeleteFromCartAction(object obj)
        {
            if (obj is CarModel car)
            {
                var resultDialog = MessageBox.Show($"Убрать {car.Name} из корзины?","Внимание",MessageBoxButton.YesNo,MessageBoxImage.Question);

                if (resultDialog == MessageBoxResult.Yes)
                {
                    if(personRepository.DeleteCarFromCart(_currentUser, car))
                    {

                        MessageBox.Show($"Авто {car.Name} убрано из резерва");
                        CartCars = personRepository.GetCarCartPerson(_currentUser);
                    }
                }
            }
        }
    }
}
