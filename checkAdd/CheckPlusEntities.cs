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

namespace CheckPlusEntities
{   //in alphabetical order by table name (at least, that's the goal)
    [Table("account")]
    class Account
    {
        [Key]
        public int Account_id { get; set; }

        [ForeignKey("entity")]
        public int Entity_id_1 { get; set; }
        public Entity Entity_1 { get; set; }

        [ForeignKey("entity")]
        public int Entity_id_2 { get; set; }
        public Entity Entity_2 { get; set; }

        public string Account_number { get; set; }
        public string Routing_number { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime? Date_end { get; set; }
    }

    [Table("account_check")]
    class Account_check
    {
        [Key]
        public int Account_check_id { get; set; }

        [ForeignKey("account")]
        public int Account_id { get; set; }
        public Account Account { get; set; }

        public double Amount { get; set; }
        public DateTime Date_received { get; set; }
    }

    [Table("account_type")]
    class Account_type
    {
        [Key]
        public int Account_type_id { get; set; }
        public string Account_type_nm { get; set; }
    }

    [Table("addr_type")]
    class Addr_type
    {
        [Key]
        public int Addr_type_id { get; set; }
        public string Addr_type_cd { get; set; }
        public string Addr_type_nm { get; set; }
    }

    [Table("address")]
    class Address
    {
        [Key]
        public int Address_id { get; set; }

        [ForeignKey("citystate")]
        public int Citystate_id { get; set; }
        public Citystate Citystate { get; set; }

        [ForeignKey("addr_type")]
        public int Addr_type_id { get; set; }
        public Addr_type Addr_Type { get; set; }

        public string Address_nm { get; set; }
    }

    [Table("citystate")]
    class Citystate
    {
        [Key]
        public int Citystate_id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string State_code { get; set; }
        public string Country { get; set; }
        public string Postal_code { get; set; }
    }

    [Table("ent_addr_rel")]
    class Ent_addr_rel
    {
        [Key]
        public int Ent_addr_rel_id { get; set; }

        [ForeignKey("entity")]
        public int Entity_id { get; set; }
        public Entity Entity { get; set; }

        [ForeignKey("address")]
        public int Address_id { get; set; }
        public Address Address { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime? Date_end { get; set; }
    }

    [Table("entity")]
    class Entity
    {
        [Key]
        public int Entity_id { get; set; }

        [ForeignKey("entity_type")]
        public int Entity_type_id { get; set; }
        public Entity_type Entity_type { get; set; }
    }


    [Table("entity_type")]
    class Entity_type
    {
        [Key]
        public int Entity_type_id { get; set; }
        public string Entity_type_nm { get; set; }
    }

    [Table("letter")]
    class Letter
    {
        [Key]
        public int Letter_id { get; set; }

        [ForeignKey("account_check")]
        public int Account_check_id { get; set; }
        public Account_check Account_check { get; set; }

        [ForeignKey("letter_stage")]
        public int Letter_stage_id { get; set; }
        public Letter_stage Letter_stage { get; set; }

        public DateTime? Date_sent { get; set; }
        public DateTime? Date_response { get; set; }
    }

    [Table("leter_stage")]
    class Letter_stage
    {
        [Key]
        public int Letter_stage_id { get; set; }
        public string Letter_stage_nm { get; set; }
        public string Default_text { get; set; }
    }

    [Table("organization")]
    class Organization
    {
        [Key]
        [ForeignKey("entity")]
        public int Organization_id { get; set; }
        public Entity Entity { get; set; }

        public string Organization_nm { get; set; }
    }

    [Table("Person")]
    class Person
    {
        [Key]
        [ForeignKey("entity")]
        public int Person_id { get; set; }
        public Entity Entity { get; set; }

        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Middle_name { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
    }

    class CheckPlusDB : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Account_check> Account_Checks { get; set; }
        public virtual DbSet<Account_type> Account_Types { get; set; }
        public virtual DbSet<Addr_type> Addr_Types { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Citystate> Citystates { get; set; }
        public virtual DbSet<Ent_addr_rel> Ent_Addr_Rels { get; set; }
        public virtual DbSet<Entity> Entities { get; set; }
        public virtual DbSet<Entity_type> Entity_Types { get; set; }
        public virtual DbSet<Letter> Letters { get; set; }
        public virtual DbSet<Letter_stage> Letter_Stages { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Person> People { get; set; }

    }
}