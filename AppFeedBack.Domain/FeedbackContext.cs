using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AppFeedBack.Domain.Entities;

namespace AppFeedBack.Domain
{
    public class FeedbackContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(x => x.Feedbacks)
                .WithRequired(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .WillCascadeOnDelete(true);
        }
    }
}