using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class ReservationData
    {
        private JsonManager<Reservation> jsonManager {get;set;}

        public ReservationData()
        {
            jsonManager = new JsonManager<Reservation>();
        }

        public bool CreateReservation(Reservation newReservation)
        {
            try
            {
                var ReservationCollection = jsonManager.GetCollection();
                ReservationCollection.Add(newReservation);
                jsonManager.SaveCollection(ReservationCollection);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CancelReservation(Guid reservationId)
        {
            try
            {
                var ReservationCollection = jsonManager.GetCollection();
                var indexReservation = ReservationCollection.FindIndex(p => p.ReservationId == reservationId);
                ReservationCollection.RemoveAt(indexReservation);
                jsonManager.SaveCollection(ReservationCollection);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Reservation> GetReservationsByUser(Guid userId)
        {
            try
            {
                var reservationCollection = jsonManager.GetCollection();
                return reservationCollection
                        .Where(p => p.UserId == userId && p.ReservationDate > DateTime.Now).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Reservation> GetReservationsHistoryByUser(Guid userId)
        {
            try
            {
                var reservationCollection = jsonManager.GetCollection();
                return reservationCollection
                        .Where(p => p.UserId == userId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
