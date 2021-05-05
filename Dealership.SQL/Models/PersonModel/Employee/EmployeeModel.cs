using Dealership.SQL.Models.TaskModel;
using System;
using System.Collections.Generic;

namespace Dealership.SQL.Models.PersonModel.Employee
{
    public class EmployeeModel : IEmployee
    {
        public long ID { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirthdate { get; set; }
        public string Phone { get; set; }
        public bool Equial(IPerson person)
        {
            return this.ID == person.ID;
        }

        public EnumPosts Post { get; set; }
        public IEnumerable<ITask> Tasks { get; set; }
        public IEnumerable<ITask> GetTasks()
        {
            throw new NotImplementedException();
        }
    }
}
