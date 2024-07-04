using Elifarma.WebReport.Code;
using Elifarma.WebReport.Models;
using System;
using System.IO;
using System.Web.Http;

namespace Elifarma.WebReport.Controllers
{
    public class ReportManagerController : ApiController
    {
       
        // POST api/<controller>
        public IHttpActionResult Post(RequestFilter request)
        {
            ProduccionReports objProduccionReports = new ProduccionReports();
            InventarioReports objInventarioReports = new InventarioReports();
            MantenimientoReports objMantenimientoReports = new MantenimientoReports();
            CuentasPorPagarReports objCuentasPorPagarReports = new CuentasPorPagarReports();
            ImportacionReports objImportacionReports = new ImportacionReports();
            ReportFile reportefile = new ReportFile();

            if (request.report.ToUpper() == "PROGRAMAPRODUCCION")
            {   
                reportefile = objProduccionReports.ProgramaProduccion(request);
            }
            else  if (request.report.ToUpper() == "KARDEXVALORIZADO")
            {
                reportefile = objInventarioReports.KardexValorizado(request);
            }
            else if (request.report.ToUpper() == "ORDENTRABAJO")
            {
                reportefile = objMantenimientoReports.OrdenTrabajo(request);
            }
            else if (request.report.ToUpper() == "REGISTROCOMPRA")
            {
                reportefile = objCuentasPorPagarReports.RegistroCompras(request);
            }
            else if (request.report.ToUpper() == "COBRANZAPROGRAMADA")
            {
                reportefile = objCuentasPorPagarReports.CobranzaProgramada(request);
            }
            else if (request.report.ToUpper() == "RENDICIONCAJACHICA")
            {
                reportefile = objCuentasPorPagarReports.RendicionCajaChica(request);
            }
            else if (request.report.ToUpper() == "IMPORTACIONES")
            {
                reportefile = objImportacionReports.Importaciones(request);
            }
            try
            {
                Byte[] bytes = File.ReadAllBytes(reportefile.reportlocation);
                String file = Convert.ToBase64String(bytes);
                reportefile.contentfile = file;
            }catch(Exception ex) { }

            return Ok(reportefile);
        }
    }
}