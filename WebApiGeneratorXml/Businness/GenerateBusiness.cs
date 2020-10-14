using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApiGeneratorXml.DataConnection.SIGE;
using WebApiGeneratorXml.Models;

namespace WebApiGeneratorXml.Businness
{
    public class GenerateBusiness
    {
       
        public async System.Threading.Tasks.Task GeneratesAsync(FindSchedule schedules)
        {
            ContextSIGE _sige = new ContextSIGE();
            Functions functions = new Functions();
            try
            {
                Schedule schedule = _sige.Schedule.Where(s => s.iScheduleID == schedules.ScheduleID).FirstOrDefault();
                if (schedule != null)
                {
                    // List<DataConnection.SIGE.Task> tasks = _sige.Task.Where(t => t.iScheduleID == schedules.ScheduleID ).ToList();
                    List<DataConnection.SIGE.Task> tasks = schedule.Task.ToList();
                    if (tasks.Count != 0)
                    {
                        List<DataConnection.SIGE.Task> tasksM = tasks.Where(t => t.cPointSourceID == "M").ToList();
                        List<DataConnection.SIGE.Task> tasksO = tasks.Where(t => t.cPointSourceID == "O").ToList();
                        List<DataConnection.SIGE.Task> tasksP = tasks.Where(t => t.cPointSourceID == "P").ToList();
                        List<DataConnection.SIGE.Task> tasksT = tasks.Where(t => t.cPointSourceID == "T").ToList();
                        if (tasksM.Count != 0) await functions.GenerateForMeterAsync(tasksM);
                        if (tasksP.Count != 0) await functions.GenerateForPointAsync(tasksP);
                        if (tasksO.Count != 0) await functions.GenerateForTypeAsync(tasksO);
                        if (tasksT.Count != 0) await functions.GenerateForTagAsync(tasksT);

                        functions.StatusSchedule(schedule.iScheduleID);
                    }


                }

            }

            catch (Exception e)
            {

            }



        }

    }
}