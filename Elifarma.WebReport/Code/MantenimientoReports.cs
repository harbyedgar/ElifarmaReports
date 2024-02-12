using CrystalDecisions.CrystalReports.Engine;
using Elifarma.WebReport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Elifarma.WebReport.Code
{
    public class MantenimientoReports
    {
        public ReportFile OrdenTrabajo(RequestFilter request)
        {
            ReportFile reportfile = new ReportFile();
            try
            {
                string tipo = request.report + "OT_";
                ReportDocument objReporte = new ReportDocument();
                string report_path = WebUtils.ReportPath("rptOrdenTrabajo.rpt");
                objReporte.Load(report_path);
                DataSet dstDatos = new DataSet();
                DbManager db = new DbManager();
                DataTable dtOrdenTrabajo = new DataTable();
               
                List<DbSqlParameter> param = new List<DbSqlParameter>();
                param.Add(new DbSqlParameter() { parametername = "@fecha_inicio", parametervalue = request.initdate });
                param.Add(new DbSqlParameter() { parametername = "@fecha_fin", parametervalue = request.enddate });

                dtOrdenTrabajo = db.ExecuteDataTable("sp_reporte_programa_produccion", param, StatementType.STOREDPROCEDURE);

                dtOrdenTrabajo.TableName = "OrdenTrabajo";

                dstDatos.Tables.Add(dtOrdenTrabajo);
                //dstDatos.Tables.Add(dtOrdenTrabajo[1]);

                objReporte.SetDataSource(dstDatos);
                string reportname = "OrdenTrabajo" + ".pdf";
                string reportlocation = WebUtils.get("ReportFileStorage") + "\\" + reportname;
                objReporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportlocation);

                //si llega a completar esta info, procede a guardar el reporte en BD y devuelve el contenido al cliente
                reportfile.idreport = Guid.NewGuid().ToString();
                reportfile.reportdate = DateTime.Now;
                reportfile.reportlocation = reportlocation;
                reportfile.reportname = reportname;
                reportfile.fieldstatus = 1;
                reportfile.userid = request.userid;

                //Guardando el Reporte 
                db.ReportFile.Add(reportfile);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                reportfile.contentfile = ex.ToString();
            }
            return reportfile;
        }

    
    }
}