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
        //db variables
        CheckPlusDB cpdb;
        Acct_holderSQLer acct_holdSQL;
        AccountSQLer accSQL;
        Acct_checkSQLer acct_chkSQL;

        pseudoDatabase database = new pseudoDatabase();
        UsersCollection uc = new UsersCollection();
        User activeUser = null;
        public ammountLabel()
        {
            InitializeComponent();
            accountsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            tabControl1.Selected += new TabControlEventHandler(TabControl1_Selected);

            //configuring db variables
            cpdb = new CheckPlusDB();
            cpdb.Database.Connection.ConnectionString = "Data Source=localhost;Initial Catalog=CheckPlus;Integrated Security=True";

            acct_holdSQL = new Acct_holderSQLer(cpdb);
            accSQL = new AccountSQLer(cpdb);
            acct_chkSQL = new Acct_checkSQLer(cpdb);
        }

        /*  FUNCTION
         *  helpful for displaying success/error messages
         *      throughout the application
         */
        public void DisplayMessageNoResponse(string inHeading, string inMessage)
        {
            MessageBox.Show(inHeading, inMessage, MessageBoxButtons.OK);
        }

        private void TabControl1_Selected(Object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex != 1 && tabControl1.SelectedIndex != 0)
            {
                if (activeUser == null)
                {
                    string message = "You must be logged in to access this page! Log in now?";
                    string caption = "User not logged in";
                    switch (MessageBox.Show(message, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            tabControl1.SelectedIndex = 1;
                            break;

                        case DialogResult.No:
                            tabControl1.SelectedIndex = 0;
                            break;

                        case DialogResult.Cancel:
                            tabControl1.SelectedIndex = 0;
                            break;

                    }
                }
            }
            
            

        }

        private void addActButton_Click(object sender, EventArgs e)
        {
            
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            string routingNumber = routingBox1.Text;
            string accountNumber = accountBox1.Text;
            //Izaac added these address strings for his data access function testing
            //Jonathan already had created the boxes
            string address = stNumBox.Text + " " + stNameBox.Text;
            string city = cityBox.Text;
            string state = stateBox.Text;
            string zip = zipBox.Text;
            //Izaac added this box and string
            string phnNum = phnNumBox.Text;

            //--------------------------------------------------------
            //start linq testing code chunk
            //--------------------------------------------------------
            Account tstAccount = accSQL.InsertAccount(
                accSQL.BuildAccount(firstName, lastName, routingNumber, accountNumber, address, city, state, zip, phnNum)
            );

            //if attempt to insert account results in finding an exitsting account
            if (tstAccount != null)
            {   //display an error message
                DisplayMessageNoResponse("Error", "Account already exists.");
            }
            else
            {   //otherwise, it was a successful account addition
                DisplayMessageNoResponse("Success!", "New account added.");
            }
            //--------------------------------------------------------
            //end linq testing code chunk
            //--------------------------------------------------------

            pseudoAccount act = new pseudoAccount(firstName, lastName, routingNumber, accountNumber);
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
            //Izaac added this string; box already existed
            string chkNum = checkNumBox.Text;
            double ammount = Convert.ToDouble(ammountBox.Text);
            //Izaac added this box and string
            DateTime dateWritten = Convert.ToDateTime(dateWrittenBox.Text);

            //--------------------------------------------------------
            //start linq testing code chunk
            //--------------------------------------------------------
            Acct_check tstAcct_check = acct_chkSQL.InsertAcct_check(
                acct_chkSQL.BuildAcct_check(acctNum, routNum, chkNum, ammount, dateWritten)
            );

            //if check record already existed
            if (tstAcct_check != null)
            {   //display an error message
                DisplayMessageNoResponse("Error", "Check already exists.");
            }
            else
            {   //otherwise, it was a successful account addition
                DisplayMessageNoResponse("Success!", "New check added.");
            }
            //--------------------------------------------------------
            //end linq testing code chunk
            //--------------------------------------------------------

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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
