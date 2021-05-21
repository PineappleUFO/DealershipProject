using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dealership.SQL.Models.CarModel.Equipment;

namespace Dealership.SQL.Models.CarModel
{
    public interface ICar
    {
         long  ID { get; set; }
        /// <summary>
        /// Характеристики и комплектация
        /// </summary>
        IEquipment Equipment { get; set; }

        string Name { get; set; }
        /// <summary>
        /// Стоимость
        /// </summary>
        long Cost { get; set; }

        /// <summary>
        /// Машин в налии
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// Цвет
        /// </summary>
        ColorStruct Color { get; set; }

        BitmapImage Photo { get; set; }

    }
}
