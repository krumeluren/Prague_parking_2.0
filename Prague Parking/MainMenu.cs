using Prague_Parking_2_0_beta.Garage;
using System;

namespace Prague_Parking_2_0_beta
{
    class MainMenu
    {
        public MyGarage Garage { get; set; }
        public string FileName { get; set; }

        #region Constructor
        public MainMenu()
        {}
        #endregion

        #region Init() - Init a MyGarage object from GarageMaker or /parks
        public bool Init()
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.WriteLine("[1] Load from GarageMaker/templates");
                Console.WriteLine("[2] Load existing in /parks");
                Console.WriteLine("[3] Exit");
                Console.Write("Option: ");

                switch (Console.ReadLine())
                {
                    #region Load from GarageMaker/templates
                    case "1":
                        {
                            //  Load and Save a GarageMaker/templates file to /parks
                            Console.Write("Enter the file name: ");
                            string fileName = Console.ReadLine();
                            string filePath = $"../../../../GarageMaker/templates/{fileName}.json";
                            GarageSerializer garageSerializer = new GarageSerializer();
                            Garage = garageSerializer.JsonDeserialize(typeof(MyGarage), filePath) as MyGarage;
                            Garage.SetReferences();
                            FileName = Garage.UISave(fileName);
                            // Then Reload it from /parks
                            Garage = MyGarage.Load(FileName);
                            Garage.FileName = FileName;

                            Garage.UIMenu();
                            break;
                        }
                    #endregion
                    #region Load existing in /parks
                    case "2":
                        {
                            Console.Write("Enter the file name: ");
                            string fileName = Console.ReadLine();
                            try
                            {
                                FileName = fileName;
                                Garage = MyGarage.Load(fileName);
                                Garage.SetReferences();
                                Garage.SetLotNumbers();
                                Garage.FileName = FileName;
                                Garage.UIMenu();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("No file found");
                            }
                            break;
                        }
                    #endregion
                    #region Exit
                    case "3":
                        {
                            isDone = true;
                            break;
                        }
                    #endregion
                    default: { break; }
                }
            }
           return isDone;
        }
        #endregion
    }
}