using App.Enumerations;
using Data;
using Models;
using Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service
{
    class DeskService
    {
        private DeskData deskData { get; set; } 

        public DeskService(DeskData deskData)
        {
            this.deskData = deskData;
        }
        public void ExecuteAction(AdminDesk menuAdminDeskSelected)
        {
            switch (menuAdminDeskSelected)
            {
                case AdminDesk.Create:
                    Desk newDesk = new Desk();
                    Console.Write("New Desk Number, ex: A-001: ");
                    newDesk.Number = Console.ReadLine();
                    Console.Write("New Desk Description: ");
                    newDesk.Description = Console.ReadLine();
                    deskData.CreateDesk(newDesk);
                    Console.Write("Desk created");
                    break;

                case AdminDesk.Edit:
                    Console.Write("Write the Desk Number: ");
                    var deskFound = deskData.FindDesk(Console.ReadLine());

                    while (deskFound == null)
                    {
                        Console.Write("Write the Desk Number: ");
                        deskFound = deskData.FindDesk(Console.ReadLine());
                    }

                    Console.Write("Desk Number, ex: A-001: ");
                    deskFound.Number = Console.ReadLine();
                    Console.Write("Desk Description: ");
                    deskFound.Description = Console.ReadLine();

                    var statusValidation = false;
                    string statusOption = "";

                    while (!statusValidation)
                    {
                        Console.Write("Desk Status, Active = 1, Inactive = 2, Block = 3: ");
                        statusOption = Console.ReadLine();
                        try
                        {
                            int statusNumber = int.Parse(statusOption);
                            if (statusNumber == 1 || statusNumber == 2 || statusNumber == 3)
                                statusValidation = true;
                            else
                                Console.WriteLine("Only accept 1, 2 or 3");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Only accept 1, 2 or 3");
                        }

                    }

                    deskFound.DeskStatus = Enum.Parse<DeskStatus>(statusOption);
                    deskData.EditDesk(deskFound);
                    Console.Write("Desk edited");
                    break;

                case AdminDesk.Delete:
                    Console.Write("Write the Desk Number: ");
                    var deskFoundDelete = deskData.FindDesk(Console.ReadLine());

                    while (deskFoundDelete == null)
                    {
                        Console.Write("Write the Desk Number: ");
                        deskFoundDelete = deskData.FindDesk(Console.ReadLine());
                    }

                    deskData.DeleteDesk(deskFoundDelete.DeskId);
                    Console.Write("Desk deleted");
                    break;

                case AdminDesk.Block:
                    Console.Write("Write the Desk Number: ");
                    var deskFoundBlock = deskData.FindDesk(Console.ReadLine());

                    while (deskFoundBlock == null)
                    {
                        Console.Write("Write the Desk Number: ");
                        deskFoundBlock = deskData.FindDesk(Console.ReadLine());
                    }

                    deskFoundBlock.DeskStatus = DeskStatus.Block;
                    deskData.EditDesk(deskFoundBlock);
                    Console.Write("Desk blocked");
                    break;
            }
        }

    }
}
