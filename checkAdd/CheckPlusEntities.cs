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
{
    [Table("entity_type")]
    class Entity_type
    {
        [Key]
        public int Entity_type_id { get; set; }
        public string Entity_type_nm { get; set; }
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

    [Table("leter_stage")]
    class Letter_stage
    {
        [Key]
        public int Letter_stage_id { get; set; }
        public string Letter_stage_nm { get; set; }
        public string Default_text { get; set; }
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

        public DateTime Date_sent { get; set; }
        public DateTime? Date_response { get; set; }
    }
}