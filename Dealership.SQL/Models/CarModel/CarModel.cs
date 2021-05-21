using Dealership.SQL.Models.CarModel.Equipment;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dealership.SQL.Models.CarModel
{
    public class CarModel : ICar
    {
        public long ID { get; set; }

        public string Name { get; set; }
        public IEquipment GetEquipment()
        {
            throw new NotImplementedException();
        }

        public IEquipment Equipment { get; set; }
        public long Cost { get; set; }
        public int Count { get; set; }
        public ColorStruct Color { get; set; }
        public BitmapImage Photo { get; set; }
    }
}
