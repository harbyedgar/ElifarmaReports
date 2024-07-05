using CrystalDecisions.CrystalReports.Engine;
using Elifarma.WebReport.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Elifarma.WebReport.Code
{
    public class ImportacionReports
    {
        public ReportFile Importaciones(RequestFilter request)
        {
            ReportFile reportfile = new ReportFile();
            try
            {
                ReportDocument objReporte = new ReportDocument();
                string report_path = WebUtils.ReportPath("rptImportaciones.rpt");
                objReporte.Load(report_path);

                DataSet dstDatos = new DataSet();
                DataSet dt = new DataSet();
                DbManager db = new DbManager();
                List<DbSqlParameter> param = new List<DbSqlParameter>();
                param.Add(new DbSqlParameter() { parametername = "@importacionId", parametervalue = request.id });
                param.Add(new DbSqlParameter() { parametername = "@monedaId", parametervalue = request.monedaId });

                //dt = db.ExecuteDataTable("Usp_Reporte_Importaciones", param, StatementType.STOREDPROCEDURE);
                //dt.TableName = "crdImportaciones";
                //dstDatos.Tables.Add(dt);


                dt = db.ExecuteDataSet("Usp_Reporte_Importaciones", param, StatementType.STOREDPROCEDURE);
                dt.Tables[0].TableName = "crdImportaciones";
                dt.Tables[1].TableName = "crdImportacionesResumen";

                //dstDatos.Tables.Add(dt.Tables[0]);
                //dstDatos.Tables.Add(dt.Tables[1]);
                dstDatos = dt;
                try
                {
                    objReporte.SetDataSource(dstDatos);
                 }
                catch (Exception ex)
                {
                    throw;
                }

                string reportname = "Importacion_" + request.id + ".pdf";
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
            }

            catch (Exception ex)
            {
                reportfile.contentfile = ex.ToString();
            }
            return reportfile;

        }
    }
}