using Newtonsoft.Json;
using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Prague_Parking_2_0_beta
{
    /// <summary>
    /// Static methods filtering and returning a list of lots
    /// </summary>
    class Query
    {
        #region ByMaxHeigth(list, max height)
        /// <summary>
        /// Filtered garage
        /// </summary>
        /// <param name="lots">The input List<Lot> </param>
        /// <param name="max">The largest heigth value to include a lot in the list</param>
        /// <returns>A filtered list with a max heigth</returns>
        static public List<Lot> ByMaxHeigth(List<Lot> lots, int max)
        {
            List<Lot> queriedLots = new List<Lot>();

            foreach (Lot lot in lots)
            {
                if (lot.Heigth <= max)
                {
                    queriedLots.Add(lot);
                }
            }
            return queriedLots;
        }
        #endregion
        #region ByMinHeigth(list, min height)
        /// <summary>
        /// Filtered list of lots
        /// </summary>
        /// <param name="lots">The input List<Lot> </param>
        /// <param name="min">The smallest heigth value to include a lot in the list</param>
        /// <returns>A list of lots with a min heigth</returns>
        static public List<Lot> ByMinHeigth(List<Lot> lots, int min)
        {
            List<Lot> queriedLots = new List<Lot>();

            foreach (Lot lot in lots)
            {
                if (lot.Heigth >= min)
                {
                    queriedLots.Add(lot);
                }
            }
            return queriedLots;
        }
        #endregion
        #region ByCharger(list, bool)
        /// <summary>
        /// Filtered list of lots
        /// </summary>
        /// <param name="lots">The input List<Lot> </param>
        /// <param name="hasCharger"></param>
        /// <returns>A list of Lots with/without charging stations</returns>
        static public List<Lot> ByCharger(List<Lot> lots, bool hasCharger)
        {
            List<Lot> queriedLots = new List<Lot>();

            foreach (Lot lot in lots)
            {
                if (lot.HasCharger == hasCharger)
                {
                    queriedLots.Add(lot);
                }
            }
            return queriedLots;
        }
        #endregion

        static public class VehicleQ
        {
            #region ById()
            /// <returns>A list of vehicles containing the id</returns>
            static public List<Vehicle> ById(List<Vehicle> list, string id)
            {
                List<Vehicle> query = new List<Vehicle>();
                foreach (Vehicle vehicle in list)
                {
                    if (vehicle.Id == id.Trim().ToUpper().Replace("-", ""))
                    {
                        query.Add(vehicle);
                    }
                }
                return query;
            }
            #endregion
            #region ByType()
            /// <returns>A list of vehicles by class</returns>
            static public List<Vehicle> ByType(List<Vehicle> list, Type type)
            {
                List<Vehicle> query = new List<Vehicle>();
                foreach (Vehicle vehicle in list)
                {
                    if (vehicle.GetType() == type)
                    {
                        query.Add(vehicle);
                    }
                }
                return query;
            }
            #endregion
            #region ByColor()
            /// <returns>A list of vehicles by color</returns>
            static public List<Vehicle> ByColor(List<Vehicle> list, string color)
            {
                List<Vehicle> query = new List<Vehicle>();

                Regex rgx = new Regex("[^a-zA-Z0-9]");
                color = rgx.Replace(color, "");

                foreach (Vehicle vehicle in list)
                {
                    if (vehicle.Color == color)
                    {
                        query.Add(vehicle);
                    }
                }
                return query;
            }
            #endregion
            #region ByFloor()
            /// <returns>A list of vehicles on a floor</returns>
            static public List<Vehicle> ByFloor(MyGarage garage, List<Vehicle> list, string n)
            {
                List<Vehicle> query = new List<Vehicle>();
                int i = 0;
                if (int.TryParse(n, out i))
                {
                    foreach (Location location in garage.Locations)
                    {
                        if (location.Index == i-1)
                        {
                            foreach (Row row in location.Rows)
                            {
                                foreach (Lot lot in row.Lots)
                                {
                                    foreach (Vehicle vehicle in lot.Vehicles)
                                    {
                                        query.Add(vehicle);
                                    }
                                }
                            }
                            return query;
                        }
                    }
                }
                return list;
            }
            #endregion
            #region ByLotIndex()
            /// <returns>A list of vehicles on a lot</returns>
            static public List<Vehicle> ByLotIndex(MyGarage garage, List<Vehicle> list, string n)
            {
                int i = 0;
                if (int.TryParse(n, out i))
                {

                    List<Vehicle> query = new List<Vehicle>();
                    foreach (Location location in garage.Locations)
                    {
                        foreach (Row row in location.Rows)
                        {
                            foreach (Lot lot in row.Lots)
                            {
                                if (lot.Number == i-1)
                                {
                                    foreach (Vehicle vehicle in lot.Vehicles)
                                    {
                                        query.Add(vehicle);
                                    }
                                    return query;
                                }
                            }
                        }
                    }
                }
                return list;
            }
            #endregion
        }
    }
}