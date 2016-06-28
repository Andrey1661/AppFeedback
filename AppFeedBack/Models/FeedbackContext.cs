using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AppFeedBack.Models
{
    public class FeedbackContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }
    }
}