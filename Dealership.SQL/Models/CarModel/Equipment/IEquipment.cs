namespace Dealership.SQL.Models.CarModel.Equipment
{
    public interface IEquipment : IEntity, IEngine, IExtras
    {
        /// <summary>
        /// Двигатель
        /// </summary>
        IEngine Engine { get; set; }

        /// <summary>
        /// Комплектация
        /// </summary>
        IExtras Extras { get; set; }

        IEquipment GetEquipment();
    }
}
