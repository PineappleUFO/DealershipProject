using System;

namespace Dealership.SQL.Models.TransactionModel
{
    public class TransactionModel : ITransaction
    {
        public long ID { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public long TransactionCost { get; set; }
        public string TransactionDate { get; set; }
        public string EmployeeName { get; set; }
    }
}
