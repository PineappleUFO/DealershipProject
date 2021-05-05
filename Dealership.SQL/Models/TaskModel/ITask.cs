using System;

namespace Dealership.SQL.Models.TaskModel
{
    public interface ITask
    {
        /// <summary>
        /// Имя задачи
        /// </summary>
        string TaskName { get; set; }

        /// <summary>
        /// Тип задачи
        /// </summary>
        TaskTypes TaskType { get; set; }

        /// <summary>
        /// Дата начала задачи
        /// </summary>
        DateTime DateStart { get; set; }

        /// <summary>
        /// Дата завершения задачи
        /// </summary>
        DateTime DateEnd { get; set; }

        /// <summary>
        /// Подробное описание задачи
        /// </summary>
        string Detail { get; set; }

    }
}
