namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Schedule")]
    public partial class Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Schedule()
        {
            Task = new HashSet<Task>();
        }

        [Key]
        public int iScheduleID { get; set; }

        [Required]       
        public string sDescription { get; set; }

        [Required]
        [StringLength(1)]
        public string cScheduleType { get; set; }

        public TimeSpan? tStartTime { get; set; }

        public TimeSpan? tEndTime { get; set; }

        public int? iRepeatInMinutes { get; set; }
        
        public DateTime? dNextRun { get; set; }

        public bool bEnabled { get; set; }

        public int iStatus { get; set; }

        public bool bExcluded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dDataStartTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dDataEndTime { get; set; }

        public bool? bForce { get; set; }

        public bool? bZeroData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
