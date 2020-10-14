namespace WebApiGeneratorXml.DataConnection.SIGE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MeasurementPoint")]
    public partial class MeasurementPoint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MeasurementPoint()
        {
            MeasurerPoint = new HashSet<MeasurerPoint>();
            MeasurementTag = new HashSet<MeasurementTag>();
        }

        [Key]
        public int iPointID { get; set; }

        [Required]
        [StringLength(50)]
        public string sPointName { get; set; }

        public int iPointTypeID { get; set; }

        public int iVoltegeLevelID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fGenerationLimitInfKw { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fGenerationLimitSupKw { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fConsumptionLimitInfKw { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fConsumptionLimitSupKw { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fToleranceMaxGenerationPercent { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fToleranceMinGenerationPercent { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fToleranceMaxConsumptionPercent { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fToleranceMinConsumptionPercent { get; set; }

        public bool bVerifyMaxConsumptionTolerance { get; set; }

        public bool bVerifyMinConsumptionTolerance { get; set; }

        public bool bVerifyMaxGenerationTolerance { get; set; }

        public bool bVerifyMinGenerationTolerance { get; set; }

        public bool bLossCompensation { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fVoltagePrimaryTP { get; set; }

        public bool bRoot3PrimaryTP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fVoltageSecondaryTP { get; set; }

        public bool bRoot3SecondaryTP { get; set; }

        [Required]
        [StringLength(20)]
        public string sRelationTP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fPrimaryChainTC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fSecondyChainTC { get; set; }

        [Required]
        [StringLength(20)]
        public string sRelationTC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fThermalFactor { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fPeakContractedDemandKw { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fOffPeakContractedDemanKw { get; set; }

        [StringLength(15)]
        public string sInstalationNumber { get; set; }

        public int iProviderWithID { get; set; }

        [StringLength(10)]
        public string sLatitudePoint { get; set; }

        [StringLength(10)]
        public string sLongitudePoint { get; set; }

        public bool bVerificationM1M2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal fToleranceM1M2Percent { get; set; }

        public bool bMissingData { get; set; }

        public bool bZeroData { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fZeroThreshold { get; set; }

        public bool bActive { get; set; }

        public virtual PointType PointType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeasurerPoint> MeasurerPoint { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeasurementTag> MeasurementTag { get; set; }
    }
}
