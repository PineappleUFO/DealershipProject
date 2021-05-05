using System;

namespace Dealership.SQL.Models.TransactionModel
{
    public interface ITransaction : IEntity
    {
        /// <summary>
        /// Тип транзакции
        /// </summary>
        TransactionTypes TransactionType { get; set; }

        /// <summary>
        /// Стоимость транзакции
        /// </summary>
        long TransactionCost { get; set; }
        /// <summary>
        /// Дата транзакции
        /// </summary>
        DateTime TransactionDate { get; set; }
    }
}

