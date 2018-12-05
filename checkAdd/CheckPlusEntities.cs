
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
{   //in alphabetical order by table name (at least, that's the goal)

    //----------------------------------------------------
    //ACCOUNT
    [Table("account")]
    public class Account
    {
        [Key]
        public int Account_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string First_name_2 { get; set; }
        public string Last_name_2 { get; set; }
        public int Bank_id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip_code { get; set; }
        public string Account_number { get; set; }
        public string Phone_number { get; set; }
    }


    //----------------------------------------------------
    //ACCT_CHECK
    [Table("acct_check")]
    public class Acct_check
    {
        [Key]
        public int Acct_check_id { get; set; }
        public int Account_id { get; set; }
        public Decimal Amount { get; set; }
        public DateTime Date_written { get; set; }
        public string Check_number { get; set; }
        public DateTime Date_received { get; set; }
        public int? Amount_paid { get; set; }
        public DateTime? Date_paid { get; set; }
        public DateTime? Response_date { get; set; }
        public DateTime? Letter1_send_date { get; set; }
        public DateTime? Letter2_send_date { get; set; }
        public DateTime? Letter3_send_date { get; set; }
        public int Client_id { get; set; }
    }


    //----------------------------------------------------
    //BANK
    [Table("bank")]
    public class Bank
    {
        [Key]
        public int Bank_id { get; set; }
        public string Bank_nm { get; set; }
        public string Routing_number { get; set; }
        public string Contact_nm { get; set; }
        public string Contact_email { get; set; }
        public string Contact_phone { get; set; }
        public string Bank_address { get; set; }
        public string Bank_city { get; set; }
        public string Bank_state { get; set; }
        public string Bank_country { get; set; }
        public string Bank_zip { get; set; }
    }


    //----------------------------------------------------
    //CLIENT
    [Table("client")]
    public class Client
    {
        [Key]
        public int Client_id { get; set; }
        public string Client_nm { get; set; }
        public Decimal Default_fee { get; set; }
        public int Days_bw_letters { get; set; }
    }


    //----------------------------------------------------
    //CP USER
    [Table("cp_user")]
    public class Cp_user
    {
        [Key]
        public int Cp_user_id { get; set; }
        public int? Client_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Username { get; set; }
        public string User_password { get; set; }
        public string User_role_cd { get; set; }
    }


    //----------------------------------------------------
    //LETTER
    [Table("letter")]
    public class Letter
    {
        [Key]
        public int Letter_stage_id { get; set; }
        public int Client_id { get; set; }
        public string Letter1_text { get; set; }
        public string Letter2_text { get; set; }
        public string Letter3_text { get; set; }
    }

    public class CheckPlusDB : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Acct_check> Acct_checks { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Cp_user> Cp_Users { get; set; }
        public virtual DbSet<Letter> Letters { get; set; }
    }
}