namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vJobs
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iJobID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iTaskID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iScheduleID { get; set; }

        public DateTime? dJobStartTime { get; set; }

        public DateTime? dJobEndTime { get; set; }

        public int? iJobStatus { get; set; }

        [StringLength(300)]
        public string sJobMessage { get; set; }

        public int? iReply { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iMeterID { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string cPointSourceID { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iSourceID { get; set; }

        public int? iTaskStatus { get; set; }
        [Key]
        [Column(Order = 13)]
        public bool bActive { get; set; }

        public DateTime? dJobDataStartTime { get; set; }
        public DateTime? dJobDataEndTime { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(30)]
        public string sDescription { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string cScheduleType { get; set; }

        public TimeSpan? tStartTime { get; set; }

        public TimeSpan? tEndTime { get; set; }

        public int? iRepeatInMinutes { get; set; }

        public DateTime? dNextRun { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool bEnabled { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iScheduleStatus { get; set; }

        [Key]
        [Column(Order = 10)]
        public bool bExcluded { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dScheduleDataStartTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dScheduleDataEndTime { get; set; }

        public bool? bForce { get; set; }

        public bool? bZeroData { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(250)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(20)]
        public string sPointSourceDescription { get; set; }

        [StringLength(200)]
        public string Signature { get; set; }
    }
}
