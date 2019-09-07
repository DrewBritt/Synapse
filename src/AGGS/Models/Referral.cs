using System;
using System.Collections.Generic;
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
    }
}
