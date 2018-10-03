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

namespace checkAdd
{
    public partial class ammountLabel : Form
    {
        CheckPlusDB cpdb;
        AccountSQLer accSQL;
        Acct_checkSQLer acct_chkSQL;

        psudoDatabase database = new psudoDatabase();
        public ammountLabel()
        {
            InitializeComponent();
            accountsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            cpdb = new CheckPlusDB();
            cpdb.Database.Connection.ConnectionString = "Data Source=localhost;Initial Catalog=CheckPlus;Integrated Security=True";
            
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

        private void addActButton_Click(object sender, EventArgs e)
        {
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            string routingNumber = routingBox1.Text;
            string accountNumber = accountBox1.Text;
            psudoAccount act = new psudoAccount(firstName, lastName, routingNumber, accountNumber);      

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
            psudoCheck check = new psudoCheck(acctNum, routNum, ammount);
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
                psudoAccount act = database.getAccountByNum(lvi.SubItems[0].Text);
                lvi.SubItems[3].Text = act.getNumOfChecks().ToString();
                lvi.SubItems[4].Text = act.getCurBal().ToString();
            }
        }
    }
}
