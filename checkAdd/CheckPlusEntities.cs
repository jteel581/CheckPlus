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
    class Account
    {
        [Key]
        public int Account_id { get; set; }
        public int Acct_holder_id { get; set; }
        public int? Acct_holder_id_2 { get; set; }
        public int Bank_id { get; set; }
        public int Address_id { get; set; }
        public string Account_number { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime? Date_end { get; set; }
        public string Phone_number { get; set; }
    }


    //----------------------------------------------------
    //ACCT_CHECK
    [Table("acct_check")]
    class Acct_check
    {
        [Key]
        public int Acct_check_id { get; set; }
        public int Account_id { get; set; }
        public double Amount { get; set; }
        public DateTime Date_written { get; set; }
        public string Check_number { get; set; }
        public DateTime Date_received { get; set; }
        public int? Amount_paid { get; set; }
        public DateTime? Date_paid { get; set; }
    }


    //----------------------------------------------------
    //ACCT HOLDER
    [Table("acct_holder")]
    class Acct_holder
    {
        [Key]
        public int Acct_holder_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Middle_name { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
    }


    //----------------------------------------------------
    //ADDRESS
    [Table("address")]
    class Address
    {
        [Key]
        public int Address_id { get; set; }
        public string Address_nm { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip_code { get; set; }
    }


    //----------------------------------------------------
    //ADDRESS
    [Table("bank")]
    class Bank
    {
        [Key]
        public int Bank_id { get; set; }
        public string Bank_nm { get; set; }
        public string Routing_number { get; set; }
        public string Contact_nm { get; set; }
        public string Contact_email { get; set; }
        public string Contact_phone { get; set; }
    }


    //----------------------------------------------------
    //CLIENT
    [Table("client")]
    class Client
    {
        [Key]
        public int Client_id { get; set; }
        public string Client_nm { get; set; }
        public double Default_fee { get; set; }
        public int Days_bw_letters { get; set; }
    }


    //----------------------------------------------------
    //CP USER
    [Table("cp_user")]
    class Cp_user
    {
        [Key]
        public int Cp_user_id { get; set; }
        public int? Client_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Username { get; set; }
        public string User_role_cd { get; set; }
    }


    //----------------------------------------------------
    //LETTER
    [Table("letter")]
    class Letter
    {
        [Key]
        public int Letter_stage_id { get; set; }
        public DateTime? Date_sent { get; set; }
        public DateTime? Date_response { get; set; }
    }

    //----------------------------------------------------
    //LETTER STAGE
    [Table("leter_stage")]
    class Letter_stage
    {
        [Key]
        public int Letter_stage_id { get; set; }
        public string Letter_stage_nm { get; set; }
        public string Default_text { get; set; }
    }
    
    class CheckPlusDB : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Acct_check> Acct_checks { get; set; }
        public virtual DbSet<Acct_holder> Acct_holders { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Cp_user> Cp_Users { get; set; }
        public virtual DbSet<Letter> Letters { get; set; }
        public virtual DbSet<Letter_stage> Letter_Stages { get; set; }
    }
}