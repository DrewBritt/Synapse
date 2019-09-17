using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AGGS.Models
{
    public partial class Referral
    {
        public int ReferralId { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public DateTime DateIssued { get; set; }
        public string Description { get; set; }

        [Column("Handled", TypeName = "bit")]
        public bool Handled { get; set; }
    }
}
