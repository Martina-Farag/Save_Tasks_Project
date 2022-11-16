using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using TasksApp.Model;

namespace TasksApp
{
    public partial class ContextDB : DbContext
    {
        public ContextDB()
            : base("name=ContextDB")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        //entities
        public DbSet<Task> Tasks { get; set; }
    }
}
