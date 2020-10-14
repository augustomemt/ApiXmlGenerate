namespace WebApiGeneratorXml.DataConnection.ION
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DataLog2
    {
        [Key]
        [Column(Order = 0)]
        public long ID { get; set; }

        public double? Value { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SourceID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short QuantityID { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "datetime2")]
        public DateTime TimestampUTC { get; set; }

        public virtual Quantity Quantity { get; set; }

        public virtual Source Source { get; set; }
    }
}
