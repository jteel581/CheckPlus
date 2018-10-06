using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkPlus
{
    class User
    {
        static int userCount = 0;
        int userId = 0;
        string firstName = "";
        string lastName = "";
        string userName = "";
        public bool adminPrivaleges = false;
        public bool supervisorPrivaleges = false;
        string password = "0000";

        public User(string first, string last, string usName, string pswd)
        {
            firstName = first;
            lastName = last;
            userName = usName;
            password = pswd;
            userId = userCount;
            userCount++;

        }

        public User(string first, string last, string usName, string pswd, bool admin, bool supervisor)
        {
            firstName = first;
            lastName = last;
            userName = usName;
            password = pswd;
            userId = userCount;
            userCount++;
            adminPrivaleges = admin;
            supervisorPrivaleges = supervisor;
        }

        public string getFirstName()
        {
            return firstName;
        }
        public string getLastName()
        {
            return lastName;
        }
        public int getUserID()
        {
            return userId;
        }

        public string getUserName()
        {
            return userName;
        }

        public bool changePassword(string oldPassword, string newPassword)
        {
            if (oldPassword == password)
            {
                password = newPassword;
                return true;
            }
            else
            {
                // complain about wrong old password and don't change

                return false;
            }
        }

        public void setPassword(string newPassword)
        {
            password = newPassword;
        }

        public string getPassword()
        {
            return password;
        }


        public void upgradeUser()
        {
            if(supervisorPrivaleges == false)
            {
                supervisorPrivaleges = true;
            }
            else if (adminPrivaleges == false)
            {
                adminPrivaleges = true;
            }
        }

        public void downgradeUser()
        {
            if(adminPrivaleges == true)
            {
                adminPrivaleges = false;
            }
            else if(supervisorPrivaleges == true)
            {
                supervisorPrivaleges = false;
            }
        }
    }
}
