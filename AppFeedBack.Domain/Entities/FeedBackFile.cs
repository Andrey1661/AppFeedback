using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFeedBack.Domain.Entities
{
    public class FeedBackFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        [ForeignKey("Feedback")]
        public Guid FeedbackId { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}
