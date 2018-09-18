using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace checkAdd
{
    public partial class ammountLabel : Form
    {
        //linq testing
        CheckPlusDB cpdb;
        PersonSQLer perSQL;
        AccountSQLer accSQL;
        //linq testing

        psudoDatabase database = new psudoDatabase();
        public ammountLabel()
        {
            InitializeComponent();

            //linq testing
            cpdb = new CheckPlusDB();
            perSQL = new PersonSQLer(cpdb);
            accSQL = new AccountSQLer(cpdb);
            //linq testing
        }

        /*  FUNCTION
         *  helpful for displaying success/error messages
         *      throughout the application
         */
        public void DisplayMessageNoResponse(string inMessage, string inCaption)
        {
            MessageBox.Show(inMessage, inCaption, MessageBoxButtons.OK);
        }

        private void addActButton_Click(object sender, EventArgs e)
        {
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            string routingNumber = routingBox1.Text;
            string accountNumber = accountBox1.Text;
            psudoAccount act = new psudoAccount(firstName, lastName, routingNumber, accountNumber);

            /*
            //start linq testing code chunk
            Person selPer = perSQL.SelectPerson(new Person()
            {   //all we really care about are First_name and Last_name for now
                Person_id = 1000,
                First_name = firstName,
                Middle_name = "",
                Last_name = lastName,
                Suffix = "",
                Title = ""
            });

            if(selPer != null)
            {
                Account insAcc = accSQL.InsertAccount(new Account()
                {
                    Account_id = (
                            from a in cpdb.Accounts
                            select a.Account_id).Max() + 1,
                    Entity_id_1 = selPer.Person_id,
                    Entity_id_2 = 1000000, //doesn't matter for now.....
                    Account_number = accountNumber,
                    Routing_number = routingNumber,
                    Date_start = DateTime.Now
                    }
                );

                //if attempt to insert account results in finding 
                //  an exitsting account
                //  display an error message
                if (insAcc != null) { DisplayMessageNoResponse("Error", "Account already exists."); }
                else { DisplayMessageNoResponse("Success!", "New account added."); }
            }
            else { DisplayMessageNoResponse("Error", "Person does not exist."); }
            //end linq testing code chunk
            */
            database.addAccount(act);
        }

        private void addChkButton_Click(object sender, EventArgs e)
        {

        }
    }
}
