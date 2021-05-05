using System.Collections.Generic;
using Dealership.SQL.Models.CarModel;

namespace Dealership.SQL.Models.PersonModel.Buyer
{
    public interface IBuyer : IPerson
    {
        /// <summary>
        /// Баланс
        /// </summary>
        long MoneyCount { get; set; }

        /// <summary>
        /// Приобретенные машины
        /// </summary>
        IEnumerable<ICar> CarsBought { get; set; }

        IEnumerable<ICar> GetCarBought();
    }
}
