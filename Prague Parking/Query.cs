using Newtonsoft.Json;
using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

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
    }
}