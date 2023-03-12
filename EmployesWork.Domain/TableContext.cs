namespace EmployesWork.Domain
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TableContext : DbContext
    {
        public TableContext()
            : base("name=Db_Work")
        {
        }

        public virtual DbSet<Employe> Employes { get; set; }
        public virtual DbSet<Shedule> Shedules { get; set; }
        public virtual DbSet<WorkDay> WorkDays { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employe>()
                .HasMany(e => e.WorkDays)
                .WithRequired(e => e.Employe)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shedule>()
                .HasMany(e => e.Employes)
                .WithOptional(e => e.Shedule)
                .HasForeignKey(e => e.ShedulesId);

            modelBuilder.Entity<Shedule>()
                .HasMany(e => e.WorkDays)
                .WithRequired(e => e.Shedule)
                .HasForeignKey(e => e.ShedulesId)
                .WillCascadeOnDelete(false);
        }
    }
}
