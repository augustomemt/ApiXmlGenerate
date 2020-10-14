using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiGeneratorXml.Models
{
    public class TaskSources
    {

        [StringLength(250)]
        public string Name { get; set; }

        public int iTaskID { get; set; }

        public int iScheduleID { get; set; }

        [Required]
        [StringLength(1)]
        public string tipo { get; set; }

        public int iSourceID { get; set; }

        public int? iStatus { get; set; }

     






    }
}