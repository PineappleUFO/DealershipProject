using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership.SQL.Models.CarModel.Equipment
{
    public class EquipmentModel : IEquipment
    {
        public EngineModel Engine { get; set; }
        public ExtrasModel Extras { get; set; }

        public IEquipment GetEquipment()
        {
            throw new NotImplementedException();
        }
    }
}
