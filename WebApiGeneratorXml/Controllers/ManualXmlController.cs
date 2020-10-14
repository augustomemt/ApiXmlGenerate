using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiGeneratorXml.Businness;
using WebApiGeneratorXml.DataConnection.SIGE;
using WebApiGeneratorXml.Models;

namespace WebApiGeneratorXml.Controllers
{
    public class ManualXmlController : ApiController
    {
       
        // POST: api/ManualXml
        public async Task<IHttpActionResult> PostAsync([FromBody]List<FindSchedule> schedules)
        {
            if(schedules == null)
            {
                return BadRequest("Campo vazio");
            }
            GenerateBusiness generate = new GenerateBusiness();
            foreach(FindSchedule schedule in schedules)
            {
                await generate.GeneratesAsync(schedule);
            }
            return Ok();
        }

       
    }
}
