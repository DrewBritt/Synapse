using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synapse.Data.Models
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
