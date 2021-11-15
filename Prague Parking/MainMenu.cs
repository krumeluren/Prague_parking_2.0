
using Prague_Parking_2_0_beta.Garage;
using System;

namespace Prague_Parking_2_0_beta
{
    class MainMenu
    {
        public Garage.Garage ThisGarage { get; set; }
        public string FileName { get; set; }

        #region Init() - Init a Garage object from GarageMaker or /parks
        public bool Init()
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                Console.WriteLine(" 1) Load from GarageMaker/templates");
                Console.WriteLine(" 2) Load existing in /parks");
                Console.WriteLine(" 3) Exit");
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
                            ThisGarage = garageSerializer.JsonDeserializeSimple(typeof(Garage.Garage), filePath) as Garage.Garage;
                            FileName = fileName;
                            ThisGarage.FileName = fileName;
                            FileName = ThisGarage.UISave();

                            // Then Reload it from /parks
                            ThisGarage = Prague_Parking_2_0_beta.Garage.Garage.Load(FileName);
                            ThisGarage.UIMenu();
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
                                ThisGarage = Garage.Garage.Load(fileName);
                                FileName = ThisGarage.FileName;
                                ThisGarage.UIMenu();
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