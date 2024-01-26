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
    public class InventarioReports
    {
        public ReportFile KardexValorizado(RequestFilter request)
        {
            ReportFile reportfile = new ReportFile();
            ReportDocument objReporte = new ReportDocument();
            string report_path = string.Empty;
            string storedname = "";

            if (request.LP_MODALIDAD == 2) // 1 DETALLADO, 2 RESUMEN
            {
                report_path = WebUtils.ReportPath("rptKardexValorizadoResumen.rpt");
            }
            else
            {
                if (request.LP_CONSIDERAR_OP == 2)  // 1 TODOS, 2 SOLO CON OP
                {
                    report_path = WebUtils.ReportPath("rptKardexValorizadoxOp.rpt");
                }
                else
                {
                    report_path = WebUtils.ReportPath("rptKardexValorizado.rpt");
                }
            }
            storedname = "USP_ERP_REPORTE_KARDEX_VALORIZADO";

            objReporte.Load(report_path);
            DataSet dstDatos = new DataSet();
            DataTable KardexValorizado = new DataTable();
            DataTable KardexValorizadoResumen = new DataTable();
            DbManager db = new DbManager();
            List<DbSqlParameter> param = new List<DbSqlParameter>();
            param.Add(new DbSqlParameter() { parametername = "@LP_EMPRESA", parametervalue = request.LP_EMPRESA });
            param.Add(new DbSqlParameter() { parametername = "@LP_ID_ALMACEN", parametervalue = DBNull.Value });
            param.Add(new DbSqlParameter() { parametername = "@LP_C_ALMACEN", parametervalue = DBNull.Value });
            param.Add(new DbSqlParameter() { parametername = "@LP_TVOUCHER", parametervalue = DBNull.Value });
            param.Add(new DbSqlParameter() { parametername = "@LP_FECHA_INI", parametervalue =  request.LP_FECHA_INI });
            param.Add(new DbSqlParameter() { parametername = "@LP_FECHA_FIN", parametervalue =  request.LP_FECHA_FIN });
            param.Add(new DbSqlParameter() { parametername = "@LP_C_ARTICULO_INI", parametervalue = request.LP_C_ARTICULO_INI });
            param.Add(new DbSqlParameter() { parametername = "@LP_C_ARTICULO_FIN", parametervalue = request.LP_C_ARTICULO_FIN });
            param.Add(new DbSqlParameter() { parametername = "@LP_RELACION_C_LINEA", parametervalue = DBNull.Value });
            param.Add(new DbSqlParameter() { parametername = "@LP_OP_INI", parametervalue = DBNull.Value });
            param.Add(new DbSqlParameter() { parametername = "@LP_OP_FIN", parametervalue = DBNull.Value });
            param.Add(new DbSqlParameter() { parametername = "@LP_CONSIDERAR_OP", parametervalue = request.LP_CONSIDERAR_OP });
            param.Add(new DbSqlParameter() { parametername = "@LP_QUIEBRE", parametervalue = request.LP_QUIEBRE });
            param.Add(new DbSqlParameter() { parametername = "@LP_SALTO_OP_X_PAGINA", parametervalue = request.LP_SALTO_OP_X_PAGINA });
            param.Add(new DbSqlParameter() { parametername = "@LP_MODALIDAD", parametervalue = request.LP_MODALIDAD });
            param.Add(new DbSqlParameter() { parametername = "@LP_RELACION_LOTE", parametervalue = DBNull.Value });
            param.Add(new DbSqlParameter() { parametername = "@LP_RELACION_ID_TRANSACCION", parametervalue = DBNull.Value });

            dstDatos = db.ExecuteDataSet(storedname, param, StatementType.STOREDPROCEDURE);

            if (request.LP_MODALIDAD == 2)
            {
                dstDatos.Tables[0].TableName = "crdKardexValorizadoResumen";
            }
            else
            {
                dstDatos.Tables[0].TableName = "crdKardexValorizado";
            }

            objReporte.SetDataSource(dstDatos);

            string reportname = "KARDEX_VALORIZADO_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".pdf";
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