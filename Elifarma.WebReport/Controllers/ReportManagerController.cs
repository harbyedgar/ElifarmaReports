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