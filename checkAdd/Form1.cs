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
            checkListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            accountsListView.FullRowSelect = true;
            tabControl1.Selected += new TabControlEventHandler(TabControl1_Selected);


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
            string stNum = stNumBox.Text;
            string stName = stNameBox.Text;
            string city = cityBox.Text;
            string state = stateBox.Text;
            string zip = zipBox.Text;

            pseudoAccount act = new pseudoAccount(firstName, lastName, routingNumber, accountNumber, stNum, city, state, zip);
            // Jonathan Teel commented this out to test without using database, if I forget to remove the comments and 
            // you pull and notice it not working remove my comment marks
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
            ListViewItem lvi = new ListViewItem(new string[] { act.getAccountNum(), act.getFirstName(), act.getLastName(), act.getNumOfChecks().ToString("0000"), act.getCurBal().ToString() });
            accountsListView.Items.Add(lvi);
            firstNameBox.Clear();
            lastNameBox.Clear();
            routingBox1.Clear();
            accountBox1.Clear();
            streetNumBox.Clear();
            streetNameBox.Clear();
            cityBox.Clear();
            zipBox.Clear();

        }

        private void addChkButton_Click(object sender, EventArgs e)
        {
            string acctNum = accountBox2.Text;
            string routNum = routingBox2.Text;
            double ammount = Convert.ToDouble(ammountBox.Text);
            string fName = fNameBox2.Text;
            string lName = lNameBox2.Text;

            int num = Convert.ToInt32(checkNumBox.Text);
            pseudoCheck check = new pseudoCheck(acctNum, routNum, ammount, num);
            var act = database.getAccountByNum(acctNum);
                ListViewItem lvi = new ListViewItem(new string[] { acctNum,  act.getFirstName(), act.getLastName(), num.ToString(), ammount.ToString()  });
            checkListView.Items.Add(lvi);
            database.getAccountByNum(acctNum).addCheck(check);
            updateAccountListView();
            updateCheckListView();
            accountBox2.Clear();
            routingBox2.Clear();
            ammountBox.Clear();
            
        }

        public void updateCheckListView()
        {
            
            checkListView.Sort();
        }

        public void updateAccountListView()
        {
            int i = 0;
            foreach (ListViewItem lvi in accountsListView.Items)
            {
                pseudoAccount act = database.getAccountByNum(lvi.SubItems[0].Text);
                if (act != null)
                {
                    lvi.SubItems[3].Text = act.getNumOfChecks().ToString();
                    lvi.SubItems[4].Text = act.getCurBal().ToString();
                }
                else
                {
                    lvi.Remove();
                }
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

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (accountNumSearchBox.Text != null && accountNameSearchBox.Text != null)
            {
                // message box choose one
                // then change rest to else if
            }
            if (accountNumSearchBox != null)
            {
                string sNum = accountNumSearchBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in accountsListView.Items)
                {
                    string actNum = lvi.SubItems[0].Text;
                    if (actNum == sNum)
                    {
                        accountsListView.Items[i].Selected = true;
                        accountsListView.TopItem = accountsListView.Items[i];
                        accountsListView.Select();
                        return;
                    }
                    i++;
                }
            }
            else if (accountNameSearchBox.Text != null)
            {
                string sName = accountNameSearchBox.Text;
                int i = 0;
                foreach(ListViewItem lvi in accountsListView.Items)
                {
                    string fName = lvi.SubItems[1].ToString();
                    string lName = lvi.SubItems[2].ToString();
                    if (fName == sName || lName == sName )
                    {
                        accountsListView.Items[i].Selected = true;
                        accountsListView.TopItem = accountsListView.Items[i];
                        accountsListView.Select();

                        return;
                    }
                    i++;
                }
            }
        }

        private void updateChecksPage_Click(object sender, EventArgs e)
        {

        }

        private void updateAcctSearchButton_Click(object sender, EventArgs e)
        {
            if (accountNumSearchBox.Text != null)
            {
                string actNum = accountNumSearchBox.Text;
                var act = database.getAccountByNum(actNum);
                fNameBox.Text = act.getFirstName();
                lNameBox.Text = act.getLastName();
                rNumBox.Text = act.getRoutingNum();
                acctNumBox.Text = act.getAccountNum();
                streetNumBox.Text = act.getStNum();
                streetNameBox.Text = act.getStName();
                updateActCityBox.Text = act.getCity();
                updateActStateBox.Text = act.getState();
                zipNumBox.Text = act.getZip();
            }
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            var act = database.getAccountByNum(accountNumSearchBox.Text);
            act.setFirstName(fNameBox.Text);
            act.setLastName(lNameBox.Text);
            act.setRoutingNum(rNumBox.Text);
            act.setAccountNum(acctNumBox.Text);
            act.setStNum(streetNumBox.Text);
            act.setStName(streetNameBox.Text);
            act.setCity(updateActCityBox.Text);
            act.setState(updateActStateBox.Text);
            act.setZip(zipNumBox.Text);


        }

        private void deleteAccountButton_Click(object sender, EventArgs e)
        {
            
        }

        private void viewChecksSearchButton_Click(object sender, EventArgs e)
        {
            if (viewCheckActNumBox.Text != null && viewCheckNameBox.Text != null)
            {
                // message box choose one
                // then change rest to else if
            }
            if (viewCheckActNumBox != null)
            {
                string sNum = viewCheckActNumBox.Text;
                string sCNum = viewCheckNumBox.Text;
                int i = 0;
                foreach (ListViewItem lvi in checkListView.Items)
                {
                    string actNum = lvi.SubItems[0].Text;
                    string cNum = lvi.SubItems[3].Text;
                    if (actNum == sNum && cNum == sCNum)
                    {
                        checkListView.Items[i].Selected = true;
                        checkListView.TopItem = accountsListView.Items[i];
                        checkListView.Select();
                        return;
                    }
                    i++;
                }
            }
            else if (viewCheckNameBox.Text != null)
            {
                string sName = viewCheckActNumBox.Text;
                string sCNum = viewCheckNumBox.Text;

                int i = 0;
                foreach (ListViewItem lvi in accountsListView.Items)
                {
                    string fName = lvi.SubItems[1].ToString();
                    string lName = lvi.SubItems[2].ToString();
                    string cNum = lvi.SubItems[3].Text;

                    if ((fName == sName || lName == sName) && cNum == sCNum)
                    {
                        checkListView.Items[i].Selected = true;
                        checkListView.TopItem = accountsListView.Items[i];
                        checkListView.Select();

                        return;
                    }
                    i++;
                }
            }
        }
    }
}
