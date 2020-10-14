namespace WebApiGeneratorXml.DataConnection.ION
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Source")]
    public partial class Source
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Source()
        {
            DataLog2 = new HashSet<DataLog2>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public short NamespaceID { get; set; }

        public short SourceTypeID { get; set; }

        public short? TimeZoneID { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Signature { get; set; }

        [Required]
        [StringLength(250)]
        public string DisplayName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DataLog2> DataLog2 { get; set; }
    }
}
