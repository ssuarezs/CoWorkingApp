using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Models.Enumerations;

namespace Data
{
    public class DeskData
    {
        private JsonManager<Desk> jsonManager {get;set;}

        public DeskData()
        {
            jsonManager = new JsonManager<Desk>();
        }


        public bool CreateDesk(Desk newDesk)
        {

            try
            {
                var deskCollection = jsonManager.GetCollection();
                deskCollection.Add(newDesk);
                jsonManager.SaveCollection(deskCollection);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EditDesk(Desk editDesk)
        {
            try
            {
                var deskCollection = jsonManager.GetCollection();
                var index = deskCollection.FindIndex(p => p.DeskId == editDesk.DeskId);
                deskCollection[index] = editDesk;
                jsonManager.SaveCollection(deskCollection);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteDesk(Guid deskId)
        {
            try
            {
                var deskCollection = jsonManager.GetCollection();
                deskCollection.Remove(deskCollection.Find(p => p.DeskId == deskId));
                jsonManager.SaveCollection(deskCollection);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Desk FindDesk(string numberDesk)
        {
            var deskCollection = jsonManager.GetCollection();
            return deskCollection.FirstOrDefault(p => p.Number == numberDesk);
        }

        public List<Desk> GetAvailableDesk()
        {
            return jsonManager.GetCollection().Where(p => p.DeskStatus == DeskStatus.Active).ToList();
        }
        public List<Desk> GetAllDesks()
        {
            return jsonManager.GetCollection().ToList();
        }
    }
}
