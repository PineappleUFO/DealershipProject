using System.Collections.Generic;
using Dealership.SQL.Models.PersonModel;
using Dealership.SQL.Models.PersonModel.Buyer;
using Dealership.SQL.Repository;
using Dealership.UI.MVVM;
using Dealership.UI.ViewModel;

namespace Dealership.UI.View.UserView
{
    public class UserViewModel : BaseViewModel
    {
        public string Title => "UserViewModel";

        public List<IPerson> BuyerModels { get; set; }
       
        public UserViewModel()
        {
            PersonRepository personRepository = new PersonRepository();
            BuyerModels = personRepository.GetAllUsers();

        }
    }
}
