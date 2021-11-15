using Prague_Parking;
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
                Console.Clear();
                Console.WriteLine("[1] Load from GarageMaker/templates");
                Console.WriteLine("[2] Load existing in /parks");
                Console.WriteLine("[3] Exit");
                Console.Write("Option: ");

                switch (Console.ReadLine())
                {
                    #region Load from GarageMaker/templates
                    case "1":
                        {
                            Console.Clear();
                            //  Load and Save a GarageMaker/templates file to /parks
                            Console.Write("Enter the file name: ");
                            string fileName = Console.ReadLine();
                            string filePath = $"../../../../GarageMaker/templates/{fileName}.json";
                            GarageSerializer garageSerializer = new GarageSerializer();
                            Garage = garageSerializer.JsonDeserializeSimple(typeof(MyGarage), filePath) as MyGarage;
                            FileName = fileName;
                            Garage.FileName = fileName;
                            FileName = Garage.UISave();

                            // Then Reload it from /parks
                            Garage = MyGarage.Load(FileName);
                            Garage.UIMenu();
                            break;
                        }
                    #endregion
                    #region Load existing in /parks
                    case "2":
                        {
                            Console.Clear();
                            Console.Write("Enter the file name: ");
                            string fileName = Console.ReadLine();
                            try
                            {
                                Garage = MyGarage.Load(fileName);
                                FileName = Garage.FileName;
                                Garage.UIMenu();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Error loading garage");
                            }
                            break;
                        }
                    #endregion
                    #region Exit
                    case "3":
                        {
                            Console.Clear();
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