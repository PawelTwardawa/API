using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using trojkaty_api.DataAccess.Models;

namespace trojkaty_api.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<ValidateQuestion> ValidateQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().HasMany(x => x.Groups).WithOne(x => x.Question);
            modelBuilder.Entity<GroupQuestion>().HasOne(x => x.Question).WithMany(x => x.Groups);

            modelBuilder.Entity<GroupQuestion>().HasKey(x => new { x.GroupId, x.QuestionId });
            modelBuilder.Entity<GroupQuestion>().HasOne(x => x.Question).WithMany(x => x.Groups).HasForeignKey(x => x.QuestionId);
            modelBuilder.Entity<GroupQuestion>().HasOne(x => x.Group).WithMany(x => x.Questions).HasForeignKey(x => x.GroupId);

            modelBuilder.Entity<UserGroup>().HasKey(x => new { x.GroupId, x.UserId });
            modelBuilder.Entity<UserGroup>().HasOne(x => x.User).WithMany(x => x.Groups).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserGroup>().HasOne(x => x.Group).WithMany(x => x.Users).HasForeignKey(x => x.GroupId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
