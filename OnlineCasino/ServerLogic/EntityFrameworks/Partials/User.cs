using SharedModels;
using SharedModels.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.EntityFrameworks
{
    public partial class User
    {
        public static User Register(string username, string password, string email, string fullName)
        {
            //Validate Email, Username, Password

            if (!Validation.ValidateUserName(username))
            {
                ServerMain.QueueClientError(ServerCommands.Register, "Invalid characters in username");
                return null;
            }

            if (!Validation.ValidateUserName(username))
            {
                ServerMain.QueueClientError(ServerCommands.Register, "Invalid characters in password");
                return null;
            }

            if (!Validation.ValidateUserName(username))
            {
                ServerMain.QueueClientError(ServerCommands.Register, "Invalid characters in email address");
                return null;
            }

            User dbUser = new User();
            try
            {
                using (OnlineCasinoEntity db = new OnlineCasinoEntity())
                {

                    //Check to see if username is taken
                    if (db.Users.Where(u => u.Username == username).Any())
                    {
                        ServerMain.QueueClientError(ServerCommands.Register, "Username already taken");
                        return null;
                    }
                        

                    dbUser.FullName = fullName;
                    dbUser.Username = username;
                    dbUser.EmailAddress = email;
                    dbUser.Salt = GenerateSalt();
                    dbUser.Password = HashPassword(password, dbUser.Salt);
                    dbUser.Balance = Properties.Settings.Default.StartingBalance;
                    dbUser.Disabled = false;
                    dbUser.CreationDate = DateTime.Now;
                    dbUser.LastLoginDate = dbUser.CreationDate;

                    db.Users.Add(dbUser);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ServerMain.WriteException("User.Register()", ex);
                ServerMain.QueueClientError(ServerCommands.Register, "Unknown error");
                dbUser = null;
            }

            return dbUser;

        }
        public static User Login(string username, string password)
        {
            User dbUser;

            try
            {
                using (OnlineCasinoEntity db = new OnlineCasinoEntity())
                {
                    dbUser = db.Users.FirstOrDefault(u => u.Username == username);

                    if (dbUser == null)
                    {
                        ServerMain.QueueClientError(ServerCommands.Login, "Invalid Username/Password");
                        return null;
                    }
                        

                    if (dbUser.Password == HashPassword(password, dbUser.Salt))
                        return dbUser;
                    else
                    {
                        ServerMain.QueueClientError(ServerCommands.Login, "Invalid Username/Password");
                        return null;
                    }
                        

                }

            }
            catch (Exception ex)
            {
                ServerMain.WriteException("EntityFrameworks.User.Login()", ex);
                ServerMain.QueueClientError(ServerCommands.Login, "Invalid Username/Password");
                return null;
            }
        }
        public static string HashPassword(string password, string salt)
        {

            SHA256 hasher = SHA256Managed.Create();
            byte[] bytes = Convert.FromBase64String(password + salt);
            bytes = hasher.ComputeHash(bytes);

            return Convert.ToBase64String(bytes);
        }

        public static bool UpdateBalance(int id, decimal balance)
        {
            try
            {
                using (OnlineCasinoEntity db = new OnlineCasinoEntity())
                {
                    User dbUser = db.Users.FirstOrDefault(u => u.UserID == id);
                    dbUser.Balance = balance;
                    db.SaveChanges();
                    return (dbUser.Balance == balance);
                }


            }
            catch (Exception ex)
            {
                ServerMain.WriteException("EntityFrameworks.User.UpdateBalance()", ex);
                return false;
            }
        }


        private static string GenerateSalt()
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[32];
            random.GetNonZeroBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
