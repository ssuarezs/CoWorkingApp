using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Models;
using App.Data.Tools;

namespace Data
{
    public class UserData
    {
        private JsonManager<User> jsonManager {get;set;}

        public UserData()
        {
            jsonManager = new JsonManager<User>();
        }

        public bool CreateAdmin()
        {
            var collection = jsonManager.GetCollection();

            if (!collection.Any(p => p.Name == "ADMIN" && 
                                    p.LastName == "ADMIN" && 
                                    p.Email == "ADMIN"))
            {
                try
                {
                    var adminUser = new User()
                    {
                        Name = "ADMIN",
                        LastName = "ADMIN",
                        Email = "ADMIN",
                        UserId = Guid.NewGuid(),
                        Password = EncryptData.EncryptText("4dmin!")
                    };

                    collection.Add(adminUser);
                    jsonManager.SaveCollection(collection);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CreateUser(User newUser)
        {
            newUser.Password = EncryptData.EncryptText(newUser.Password);

            try
            {
                var userCollection = jsonManager.GetCollection();
                userCollection.Add(newUser);
                jsonManager.SaveCollection(userCollection);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EditUser(User editUser)
        {
            editUser.Password = EncryptData.EncryptText(editUser.Password);

            try
            {
                var userCollection = jsonManager.GetCollection();
                var index = userCollection.FindIndex(p => p.UserId == editUser.UserId);
                userCollection[index] = editUser;
                jsonManager.SaveCollection(userCollection);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteUser(Guid userId)
        {
            try
            {
                var userCollection = jsonManager.GetCollection();
                userCollection.Remove(userCollection.Find(p => p.UserId == userId));
                jsonManager.SaveCollection(userCollection);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User FindUser(string email)
        {
                var userCollection = jsonManager.GetCollection();
                return userCollection.FirstOrDefault(p => p.Email == email);
        }

        public User Login(string User, string Password, bool isAdmin = false)
        {
            if (isAdmin) User = "ADMIN";
            var userCollection = jsonManager.GetCollection();
            var passwordHash = EncryptData.EncryptText(Password);
            var userFound = userCollection.FirstOrDefault(p => p.Name == User && p.Password == passwordHash);

            return userFound;
        }
    }
}
