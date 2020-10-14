namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Job")]
    public partial class Job
    {
        [Key]
        public int iJobID { get; set; }

        public int iTaskID { get; set; }

        public DateTime? dJobStartTime { get; set; }

        public DateTime? dJobEndTime { get; set; }
        public DateTime? dDataStartTime { get; set; }

        public DateTime? dDataEndTime { get; set; }

        public int? iStatus { get; set; }

        [StringLength(300)]
        public string sMessage { get; set; }

        public int? iReply { get; set; }

        public int iMeterID { get; set; }

        public bool bActive { get; set; }

        public virtual Task Task { get; set; }
    }
}
