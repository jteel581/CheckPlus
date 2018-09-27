using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace checkPlus
{
    public partial class ammountLabel : Form
    {
        CheckPlusDB cpdb;
        PersonSQLer perSQL;
        AccountSQLer accSQL;
        Account_checkSQLer acc_chkSQL;

        pseudoDatabase database = new pseudoDatabase();
        UsersCollection uc = new UsersCollection();
        User activeUser = null;
        public ammountLabel()
        {
            InitializeComponent();
            accountsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            cpdb = new CheckPlusDB();
            cpdb.Database.Connection.ConnectionString = "Data Source=localhost;Initial Catalog=CheckPlus;Integrated Security=True";

            perSQL = new PersonSQLer(cpdb);
            accSQL = new AccountSQLer(cpdb);
            acc_chkSQL = new Account_checkSQLer(cpdb);
        }

        /*  FUNCTION
         *  helpful for displaying success/error messages
         *      throughout the application
         */
        public void DisplayMessageNoResponse(string inHeading, string inMessage)
        {
            MessageBox.Show(inHeading, inMessage, MessageBoxButtons.OK);
        }

        private void addActButton_Click(object sender, EventArgs e)
        {
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            string routingNumber = routingBox1.Text;
            string accountNumber = accountBox1.Text;
            pseudoAccount act = new pseudoAccount(firstName, lastName, routingNumber, accountNumber);

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

            database.addAccount(act);
            ListViewItem lvi = new ListViewItem(new string[] { act.getAccountNum(), act.getFirstName(), act.getLastName(), act.getNumOfChecks().ToString("0000"), act.getCurBal().ToString() });
            accountsListView.Items.Add(lvi);
            firstNameBox.Clear();
            lastNameBox.Clear();
            routingBox1.Clear();
            accountBox1.Clear();

        }

        private void addChkButton_Click(object sender, EventArgs e)
        {
            string acctNum = accountBox2.Text;
            string routNum = routingBox2.Text;
            double ammount = Convert.ToDouble(ammountBox.Text);
            pseudoCheck check = new pseudoCheck(acctNum, routNum, ammount);
            database.getAccountByNum(acctNum).addCheck(check);
            updateListView();
            accountBox2.Clear();
            routingBox2.Clear();
            ammountBox.Clear();
            
        }
        public void updateListView()
        {
            int i = 0;
            foreach (ListViewItem lvi in accountsListView.Items)
            {
                pseudoAccount act = database.getAccountByNum(lvi.SubItems[0].Text);
                lvi.SubItems[3].Text = act.getNumOfChecks().ToString();
                lvi.SubItems[4].Text = act.getCurBal().ToString();
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string uName = usernameBox.Text;
            string pWord = passwordBox.Text;

            foreach(User usr in uc.getUsers())
            {
                if (usr.getUserName() == uName && usr.getPassword() == pWord)
                {
                    activeUser = usr;
                    return;
                }
            }
            // complain that username or password is incorrect
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            activeUser = null;
        }
    }
}
