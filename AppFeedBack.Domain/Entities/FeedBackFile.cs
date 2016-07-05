using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFeedBack.Domain.Entities
{
    /// <summary>
    /// Модель прикрепленного к отзыву файла
    /// </summary>
    public class FeedbackFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Физический путь к файлу, хранящемуся на сервере
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        [ForeignKey("Feedback")]
        public Guid FeedbackId { get; set; }

        /// <summary>
        /// Ссылка на родительский отзыв
        /// </summary>
        public virtual Feedback Feedback { get; set; }

        public FeedbackFile() { }

        public FeedbackFile(string path)
        {
            FilePath = path;
        }
    }
}
