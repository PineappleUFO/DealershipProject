using System.Collections.Generic;
using Dealership.SQL.Models.TaskModel;

namespace Dealership.SQL.Models.PersonModel.Employee
{
    interface IEmployee : IPerson
    {
        EnumPosts Post { get; set; }
        IEnumerable<ITask> Tasks { get; set; }

        IEnumerable<ITask> GetTasks();
    }
}
