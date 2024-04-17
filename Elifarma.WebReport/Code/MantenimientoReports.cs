using CrystalDecisions.CrystalReports.Engine;
using Elifarma.WebReport.Models;
using NPOI.POIFS.Crypt.Dsig;
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
            ReportDocument objReporte = new ReportDocument();
            string report_path = WebUtils.ReportPath("rptOdenTrabajoMantenimiento.rpt");
            objReporte.Load(report_path);

            DataTable dt = new DataTable();
            DataSet dstDatos = new DataSet();
            DbManager db = new DbManager();
            List<DbSqlParameter> param = new List<DbSqlParameter>();
            param.Add(new DbSqlParameter() { parametername = "@LP_NUM_OT", parametervalue = request.LP_NUM_OT });

            dt = db.ExecuteDataTable("ERP_MAN_ORDEN_TRABAJO_REPORTE", param, StatementType.STOREDPROCEDURE);
            dt.TableName = "crdOrdenTrabajoMantenimiento";
            dstDatos.Tables.Add(dt);

            try
            {
                objReporte.SetDataSource(dstDatos);
            }
            catch (Exception ex)
            {
                throw;
            }

            string reportname = "ORDEN_TRABAJO_MAN_" + request.LP_NUM_OT + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".pdf";
            string reportlocation = WebUtils.get("ReportFileStorage") + "\\" + reportname;
            objReporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportlocation);

            reportfile.idreport = Guid.NewGuid().ToString();
            reportfile.reportdate = DateTime.Now;
            reportfile.reportlocation = reportlocation;
            reportfile.reportname = reportname;
            reportfile.fieldstatus = 1;
            reportfile.userid = request.userid;

            //Guardando el Reporte 
            db.ReportFile.Add(reportfile);
            db.SaveChanges();

            return reportfile;

        }
    }
}