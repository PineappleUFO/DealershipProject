namespace Dealership.SQL.Models.CarModel.Equipment
{
    public interface IEquipment 
    {
        /// <summary>
        /// Двигатель
        /// </summary>
        EngineModel Engine { get; set; }

        /// <summary>
        /// Комплектация
        /// </summary>
        ExtrasModel Extras { get; set; }

        IEquipment GetEquipment();
    }
}
