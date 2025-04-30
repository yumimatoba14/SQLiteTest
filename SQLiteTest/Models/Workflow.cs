using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLiteTest.Models
{
    [Table("Workflows")]
    public class Workflow
    {
        public Workflow()
        {
            Tasks = new HashSet<Task>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Task> Tasks { get; }
    }
}
