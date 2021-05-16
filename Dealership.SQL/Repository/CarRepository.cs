using Dealership.SQL.Common;
using Dealership.SQL.Extensions;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Models.CarModel.Equipment;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;

namespace Dealership.SQL.Repository
{
    public class CarRepository
    {
        public List<ICar> GetAllCars()
        {
            List<ICar> tempListCars = new List<ICar>();
            string Querry =
            @"SELECT carCo.*,car.name as carName,car.id as carID,car.price,car.count,carEn.ID as carEnID,carEn.Name as carEnName,carEn.Power,carEn.Expenditure,carEx.ID as carExID,carEx.AndroidSystem,carEx.CastWheel,carEx.CruiseControl
            FROM Cars car,CarColors carCo, CarExtra carEx,CarsEngine carEn, Cars_Models carMo
            WHERE
            car.ModelID = carMo.ID AND
            car.ColorID = carCo.ID AND
            car.EngineID = carEn.ID AND
            car.ExtrasID = carEx.ID
            GROUP by car.ID";

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
                            {
                                var carModel = new CarModel();

                                carModel.Color = new ColorStruct()
                                {
                                    ColorName = reader["ColorName"].ToString(),
                                    ColorCode = reader["ColorCode"].ToString()
                                };
                                carModel.Cost = Convert.ToInt64(reader["Price"]);
                                carModel.Count = Convert.ToInt32(reader["Count"]);
                                carModel.ID = Convert.ToInt64(reader["carID"]);
                                carModel.Name = reader["carName"].ToString();
                                carModel.Equipment = EquipmentModelLoad(reader);


                                tempListCars.Add(carModel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e.FromMethod("Не удалось получить коллекцию машин из БД", MethodBase.GetCurrentMethod());
            }

            return tempListCars;
        }

        /// <summary>
        /// Получаем с БД оборудование и доп.опции и возвращаем готовую модель
        /// </summary>
        private EquipmentModel EquipmentModelLoad(SQLiteDataReader reader)
        {
            if (!reader.HasRows) return null;

            EquipmentModel equipment = new EquipmentModel
            {
                Engine = new EngineModel()
                {
                    ID = Convert.ToInt64(reader["carEnID"]),
                    Name = reader["carEnName"].ToString(),
                    Power = Convert.ToDouble(reader["Power"]),
                    Expenditure = Convert.ToDouble(reader["Expenditure"])
                },
                Extras = new ExtrasModel()
                {
                    ID = Convert.ToInt64(reader["carExID"]),
                    AndroidSystem = Convert.ToBoolean(reader["AndroidSystem"]),
                    CastWheel = Convert.ToBoolean(reader["CastWheel"]),
                    CruiseControl = Convert.ToBoolean(reader["CruiseControl"])
                }
            };

            return equipment;
        }
    }
}
