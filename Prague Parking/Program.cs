using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Prague_Parking
{
    class Program
    {




        // initierar parkeringsplatser och kör MainMenu()
        #region Main()
        static void Main(string[] args)
        {
            Console.WriteLine("V 2.0 beta");
            string[] parkingGarage = new string[100];
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                parkingGarage[i] = $"P{i}*#";
            }

            //  Initiella Temporära Test platser. Tas bort innan kunden får programmet.
            parkingGarage[0] = $"P{0}*P-A11-7:30#";
            parkingGarage[1] = $"P{1}*#MC-C25-09:30";
            parkingGarage[5] = $"P{5}*MC-NA123-6:30#MC-BV12-1:17";
            parkingGarage[12] = $"P{12}*P-POA-2:30#";
            parkingGarage[36] = $"P{36}*MC-AB12-4:30#";
            parkingGarage[37] = $"P{37}*P-P45-4:30#";
            parkingGarage[55] = $"P{55}*P-LUT-1:30#";
            parkingGarage[78] = $"P{78}*MC-K12-2:30#MC-K13-6:15";
            parkingGarage[90] = $"P{90}*P-87D-0:30#";
            parkingGarage[91] = $"P{91}*MC-OP1-0:20#";
            
            MainMenu(parkingGarage);
        }
        #endregion




        // MainMenu - Alla metoder leder tillbaka hit.
        // Består av 4 val. exempel..
        // Lägg till ett fordon.        CreateVehicle() -> return "MC-ABC123-14:30" -> AddVehicle("MC-ABC123-14:30")
        // Flytta fordon.               MoveVehicle("ABC123").. inuti körs ( DeleteVehicle("ABC123") -> return "MC-ABC123-14:30" -> AddVehicle("MC-ABC123-14:30") )
        // Ta bort fordon.              DeleteVehicle("ABC123") -> tar bort "MC-ABC123-14:30" -> TimeSince("14:30")
        // Visa alla platser.           ShowAllLots()
        // Sök på ett fordon            SearchVehicle("ABC123") -> visar platsen
        // Sök på en plats              PrintContent("1") -> Fordon1, Fordon2
        #region MainMenu()
        static void MainMenu(string[] parkingGarage)
        {
            Console.WriteLine("Huvudmeny");
            Console.WriteLine("[1] Lägg till ett nytt fordon ");         
            Console.WriteLine("[2] Flytta ett fordon");
            Console.WriteLine("[3] Ta bort ett fordon");
            Console.WriteLine("[4] Visa platser");
            Console.WriteLine("[5] Sök på ett fordon");
            Console.WriteLine("[6] Sök på en plats");
            Console.WriteLine("");
            Console.Write("Val: ");
            string option = Console.ReadLine();
            Console.WriteLine("");
            switch (option)
            {
                case "1":
                    {
                        string newVehicle = CreateNewVechile(parkingGarage);
                        AddVehicle(parkingGarage, newVehicle);
                        break;
                    }
                case "2":
                    {
                        Console.Write("Sök efter registreringsnummer att flytta: ");
                        string search = Console.ReadLine();
                        Console.WriteLine("");
                        MoveVehicle(parkingGarage, search);
                        break;
                    }
                case "3":
                    {
                        Console.Write("Sök efter registreringsnummer att ta bort: ");
                        string search = Console.ReadLine();
                        Console.WriteLine("");
                        var deletedVehicleData = DeleteVehicle(parkingGarage, search);
                        TimeSince(deletedVehicleData.Item3);
                        break;
                    }
                case "4":
                    {
                        ShowAllLots(parkingGarage);
                        break;
                    }
                case "5":
                    {
                        Console.Write("Sök ett registreringsnummer: ");
                        string search = Console.ReadLine();
                        int lot = SearchVehicle(parkingGarage, search );
                        Console.WriteLine("");
                        if (lot >= 0){
                            Console.WriteLine("Fordonet är på plats P" + (lot+1) + "\n");
                        } else { Console.WriteLine("Hittade inget fordon på din sökning:" + search + "\n"); }
                        
                        break;
                    }
                case "6":
                    {
                        Console.Write("Sök en plats: ");
                        string lotStr = Console.ReadLine();
                        PrintContent(parkingGarage, lotStr);

                        
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Ogiltigt\n");
                        MainMenu(parkingGarage);
                        break;
                    }
            }
            MainMenu(parkingGarage);
        }
        #endregion




        // CreateNewVehicle() låter användaren skapa en ny fordonsträng och fordonstyp
        //      Tar HH:mm tidsstämpel med DateTime
        //      Returnerar fordonsträng format (TYP-REGNR-TID)
        #region CreateNewVehicle()
        static string CreateNewVechile(string[] parkingGarage)
        {
            string newVehicle;

            Console.WriteLine("Ange ett fordonstyp: ");
            Console.WriteLine("[1] MC ");
            Console.WriteLine("[2] Personbil ");
            Console.WriteLine("[3] Gå till Huvudmeny\n");
            Console.Write("Val: ");
            string typeSelected = Console.ReadLine();
            Console.WriteLine("");

            string type = "";
            switch (typeSelected)
            {
                case "1": type = "MC"; break;
                case "2": type = "P"; break;
                case "3": MainMenu(parkingGarage); break;
                default: Console.WriteLine("Ogiltigt\n"); CreateNewVechile(parkingGarage); break;
            }


            string pattern = "[\\~#%&*{}/:<>?|\"-]";
            string replacement = " ";
            Regex regEx = new Regex(pattern);

            Console.Write("Ange ett registreringsnummer: ");
            string numberPlate = Console.ReadLine();
            numberPlate = FilterChars(numberPlate).ToUpper();
            Console.WriteLine("");

            string date = DateTime.Now.ToString("HH:mm");

            newVehicle = $"{type}-{numberPlate}-{date}";
            Console.WriteLine($"{newVehicle}\nKorrekt?\n[1] Ja\n[2] Gör om\n[3] Gå till Huvudmeny\n");
            Console.Write("Val: ");
            string confirm = Console.ReadLine();
            Console.WriteLine("");
            //Fordonsträng skapad

            switch (confirm)
            {
                case "1": /*foundLot = ReturnLots(parkingGarage, newVehicle)*/ ; break;
                case "2": CreateNewVechile(parkingGarage); break;
                case "3": MainMenu(parkingGarage); break;
                default: Console.WriteLine("Ogiltigt\n"); CreateNewVechile(parkingGarage); break;
            }

            return newVehicle;
        }
        #endregion



        //  Ersätter symboler med tomrum
        //      Returnerar filtrerad sträng
        #region FilterChars(string str)
        private static string FilterChars(string str)
        {
            return str.Replace("*", "").Replace("#", "").Replace("-", "").Replace(" ", "").Replace(":", "");
        }
        #endregion




        //  MoveVehicle(sökord)
        //      kör DeleteVehicle(sökord, isMoving = true).. isMoving är för att skippa en dialog
        //          Om DeleteVehicle() lyckades att ta bort något fordon..
        //          kör AddVehicle(deletedVehicle, deletedVehicleLot) 
        #region MoveVehicle("ABC123")
        static void MoveVehicle(string[] parkingGarage, string search)
        {
            var deletedVehicleData = DeleteVehicle(parkingGarage, search, true);
            string deletedVehicle = deletedVehicleData.Item1;
            int deletedVehicleLot = deletedVehicleData.Item2;
            if (deletedVehicleData.Item1 != "")
            {
                Console.WriteLine("Flyttar " + deletedVehicle);
                Console.WriteLine("");
                AddVehicle(parkingGarage, deletedVehicle, deletedVehicleLot);
            }
        }
        #endregion




        //  DeleteVehicle(search, isMoving = false) 
        //      letar efter ditt sökord (regnr), tar bort den om den hittar, annars misslyckas.
        //          Returnerar:
        //          string av borttaget fordon t.ex. "MC-ABC123-14:30" (eller "" om misslyckas),
        //          int av platsen som den togs bort ifrån (0-99, eller -1 om misslyckas) 
        //          en tidsstämpel - "14:30" (Eller "" om misslyckas)
        #region DeleteVehicle("ABC123", isMoving = false)
        static (string, int, string) DeleteVehicle(string[]  parkingGarage, string search, bool isMoving = false)
        {
            search = FilterChars(search).ToUpper();

            //return värdet
            string deletedVehicle = "";
            string date = "";

            int lot = SearchVehicle(parkingGarage, search);
            if(lot < 0)
            {
                Console.WriteLine("Hittade Inget på parkeringen av din sökning: " + search);
                Console.WriteLine("");
            }
            else
            {

                string[] fullLot = parkingGarage[lot].Split("*");
                string[] halfLots = fullLot[1].Split("#");
                //Gå igenom varje halva av platsen
                for (int i = 0; i < halfLots.Length; i++)
                {
                    //om den inte är tom
                    if (halfLots[i] != "")
                    {
                        //plocka ut regnr
                        string[] vehicleData = halfLots[i].Split("-");
                        string numberPlate = vehicleData[1];
                        //om regnr == din search
                        if (search == numberPlate)
                        {

                            if (isMoving == false)
                            {
                                Console.WriteLine("Ta bort " + halfLots[i] + " från P" + (lot + 1) + "?");
                                Console.WriteLine("Detta är permanent!");
                                Console.WriteLine("[1] för att godkänna\n");
                                Console.Write("Val: ");
                                string answer = Console.ReadLine();
                                Console.WriteLine("");
                                switch (answer)
                                {
                                    case "1": break;
                                    default: Console.WriteLine("Backar..\n"); MainMenu(parkingGarage); break;
                                }
                            }
                            
                            //Få ut tid-string t.ex. "14:30"
                            string[] data = halfLots[i].Split("-");
                            date = data[2];
                            //Spara den till deletedVehicle.. för return nedan
                            deletedVehicle = halfLots[i];
                            //Töm string halfLots[i]. 
                            halfLots[i] = "";
                        }
                    }
                }
                //Uppdatera parkeringsplatsens innehåll
                string updatedLot = fullLot[0] + "*" + halfLots[0] + "#" + halfLots[1];
                parkingGarage[lot] = updatedLot;
                Console.WriteLine("Tar bort fordon: " + deletedVehicle + " från P" + (lot+1));
                Console.WriteLine("");


            }
            return (deletedVehicle, lot, date);
        }
        #endregion




        //  AddVehicle tar (vehicle, (Optional) deletedVehicleLot)
        //      Kör VehicleType() som returnerar "P" eller "MC" 
        //      Kör FindLot() som returnerar en leding plats (eller -1 om misslyckas, då går ur metoden)
        //      OM deletedVehicleLot anges kommer konsolen skriva ut instruktion för personen att flytta fordonet från den platsen till den nya på riktigt.
        //          Sen sätter den in fordonssträngen på 'rätt plats'
        #region AddVehicle("MC-ABC123-14:30")
        static void AddVehicle(string[] parkingGarage, string vehicle, int deletedVehicleLot = -1)
        {
            //   temporärt minne för att skicka tillbaka om funktionen måste startas om, eftersom deletedVehicleLot annars överskrider med standardvärde -1
            int tempdeletedVehicleLot = deletedVehicleLot;
            //  Hitta en plats eller välj en
            Console.WriteLine("[1] Välj plats åt mig");
            Console.WriteLine("[2] Välj en egen plats");
            string option = Console.ReadLine();
            Console.WriteLine("");

            string setLot = "-1";
            int lot = -1;

            switch (option)
            {
                case "1": {
                        lot = FindLot(parkingGarage, vehicle);
                        break;
                    }
                default:
                {
                    Console.Write("Ange en plats:");
                    setLot = Console.ReadLine();
                    Console.WriteLine("");
                    try
                    {
                        lot = int.Parse(setLot) - 1;

                        if(lot >= 0 && lot <= 99)
                        {
                            lot = FindSpecificLot(parkingGarage, vehicle, VehicleType(vehicle), lot);
                        }
                        else
                        {
                            Console.WriteLine("Angivet nummer finns inte parkeringen: P" + (lot + 1));
                            Console.WriteLine("Försök igen\n");
                            AddVehicle(parkingGarage, vehicle, deletedVehicleLot = tempdeletedVehicleLot);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Inte ett tal! Gör om.\n");
                        AddVehicle(parkingGarage, vehicle, deletedVehicleLot = tempdeletedVehicleLot);
                    }
                    break;
                }
            }



            // Om platsen du angav att lägga till fordonet på returnerar -1 
            if (lot < 0 && option != "1")
            {
                Console.WriteLine("Varning!\nMisslyckades att lägga till fordonet. Det är upptaget");
                PrintContent(parkingGarage, setLot);
            }

            // Om den inte hittat någon lot (returnerar -1) Det är fullt.
            else if (lot < 0)
            {
                Console.WriteLine("Varning!\nMisslyckades att lägga till fordonet. Är det fullt på parkeringen?");
                Console.WriteLine("");
            }

            //  Om den hittat en plats
            else
            {
                //  lägger till nytt fordon.. Gå vidare eller ångra..
                if(deletedVehicleLot < 0) 
                {
                    Console.WriteLine("Kör " + vehicle + " till P" + (lot + 1));
                    Console.WriteLine("\n[1] Klart");
                    Console.WriteLine("[2] Ångra\n");
                    Console.Write("Val: ");
                    string answer = Console.ReadLine();
                    Console.WriteLine("");
                    switch (answer)
                    {
                        case "1": 
                            Console.WriteLine("Lägger till " + vehicle + " på P" + (lot + 1) + "\n");
                            break;
                        default:
                            Console.WriteLine("Backar..\n");
                            MainMenu(parkingGarage);
                            break;
                    }
                }

                //  Du flyttar ett existerade fordon
                else
                {
                    //  Det fordonet du flyttar redan är på samma plats FindLot() returnerar. Informera användaren.
                    if(deletedVehicleLot == lot)
                    {
                        Console.WriteLine("Detta fordon är redan på den bästa platsen..\n");
                    }

                    //  Flytta fordonet till den nya platsen eller ångra ( flytta tillbaka fordonet till sin tidigare plats ) 
                    else
                    {
                        Console.WriteLine("Kör " + vehicle + " från P" + (deletedVehicleLot + 1) + " till P" + (lot + 1) + "\n");
                        Console.WriteLine("[1] Klart");
                        Console.WriteLine("[2] Ångra\n");
                        Console.Write("Val: ");
                        string answer = Console.ReadLine();
                        Console.WriteLine("");
                        switch (answer)
                        {
                            case "1":
                                Console.WriteLine("Lägger till " + vehicle + " på P" + (lot + 1) + "\n");
                                break;
                            default:
                                lot = deletedVehicleLot;
                                Console.WriteLine("Lägger tillbaka " + vehicle + " på P" + (lot + 1) + "\n");
                                break;
                        }
                    }
                }

                //Sätt in fordonet på rätt ställe på platsen
                string[] parkingLot = parkingGarage[lot].Split("*");
                string[] halfLots = parkingLot[1].Split("#");
                string vehicleType = VehicleType(vehicle); //'P' eller 'MC'
                if (vehicleType == "MC")
                {
                    if (halfLots[0] == "")
                    {
                        //  MC-ABC123-14:30 + # + MC-EFG456-16:15
                        parkingLot[1] = vehicle + "#" + halfLots[1];
                    }
                    else if (halfLots[1] == "")
                    {
                        //  MC-EFG456-16:15 + # + MC-ABC123-14:30
                        parkingLot[1] = halfLots[0] + "#" + vehicle;
                    }
                }
                else if (vehicleType == "P")
                {
                    //  P-ABC123 + #
                    parkingLot[1] = vehicle + "#" + "";
                }
                parkingGarage[lot] = parkingLot[0] + "*" + parkingLot[1];
            }
            MainMenu(parkingGarage);
        }
        #endregion



        //  FindSpecificLot ( fordonsträng, plats)
        //      Den kollar om fordonet är MC eller personbil (P) med VehicleType()
        //          Returnerar:
        //          En int av parkingGarage[] om fordonet 'får plats'
        //          Om den inte får plats returnerar den -1
        #region FindSpecificLot("MC-ABC123-14:30", "MC", 3)
        private static int FindSpecificLot(string[] parkingGarage, string vehicle, string vehicleType,  int i)
        {
            //"P", "FORDON#FORDON"
            string[] fullLot = parkingGarage[i].Split("*");

            //"FORDON", "FORDON"
            string[] halfLot = fullLot[1].Split("#");


            //Om du sätter in ett fordon
            if (vehicleType == "P")
            {
                //Både platser måste vara tomma
                if (halfLot[0] == "" && halfLot[1] == "")
                {
                    //din personbil får plats
                    string[] p = parkingGarage[i].Split("*");
                    int pN = int.Parse(p[0].Substring(1)) + 1;
                    string[] pS = p[1].Split("#");
                    Console.WriteLine(
                                    "{0,0}\t{1,5}\t{2,-20}\t{3,-20}", "Hittade plats:", "P" + pN.ToString(), pS[0], pS[1] + "\n");
                    return i;
                }
            }
            //Om du sätter in en MC
            else if (vehicleType == "MC")
            {

                //Om en av platserna är tomma och den första inte är en personbil
                if ((halfLot[0] == "" || halfLot[1] == "") &&
                        VehicleType(halfLot[0]) != "P")
                {
                    //din MC får plats
                    string[] p = parkingGarage[i].Split("*");
                    int pN = int.Parse(p[0].Substring(1)) + 1;
                    string[] pS = p[1].Split("#");
                    Console.WriteLine(
                                     "{0,0}\t{1,5}\t{2,-20}\t{3,-20}", "Hittade plats:", "P" + pN.ToString(), pS[0], pS[1] + "\n");
                    return i;
                }

            }
            return -1;   
        }
        #endregion




        //  SearchVehicle(sökord)
        //      söker efter platsen av ett fordon ("ABC123")
        //          returnerar 0-99, (-1 om inte finns)
        #region SearchVehicle("ABC123")
        static int SearchVehicle(string[] parkingGarage, string search)
        {
            search = FilterChars(search).ToUpper();

            for (int i = 0; i < parkingGarage.Length; i++)
            {
                string[] fullLot = parkingGarage[i].Split("*");
                string[] halfLots = fullLot[1].Split("#");
                foreach (string halfLot in halfLots)
                {
                    if (halfLot != "")
                    {
                        // MC ABC123 14:30
                        string[] vehicleData = halfLot.Split("-");

                        if (vehicleData[1] == search)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
        #endregion




        //  VehicleType(fordonsträng)
        //      returnerar "MC" eller "P"
        #region VehicleType("MC-ABC123-14:30")
        static string VehicleType(string vehicle)
        {
            //MC-ABC123-14:30#
            string[] split = vehicle.Split("-");
            //MC
            string vehicleType = split[0];
            return vehicleType;
        }
        #endregion




        //  FindLot(fordonsträng)
        //      Den kollar om fordonet är MC eller personbil (P) med VehicleType()
        //      Loopar igenom alla parkingringsplatser och kör FindSpecificLot() för varje
        //          Returnerar:
        //          En int av parkingGarage[] där fordonet 'får plats'
        //          Om den inte hittar någon ledig plats returnerar den -1
        #region FindLot("MC-ABC123-14:30")
        static int FindLot(string[] parkingGarage, string vehicle)
        {
            int foundLot = -1;

            //returnerar MC eller P
            string vehicleType = VehicleType(vehicle);

            for (int i = 0; i < parkingGarage.Length; i++)
            {

                foundLot = FindSpecificLot(parkingGarage, vehicle, vehicleType, i);
                if(foundLot != -1)
                {
                    return i;
                }
 
            }
            Console.WriteLine("\nHittade ingen plats i parkeringen \n");
            return foundLot;
        }
        #endregion




        //  ShowAllLots() loopar igenom alla lots och skriver ut dem på konsolen
        #region ShowAllLots()
        private static void ShowAllLots(string[] parkingGarage)
        {
            Console.WriteLine("{0,5}\t{1,-20}\t{2,-20}", "Plats", "Fordon1", "Fordon2");
            foreach (string lot in parkingGarage)
            {
                string[] p = lot.Split("*");
                int pN = int.Parse(p[0].Substring(1)) + 1;
                string[] pS = p[1].Split("#");
                Console.WriteLine(
                                "{0,5}\t{1,-20}\t{2,-20}", "P" + pN.ToString(), pS[0], pS[1]);
            }
            Console.WriteLine("");
        }
        #endregion




        //  Får en tid (t.ex. "14:30") och jämför med nuvarande tid.
        //      Skriver ut hur långt det gått sen dess.
        #region TimeSince("14:30")
        private static void TimeSince(string date)
        {
            string[] time = date.Split(":");
            int hour = int.Parse(time[0]);
            int minutes = int.Parse(time[1]);

            #region DateTime timeWhenAdded och timeNow
            DateTime timeWhenAdded = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                hour,
                minutes,
                0);

            DateTime timeNow = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                0);

            #endregion
            //Jämför timeWhenAdded och timeNow
            var since = timeNow.TimeOfDay - timeWhenAdded.TimeOfDay;

            Console.WriteLine("Fordonet har varit på platsen i..");
            Console.WriteLine(Math.Floor(since.TotalMinutes) + " minuter");

            Console.Write(since.Hours + " timmar ");
            Console.WriteLine(since.Minutes + " minuter");

            Console.WriteLine("");
        }
        #endregion




        //  PrintContent( "0" - "99" )
        //      Skriver ut innehållet på platsen om den lyckas.
        //      tex. 1. MC-ABC123-14:30
        //           2. MC-DEF456-15:10
        #region PrintContent("0" - "99")
        private static void PrintContent(string[] parkingGarage, string lotStr)
        {
            try
            {
                int lot = int.Parse(lotStr) - 1;
                if (lot >= 0 && lot <= 99)
                {
                    string[] content = parkingGarage[lot].Split("*");
                    string[] vehicles = content[1].Split("#");
                    Console.WriteLine("Fordon på platsen: ");
                    Console.WriteLine("1." + vehicles[0]);
                    Console.WriteLine("2." + vehicles[1]);
                    Console.WriteLine("");
                }
                else Console.WriteLine("Platsen får bara vara inom 1-100\n");
            }
            catch (Exception) { Console.WriteLine("Inte ett giltigt nummer\n"); }
        }
        #endregion

    }
}