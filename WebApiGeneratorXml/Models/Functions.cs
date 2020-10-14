using SMFXmlServer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApiGeneratorXml.DataConnection.ION;
using WebApiGeneratorXml.DataConnection.SIGE;
using WebApWebApiGeneratorXml.Models;

namespace WebApiGeneratorXml.Models
{
    public class Functions
    {
        
        public async System.Threading.Tasks.Task GenarateXmlAsync(vMeter source,List<FileXMLPath> pathFile , DataConnection.SIGE.Task task)
        {
            try
            {
                BaseContexION ion = new BaseContexION();
                ContextSIGE _sige = new ContextSIGE();
                int status = Convert.ToInt32(StatusEnum.WaitProcess);
                MeasurerPoint meter = _sige.MeasurerPoint.Where(m => m.iSourceID == source.iSourceID).FirstOrDefault();
                List<Job> jobs = _sige.Job.Where(j => j.iTaskID == task.iTaskID  && j.iMeterID == source.iSourceID && j.bActive == true).Where(j => j.iStatus == 4 || j.iStatus == 1).ToList();
                foreach (Job jb in jobs)
                {
                    vJobs jbs = (from js in _sige.vJobs
                                  where js.iJobID == jb.iJobID
                                  select js).FirstOrDefault();

                    Job j = _sige.Job.Where(js => js.iJobID == jb.iJobID).FirstOrDefault();
                    j.dJobStartTime = DateTime.Now;                    
                    _sige.SaveChanges();
                    SqlParameter p1 = new SqlParameter("@SID", source.iSourceID);
                    SqlParameter p2 = new SqlParameter("@DataI", jb.dDataStartTime.Value.AddMinutes(5).ToString("yyyy-MM-dd HH:mm"));
                    SqlParameter p3 = new SqlParameter("@DataF", jb.dDataEndTime.Value.ToString("yyyy-MM-dd"));
                    SqlParameter p4 = new SqlParameter("@ZeroDate",jbs.bZeroData.Value);
                    object[] parameters = new object[] { p1, p2, p3 , p4};
                    List<ReveneuLog> dataLogs = _sige.Database.SqlQuery<ReveneuLog>(@"EXEC sp_RevenueLog2 @SourceId = @SID, @RecorderName = '0x800', @DtInicio = @DataI, @DtFinal = @DataF , @ZeroData = @ZeroDate", parameters).ToList();
                    if (dataLogs.Count != 0)
                    {
                        XMLColeta xmlObject = new XMLColeta();
                        bool comp = source.bLossCompensation;
                        foreach (var d in dataLogs)
                        {
                            //xmlObject.SetLeituraEnergia(d.TimestampUtc, Convert.ToDecimal(d.kWh_Forn), Convert.ToDecimal(d.kWh_Rec), Convert.ToDecimal(d.kVARh_Forn), Convert.ToDecimal(d.kVARh_Rec));
                            //xmlObject.SetLeituraEng(d.TimestampUtc, Convert.ToDecimal(d.Vln_a_mean), Convert.ToDecimal(d.Vln_b_mean), Convert.ToDecimal(d.Vln_c_mean), Convert.ToDecimal(d.Ia_mean), Convert.ToDecimal(d.Ib_mean), Convert.ToDecimal(d.Ic_mean));
                            xmlObject.SetLeituraEnergia(d.TimestampUtc, (Convert.ToDecimal(d.kWh_Forn) == null? 0: Convert.ToDecimal(d.kWh_Forn)),( Convert.ToDecimal(d.kWh_Rec) == null? 0: Convert.ToDecimal(d.kWh_Rec)), (Convert.ToDecimal(d.kVARh_Forn) == null ? 0: Convert.ToDecimal(d.kVARh_Forn)), (Convert.ToDecimal(d.kVARh_Rec) == null ? 0: Convert.ToDecimal(d.kVARh_Rec)));
                            xmlObject.SetLeituraEng(d.TimestampUtc, (Convert.ToDecimal(d.Vln_a_mean) == null?0: Convert.ToDecimal(d.Vln_a_mean)), (Convert.ToDecimal(d.Vln_b_mean) == null ?0: Convert.ToDecimal(d.Vln_b_mean)), (Convert.ToDecimal(d.Vln_c_mean) == null? 0: Convert.ToDecimal(d.Vln_c_mean)), (Convert.ToDecimal(d.Ia_mean) == null ?0: Convert.ToDecimal(d.Ia_mean)), (Convert.ToDecimal(d.Ib_mean) == null?0: Convert.ToDecimal(d.Ib_mean)),( Convert.ToDecimal(d.Ic_mean) == null ?0 : Convert.ToDecimal(d.Ic_mean)));
                            //if (comp != false)
                            //{
                            xmlObject.SetLeituraCompPerda(d.TimestampUtc, (Convert.ToDecimal(d.kWh_FornCP) == null ? 0 : Convert.ToDecimal(d.kWh_FornCP)),(Convert.ToDecimal(d.kWh_RecCP) == null ? 0 : Convert.ToDecimal(d.kWh_RecCP)), (Convert.ToDecimal(d.kVARh_FornCP) == null ? 0 : Convert.ToDecimal(d.kVARh_FornCP)), (Convert.ToDecimal(d.kVARh_RecCP) == null ? 0 : Convert.ToDecimal(d.kVARh_RecCP)));
                            //}
                        }
                        xmlObject.SetNmroSerie(source.Signature);
                        xmlObject.SetNmroMae(source.sCodeCCEE);
                        xmlObject.SetConstIntegEnergia(300);
                        xmlObject.SetConstIntegEng(300);
                        //if (source.bLossCompensation == true)
                        //{
                        xmlObject.SetConstIntegCompPerda(300);
                        //}
                        string fileName = "\\SCDE_" + source.sCodeCCEE + "_" + "ME_" + jb.dDataStartTime.Value.ToString("yyyyMMdd") + "_" + jb.dDataEndTime.Value.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".xml";
                        foreach ( FileXMLPath path in pathFile)
                        {
                            string pathFiles = path.sPath + fileName;
                            xmlObject.Commit(pathFiles);
                        }
                        if (dataLogs.Count == 288)
                        { 
                            if(jbs.bZeroData != true)
                            {
                                j.dJobEndTime = DateTime.Now;
                                j.sMessage = "";
                                j.iReply += 1;
                                if (meter.dLastSuccessXML < dataLogs.Last().TimestampUtc.Date)
                                {
                                    meter.dLastSuccessXML = dataLogs.Last().TimestampUtc.Date;
                                }
                                j.iStatus = Convert.ToInt32(StatusEnum.Sucessfull);
                                
                            }
                            else
                            {
                                j.dJobEndTime = DateTime.Now;
                                j.sMessage = "Lacunas preenchidas com zeros";
                                j.iReply += 1;
                                if (meter.dLastSuccessXML < dataLogs.Last().TimestampUtc.Date)
                                {
                                   meter.dLastSuccessXML = dataLogs.Last().TimestampUtc.Date;
                                }
                                j.iStatus = Convert.ToInt32(StatusEnum.Sucessfull);

                                
                            }
                           
                            
                        }
                        else
                        {
                            SqlParameter ps1 = new SqlParameter("@SID", source.iSourceID);
                            SqlParameter ps2 = new SqlParameter("@DataI", j.dDataStartTime.Value.AddMinutes(5).ToString("yyyy-MM-dd HH:mm"));
                            SqlParameter ps3 = new SqlParameter("@DataF", j.dDataEndTime.Value.ToString("yyyy-MM-dd"));
                            object[] parameterss = new object[] { ps1, ps2, ps3 };
                            List<DataFault> dataFault = _sige.Database.SqlQuery<DataFault>(@"EXEC sp_verify_nreg_datalog2 @SourceId = @SID, @RecorderName = '0x800', @DtInicio = @DataI, @DtFinal = @DataF", parameterss).ToList();
                            int i = 0;
                            string horaF = "Horas Faltantes(Lacunas): ";
                            foreach (DataFault d in dataFault)
                            {
                                if (d.Hora != i)
                                {
                                    for( int s = i; s <= d.Hora; s++)
                                    {
                                        horaF += s.ToString() + ", ";
                                    }
                                    i = d.Hora;
                                    i++;
                                    continue;
                                }
                                if (d.NReg != 12)
                                {
                                    horaF += d.Hora.ToString() + ", ";
                                }
                                i++;
                            }
                            string sMessage = horaF.Substring(0, horaF.Length - 2);                            
                            j.dJobEndTime = DateTime.Now;
                            j.sMessage = sMessage;
                            j.iReply += 1;
                            if ( meter.dLastSuccessXML < dataLogs.Last().TimestampUtc.Date)
                            {
                                meter.dLastSuccessXML = dataLogs.Last().TimestampUtc.Date;

                            }
                            j.iStatus = Convert.ToInt32(StatusEnum.PartialSucess);
                           
                        }
                        _sige.SaveChanges();
                    }
                    else
                    {
                        Job jbss = _sige.Job.Where(js => js.iJobID == jb.iJobID).FirstOrDefault();
                        jbss.dJobEndTime = DateTime.Now;
                        jbss.sMessage = "Dados Faltantes";
                        jbss.iReply += 1;                      
                        jbss.iStatus = Convert.ToInt32(StatusEnum.Fail);
                        _sige.SaveChanges();

                    }

                }

            }
            catch(Exception e)
            {

            }
           
        }
        //Method generate for Meter
        public async System.Threading.Tasks.Task GenerateForMeterAsync(List<DataConnection.SIGE.Task> tasksM)
        {
            ContextSIGE _sige = new ContextSIGE();
            foreach (DataConnection.SIGE.Task task in tasksM)
            {
                DataConnection.SIGE.Task ts = _sige.Task.Where(t => t.iTaskID == task.iTaskID).FirstOrDefault();
                ts.iStatus = Convert.ToInt32(StatusEnum.Processing);
                _sige.SaveChanges();
                 ContextSIGE _sige1 = new ContextSIGE();
                int sourceId = task.iSourceID;
                vMeter meter = (from m in _sige1.vMeter
                                where m.iSourceID == sourceId
                                select m).FirstOrDefault();
                if( meter != null)
                {
                    List<FileXMLPath> paths = _sige1.FileXMLPath.Where(p => p.iPointTypeID == meter.iPointTypeID).ToList();
                    await GenarateXmlAsync(meter, paths, task);
                    await StatusTask(task);
                }
                

            }
        }
        public async System.Threading.Tasks.Task GenerateForPointAsync(List<DataConnection.SIGE.Task> tasksP)
        {
            ContextSIGE _sige = new ContextSIGE();
            foreach (DataConnection.SIGE.Task task in tasksP)
            {
                DataConnection.SIGE.Task ts = _sige.Task.Where(t => t.iTaskID == task.iTaskID).FirstOrDefault();
                ts.iStatus = Convert.ToInt32(StatusEnum.Processing);
                _sige.SaveChanges();
                int sourceId = task.iSourceID;
                List<vMeter> meters = (from m in _sige.vMeter
                                       where m.iPointID == sourceId
                                       select m).ToList();
                if(meters.Count != 0)
                {
                    foreach (vMeter meter in meters)
                    {
                        List<FileXMLPath> paths = _sige.FileXMLPath.Where(p => p.iPointTypeID == meter.iPointTypeID).ToList();
                        await GenarateXmlAsync(meter, paths, task);

                    }
                    await StatusTask(task);
                }
                
            }
        }
        public async System.Threading.Tasks.Task GenerateForTypeAsync(List<DataConnection.SIGE.Task> tasksO)
        {
            ContextSIGE _sige = new ContextSIGE();
            foreach(DataConnection.SIGE.Task task in tasksO)
            {
                DataConnection.SIGE.Task ts = _sige.Task.Where(t => t.iTaskID == task.iTaskID).FirstOrDefault();
                ts.iStatus = Convert.ToInt32(StatusEnum.Processing);
                _sige.SaveChanges();

                int sourceId = task.iSourceID;
                List<vMeter> meters = (from m in _sige.vMeter
                                       where m.iPointTypeID == sourceId
                                       select m).ToList();
                if (meters.Count != 0)
                {
                    foreach (vMeter meter in meters)
                    {
                        List<FileXMLPath> paths = _sige.FileXMLPath.Where(p => p.iPointTypeID == meter.iPointTypeID).ToList();
                        await GenarateXmlAsync(meter, paths, task);

                    }

                    await StatusTask(task);
                }
            }
 
        }
        public async System.Threading.Tasks.Task GenerateForTagAsync(List<DataConnection.SIGE.Task> tasksT)
        {
            ContextSIGE _sige = new ContextSIGE();
            foreach(DataConnection.SIGE.Task task in tasksT)
            {
                DataConnection.SIGE.Task ts = _sige.Task.Where(t => t.iTaskID == task.iTaskID).FirstOrDefault();
                ts.iStatus = Convert.ToInt32(StatusEnum.Processing);
                _sige.SaveChanges();

                int TsourceId = task.iSourceID;
                //_sige.vMeterByTag.Where(mt => mt.iTagID == task.iSourceID).ToList();
                List<vMeterByTag> meterTag = (from mt in _sige.vMeterByTag
                                              where mt.iTagID == TsourceId
                                              select mt).ToList();

                if (meterTag.Count != 0)
                {
                    foreach (vMeterByTag byTag in meterTag)
                    {

                        int byId = byTag.iSourceID;
                        //_sige.vMeter.Where(m => m.iSourceID == byTag.iSourceID).FirstOrDefault();
                        vMeter meter = (from m in _sige.vMeter
                                        where m.iSourceID == byId
                                        select m).FirstOrDefault();
                        List<FileXMLPath> paths = _sige.FileXMLPath.Where(p => p.iPointTypeID == meter.iPointTypeID).ToList();
                        await GenarateXmlAsync(meter, paths, task);

                    }
                    await StatusTask(task);
                }
                
            }
        }
        
