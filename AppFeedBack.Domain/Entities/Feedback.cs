using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppFeedBack.Domain.Entities
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FilePath { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime PostDate { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<FeedBackFile> AttachedFiles { get; set; }

        public Feedback()
        {
            AttachedFiles = new List<FeedBackFile>();
        }
    }
}