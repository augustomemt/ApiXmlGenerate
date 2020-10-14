namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MeasurementTag")]
    public partial class MeasurementTag
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MeasurementTag()
        {
            MeasurementPoint = new HashSet<MeasurementPoint>();
        }

        [Key]
        public int iMeasurementTagID { get; set; }

        [StringLength(15)]
        public string sName { get; set; }

        [StringLength(50)]
        public string sDescription { get; set; }

        public bool bActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeasurementPoint> MeasurementPoint { get; set; }
    }
}
