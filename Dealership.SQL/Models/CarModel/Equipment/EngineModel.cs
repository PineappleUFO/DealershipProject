
namespace Dealership.SQL.Models.CarModel.Equipment
{
    public class EngineModel : IEngine
    {
        public long ID { get; set; }
        public double Power { get; set; }
        public double Expenditure { get; set; }
        public string Name { get; set; }

    }
}
