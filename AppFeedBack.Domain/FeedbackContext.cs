using System.Data.Entity;
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

            modelBuilder.Entity<Feedback>()
                .HasMany(t => t.AttachedFiles)
                .WithRequired(t => t.Feedback)
                .HasForeignKey(t => t.FeedbackId)
                .WillCascadeOnDelete(true);
        }
    }
}