using System.ComponentModel.DataAnnotations.Schema;

namespace AGGS.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte GradeLevel { get; set; }
    }
}
