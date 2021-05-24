using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Models.TransactionModel;

namespace Dealership.SQL.Models.OrdersModels
{
    public interface IOrderModel
    {
        long ID { get; set; }
        ITransaction Transaction { get; set; }

        ICar Car { get; set; }
    }

    public class OrderModel : IOrderModel
    {
        public long ID { get; set; }
        public ITransaction Transaction { get; set; }
        public ICar Car { get; set; }
    }
}
