namespace Dealership.SQL.Models.CarModel.Equipment
{
    public interface IExtras : IEntity
    {
        /// <summary>
        /// Мультимедийная система на Андроид
        /// </summary>
        bool AndroidSystem { get; set; }

        /// <summary>
        /// Литые диски
        /// </summary>
        bool CastWheel { get; set; }

        /// <summary>
        /// Круиз контроль
        /// </summary>
        bool CruiseControl { get; set; }
    }
}
