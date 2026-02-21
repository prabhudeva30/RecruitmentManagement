using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class CandidateEntity
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Email { get; set; }
    }
}