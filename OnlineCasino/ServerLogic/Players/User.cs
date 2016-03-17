using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    class User //sign or register
    {
        public int UserID;
        public string Username;
        public string Password;
        public string EmailAddress;
        public double Balance;

        public User()
        {
         
        }
        public User(int userid, string username, string pwd, string email, double money) {
            //set user info to the class user info variables


        }
        public void GetUser() { //returns user info. all at once.


        }
        public int Get_UserID(){ return UserID; }


    

       

    }
}
