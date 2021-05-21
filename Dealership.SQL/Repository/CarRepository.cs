using Dealership.SQL.Common;
using Dealership.SQL.Extensions;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Models.CarModel.Equipment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dealership.SQL.Repository
{
    public class CarRepository
    {
        public List<ICar> GetAllCars()
        {
            List<ICar> tempListCars = new List<ICar>();
            string Querry =
            @"SELECT carCo.*,carCo.ID as carCoID,car.name as carName,car.id as carID,car.price,car.count,carEn.ID as carEnID,carEn.Name as carEnName,carEn.Power,carEn.Expenditure,carEx.ID as carExID,carEx.AndroidSystem,carEx.CastWheel,carEx.CruiseControl
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
                                    ColorCode = reader["ColorCode"].ToString(),
                                    ID = Convert.ToInt64(reader["carCoID"])
                                };
                                carModel.Cost = Convert.ToInt64(reader["Price"]);
                                carModel.Count = Convert.ToInt32(reader["Count"]);
                                carModel.ID = Convert.ToInt64(reader["carID"]);
                                
                                carModel.Name = reader["carName"].ToString();
                                carModel.Equipment = EquipmentModelLoad(reader);
                                carModel.Photo = GetPhotoFromBd(carModel.ID);

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

        private BitmapImage GetPhotoFromBd(long IdCar)
        {
            byte[] _dbImageBytes = null;
            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
                string Querry = $"select photo from cars where id='{IdCar}'";
               

                using (var command = new SQLiteCommand(Querry, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(reader["photo"]!=DBNull.Value)
                                _dbImageBytes = (byte[]) reader["photo"];
                        }
                    }
                }
            }

            if (_dbImageBytes == null) return null;

            var image = new BitmapImage();
            using (var mem = new MemoryStream(_dbImageBytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;

        }

        /// <summary>
        /// Получить список всех двигателей
        /// </summary>
        /// <returns></returns>
        public List<IEngine> GetAllEngines()
        {
            List<IEngine> tempListEngine = new List<IEngine>();
            string Querry =
            @"SELECT * from CarsEngine";

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
                                var engineModel = new EngineModel();
                                engineModel.ID = Convert.ToInt64(reader["ID"]);
                                engineModel.Name = reader["name"].ToString();
                                engineModel.Power = Convert.ToDouble(reader["power"]);
                                engineModel.Expenditure = Convert.ToDouble(reader["Expenditure"]);

                                tempListEngine.Add(engineModel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e.FromMethod("Не удалось получить коллекцию двигателей из БД", MethodBase.GetCurrentMethod());
            }

            return tempListEngine;
        }

        /// <summary>
        /// Добавляем машину в БД
        /// </summary>
        public bool AddNewCarInDb(ICar car,byte[] imageByte)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(car.Name))
            {
                throw new Exception().FromMethod($"Значение {nameof(car.Name)} не может быть пустым", MethodBase.GetCurrentMethod());
            }

            if(car.Cost < 100000)
            {
                throw new Exception().FromMethod($"Значение {nameof(car.Cost)} не может быть меньше 100 000", MethodBase.GetCurrentMethod());
            }

            if (car.Count < 1)
            {
                throw new Exception().FromMethod($"Значение {nameof(car.Count)} не может быть меньше 1", MethodBase.GetCurrentMethod());
            }

            if(!new BrushConverter().IsValid($"#{car.Color.ColorCode}"))
            {
                throw new Exception().FromMethod($"Значение {nameof(car.Color.ColorName)} не валидное", MethodBase.GetCurrentMethod());
            }

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
             Connection.Open();
                //ролучаем новый id авто
                long newCarId = GetNewCarID();

                //Получаем новый id для цвета
                long newCarColorId = GetNewCarColorID();

                //Получаем новый id для доп.опций
                long newCarExtraId = GetNewExtraID();

                string commandTextCollor = "INSERT INTO CarColors ([ID],[ColorName], [ColorCode]) VALUES(@ID, @ColorName, @ColorCode)";

                int CountAddInColorTable = 0;
                using (var Command = new SQLiteCommand(commandTextCollor, Connection))
                {
                    Command.Parameters.AddWithValue("@ID",newCarColorId);
                    Command.Parameters.AddWithValue("@ColorName", car.Color.ColorName);
                    Command.Parameters.AddWithValue("@ColorCode", car.Color.ColorCode);

                    CountAddInColorTable = Command.ExecuteNonQuery();
                }

                if (CountAddInColorTable == 0)
                {
                    throw new Exception().FromMethod("Ошибка записи в таблицу CarColors",MethodBase.GetCurrentMethod());
                }

                string commandTextExtra = "INSERT INTO CarExtra ([ID],[ID_Car], [AndroidSystem],[CastWheel],[CruiseControl]) VALUES(@ID, @ID_Car, @AndroidSystem, @CastWheel, @CruiseControl)";
                int CountAddInExtraTable = 0;
                using (var Command = new SQLiteCommand(commandTextExtra, Connection))
                {
                    Command.Parameters.AddWithValue("@ID", newCarExtraId); 
                    Command.Parameters.AddWithValue("@ID_Car", newCarId);
                    Command.Parameters.AddWithValue("@AndroidSystem", Convert.ToInt32(car.Equipment.Extras.AndroidSystem));
                    Command.Parameters.AddWithValue("@CastWheel", Convert.ToInt32(car.Equipment.Extras.CastWheel));
                    Command.Parameters.AddWithValue("@CruiseControl", Convert.ToInt32(car.Equipment.Extras.CruiseControl));

                    CountAddInExtraTable = Command.ExecuteNonQuery();
                }

                if (CountAddInExtraTable == 0)
                {
                    throw new Exception().FromMethod("Ошибка записи в таблицу CarExtra", MethodBase.GetCurrentMethod());
                }

                string commandText = "INSERT INTO Cars ([ID],[Name], [ModelID], [ColorID], [Price],[EngineID],[ExtrasID],[Count],[Photo]) VALUES(@ID, @Name, @ModelID, @ColorID, @Price, @EngineID, @ExtrasID, @Count, @Photo)";

                using (var Command = new SQLiteCommand(commandText, Connection))
                {


                    Command.Parameters.AddWithValue("@ID", newCarId); 
                    Command.Parameters.AddWithValue("@Name", car.Name);
                    Command.Parameters.AddWithValue("@ModelID", 1);
                    Command.Parameters.AddWithValue("@ColorID", newCarColorId); 
                    Command.Parameters.AddWithValue("@Price", car.Cost);
                    Command.Parameters.AddWithValue("@EngineID", car.Equipment.Engine.ID);
                    Command.Parameters.AddWithValue("@ExtrasID", newCarExtraId); 
                    Command.Parameters.AddWithValue("@Count", car.Count);
                    Command.Parameters.AddWithValue("@Photo", imageByte);

                    var returnResutCars = Command.ExecuteNonQuery();

                    result = returnResutCars > 0;
                }


            }

            return result;
        }


        /// <summary>
        /// Изменяем машину в БД
        /// </summary>
        public bool EditCarInDb(ICar car, byte[] imageByte)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(car.Name))
            {
                throw new Exception().FromMethod($"Значение {nameof(car.Name)} не может быть пустым", MethodBase.GetCurrentMethod());
            }

            if (car.Cost < 100000)
            {
                throw new Exception().FromMethod($"Значение {nameof(car.Cost)} не может быть меньше 100 000", MethodBase.GetCurrentMethod());
            }

            if (car.Count < 1)
            {
                throw new Exception().FromMethod($"Значение {nameof(car.Count)} не может быть меньше 1", MethodBase.GetCurrentMethod());
            }

           

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
              

                string commandTextCollor = $"UPDATE CarExtra SET AndroidSystem=:AndroidSystem, CastWheel=:CastWheel, CruiseControl=:CruiseControl WHERE ID_Car = {car.ID}";

                int CountAddInColorTable = 0;
                using (var Command = new SQLiteCommand(commandTextCollor, Connection))
                {
                    Command.Parameters.Add("AndroidSystem",DbType.Int32).Value = Convert.ToInt32(car.Equipment.Extras.AndroidSystem);
                    Command.Parameters.Add("CastWheel", DbType.Int32).Value = Convert.ToInt32(car.Equipment.Extras.CastWheel);
                    Command.Parameters.Add("CruiseControl", DbType.Int32).Value = Convert.ToInt32(car.Equipment.Extras.CruiseControl);

                    //Command.Parameters.AddWithValue("@AndroidSystem", Convert.ToInt32(car.Equipment.Extras.AndroidSystem));
                    //Command.Parameters.AddWithValue("@CastWheel", Convert.ToInt32(car.Equipment.Extras.CastWheel));
                    //Command.Parameters.AddWithValue("@CruiseControl", Convert.ToInt32(car.Equipment.Extras.CruiseControl));

                    CountAddInColorTable = Command.ExecuteNonQuery();
                }

                if (CountAddInColorTable == 0)
                {
                    throw new Exception().FromMethod("Ошибка записи в таблицу CarExtra", MethodBase.GetCurrentMethod());
                }

                //string commandTextExtra = "INSERT INTO CarExtra ([ID],[ID_Car], [AndroidSystem],[CastWheel],[CruiseControl]) VALUES(@ID, @ID_Car, @AndroidSystem, @CastWheel, @CruiseControl)";
                //int CountAddInExtraTable = 0;
                //using (var Command = new SQLiteCommand(commandTextExtra, Connection))
                //{
                //    Command.Parameters.AddWithValue("@ID", newCarExtraId);
                //    Command.Parameters.AddWithValue("@ID_Car", newCarId);
                //    Command.Parameters.AddWithValue("@AndroidSystem", Convert.ToInt32(car.Equipment.Extras.AndroidSystem));
                //    Command.Parameters.AddWithValue("@CastWheel", Convert.ToInt32(car.Equipment.Extras.CastWheel));
                //    Command.Parameters.AddWithValue("@CruiseControl", Convert.ToInt32(car.Equipment.Extras.CruiseControl));

                //    CountAddInExtraTable = Command.ExecuteNonQuery();
                //}

                //if (CountAddInExtraTable == 0)
                //{
                //    throw new Exception().FromMethod("Ошибка записи в таблицу CarExtra", MethodBase.GetCurrentMethod());
                //}

                //string commandText = "INSERT INTO Cars ([ID],[Name], [ModelID], [ColorID], [Price],[EngineID],[ExtrasID],[Count],[Photo]) VALUES(@ID, @Name, @ModelID, @ColorID, @Price, @EngineID, @ExtrasID, @Count, @Photo)";

                //using (var Command = new SQLiteCommand(commandText, Connection))
                //{


                //    Command.Parameters.AddWithValue("@ID", newCarId);
                //    Command.Parameters.AddWithValue("@Name", car.Name);
                //    Command.Parameters.AddWithValue("@ModelID", 1);
                //    Command.Parameters.AddWithValue("@ColorID", newCarColorId);
                //    Command.Parameters.AddWithValue("@Price", car.Cost);
                //    Command.Parameters.AddWithValue("@EngineID", car.Equipment.Engine.ID);
                //    Command.Parameters.AddWithValue("@ExtrasID", newCarExtraId);
                //    Command.Parameters.AddWithValue("@Count", car.Count);
                //    Command.Parameters.AddWithValue("@Photo", imageByte);

                //    var returnResutCars = Command.ExecuteNonQuery();

                //    result = returnResutCars > 0;
                //}


            }

            return result;
        }

        /// <summary>
        /// Добавить новый двигатель в БД
        /// </summary>
        /// <param name="engine">Модель двигателя</param>
        /// <returns>True-если добавлен</returns>
        public bool WriteNewEngineInDb(IEngine engine)
        {
            bool returnResult = false;


            if (string.IsNullOrWhiteSpace(engine.Name))
            {
                throw  new Exception().FromMethod($"Значение {nameof(engine.Name)} не может быть пустым",MethodBase.GetCurrentMethod());
            }

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                string commandText = "INSERT INTO CarsEngine ([Name], [Power], [Expenditure]) VALUES(@Name, @Power, @Expenditure)";
                using (var Command = new SQLiteCommand(commandText, Connection))
                {
                    Command.Parameters.AddWithValue("@Name", engine.Name); 
                    Command.Parameters.AddWithValue("@Power", engine.Power);
                    Command.Parameters.AddWithValue("@Expenditure", engine.Expenditure);

                    Connection.Open();
                    var returnCountRow = Command.ExecuteNonQuery();

                    returnResult = returnCountRow > 0;
                }
                 
            
            }

            return returnResult;

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


        private long GetNewCarID()
        {

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
                string commandText = "select seq from sqlite_sequence s where s.name = 'Cars'";
                using (var Command = new SQLiteCommand(commandText, Connection))
                {
                    return Convert.ToInt64(Command.ExecuteScalar()) + 1;
                }
            }
        }
        private long GetNewExtraID()
        {

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
                string commandText = "select seq from sqlite_sequence s where s.name = 'CarExtra'";
                using (var Command = new SQLiteCommand(commandText, Connection))
                {
                    return Convert.ToInt64(Command.ExecuteScalar()) + 1;
                }
            }
        }

        private long GetNewCarColorID()
        {

            using (SQLiteConnection Connection = new SQLiteConnection(SqliteConnectionString.ConnectionString))
            {
                Connection.Open();
                string commandText = "select seq from sqlite_sequence s where s.name = 'CarColors'";
                using (var Command = new SQLiteCommand(commandText, Connection))
                {
                    return Convert.ToInt64(Command.ExecuteScalar()) + 1;
                }
            }
        }

    }
}
