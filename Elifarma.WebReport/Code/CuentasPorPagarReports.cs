using CrystalDecisions.CrystalReports.Engine;
using Elifarma.WebReport.Models;
using Microsoft.Win32;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Elifarma.WebReport.Code
{
    public class CuentasPorPagarReports
    {
        public ReportFile RegistroCompras(RequestFilter request)
        {
            ReportFile reportfile = new ReportFile();
            ReportDocument objReporte = new ReportDocument();
            try
            {
                string report_path = string.Empty;
                string storedname = "";
                report_path = WebUtils.ReportPath("rptRegistroCompra.rpt");
                storedname = "USP_REPORTE_REGISTROCOMPRAS";

                objReporte.Load(report_path);

                DataSet dstDatos = new DataSet();
                DbManager db = new DbManager();
                DataTable dtRegistroCompra = new DataTable();

                List<DbSqlParameter> param = new List<DbSqlParameter>();
                param.Add(new DbSqlParameter() { parametername = "@CCIA", parametervalue = request.LP_EMPRESA });
                param.Add(new DbSqlParameter() { parametername = "@FECHAREG_INI", parametervalue = request.LP_FECHA_INI });
                param.Add(new DbSqlParameter() { parametername = "@FECHAREG_FIN", parametervalue = request.LP_FECHA_FIN });
                param.Add(new DbSqlParameter() { parametername = "@FEMISION_INI", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@FEMISION_FIN", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@ID_CTACTE", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@TIPO_COMPRA", parametervalue = request.LP_MODALIDAD });
                param.Add(new DbSqlParameter() { parametername = "@TIP_DOCU", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@MONEDA", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@ver_resumen", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@NORDEN", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@NQUIEBRE", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@F_IMPRESION", parametervalue = DBNull.Value });
                //param.Add(new DbSqlParameter() { parametername = "@fecha_fin", parametervalue = request.enddate });

                dtRegistroCompra = db.ExecuteDataTable(storedname, param, StatementType.STOREDPROCEDURE);
                dtRegistroCompra.TableName = "Compras";

                dstDatos.Tables.Add(dtRegistroCompra);

                objReporte.SetDataSource(dstDatos);

                string reportname = "Registro_Compras" + ".pdf";
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

        public ReportFile CobranzaProgramada(RequestFilter request)
        {
            ReportFile reportfile = new ReportFile();
            ReportDocument objReporte = new ReportDocument();
            try
            {
                string report_path = string.Empty;
                string storedname = "";
                report_path = WebUtils.ReportPath("rptCobranzasProgramadas.rpt");
                storedname = "USP_ESTADO_CTA_CTE";

                objReporte.Load(report_path);

                DataSet dstDatos = new DataSet();
                DbManager db = new DbManager();
                DataTable dtCobranzaProgramada = new DataTable();

                List<DbSqlParameter> param = new List<DbSqlParameter>();
                param.Add(new DbSqlParameter() { parametername = "@CCIA", parametervalue = request.LP_EMPRESA });
                param.Add(new DbSqlParameter() { parametername = "@FECHAREG_INI", parametervalue = request.LP_FECHA_INI });
                param.Add(new DbSqlParameter() { parametername = "@FECHAREG_FIN", parametervalue = request.LP_FECHA_FIN });
                param.Add(new DbSqlParameter() { parametername = "@FEMISION_INI", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@FEMISION_FIN", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@ID_CTACTE", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@TIPO_COMPRA", parametervalue = request.LP_MODALIDAD });
                param.Add(new DbSqlParameter() { parametername = "@TIP_DOCU", parametervalue = DBNull.Value });
                param.Add(new DbSqlParameter() { parametername = "@MONEDA", parametervalue = DBNull.Value });

                //param.Add(new DbSqlParameter() { parametername = "@fecha_fin", parametervalue = request.enddate });

                dtCobranzaProgramada = db.ExecuteDataTable(storedname, param, StatementType.STOREDPROCEDURE);
                dtCobranzaProgramada.TableName = "Cobranza";

                dstDatos.Tables.Add(dtCobranzaProgramada);

                objReporte.SetDataSource(dstDatos);

                string reportname = "Cobranza_programada" + ".pdf";
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



        //public ReportFile KardexValorizado(RequestFilter request)
        //{
        //    ReportFile reportfile = new ReportFile();
        //    ReportDocument objReporte = new ReportDocument();
        //    string report_path = string.Empty;
        //    string storedname = "";

        //    if (request.LP_MODALIDAD == 2) // 1 DETALLADO, 2 RESUMEN
        //    {
        //        report_path = WebUtils.ReportPath("rptKardexValorizadoResumen.rpt");
        //    }
        //    else
        //    {
        //        if (request.LP_CONSIDERAR_OP == 2)  // 1 TODOS, 2 SOLO CON OP
        //        {
        //            report_path = WebUtils.ReportPath("rptKardexValorizadoxOp.rpt");
        //        }
        //        else
        //        {
        //            report_path = WebUtils.ReportPath("rptKardexValorizado.rpt");
        //        }
        //    }
        //    storedname = "USP_ERP_REPORTE_KARDEX_VALORIZADO";

        //    objReporte.Load(report_path);
        //    DataSet dstDatos = new DataSet();
        //    DataTable KardexValorizado = new DataTable();
        //    DataTable KardexValorizadoResumen = new DataTable();
        //    DbManager db = new DbManager();
        //    List<DbSqlParameter> param = new List<DbSqlParameter>();
        //    param.Add(new DbSqlParameter() { parametername = "@LP_EMPRESA", parametervalue = request.LP_EMPRESA });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_ID_ALMACEN", parametervalue = DBNull.Value });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_C_ALMACEN", parametervalue = DBNull.Value });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_TVOUCHER", parametervalue = DBNull.Value });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_FECHA_INI", parametervalue =  request.LP_FECHA_INI });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_FECHA_FIN", parametervalue =  request.LP_FECHA_FIN });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_C_ARTICULO_INI", parametervalue = request.LP_C_ARTICULO_INI });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_C_ARTICULO_FIN", parametervalue = request.LP_C_ARTICULO_FIN });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_RELACION_C_LINEA", parametervalue = DBNull.Value });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_OP_INI", parametervalue = DBNull.Value });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_OP_FIN", parametervalue = DBNull.Value });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_CONSIDERAR_OP", parametervalue = request.LP_CONSIDERAR_OP });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_QUIEBRE", parametervalue = request.LP_QUIEBRE });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_SALTO_OP_X_PAGINA", parametervalue = request.LP_SALTO_OP_X_PAGINA });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_MODALIDAD", parametervalue = request.LP_MODALIDAD });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_RELACION_LOTE", parametervalue = DBNull.Value });
        //    param.Add(new DbSqlParameter() { parametername = "@LP_RELACION_ID_TRANSACCION", parametervalue = DBNull.Value });

        //    dstDatos = db.ExecuteDataSet(storedname, param, StatementType.STOREDPROCEDURE);

        //    if (request.LP_MODALIDAD == 2)
        //    {
        //        dstDatos.Tables[0].TableName = "crdKardexValorizadoResumen";
        //    }
        //    else
        //    {
        //        dstDatos.Tables[0].TableName = "crdKardexValorizado";
        //    }

        //    objReporte.SetDataSource(dstDatos);

        //    string reportname = "KARDEX_VALORIZADO_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".pdf";
        //    string reportlocation = WebUtils.get("ReportFileStorage") + "\\" + reportname;
        //    objReporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportlocation);

        //    reportfile.idreport = Guid.NewGuid().ToString();
        //    reportfile.reportdate = DateTime.Now;
        //    reportfile.reportlocation = reportlocation;
        //    reportfile.reportname = reportname;
        //    reportfile.fieldstatus = 1;
        //    reportfile.userid = request.userid;

        //    //Guardando el Reporte 
        //    db.ReportFile.Add(reportfile);
        //    db.SaveChanges();

        //    return reportfile;
        //}
    }
}