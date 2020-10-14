namespace WebApiGeneratorXml.DataConnection.ION
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quantity")]
    public partial class Quantity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quantity()
        {
            DataLog2 = new HashSet<DataLog2>();
        }

        public short ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Base { get; set; }

        [StringLength(200)]
        public string Unit { get; set; }

        [StringLength(200)]
        public string Phase { get; set; }

        [StringLength(200)]
        public string Type { get; set; }

        [StringLength(200)]
        public string Direction { get; set; }

        [StringLength(200)]
        public string Misc { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DataLog2> DataLog2 { get; set; }
    }
}
