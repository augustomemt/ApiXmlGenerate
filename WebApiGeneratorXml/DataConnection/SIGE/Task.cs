namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Task
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Task()
        //{

        //}

        //[Key]
        //public int iTaskID { get; set; }

        //public int iScheduleID { get; set; }

        //[Required]
        //[StringLength(1)]
        //public string cPointSourceID { get; set; }

        //public int iSourceID { get; set; }

        //public int iStatus { get; set; }


        ////public virtual PointSource PointSource { get; set; }

        //public virtual Schedule Schedule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            Job = new HashSet<Job>();
        }

        [Key]
        public int iTaskID { get; set; }

        public int iScheduleID { get; set; }

        [Required]
        [StringLength(1)]
        public string cPointSourceID { get; set; }

        public int iSourceID { get; set; }

        public int? iStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Job { get; set; }

        public virtual PointSource PointSource { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}
