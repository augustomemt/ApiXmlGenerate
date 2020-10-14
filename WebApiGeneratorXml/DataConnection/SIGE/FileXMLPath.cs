namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileXMLPath")]
    public partial class FileXMLPath
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iFilePathID { get; set; }

        [StringLength(100)]
        public string sPath { get; set; }

        public int? iPointTypeID { get; set; }

        public virtual PointType PointType { get; set; }
    }
}
