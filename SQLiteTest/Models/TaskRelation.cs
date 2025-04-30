using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLiteTest.Models
{
    [Table("TaskRelations")]
    public class TaskRelation
    {
        [Required]
        public int WorkflowId { get; set; }

        [Required]
        public int PrevTaskSubId { get; set; }

        [Required]
        public int NextTaskSubId { get; set; }

        public virtual Task PrevTask { get; set; }

        public virtual Task NextTask { get; set; }
    }
}
