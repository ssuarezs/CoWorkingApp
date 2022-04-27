using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using App.Data.Tools;
using App.Enumerations;
using Data;
using Models;

namespace App.Service
{
    class UserService
    {
        private UserData userData { get; set; } 
        private DeskData deskData { get; set; } 
        private ReservationData reservationData { get; set; } 

        public UserService(UserData userData, DeskData deskData)
        {
            this.userData = userData;
            this.deskData = deskData;
            this.reservationData = new ReservationData();
        }

        public void ExecuteAction(AdminUser menuAdminUserSelected)
        {
            switch (menuAdminUserSelected)
            {
                case AdminUser.Create:
                User newUser = new User();
                    Console.Write("New User Name: ");
                    newUser.Name = Console.ReadLine();
                    Console.Write("New User Last Name: ");
                    newUser.LastName = Console.ReadLine();
                    Console.Write("New User Email: ");
                    newUser.Email = Console.ReadLine();
                    Console.Write("New User Password: ");
                    newUser.Password = EncryptData.GetPassword();
                    var userCreated = userData.CreateUser(newUser);
                    if (userCreated)
                        Console.Write("User Created");
                    else
                        Console.Write("User Not Created");
                    break;

                case AdminUser.Edit:
                    Console.WriteLine("Write user email: ");
                    var userFound = userData.FindUser(Console.ReadLine());

                    while (userFound == null)
                    {
                        Console.WriteLine("Write user email: ");
                        userFound = userData.FindUser(Console.ReadLine());
                    }
                    Console.Write("User Name: ");
                    userFound.Name = Console.ReadLine();
                    Console.Write("User Last Name: ");
                    userFound.LastName = Console.ReadLine();
                    Console.Write("User Email: ");
                    userFound.Email = Console.ReadLine();
                    Console.Write("User Password: ");
                    userFound.Password = EncryptData.GetPassword();
                    userData.EditUser(userFound);
                    Console.Write("User Edited");
                    break;

                case AdminUser.Delete:
                    Console.WriteLine("Write user email: ");
                    var userToDelete = userData.FindUser(Console.ReadLine());

                    while (userToDelete == null)
                    {
                        Console.WriteLine("Write user email: ");
                        userToDelete = userData.FindUser(Console.ReadLine());
                    }
                    Console.WriteLine($"Are you sure to delete {userToDelete.Name} {userToDelete.LastName}'s user? y/n");
                    if(Console.ReadLine() == "y")
                    {
                        userData.DeleteUser(userToDelete.UserId);
                        Console.Write("User Deleted");
                    }
                    
                    break;
                case AdminUser.ChangePassword:
                    Console.WriteLine("Write user email: ");
                    var userFoundPassword = userData.FindUser(Console.ReadLine());

                    while (userFoundPassword == null)
                    {
                        Console.WriteLine("Write user email: ");
                        userFoundPassword = userData.FindUser(Console.ReadLine());
                    }

                    Console.Write("User Password: ");
                    userFoundPassword.Password = EncryptData.GetPassword();
                    userData.EditUser(userFoundPassword);
                    Console.Write("Password changed");
                    break;
            }
        }

        public void ExecuteActionByUser(MenuUser menuUserSel, User user)
        {
            switch (menuUserSel)
            {
                case MenuUser.Reserve:
                    Console.WriteLine("Available Desks");
                    var deskList = deskData.GetAvailableDesk();
                    foreach (var item in deskList)
                    {
                        Console.WriteLine($"Desk {item.Number} - {item.Description}");
                    }

                    Console.Write("Write the Desk Number: ");
                    var newReservation = new Reservation();
                    var deskFound = deskData.FindDesk(Console.ReadLine());
                    var dateSelected = new DateTime();

                    while (deskFound == null)
                    {
                        Console.Write("Write the Desk Number: ");
                        deskFound = deskData.FindDesk(Console.ReadLine());
                    }

                    while (dateSelected.Year == 0001)
                    {
                        Console.Write("Write the reserve date (dd-mm-yyyy): ");
                        DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, DateTimeStyles.None, out dateSelected);
                    }

                    newReservation.UserId = user.UserId;
                    newReservation.DeskId = deskFound.DeskId;
                    newReservation.ReservationDate = dateSelected;

                    reservationData.CreateReservation(newReservation);
                    Console.Write("Reservation created!");
                    break;

                case MenuUser.Cancel:
                    Console.WriteLine("Actual Reservations");
                    var userReservationList = reservationData.GetReservationsByUser(user.UserId).ToList();
                    var deskUserList = deskData.GetAllDesks();
                    int indexReservation = 1;
                    foreach (var item in userReservationList)
                    {
                        var desk = deskUserList.FirstOrDefault(p => p.DeskId == item.DeskId);
                        var dateR = item.ReservationDate.ToString("dd-MM-yyy");
                        Console.WriteLine($"{indexReservation}: Desk {desk.Number} => {dateR}");
                        indexReservation++;
                    }

                    var indexReservationSelected = 0;
                    while(indexReservationSelected < 1 || indexReservationSelected >= indexReservation)
                    {
                        Console.Write("Write the reservation's index to cancel: ");
                        indexReservationSelected = int.Parse(Console.ReadLine());
                    }

                    var reservationToDelete = userReservationList[indexReservationSelected-1];
                    reservationData.CancelReservation(reservationToDelete.ReservationId);
                    Console.Write("Reservation canceled!");
                    break;

                case MenuUser.History:
                    Console.WriteLine("Actual Reservations");
                    var userReservationHistory = reservationData.GetReservationsHistoryByUser(user.UserId).ToList();
                    var deskUserHistory = deskData.GetAllDesks();
                    foreach (var item in userReservationHistory)
                    {
                        var desk = deskUserHistory.FirstOrDefault(p => p.DeskId == item.DeskId);
                        var dateR = item.ReservationDate.ToString("dd-MM-yyy");
                        var isActive = item.ReservationDate > DateTime.Now ? "(Active)" : "";
                        Console.ForegroundColor = item.ReservationDate > DateTime.Now ? ConsoleColor.Green : ConsoleColor.DarkGray;
                        Console.WriteLine($"> Desk {desk.Number} => {dateR} {isActive}");
                        Console.ResetColor();
                    }
                    break;

                case MenuUser.ChangePassword:
                    Console.Write("User New Password: ");
                    user.Password = EncryptData.GetPassword();
                    userData.EditUser(user);
                    Console.Write("Password changed!");
                    break;
            }
        }

        public User LoginUser(bool isAdmin)
        {
            bool loginResult = false;
            while (!loginResult)
            {
                Console.WriteLine("---User Login---");

                Console.Write("User: ");
                var userLogin = Console.ReadLine();

                Console.Write("Password: ");
                var passwordLogin = EncryptData.GetPassword();

                var userLogged = userData.Login(userLogin, passwordLogin, isAdmin);
                loginResult = userLogged != null;

                if (!loginResult) Console.WriteLine("Wrong User or Password");
                else return userLogged;
            }

            return null;
        }
    }
}
