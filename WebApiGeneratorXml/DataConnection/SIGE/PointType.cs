namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PointType")]
    public partial class PointType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PointType()
        {
            FileXMLPath = new HashSet<FileXMLPath>();
            MeasurementPoint = new HashSet<MeasurementPoint>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPointTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string sDescription { get; set; }

        public bool bActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileXMLPath> FileXMLPath { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeasurementPoint> MeasurementPoint { get; set; }
    }
}
