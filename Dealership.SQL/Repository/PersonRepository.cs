using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dealership.SQL.Common;
using Dealership.SQL.Extensions;
using Dealership.SQL.Models.PersonModel;
using Dealership.SQL.Models.PersonModel.Buyer;

namespace Dealership.SQL.Repository
{
    public class PersonRepository
    {
        public List<IPerson> GetAllUsers()
        {
            List<IPerson> tempListPerson = new List<IPerson>();
            string Querry = "SELECT u.id,u.Famaly || ' ' ||u.Name ||' '|| u.Surname FIO,u.AccessID,u.DateOfBirthday FROM Users u";

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
    }
}
