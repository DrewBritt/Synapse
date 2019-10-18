namespace Synapse.Data.Models
{
    public partial class Grade
    {
        public int GradeId { get; set; }
        public int AssignmentId { get; set; }
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public string GradeValue { get; set; }
    }
}
