namespace WebApiGeneratorXml.DataConnection.ION
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BaseContexION : DbContext
    {
        public BaseContexION()
            : base("name=BaseContexION")
        {
        }

        public virtual DbSet<Alarm> Alarm { get; set; }
        public virtual DbSet<Quantity> Quantity { get; set; }
        public virtual DbSet<Source> Source { get; set; }
        public virtual DbSet<DataLog2> DataLog2 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quantity>()
                .HasMany(e => e.DataLog2)
                .WithRequired(e => e.Quantity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Source>()
                .HasMany(e => e.DataLog2)
                .WithRequired(e => e.Source)
                .WillCascadeOnDelete(false);
        }
    }
}