        //Atualiza Status Task
        public async System.Threading.Tasks.Task StatusTask(DataConnection.SIGE.Task task)
        {
            ContextSIGE _sige = new ContextSIGE();
            try
            {
                int taskID = task.iTaskID;
                List<Job> job = _sige.Job.Where(j => j.iTaskID == task.iTaskID).ToList();
                int statusS = Convert.ToInt32(StatusEnum.Sucessfull);
                int statusF = Convert.ToInt32(StatusEnum.Fail);
                List<Job> jobTask = job.Where(t => t.iTaskID == taskID).ToList();
                int JobSsucess = jobTask.Where(t => t.iStatus == statusS).Count();
                int JobSfail = jobTask.Where(t => t.iStatus == statusF).Count();
                DataConnection.SIGE.Task tasks = _sige.Task.Where(t => t.iTaskID == task.iTaskID).FirstOrDefault();
                if ( jobTask.Count == JobSsucess)
                {
                    tasks.iStatus = Convert.ToInt32(StatusEnum.Sucessfull);
                }
                else if(jobTask.Count == JobSfail)
                {
                    tasks.iStatus = Convert.ToInt32(StatusEnum.Fail);
                }
                else
                {
                    tasks.iStatus = Convert.ToInt32(StatusEnum.PartialSucess);
                }
                _sige.SaveChanges();
            }
            catch
            {

            }

        }
        //Atualiza schedule
        public  void StatusSchedule(int schedule)
        {
            try
            {
                ContextSIGE _sige = new ContextSIGE();
                List<DataConnection.SIGE.Task> tasks = _sige.Task.Where(t => t.iScheduleID == schedule).ToList();
                int statusS = Convert.ToInt32(StatusEnum.Sucessfull);
                int statusF = Convert.ToInt32(StatusEnum.Fail);
                List<DataConnection.SIGE.Task> scheduleTask = tasks.Where(t => t.iScheduleID == schedule).ToList();
                int Tasksucess = scheduleTask.Where(t => t.iStatus == statusS).Count();
                int Taskfail = scheduleTask.Where(t => t.iStatus == statusF).Count();
                Schedule sched = _sige.Schedule.Where(s => s.iScheduleID == schedule).FirstOrDefault();

                int scheduleID = sched.iScheduleID;
                List<vJobs> jobs = (from j in _sige.vJobs
                                    where j.iScheduleID == scheduleID
                                    select j).Where(j=> j.bActive == true).ToList();
                if(sched.cScheduleType == "A")
                {
                    if(sched.dNextRun == null)
                    {
                        sched.dNextRun = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        _sige.SaveChanges();
                    }
                }
                //Sucesso
                if (scheduleTask.Count == Tasksucess)
                {
                    
                    sched.iStatus = Convert.ToInt32(StatusEnum.Sucessfull);
                    if( sched.cScheduleType == "A")
                    {
                        sched.dNextRun = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " " + sched.tStartTime.ToString());
                        sched.iStatus = Convert.ToInt32(StatusEnum.WaitProcess);

                        foreach( var ts in tasks)
                        {
                            ts.iStatus = Convert.ToInt32(StatusEnum.WaitProcess);
                        }
                    }                   

                    foreach( vJobs jb in jobs)
                    {
                        Job jbs = _sige.Job.Where(j => j.iJobID == jb.iJobID).FirstOrDefault();
                        jbs.bActive = false;
                        
                    }                 
                    

                }//Falha
                else if(scheduleTask.Count == Taskfail)
                {
                    sched.iStatus = Convert.ToInt32(StatusEnum.Fail);
                    
                    if( sched.cScheduleType == "A")
                    {
                        if (TimeSpan.Parse(sched.dNextRun.Value.AddMinutes(Convert.ToDouble(sched.iRepeatInMinutes)).ToString("HH:mm:ss")) <= sched.tEndTime.Value)
                        {
                            sched.dNextRun = sched.dNextRun.Value.AddMinutes(Convert.ToDouble(sched.iRepeatInMinutes));
                        }
                        else
                        {
                            sched.dNextRun = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " " + sched.tStartTime.ToString());
                            sched.iStatus = Convert.ToInt32(StatusEnum.WaitProcess);
                            foreach (var ts in tasks)
                            {
                                ts.iStatus = Convert.ToInt32(StatusEnum.WaitProcess);
                            }
                            foreach (vJobs jb in jobs)
                            {
                                Job jbs = _sige.Job.Where(j => j.iJobID == jb.iJobID).FirstOrDefault();
                                jbs.bActive = false;

                            }
                        }
                    }
                    else
                    {
                        foreach (vJobs jb in jobs)
                        {
                            Job jbs = _sige.Job.Where(j => j.iJobID == jb.iJobID).FirstOrDefault();
                            jbs.bActive = false;

                        }

                    }

                }//Parcial
                else
                {
                    sched.iStatus = Convert.ToInt32(StatusEnum.PartialSucess);
                    if (sched.cScheduleType == "A")
                    {
                        if (TimeSpan.Parse(sched.dNextRun.Value.AddMinutes(Convert.ToDouble(sched.iRepeatInMinutes)).ToString("HH:mm:ss")) <= sched.tEndTime.Value)
                        {
                            sched.dNextRun = sched.dNextRun.Value.AddMinutes(Convert.ToDouble(sched.iRepeatInMinutes));
                        }
                        else
                        {
                            sched.dNextRun = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " " + sched.tStartTime.ToString());
                            sched.iStatus = Convert.ToInt32(StatusEnum.WaitProcess);

                            foreach (var ts in tasks)
                            {
                                ts.iStatus = Convert.ToInt32(StatusEnum.WaitProcess);
                            }
                            foreach (vJobs jb in jobs)
                            {
                                Job jbs = _sige.Job.Where(j => j.iJobID == jb.iJobID).FirstOrDefault();
                                jbs.bActive = false;

                            }
                        }
                    }
                    else
                    {
                        foreach (vJobs jb in jobs)
                        {
                            Job jbs = _sige.Job.Where(j => j.iJobID == jb.iJobID).FirstOrDefault();
                            jbs.bActive = false;

                        }

                    }

                }
                _sige.SaveChanges();

            }
            catch(DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;

            }


        }



    }
}