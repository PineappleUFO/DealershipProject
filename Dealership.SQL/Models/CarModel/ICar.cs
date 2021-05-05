using Dealership.SQL.Models.CarModel.Equipment;

namespace Dealership.SQL.Models.CarModel
{
    public interface ICar
    {
        /// <summary>
        /// Характеристики и комплектация
        /// </summary>
        IEquipment Equipment { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        long Cost { get; set; }

        /// <summary>
        /// Машин в налии
        /// </summary>
        int Count { get; set; }
    }
}
