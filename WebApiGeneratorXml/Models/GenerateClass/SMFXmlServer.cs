using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace SMFXmlServer
{
    public interface IColeta
    {
        
        void SetNmroSerie(string nmro_serie);
    
        void SetNmroMae(string nmro_mae);
    
        void SetProgCol(string prog_col);

        void SetRelacaoTppri(double relacaotppri);
    
        // bool relacaotppriFieldSpecified;
    
        void SetRelacaoTpsec(double relacaotpsec);
    
        // bool relacaotpsecFieldSpecified;
    
        void SetRelacaoTcpri(double relacaotcpri);
    
        // bool relacaotcpriFieldSpecified;

        void SetRelacaoTcsec(double relacaotcsec);
    
        // bool relacaotcsecFieldSpecified;

        void SetRelacaoTp(double relacaotp);
    
        // bool relacaotpFieldSpecified;

        void SetRelacaoTc(double relacaotc);
    
        // bool relacaotcFieldSpecified;

        int Commit(string xmlpath);

        int Validate(string xmlpath, string xsdpath);

        string[] getValidationErrors();

    }

    public interface IAuditoria
    {
        void SetConstIntegAud(int const_integ);

        void SetRtcAud(int rtc);

        void SetRtpAud(decimal rtp);

        void SetTcPriAud(int tc_pri);

        void SetTcSecAud(int tc_sec);

        void SetTpPriAud(int tp_pri);

        void SetTpSecAud(decimal tp_sec);

        int SetLeituraAud(System.DateTime datahora, decimal aud_ativa_in, 
            decimal aud_ativa_out, decimal aud_cp_atv_in, bool aud_cp_atv_inSpecified,
            decimal aud_cp_atv_out, bool aud_cp_atv_outSpecified);

    }

    public interface ICompPerda
    {
        void SetConstIntegCompPerda(int const_integ);

        int SetLeituraCompPerda(System.DateTime datahora, decimal cp_atv_in,
            decimal cp_atv_out, decimal cp_rtv_in, decimal cp_rtv_out);
    }

    public interface IEnergia
    {
        //private coletaEnergiaLeitura_energ[] leitura_energField;

        void SetConstIntegEnergia(int const_integ);

        int SetLeituraEnergia(System.DateTime datahora, decimal e_atv_in, 
            decimal e_atv_out, decimal e_rtv_in, decimal e_rtv_out);
    }

    public interface IEngenharia
    {
        void SetConstIntegEng(int const_integ);

        int SetLeituraEngTensao(System.DateTime datahora, decimal t_fase_a, 
            decimal t_fase_b, decimal t_fase_c);

        int SetLeituraEngCorrente(System.DateTime datahora, decimal c_fase_a, 
            decimal c_fase_b, decimal c_fase_c);

        int SetLeituraEng(System.DateTime datahora, decimal t_fase_a,
            decimal t_fase_b, decimal t_fase_c, decimal c_fase_a,
            decimal c_fase_b, decimal c_fase_c);
    }

    public interface ILeituraAlarme
    {
        int SetLeituraAlarme(System.DateTime datahora, string milisegundos, 
            bool milisegundosSpecified, int prioridade, bool prioridadeFieldSpecified, 
            string causa, string causa_valor, string efeito, string efeito_valor);
    }

    public interface ILeituraQualidade
    {
        int SetLeituraQualidade(System.DateTime datahora, string milisegundos,
            decimal dist_nom, bool dist_nomSpecified, int dur_dist, decimal sag_limit,
            decimal swell_limit, decimal val_max1, decimal val_min1, decimal val_max2,
            decimal val_min2, decimal val_max3, decimal val_min3);
    }

    public class XMLColeta : IColeta, IAuditoria, ICompPerda, IEnergia, IEngenharia, 
        ILeituraAlarme, ILeituraQualidade
    {
        private coleta Coleta;
        private coletaMedidor Medidor;
        private coletaEnergia Energia;
        private coletaEngenharia Engenharia;
        private coletaAuditoria Auditoria;
        private coletaComp_perda CompPerda;
        private coletaEnergiaLeitura_energ[] leituraEnergia;
        private int energiaIndex;
        private coletaLeitura_alme[] Alarmes;    
        private int alarmeIndex;
        private coletaEngenhariaLeitura_eng[] leituraEngenharia;
        private int engenhariaIndex;
        private coletaLeitura_qlde[] leituraQlde;
        private int qldeIndex;
        private coletaAuditoriaLeitura_aud[] leituraAud;
        private int audIndex;
        private coletaComp_perdaLeitura_cp[] leituraCompPerda;
        private int compPerdaIndex;
        private static string numeroMae = "";
        private static int validationError = 0;
        private static string[] validationErrorArray;
        private static int validationErrorIndex;
        private bool initialized = false;

        public XMLColeta()
        {
            Coleta = new coleta();
            Medidor = new coletaMedidor();
            Energia = new coletaEnergia();
            Engenharia = new coletaEngenharia();
            Auditoria = new coletaAuditoria();
            CompPerda = new coletaComp_perda();
            leituraEnergia = new coletaEnergiaLeitura_energ[300];
            energiaIndex = 0;
            Alarmes = new coletaLeitura_alme[300];
            alarmeIndex = 0;
            leituraEngenharia = new coletaEngenhariaLeitura_eng[300];
            engenhariaIndex = 0;
            leituraQlde = new coletaLeitura_qlde[300];
            qldeIndex = 0;
            leituraAud = new coletaAuditoriaLeitura_aud[300];
            audIndex = 0;
            leituraCompPerda = new coletaComp_perdaLeitura_cp[300];
            compPerdaIndex = 0;
            validationErrorArray = new string[1];
            validationErrorIndex = 0;
            initialized = true;
        }

        private decimal decimalAdjust(decimal value, int decimals)
        {
            decimal adjust = 1e-28m;

            return (decimal)Math.Round((value + adjust), decimals, 
                MidpointRounding.AwayFromZero);
        }
        
        //Start IColeta
        #region IColeta Members

        public void SetNmroSerie(string nmro_serie)
        {
            if (initialized)
            {
                Medidor.nmro_serie = nmro_serie;
                Coleta.medidor = Medidor;
            }
        }

        public void SetNmroMae(string nmro_mae)
        {
            if (initialized)
            {
                Medidor.nmro_mae = nmro_mae;
                Coleta.medidor = Medidor;
                numeroMae = nmro_mae;
            }
        }

        public void SetProgCol(string prog_col)
        {
            if (initialized)
            {
                Medidor.prog_col = prog_col;
                Coleta.medidor = Medidor;
            }
        }

        public void SetRelacaoTppri(double relacaotppri)
        {
            if (initialized)
            {
                Medidor.relacaotppri = relacaotppri;
                Medidor.relacaotppriSpecified = true;
                Coleta.medidor = Medidor;
            }
        }

        // bool relacaotppriFieldSpecified;

        public void SetRelacaoTpsec(double relacaotpsec)
        {
            if (initialized)
            {
                Medidor.relacaotpsec = relacaotpsec;
                Medidor.relacaotpsecSpecified = true;
                Coleta.medidor = Medidor;
            }
        }

        // bool relacaotpsecFieldSpecified;

        public void SetRelacaoTcpri(double relacaotcpri)
        {
            if (initialized)
            {
                Medidor.relacaotcpri = relacaotcpri;
                Medidor.relacaotcpriSpecified = true;
                Coleta.medidor = Medidor;
            }
        }

        // bool relacaotcpriFieldSpecified;

        public void SetRelacaoTcsec(double relacaotcsec)
        {
            if (initialized)
            {
                Medidor.relacaotcsec = relacaotcsec;
                Medidor.relacaotcsecSpecified = true;
                Coleta.medidor = Medidor;
            }
        }

        // bool relacaotcsecFieldSpecified;

        public void SetRelacaoTp(double relacaotp)
        {
            if (initialized)
            {
                Medidor.relacaotp = relacaotp;
                Medidor.relacaotpSpecified = true;
                Coleta.medidor = Medidor;
            }
        }

        // bool relacaotpFieldSpecified;

        public void SetRelacaoTc(double relacaotc)
        {
            if (initialized)
            {
                Medidor.relacaotc = relacaotc;
                Medidor.relacaotcSpecified = true;
                Coleta.medidor = Medidor;
            }
        }

        // bool relacaotcFieldSpecified;

        public int Commit(string xmlpath)
        {
            if (initialized)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(Coleta.GetType());
                    TextWriter writer = new StreamWriter(xmlpath);
                    serializer.Serialize(writer, Coleta);
                    writer.Close();
                }
                catch (Exception e)
                {
                    //Console.Error.Write(e);
                    if (!EventLog.SourceExists(Coleta.medidor.nmro_mae))
                        EventLog.CreateEventSource(Coleta.medidor.nmro_mae, 
                            "Application");

                    EventLog.WriteEntry(Coleta.medidor.nmro_mae, e.ToString(), 
                        EventLogEntryType.Error);

                    return -1;
                }
                return 0;
            }
            else return -2;
        }

        public int Validate(string xmlpath, string xsdpath)
        {
          
            validationError = 0;

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, xsdpath);
                settings.ValidationEventHandler += LocalValidationHandler;
                using (XmlReader r = XmlReader.Create(xmlpath, settings))
                    while (r.Read()) ;
            }
            catch (Exception e)
            {
                //Console.Error.Write(xsve);
                validationError = -2;
                if (!EventLog.SourceExists(Coleta.medidor.nmro_mae))
                    EventLog.CreateEventSource(Coleta.medidor.nmro_mae, 
                        "Application");

                EventLog.WriteEntry(Coleta.medidor.nmro_mae, e.ToString(),
                    EventLogEntryType.Error);

            }
            return validationError;
        }

        static void LocalValidationHandler(object sender, ValidationEventArgs e)
        {
            string msg = e.Exception.Message + " Line: " + e.Exception.LineNumber + " Position: " + 
                e.Exception.LinePosition + " .";

            if (!EventLog.SourceExists(numeroMae))
                EventLog.CreateEventSource(numeroMae, "Application");

            EventLog.WriteEntry(numeroMae, msg, EventLogEntryType.Error );

            if (validationErrorIndex >= validationErrorArray.Length)
            {
                Array.Resize(ref validationErrorArray, validationErrorArray.Length + 1);
            }

            validationErrorArray[validationErrorIndex++] = msg;

            validationError = -1;
        }

        public string[] getValidationErrors()
        {
            return validationErrorArray;
        }


        #endregion
        //End IColeta

        //Start IEnergia
        #region IEnergia Members

        public void SetConstIntegEnergia(int const_integ)
        {
            if (initialized)
            {
                Energia.const_integ = const_integ;
                Coleta.energia = Energia;
            }
        }

        public int SetLeituraEnergia(System.DateTime datahora, decimal e_atv_in, 
            decimal e_atv_out, decimal e_rtv_in, decimal e_rtv_out)
        {
            if (initialized)
            {
                leituraEnergia[energiaIndex] = new coletaEnergiaLeitura_energ();
                leituraEnergia[energiaIndex].data = datahora.ToString("yyyy-MM-dd");
                leituraEnergia[energiaIndex].hora = datahora.ToString("HH:mm:ss");
                leituraEnergia[energiaIndex].e_atv_in = decimalAdjust(e_atv_in, 3); 
                leituraEnergia[energiaIndex].e_atv_out = decimalAdjust(e_atv_out, 3); 
                leituraEnergia[energiaIndex].e_rtv_in = decimalAdjust(e_rtv_in, 3); 
                leituraEnergia[energiaIndex++].e_rtv_out = decimalAdjust(e_rtv_out, 3);
                Energia.leitura_energ = leituraEnergia;
                Coleta.energia = Energia;
                if (energiaIndex >= leituraEnergia.Length)
                {
                    Array.Resize(ref leituraEnergia, leituraEnergia.Length + 300);
                }

                return energiaIndex;
            }
            else return -2;
        }

        #endregion
        //End IEnergia

        // Start ILeituraAlarme
        #region ILeituraAlarme Members

        public int SetLeituraAlarme(System.DateTime datahora, string milisegundos, 
            bool milisegundosSpecified, int prioridade, bool prioridadeFieldSpecified, 
            string causa, string causa_valor, string efeito, string efeito_valor)
        {
            if (initialized)
            {
                Alarmes[alarmeIndex] = new coletaLeitura_alme();
                Alarmes[alarmeIndex].prioridade = prioridade;
                Alarmes[alarmeIndex].prioridadeSpecified = prioridadeFieldSpecified;
                Alarmes[alarmeIndex].causa = causa;
                Alarmes[alarmeIndex].causa_valor = causa_valor;
                Alarmes[alarmeIndex].data = datahora.ToString("yyyy-MM-dd");
                Alarmes[alarmeIndex].hora = datahora.ToString("HH:mm:ss");
                if (milisegundosSpecified) 
                    Alarmes[alarmeIndex].milisegundos = milisegundos;

                Alarmes[alarmeIndex].efeito = efeito;
                Alarmes[alarmeIndex++].efeito_valor = efeito_valor;
                Coleta.alarme = Alarmes;

                if (alarmeIndex >= Alarmes.Length)
                {
                    Array.Resize(ref Alarmes, Alarmes.Length + 300);
                }

                return alarmeIndex;
            }
            else return -2;
        }

        #endregion
        // End ILeituraAlarme

        // Start IEngenharia

        #region IEngenharia Members

        public void SetConstIntegEng(int const_integ)
        {
            if (initialized)
            {
                Engenharia.const_integ = const_integ;
                Coleta.engenharia = Engenharia;
            }
        }

        public int SetLeituraEngTensao(System.DateTime datahora, decimal t_fase_a,
            decimal t_fase_b, decimal t_fase_c)
        {
            if (initialized)
            {
                leituraEngenharia[engenhariaIndex] = new coletaEngenhariaLeitura_eng();
                leituraEngenharia[engenhariaIndex].data = 
                    datahora.ToString("yyyy-MM-dd");
                leituraEngenharia[engenhariaIndex].hora = 
                    datahora.ToString("HH:mm:ss");
                leituraEngenharia[engenhariaIndex].tensao = 
                    new coletaEngenhariaLeitura_engTensao();
                leituraEngenharia[engenhariaIndex].tensao.t_fase_a = 
                    decimalAdjust(t_fase_a, 2); 
                leituraEngenharia[engenhariaIndex].tensao.t_fase_aSpecified = true;
                leituraEngenharia[engenhariaIndex].tensao.t_fase_b = 
                    decimalAdjust(t_fase_b, 2); 
                leituraEngenharia[engenhariaIndex].tensao.t_fase_bSpecified = true;
                leituraEngenharia[engenhariaIndex].tensao.t_fase_c = 
                    decimalAdjust(t_fase_c, 2); 
                leituraEngenharia[engenhariaIndex].tensao.t_fase_cSpecified = true;

                leituraEngenharia[engenhariaIndex].corrente = 
                    new coletaEngenhariaLeitura_engCorrente();               
                leituraEngenharia[engenhariaIndex].corrente.c_fase_aSpecified = false;
                leituraEngenharia[engenhariaIndex].corrente.c_fase_bSpecified = false;
                leituraEngenharia[engenhariaIndex++].corrente.c_fase_cSpecified = false;

                Engenharia.leitura_eng = leituraEngenharia;
                Coleta.engenharia = Engenharia;
                if (engenhariaIndex >= leituraEngenharia.Length)
                {
                    Array.Resize(ref leituraEngenharia, 
                        leituraEngenharia.Length + 300);
                }

                return engenhariaIndex;
            }
            else return -2;

        }

        public int SetLeituraEngCorrente(System.DateTime datahora, decimal c_fase_a,
            decimal c_fase_b, decimal c_fase_c)
        {
            if (initialized)
            {
                leituraEngenharia[engenhariaIndex] = 
                    new coletaEngenhariaLeitura_eng();
                leituraEngenharia[engenhariaIndex].data = 
                    datahora.ToString("yyyy-MM-dd");
                leituraEngenharia[engenhariaIndex].hora = 
                    datahora.ToString("HH:mm:ss");

                leituraEngenharia[engenhariaIndex].tensao = 
                    new coletaEngenhariaLeitura_engTensao();                
                leituraEngenharia[engenhariaIndex].tensao.t_fase_aSpecified = false;
                leituraEngenharia[engenhariaIndex].tensao.t_fase_bSpecified = false;
                leituraEngenharia[engenhariaIndex].tensao.t_fase_cSpecified = false;

                leituraEngenharia[engenhariaIndex].corrente = 
                    new coletaEngenhariaLeitura_engCorrente();
                leituraEngenharia[engenhariaIndex].corrente.c_fase_a = 
                    decimalAdjust(c_fase_a, 2); 
                leituraEngenharia[engenhariaIndex].corrente.c_fase_aSpecified = true;
                leituraEngenharia[engenhariaIndex].corrente.c_fase_b = 
                    decimalAdjust(c_fase_b, 2); 
                leituraEngenharia[engenhariaIndex].corrente.c_fase_bSpecified = true;
                leituraEngenharia[engenhariaIndex].corrente.c_fase_c = 
                    decimalAdjust(c_fase_c, 2); 
                leituraEngenharia[engenhariaIndex++].corrente.c_fase_cSpecified = true;

                Engenharia.leitura_eng = leituraEngenharia;
                Coleta.engenharia = Engenharia;
                if (engenhariaIndex >= leituraEngenharia.Length)
                {
                    Array.Resize(ref leituraEngenharia, 
                        leituraEngenharia.Length + 300);
                }

                return engenhariaIndex;
            }
            else return -2;
        }

        public int SetLeituraEng(System.DateTime datahora, decimal t_fase_a,
            decimal t_fase_b, decimal t_fase_c, decimal c_fase_a,
            decimal c_fase_b, decimal c_fase_c)
        {
            if (initialized)
            {
                leituraEngenharia[engenhariaIndex] = 
                    new coletaEngenhariaLeitura_eng();
                leituraEngenharia[engenhariaIndex].data = 
                    datahora.ToString("yyyy-MM-dd");
                leituraEngenharia[engenhariaIndex].hora = 
                    datahora.ToString("HH:mm:ss");

                leituraEngenharia[engenhariaIndex].tensao = 
                    new coletaEngenhariaLeitura_engTensao();
                leituraEngenharia[engenhariaIndex].tensao.t_fase_a = 
                    decimalAdjust(t_fase_a, 2); 
                leituraEngenharia[engenhariaIndex].tensao.t_fase_aSpecified = true;
                leituraEngenharia[engenhariaIndex].tensao.t_fase_b = 
                    decimalAdjust(t_fase_b, 2); 
                leituraEngenharia[engenhariaIndex].tensao.t_fase_bSpecified = true;
                leituraEngenharia[engenhariaIndex].tensao.t_fase_c = 
                    decimalAdjust(t_fase_c, 2); 
                leituraEngenharia[engenhariaIndex].tensao.t_fase_cSpecified = true;

                leituraEngenharia[engenhariaIndex].corrente = 
                    new coletaEngenhariaLeitura_engCorrente();
                leituraEngenharia[engenhariaIndex].corrente.c_fase_a = 
                    decimalAdjust(c_fase_a, 2); 
                leituraEngenharia[engenhariaIndex].corrente.c_fase_aSpecified = true;
                leituraEngenharia[engenhariaIndex].corrente.c_fase_b = 
                    decimalAdjust(c_fase_b, 2); 
                leituraEngenharia[engenhariaIndex].corrente.c_fase_bSpecified = true;
                leituraEngenharia[engenhariaIndex].corrente.c_fase_c = 
                    decimalAdjust(c_fase_c, 2); 
                leituraEngenharia[engenhariaIndex++].corrente.c_fase_cSpecified = true;

                Engenharia.leitura_eng = leituraEngenharia;
                Coleta.engenharia = Engenharia;
                if (engenhariaIndex >= leituraEngenharia.Length)
                {
                    Array.Resize(ref leituraEngenharia, 
                        leituraEngenharia.Length + 300);
                }

                return engenhariaIndex;
            }
            else return -2;
        }

        #endregion
        
        // End IEngenharia

        // Start ILeituraQualidade

        #region ILeituraQualidade Members

        public int SetLeituraQualidade(System.DateTime datahora, string milisegundos,
            decimal dist_nom, bool dist_nomSpecified, int dur_dist, decimal sag_limit,
            decimal swell_limit, decimal val_max1, decimal val_min1, decimal val_max2,
            decimal val_min2, decimal val_max3, decimal val_min3)
        {
            if (initialized)
            {
                leituraQlde[qldeIndex] = new coletaLeitura_qlde();
                leituraQlde[qldeIndex].data = datahora.ToString("yyyy-MM-dd");
                leituraQlde[qldeIndex].hora = datahora.ToString("HH:mm:ss");
                leituraQlde[qldeIndex].milisegundos = milisegundos;
                leituraQlde[qldeIndex].dist_nom = decimalAdjust(dist_nom, 2); 
                leituraQlde[qldeIndex].dist_nomSpecified = dist_nomSpecified;
                leituraQlde[qldeIndex].dur_dist = dur_dist;
                leituraQlde[qldeIndex].sag_limit = decimalAdjust(dist_nom, 2); 
                leituraQlde[qldeIndex].swell_limit = decimalAdjust(dist_nom, 2); 

                leituraQlde[qldeIndex].val_1 = new coletaLeitura_qldeVal_1();
                leituraQlde[qldeIndex].val_1.val_max1 = decimalAdjust(dist_nom, 2);
                leituraQlde[qldeIndex].val_1.val_max1Specified = true;
                leituraQlde[qldeIndex].val_1.val_min1 = decimalAdjust(dist_nom, 2);
                leituraQlde[qldeIndex].val_1.val_min1Specified = true;

                leituraQlde[qldeIndex].val_2 = new coletaLeitura_qldeVal_2();
                leituraQlde[qldeIndex].val_2.val_max2 = decimalAdjust(dist_nom, 2); 
                leituraQlde[qldeIndex].val_2.val_max2Specified = true;
                leituraQlde[qldeIndex].val_2.val_min2 = decimalAdjust(dist_nom, 2);
                leituraQlde[qldeIndex].val_2.val_min2Specified = true;

                leituraQlde[qldeIndex].val_3 = new coletaLeitura_qldeVal_3();
                leituraQlde[qldeIndex].val_3.val_max3 = decimalAdjust(dist_nom, 2); 
                leituraQlde[qldeIndex].val_3.val_max3Specified = true;
                leituraQlde[qldeIndex].val_3.val_min3 = decimalAdjust(dist_nom, 2);
                leituraQlde[qldeIndex].val_3.val_min3Specified = true;

                qldeIndex++;
                Coleta.qualidade = leituraQlde;

                if (qldeIndex >= leituraQlde.Length)
                {
                    Array.Resize(ref leituraQlde, leituraQlde.Length + 300);
                }

                return qldeIndex;
            }
            else return -2;
        }

        #endregion
       
        // End ILeituraQualidade

        // Start IAuditoria 

        #region IAuditoria Members

        public void SetConstIntegAud(int const_integ)
        {
            if (initialized)
            {
                Auditoria.const_integ = const_integ;
                Coleta.auditoria = Auditoria;
            }
        }

        public void SetRtcAud(int rtc)
        {
            if (initialized)
            {
                Auditoria.rtc = rtc;
                Auditoria.rtcSpecified = true;
                Coleta.auditoria = Auditoria;
            }
        }

        public void SetRtpAud(decimal rtp)
        {
            if (initialized)
            {
                Auditoria.rtp = decimalAdjust(rtp, 2); 
                Auditoria.rtpSpecified = true;
                Coleta.auditoria = Auditoria;
            }
        }

        public void SetTcPriAud(int tc_pri)
        {
            if (initialized)
            {
                Auditoria.tc_pri = tc_pri;
                Auditoria.tc_priSpecified = true;
                Coleta.auditoria = Auditoria;
            }
        }

        public void SetTcSecAud(int tc_sec)
        {
            if (initialized)
            {
                Auditoria.tc_sec = tc_sec;
                Auditoria.tc_secSpecified = true;
                Coleta.auditoria = Auditoria;
            }
        }

        public void SetTpPriAud(int tp_pri)
        {
            if (initialized)
            {
                Auditoria.tp_pri = tp_pri;
                Auditoria.tp_priSpecified = true;
                Coleta.auditoria = Auditoria;
            }
        }

        public void SetTpSecAud(decimal tp_sec)
        {
            if (initialized)
            {
                Auditoria.tp_sec = decimalAdjust(tp_sec, 2); 
                Auditoria.tp_secSpecified = true;
                Coleta.auditoria = Auditoria;
            }
        }

        public int SetLeituraAud(System.DateTime datahora, decimal aud_ativa_in,
            decimal aud_ativa_out, decimal aud_cp_atv_in, bool aud_cp_atv_inSpecified,
            decimal aud_cp_atv_out, bool aud_cp_atv_outSpecified)
        {
            if (initialized)
            {
                leituraAud[audIndex] = new coletaAuditoriaLeitura_aud();
                leituraAud[audIndex].data = datahora.ToString("yyyy-MM-dd");
                leituraAud[audIndex].hora = datahora.ToString("HH:mm:ss");
                leituraAud[audIndex].aud_ativa_in = decimalAdjust(aud_ativa_in, 3);
                leituraAud[audIndex].aud_ativa_out = decimalAdjust(aud_ativa_out, 3); 
                leituraAud[audIndex].aud_cp_atv_in = decimalAdjust(aud_cp_atv_in, 3); 
                leituraAud[audIndex].aud_cp_atv_inSpecified = aud_cp_atv_inSpecified;
                leituraAud[audIndex].aud_cp_atv_out = decimalAdjust(aud_cp_atv_out, 3);
                leituraAud[audIndex++].aud_cp_atv_outSpecified = 
                    aud_cp_atv_outSpecified;
                Auditoria.leitura_aud = leituraAud;
                Coleta.auditoria = Auditoria;
                if (audIndex >= leituraAud.Length)
                {
                    Array.Resize(ref leituraAud, leituraAud.Length + 300);
                }

                return energiaIndex;
            }
            else return -2;
        }

        #endregion

        // End IAuditoria 

        // Start ICompPerda

        #region ICompPerda Members

        public void SetConstIntegCompPerda(int const_integ)
        {
            if (initialized)
            {
                CompPerda.const_integ = const_integ;
                Coleta.comp_perda = CompPerda;
            }
        }

        public int SetLeituraCompPerda(DateTime datahora, decimal cp_atv_in, 
            decimal cp_atv_out, decimal cp_rtv_in, decimal cp_rtv_out)
        {
            if (initialized)
            {
                leituraCompPerda[compPerdaIndex] = 
                    new coletaComp_perdaLeitura_cp();
                leituraCompPerda[compPerdaIndex].data = 
                    datahora.ToString("yyyy-MM-dd");
                leituraCompPerda[compPerdaIndex].hora = 
                    datahora.ToString("HH:mm:ss");
                leituraCompPerda[compPerdaIndex].cp_atv_in = 
                    decimalAdjust(cp_atv_in, 3); 
                leituraCompPerda[compPerdaIndex].cp_atv_out = 
                    decimalAdjust(cp_atv_out, 3); 
                leituraCompPerda[compPerdaIndex].cp_rtv_in = 
                    decimalAdjust(cp_rtv_in, 3); 
                leituraCompPerda[compPerdaIndex++].cp_rtv_out = 
                    decimalAdjust(cp_rtv_out, 3); 
                CompPerda.leitura_cp = leituraCompPerda;
                Coleta.comp_perda = CompPerda;
                if (compPerdaIndex >= leituraCompPerda.Length)
                {
                    Array.Resize(ref leituraCompPerda, 
                        leituraCompPerda.Length + 300);
                }

                return compPerdaIndex;
            }
            else return -2;
        }

        #endregion

        // End ICompPerda
    }
}
