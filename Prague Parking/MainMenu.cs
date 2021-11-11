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
        public void Init()
        {
            Console.WriteLine("[1] Load from GarageMaker/templates");
            Console.WriteLine("[2] Load existing in /parks");
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
                default: { break; }
            }
        }
        #endregion
    }
}