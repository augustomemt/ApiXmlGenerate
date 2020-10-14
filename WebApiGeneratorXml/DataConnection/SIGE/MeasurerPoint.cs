namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MeasurerPoint")]
    public partial class MeasurerPoint
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPointID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iSourceID { get; set; }

        [StringLength(1)]
        public string cMeasurerFunction { get; set; }

        [StringLength(14)]
        public string sCodeCCEE { get; set; }

        [StringLength(1)]
        public string cContentIN { get; set; }

        [StringLength(1)]
        public string cContentOUT { get; set; }

        public DateTime? dLastSuccessXML { get; set; }

        public DateTime? dLastCalibration { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dGenerationLimitInf { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dGenerationLimitSup { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dConsumptionLimitSup { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dConsumptionLimitInf { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dPeakContractedDemand { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dOffPeakContractedDemand { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dVerificationM1M2 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dMissingData { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dZeroData { get; set; }

        public virtual MeasurementPoint MeasurementPoint { get; set; }
    }
}
