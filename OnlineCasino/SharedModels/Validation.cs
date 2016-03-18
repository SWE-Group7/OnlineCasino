using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedModels
{
    public static class Validation
    {
        public static bool ValidateUserName(string username)
        {
            string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";
            char[] charArr = username.ToCharArray();

            foreach (char ch in charArr)
            {
                if (validCharacters.IndexOf(ch) == -1)
                    return false;
            }

            return true;
        }

        public static bool ValidatePassword(string password)
        {
            string validCharacters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_!@#$%^&*()-=+.,<>/?\|~";
            char[] charArr = password.ToCharArray();

            foreach (char ch in charArr)
            {
                if (validCharacters.IndexOf(ch) == -1)
                    return false;
            }

            return true;
        }

        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}
