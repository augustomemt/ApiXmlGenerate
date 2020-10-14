namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PointSource")]
    public partial class PointSource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PointSource()
        {
            Task = new HashSet<Task>();
        }

        [Key]
        [StringLength(1)]
        public string cPointSourceID { get; set; }

        [Required]
        [StringLength(20)]
        public string sDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
