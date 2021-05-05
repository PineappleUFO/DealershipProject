using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dealership.SQL.Models.CarModel;

namespace Dealership.SQL.Models.PersonModel.Buyer
{
    public class BuyerModel:IBuyer
    {
        public long ID { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirthdate { get; set; }
        public string Phone { get; set; }
        public long MoneyCount { get; set; }
        public IEnumerable<ICar> CarsBought { get; set; }
        public IEnumerable<ICar> GetCarBought()
        {
            throw new NotImplementedException();
        }
        public bool Equial(IPerson person)
        {
            return this.ID == person.ID;
        }
    }
}
