﻿using SharedModels;
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
            if (!(Validation.ValidateUserName(username) && Validation.ValidatePassword(password) && Validation.ValidateEmail(email)))
                return null;

            User dbUser = new User();

            dbUser.FullName = fullName;
            dbUser.Username = username;
            dbUser.EmailAddress = email;
            dbUser.Salt = GenerateSalt();
            dbUser.Password = HashPassword(password, dbUser.Salt);
            dbUser.Balance = Properties.Settings.Default.StartingBalance;
            dbUser.Disabled = false;
            dbUser.CreationDate = DateTime.Now;
            dbUser.LastLoginDate = dbUser.CreationDate;

            try
            {
                using (OnlineCasinoEntity db = new OnlineCasinoEntity())
                {
                    db.Users.Add(dbUser);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ServerMain.WriteException("User.Register()", ex);
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
                        return null;

                    if (dbUser.Password == HashPassword(password, dbUser.Salt))
                        return dbUser;
                    else
                        return null;

                }
            }
            catch (Exception ex)
            {
                ServerMain.WriteException("EntityFrameworks.User.Login()", ex);
                return null;
            }
        }
        public static string HashPassword(string password, string salt)
        {

            SHA256 hasher = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            bytes = hasher.ComputeHash(bytes);

            return Encoding.UTF8.GetString(bytes);
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
            Random random = new Random();
            byte[] bytes = new byte[32];
            random.NextBytes(bytes);

            //Casts byte[32] to string
            string salt = new string(bytes.Select(b => (char)b).ToArray());
            return salt;
        }
    }
}