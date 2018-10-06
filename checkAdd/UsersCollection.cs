using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkPlus
{
    class UsersCollection
    {
        List<User> users;
        public List<User> getUsers()
        {
            return users;
        }
        public UsersCollection()
        {
            users = new List<User>();
            User usr = new User("Admin", "Administrator", "admin", "administrator", true, true);
            users.Add(usr);
            usr = new User("Sup", "Supervisor", "sup", "supervisor", false, true);
            users.Add(usr);
            usr = new User("Joe", "Shmo", "jshmo", "jshmoiscool");
            users.Add(usr);
        }

        public void addUser(User usr)
        {
            users.Add(usr);
        }

        public bool deleteByName(string fName, string lName)
        {
            foreach (User usr in users)
            {
                if (usr.getFirstName() == fName && usr.getLastName() == lName)
                {
                    users.Remove(usr);
                    return true;
                }
            }
            return false;
        }

        public bool deleteByNum(int userID)
        {
            foreach (User usr in users)
            {
                if (usr.getUserID() == userID)
                {
                    users.Remove(usr);
                    return true;
                }

            }
            return false;
        }
    }
}
