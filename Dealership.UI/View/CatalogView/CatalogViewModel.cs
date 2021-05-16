using System.Collections.Generic;
using System.Configuration;
using System.Windows.Documents;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Repository;
using Dealership.UI.MVVM;
using Dealership.UI.ViewModel;

namespace Dealership.UI.View.CatalogView
{
    public class CatalogViewModel : BaseViewModel
    {
        private  List<ICar> _allCarsCollection;

        public List<ICar> AllCarsCollection
        {
            get => _allCarsCollection;
            set => SetProperty(ref _allCarsCollection, value);
        }
    

        public CatalogViewModel()
        {
            CarRepository carRepository = new CarRepository();
            AllCarsCollection = carRepository.GetAllCars();
        }


    }
}
