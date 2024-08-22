using Microsoft.EntityFrameworkCore;
using to_do1;

namespace to_do1
{
    public class toDoContext:DbContext
    {
       
        public DbSet<person> People { get; set; }
        public DbSet<task> tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-96AGL16\\MYSQL;Initial Catalog=tododatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False");
        }  
        public toDoContext() { }
       /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Task>()
                .HasKey(t => t.Id);
        }
       */
    }
}