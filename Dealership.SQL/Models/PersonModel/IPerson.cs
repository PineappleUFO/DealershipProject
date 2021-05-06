using System;

namespace Dealership.SQL.Models.PersonModel
{
    public interface IPerson : IEntity
    {
        /// <summary>
        /// ФИО
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        string DateOfBirthdate { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        string Phone { get; set; }

        bool Equial(IPerson person);
    }
}
