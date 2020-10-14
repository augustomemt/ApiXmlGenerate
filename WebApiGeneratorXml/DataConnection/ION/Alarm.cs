namespace WebApiGeneratorXml.DataConnection.ION
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Alarm")]
    public partial class Alarm
    {
        public int ID { get; set; }

        public int AlarmDefinitionID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartTimestampUTC { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndTimestampUTC { get; set; }

        public bool IsActive { get; set; }

        public int? AcknowledgementID { get; set; }

        public int? PQEventID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastModifiedUTC { get; set; }
    }
}
