using Dealership.SQL.Common;
using Dealership.SQL.Extensions;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Models.OrdersModels;
using Dealership.SQL.Models.PersonModel;
using Dealership.SQL.Models.PersonModel.Buyer;
using Dealership.SQL.Models.PersonModel.Employee;
using Dealership.SQL.Models.TransactionModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Documents;

namespace Dealership.SQL.Repository
{
    public class PersonRepository
    {
        public List<IPerson> GetAllUsers()
        {
            List<IPerson> tempListPerson = new List<IPerson>();
            string Querry = "SELECT  u.FIO,u.DateOfBirthday FROM Employeers u";

            try
            {
                using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
                {
                    Connection.Open();
                    using (var command = new SQLiteCommand(Querry, Connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                tempListPerson.Add(new BuyerModel() { ID = Convert.ToInt64(reader["ID"]), FullName = reader["FIO"].ToString(), DateOfBirthdate = reader["DateOfBirthday"].ToString() });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e.FromMethod("Не удалось получить пользователей из БД", MethodBase.GetCurrentMethod());
            }

            return tempListPerson;
        }

        public IBuyer CurrentUser()
        {
            IBuyer buyer = new BuyerModel();

            try
            {
                using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
                {
                    Connection.Open();
                    string Querry = "Select * FROM Buyers Where ID = 1";
                    using (var command = new SQLiteCommand(Querry, Connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                buyer.ID = Convert.ToInt64(reader["id"]);
                                buyer.FullName = reader["fio"].ToString();
                                buyer.DateOfBirthdate = reader["DateOfBirthday"].ToString();
                                buyer.Phone = reader["phone"].ToString();
                                buyer.MoneyCount = Convert.ToInt64(reader["MoneyCount"]);

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e.FromMethod("Не удалось получить пользователей из БД", MethodBase.GetCurrentMethod());
            }

            return buyer;
        }

        public List<ICar> GetCarCartPerson(IPerson person)
        {
            List<ICar> tempListCars = new List<ICar>();
            string Querry = $"Select * from cars c,cart ct where ct.id_Car = c.id and ct.Buyer_ID = {person.ID}";

            try
            {
                using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
                {
                    Connection.Open();
                    using (var command = new SQLiteCommand(Querry, Connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                tempListCars.Add(new CarModel() { ID = Convert.ToInt64(reader["ID"]), Name = reader["name"].ToString(), Cost = Convert.ToInt64(reader["Price"]) });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e.FromMethod("Не удалось получить корзину пользователя из БД", MethodBase.GetCurrentMethod());
            }

            return tempListCars;
        }


        public bool DeleteCarFromCart(IPerson person, ICar car)
        {
            bool result = false;
            if (car != null)
            {
                using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
                {
                    Connection.Open();
                    string commandText = $"DELETE FROM Cart WHERE Buyer_ID = {person.ID} and ID_car = {car.ID}";
                    using (var Command = new SQLiteCommand(commandText, Connection))
                    {
                        var returnCountRow = Command.ExecuteNonQuery();
                        result = returnCountRow > 0;
                    }


                }
            }

            return result;
        }

        public bool AddCarInOrder(IPerson person, ICar car)
        {
            bool result = false;
            if (car != null)
            {
                using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
                {
                    Connection.Open();

                    //1.Вычисляем сотрудника для привязки к заказу
                    string GetMinTaskEmployee = @"select min(countTask) countTask,id,fio FROM
                                                (Select count(e.ID) as countTask,e.fio,e.id 
                                                FROM Employeers e, EmployeesTasks t 
                                                WHERE e.PostID = 2 AND e.id=t.EmployeeID
                                                GROUP by e.id) LIMIT 1;";
                    EmployeeModel employeeModel = new EmployeeModel();
                    using (var Command = new SQLiteCommand(GetMinTaskEmployee, Connection))
                    {
                        using (var reader = Command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employeeModel.ID = Convert.ToInt64(reader["id"]);
                                employeeModel.FullName = reader["fio"].ToString();
                            }


                        }
                    }


                    //2.Генерируем сотруднику задачу
                    string addTaskEmployee = "INSERT INTO Tasks ([ID],[TaskName], [Details], [TaskTypeID],[Datestart]) VALUES(@ID,@TaskName, @Details, @TaskTypeID, @Datestart)";
                    var newTaskId = GetNewTaskID();
                    using (var Command = new SQLiteCommand(addTaskEmployee, Connection))
                    {
                        Command.Parameters.AddWithValue("@ID", newTaskId);
                        Command.Parameters.AddWithValue("@TaskName", $"Оформление заказа на авто: {car.Name}");
                        Command.Parameters.AddWithValue("@Details", $"Покупатель {person.FullName}");
                        Command.Parameters.AddWithValue("@TaskTypeID", "4");
                        Command.Parameters.AddWithValue("@Datestart", DateTime.Now.ToString("g"));


                        if (Command.ExecuteNonQuery() == 0)
                        {
                            Command.Transaction.Rollback();
                            throw new Exception().FromMethod("Произошла ошибка при генерации задачи для сотрудника", MethodBase.GetCurrentMethod(), person, car);
                        }
                    }

                    //3.Связываем задачу и сотрудника
                    string addTaskEmployeeLink = "INSERT INTO EmployeesTasks ([EmployeeID], [TaskID]) VALUES(@EmployeeID, @TaskID)";
                    using (var Command = new SQLiteCommand(addTaskEmployeeLink, Connection))
                    {
                        Command.Parameters.AddWithValue("@EmployeeID", employeeModel.ID);
                        Command.Parameters.AddWithValue("@TaskID", newTaskId);

                        if (Command.ExecuteNonQuery() == 0)
                        {
                            Command.Transaction.Rollback();
                            throw new Exception().FromMethod("Произошла ошибка при связывании задачи для сотрудника", MethodBase.GetCurrentMethod(), person, car);
                        }
                    }

                    //4.Делаем транзакцию
                    var IdTransaction = GetNewTransactionID();
                    string AddTransaction = "INSERT INTO Transactions ([ID], [TransactionType], [TransactionCost], [TransactionDate], [EmployeeID], [BuyerID]) VALUES(@ID, @TransactionType, @TransactionCost, @TransactionDate, @EmployeeID, @BuyerID)";
                    using (var Command = new SQLiteCommand(AddTransaction, Connection))
                    {
                        Command.Parameters.AddWithValue("@ID", IdTransaction);
                        Command.Parameters.AddWithValue("@TransactionType", "2");
                        Command.Parameters.AddWithValue("@TransactionCost", car.Cost);
                        Command.Parameters.AddWithValue("@TransactionDate", DateTime.Now.ToString("g"));
                        Command.Parameters.AddWithValue("@EmployeeID", employeeModel.ID);
                        Command.Parameters.AddWithValue("@BuyerID", person.ID);

                        if (Command.ExecuteNonQuery() == 0)
                        {
                            Command.Transaction.Rollback();
                            throw new Exception().FromMethod("Произошла ошибка при генерации транзакции", MethodBase.GetCurrentMethod(), person, car);
                        }
                    }

                    //5.Добавляем в таблицу заказов
                    string AddOtder = "INSERT INTO Orders ([Car_ID], [Transaction_ID]) VALUES(@Car_ID, @Transaction_ID)";
                    using (var Command = new SQLiteCommand(AddOtder, Connection))
                    {
                        Command.Parameters.AddWithValue("@Car_ID", car.ID);
                        Command.Parameters.AddWithValue("@Transaction_ID", IdTransaction);
                        Command.ExecuteNonQuery();
                    }


                }

                //6.Удаляем из корзины
                DeleteCarFromCart(person, car);
            }

            return true;
        }


        public List<IOrderModel> GetAllOrders(IPerson person)
        {
            List<IOrderModel> orderModels = new List<IOrderModel>();
           
            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
                string commandText = $@"SELECT t.id,t.TransactionCost,t.TransactionDate,e.fio,c.name 
                                        from Transactions t,Orders o,Employeers e,Cars c 
                                        WHERE c.id = o.Car_ID AND o.Transaction_ID = t.id and t.EmployeeID = e.id
                                        and t.BuyerID = {person.ID}";

                using (var Command = new SQLiteCommand(commandText, Connection))
                {

                    using (var reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderModel orderModel = new OrderModel();
                            orderModel.ID = Convert.ToInt64(reader["id"]);
                            orderModel.Transaction = new TransactionModel()
                            {
                                EmployeeName = reader["FIO"].ToString(),
                                TransactionDate = reader["TransactionDate"].ToString(),
                                TransactionCost = Convert.ToInt64(reader["TransactionCost"])
                            };
                            orderModel.Car = new CarModel(){Name = reader["name"].ToString()};
                            
                            orderModels.Add(orderModel);
                        }


                    }

                 
                }
            }

            return orderModels;
        }


        private long GetNewTransactionID()
        {

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
                string commandText = "select seq from sqlite_sequence s where s.name = 'Transactions'";
                using (var Command = new SQLiteCommand(commandText, Connection))
                {
                    return Convert.ToInt64(Command.ExecuteScalar()) + 1;
                }
            }
        }

        private long GetNewTaskID()
        {

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
                string commandText = "select seq from sqlite_sequence s where s.name = 'Tasks'";
                using (var Command = new SQLiteCommand(commandText, Connection))
                {
                    return Convert.ToInt64(Command.ExecuteScalar()) + 1;
                }
            }
        }

    }
}
