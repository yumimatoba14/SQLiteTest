using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLiteTest.Models
{
    [Table("Tasks")]
    public class Task
    {
        [Key]
        public int WorkflowId { get; set; }

        /// <summary>
        /// SubId shall be identical in a workflow.
        /// </summary>
        [Key]
        public int SubId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Remark { get; set; }

        public virtual Workflow Workflow { get; set; }
    }
}
