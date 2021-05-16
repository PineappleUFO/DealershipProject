namespace Dealership.SQL.Models.CarModel.Equipment
{
    public class ExtrasModel : IExtras
    {
        public long ID { get; set; }
        public bool AndroidSystem { get; set; }
        public bool CastWheel { get; set; }
        public bool CruiseControl { get; set; }
    }
}
