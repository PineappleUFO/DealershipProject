using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Dealership.SQL.Models.CarModel;
using Dealership.SQL.Models.CarModel.Equipment;
using Dealership.SQL.Repository;
using Dealership.UI.Annotations;
using Dealership.UI.MVVM;
using Dealership.UI.MVVM.Commands;

namespace Dealership.UI.View.CatalogView.CatalogEditOrAdd
{
    public class CatalogEditOrAddViewModel : BaseViewModel
    {
        public string CodeColor
        {
            get => _codeColor;
            set => SetProperty(ref _codeColor , value);
        }


        public string NameColor
        {
            get => _nameColor;
            set => SetProperty(ref _nameColor , value);
        }

        [CanBeNull]
        public FileInfo FileInfoPhoto
        {
            get => _fileInfoPhoto;
            set => SetProperty(ref _fileInfoPhoto , value);
        }


        public IEngine NewEngine
        {
            get => _newEngine;
            set => SetProperty(ref _newEngine , value);
        }

        public ICar CurrentCar
        {
            get => _currentCar;
            set => SetProperty(ref _currentCar, value);
        }

        public List<IEngine> AllEngines
        {
            get => _allEngines;
            set => SetProperty(ref _allEngines,value);
        }

        public Visibility SelectEngineVisibility
        {
            get => _selectEngineVisibility;
            set => SetProperty(ref _selectEngineVisibility, value);
        }

        public Visibility AddEngineVisibility
        {
            get => _addEngineVisibility;
            set => SetProperty(ref _addEngineVisibility,value);
        }

        readonly CarRepository carRepository =  new CarRepository();
        private List<IEngine> _allEngines;
        private Visibility _selectEngineVisibility = Visibility.Visible;
        private Visibility _addEngineVisibility = Visibility.Collapsed;
        private IEngine _newEngine;
        private ICar _currentCar;
        private byte[] photoBytes;
        private ColorStruct _newColors;
        private string _codeColor;
        private string _nameColor;
        private FileInfo _fileInfoPhoto;

        public DelegateCommand AddNewEngine { get; set; }
        public DelegateCommand BackEngine { get; set; }
        public DelegateCommand SaveEngineInDB { get; set; }
        public DelegateCommand AddPhoto { get; set; }

        public DelegateCommand AddCarInDB { get; set; }


        private bool IsEdit { get; set; } = false;
        /// <summary>
        /// Изменяем параметры авто
        /// </summary>
        /// <param name="car"></param>
        public CatalogEditOrAddViewModel(ICar car)
        {
            CurrentCar = car;
            IsEdit = true;
            InitCommandAndEngine();
        }

     

        /// <summary>
        /// Добавляем авто
        /// </summary>
        public CatalogEditOrAddViewModel()
        {
            IsEdit = false;
            InitCommandAndEngine();
        }

        private void InitCommandAndEngine()
        {
            AllEngines = carRepository.GetAllEngines();

            AddNewEngine = new DelegateCommand(AddNewEngineAction);
            BackEngine = new DelegateCommand(BackEngineAction);
            SaveEngineInDB = new DelegateCommand(SaveEngineInDBAction);
            AddPhoto = new DelegateCommand(AddPhotoAction);
            AddCarInDB = new DelegateCommand(AddCarInDBAction);
        }

        private void AddCarInDBAction(object obj)
        {
           CurrentCar.Color = new ColorStruct(){ColorName = NameColor,ColorCode = CodeColor};
           try
           {
               if (!IsEdit)
               {
                   carRepository.AddNewCarInDb(CurrentCar, photoBytes);
               }
               else
               {
                   carRepository.EditCarInDb(CurrentCar, photoBytes);
               }
           }
           catch (Exception e)
           {
               MessageBox.Show($"Возникло исключение:{e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
           }
        }

        private void AddPhotoAction(object obj)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };

            bool? result = dlg.ShowDialog();

           
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                FileInfoPhoto = new FileInfo(filename);

                if (FileInfoPhoto.Exists)
                {
                    long _numBytes = FileInfoPhoto.Length;
                  
                    using (var _fileStream = new FileStream(FileInfoPhoto.FullName, FileMode.Open, FileAccess.Read))
                    {
                        BinaryReader _binReader = new BinaryReader(_fileStream);
                        photoBytes = _binReader.ReadBytes((int)_numBytes); // изображение в байтах

                    }
                }
            }
        }

        private void SaveEngineInDBAction(object obj)
        {
            if (NewEngine != null)
            {
                bool result = false;
                try
                {
                   result=  carRepository.WriteNewEngineInDb(NewEngine);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Возникло исключение:{e.Message}","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                }

                if (result)
                {
                    MessageBox.Show("Запись нового двигателя в базу успешно завершена", "Ок", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    BackEngine.Execute(null);
                    AllEngines = carRepository.GetAllEngines();
                }
            }
        }

        private void BackEngineAction(object obj)
        {
            SelectEngineVisibility = Visibility.Visible;
            AddEngineVisibility = Visibility.Collapsed;
        }

        private void AddNewEngineAction(object obj)
        {
                NewEngine = new EngineModel();
                SelectEngineVisibility = Visibility.Collapsed;
                 AddEngineVisibility = Visibility.Visible;
        }

       
    }
}
