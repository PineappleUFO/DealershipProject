using Dealership.SQL.Models.CarModel.Equipment;
using System;

namespace Dealership.SQL.Models.CarModel
{
    public class CarModel : ICar
    {
        public long ID { get; set; }
        public IEquipment GetEquipment()
        {
            throw new NotImplementedException();
        }

        public IEquipment Equipment { get; set; }
        public long Cost { get; set; }
        public int Count { get; set; }
    }
}
