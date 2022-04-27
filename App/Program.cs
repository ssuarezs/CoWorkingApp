using Data;
using System;
using App.Enumerations;
using App.Service;
using App.Data.Tools;
using Models;
using App.Tools;

namespace App
{
    class Program
    {
        static User ActiveUser { get; set; }
        static UserData UserDataService { get; set; } = new UserData();
        static DeskData DeskDataService { get; set; } = new DeskData();

        static UserService UserLogicService = new UserService(UserDataService, DeskDataService);
        static DeskService DeskLogicService = new DeskService(DeskDataService);

        static void Main(string[] args)
        {
            string rolSelected = "";

            Console.WriteLine("Welcome to CoWorking!");
            Console.WriteLine("");

            while (rolSelected != "1" && rolSelected != "2")
            {
                Console.WriteLine("1 = Admin, 2 = User");
                rolSelected = Console.ReadLine();
            }

            if (Enum.Parse<UserRole>(rolSelected) == UserRole.Admin)
            {
                UserLogicService.LoginUser(true);
                SpinnerManager.Show();
                Console.WriteLine("");

                var loginResult = true;

                while (loginResult)
                {

                    string menuAdminSelected = "";
                    while (menuAdminSelected != "1" && menuAdminSelected != "2" && menuAdminSelected != "3")
                    {
                        Console.WriteLine("---Admin Actions---");
                        Console.WriteLine("1 = Manage Desks , 2 = Manage Users, 3 = exit");
                        menuAdminSelected = Console.ReadLine();
                        if (menuAdminSelected == "3")
                            loginResult = false;
                    }

                    if (Enum.Parse<MenuAdmin>(menuAdminSelected) == MenuAdmin.ManageDesk)
                    {
                        string menuDeskSelected = "";
                        while (menuDeskSelected != "1" && menuDeskSelected != "2" && menuDeskSelected != "3" && menuDeskSelected != "4")
                        {
                            Console.WriteLine("Manage Deks");
                            Console.WriteLine("1 = Create, 2 = Edit, 3 = Delete, 4 = Block");
                            menuDeskSelected = Console.ReadLine();
                        }

                        AdminDesk menuAdminDeskSelected = Enum.Parse<AdminDesk>(menuDeskSelected);
                        DeskLogicService.ExecuteAction(menuAdminDeskSelected);
                    }
                    else if (Enum.Parse<MenuAdmin>(menuAdminSelected) == MenuAdmin.ManageUsers)
                    {
                        string menuUserSelected = "";
                        while (menuUserSelected != "1" && menuUserSelected != "2" && menuUserSelected != "3" && menuUserSelected != "4")
                        {
                            Console.WriteLine("Manage User");
                            Console.WriteLine("1 = Create, 2 = Edit, 3 = Delete, 4 = Change Password");
                            menuUserSelected = Console.ReadLine();
                        }

                        AdminUser menuAdminUserSelected = Enum.Parse<AdminUser>(menuUserSelected);
                        UserLogicService.ExecuteAction(menuAdminUserSelected);
                    }

                    Console.WriteLine("");
                    menuAdminSelected = "";

                }
            }
            else if (Enum.Parse<UserRole>(rolSelected) == UserRole.User)
            {
                ActiveUser =  UserLogicService.LoginUser(false);

                var loginResult = true;

                while (loginResult)
                {
                    string menuUserSelected = "";
                    while (menuUserSelected != "1" && menuUserSelected != "2" && menuUserSelected != "3" && menuUserSelected != "4"  && menuUserSelected != "0")
                    {
                        Console.WriteLine("---User Actions---");
                        Console.WriteLine("1 = Reserve Desk, 2 = Cancel Reserve, 3 = History, 4 = Change Password, 0 = exit");
                        menuUserSelected = Console.ReadLine();
                        if (menuUserSelected == "0")
                            loginResult = false;
                    }

                    MenuUser menuUserSel = Enum.Parse<MenuUser>(menuUserSelected);
                    UserLogicService.ExecuteActionByUser(menuUserSel, ActiveUser);

                    menuUserSelected = "";
                    Console.WriteLine("");
                }
            }
        }
    }
}
