namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ContextSIGE : DbContext
    {
        public ContextSIGE()
            : base("name=ContextSIGE")
        {
        }
        public virtual DbSet<FileXMLPath> FileXMLPath { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<MeasurementPoint> MeasurementPoint { get; set; }
        public virtual DbSet<MeasurementTag> MeasurementTag { get; set; }
        public virtual DbSet<MeasurerPoint> MeasurerPoint { get; set; }
        public virtual DbSet<PointSource> PointSource { get; set; }
        public virtual DbSet<PointType> PointType { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<vJobs> vJobs { get; set; }
        public virtual DbSet<vMeter> vMeter { get; set; }
        public virtual DbSet<vMeterByTag> vMeterByTag { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileXMLPath>()
                .Property(e => e.sPath)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.sMessage)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.sPointName)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fGenerationLimitInfKw)
                .HasPrecision(9, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fGenerationLimitSupKw)
                .HasPrecision(9, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fConsumptionLimitInfKw)
                .HasPrecision(9, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fConsumptionLimitSupKw)
                .HasPrecision(9, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fToleranceMaxGenerationPercent)
                .HasPrecision(5, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fToleranceMinGenerationPercent)
                .HasPrecision(5, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fToleranceMaxConsumptionPercent)
                .HasPrecision(5, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fToleranceMinConsumptionPercent)
                .HasPrecision(5, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fVoltagePrimaryTP)
                .HasPrecision(7, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fVoltageSecondaryTP)
                .HasPrecision(7, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.sRelationTP)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fPrimaryChainTC)
                .HasPrecision(7, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fSecondyChainTC)
                .HasPrecision(7, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.sRelationTC)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fThermalFactor)
                .HasPrecision(7, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fPeakContractedDemandKw)
                .HasPrecision(9, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fOffPeakContractedDemanKw)
                .HasPrecision(9, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.sInstalationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.sLatitudePoint)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.sLongitudePoint)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fToleranceM1M2Percent)
                .HasPrecision(5, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .Property(e => e.fZeroThreshold)
                .HasPrecision(7, 2);

            modelBuilder.Entity<MeasurementPoint>()
                .HasMany(e => e.MeasurerPoint)
                .WithRequired(e => e.MeasurementPoint)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MeasurementPoint>()
                .HasMany(e => e.MeasurementTag)
                .WithMany(e => e.MeasurementPoint)
                .Map(m => m.ToTable("MeasurementPoint_MeasurementTag").MapLeftKey("iPointID").MapRightKey("iTagID"));

            modelBuilder.Entity<MeasurementTag>()
                .Property(e => e.sName)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurementTag>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<MeasurerPoint>()
                .Property(e => e.cMeasurerFunction)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MeasurerPoint>()
                .Property(e => e.sCodeCCEE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MeasurerPoint>()
                .Property(e => e.cContentIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MeasurerPoint>()
                .Property(e => e.cContentOUT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PointSource>()
                .Property(e => e.cPointSourceID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PointSource>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<PointSource>()
                .HasMany(e => e.Task)
                .WithRequired(e => e.PointSource)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PointType>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<PointType>()
                .HasMany(e => e.MeasurementPoint)
                .WithRequired(e => e.PointType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Schedule>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Schedule>()
                .Property(e => e.cScheduleType)
                .IsFixedLength()
                .IsUnicode(false);

            

            modelBuilder.Entity<Task>()
                .Property(e => e.cPointSourceID)
                .IsFixedLength()
                .IsUnicode(false);

            

          

            modelBuilder.Entity<vJobs>()
                .Property(e => e.cPointSourceID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<vJobs>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<vJobs>()
                .Property(e => e.cScheduleType)
                .IsFixedLength()
                .IsUnicode(false);
            

            modelBuilder.Entity<vMeter>()
                .Property(e => e.sPointName)
                .IsUnicode(false);

            modelBuilder.Entity<vMeter>()
                .Property(e => e.sCodeCCEE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<vMeter>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<vMeter>()
                .Property(e => e.sRelationTP)
                .IsUnicode(false);

            modelBuilder.Entity<vMeter>()
                .Property(e => e.sRelationTC)
                .IsUnicode(false);

            modelBuilder.Entity<vMeterByTag>()
                .Property(e => e.sPointName)
                .IsUnicode(false);

            modelBuilder.Entity<vMeterByTag>()
                .Property(e => e.sName)
                .IsUnicode(false);

            modelBuilder.Entity<vMeterByTag>()
                .Property(e => e.sCodeCCEE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<vMeterByTag>()
                .Property(e => e.sRelationTP)
                .IsUnicode(false);

            modelBuilder.Entity<vMeterByTag>()
                .Property(e => e.sRelationTC)
                .IsUnicode(false);
        }
    }
}
