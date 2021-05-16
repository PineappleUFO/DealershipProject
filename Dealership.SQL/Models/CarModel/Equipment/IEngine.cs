namespace Dealership.SQL.Models.CarModel.Equipment
{
    public interface IEngine:IEntity
    {
        /// <summary>
        /// Мощность двигателя
        /// </summary>
        double Power { get; set; }

        /// <summary>
        /// Расход на 100 км
        /// </summary>
        double Expenditure { get; set; }

        /// <summary>
        /// Наименование движка
        /// </summary>
        string Name { get; set; }
    }
}
