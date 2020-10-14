namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vMeter")]
    public partial class vMeter
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPointID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string sPointName { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iSourceID { get; set; }

        [StringLength(14)]
        public string sCodeCCEE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Signature { get; set; }

        public DateTime? dLastSuccessXML { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPointTypeID { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string sDescription { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(20)]
        public string sRelationTP { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(20)]
        public string sRelationTC { get; set; }

        public bool bLossCompensation { get; set; }
    }
}
