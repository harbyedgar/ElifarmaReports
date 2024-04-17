using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Elifarma.WebReport.Code
{
    public class RequestFilter
    {
        public int userid { get; set; }
        public string report { get; set; } 
        public string username { get; set; }
        public string shipmentnumber { get; set; }
        public string customerid { get; set; }
        public string initdate { get; set; }
        public string kardextype { get; set; }
        public string reporttype { get; set; }
        public string enddate { get; set; }
        public string mrid { get; set; }
        public string id { get; set; }
        public string qid { get; set; }
        public string ControllerReport { get; set; }
        public string shipping_id { get; set; }
        public string filename { get; set; }

        public string LP_EMPRESA { get; set; }
        public string LP_ID_ALMACEN { get; set; }
        public string LP_C_ALMACEN { get; set; }
        public string LP_TVOUCHER { get; set; }
        public string LP_FECHA_INI { get; set; }
        public string LP_FECHA_FIN { get; set; }
        public string LP_C_ARTICULO_INI { get; set; }
        public string LP_C_ARTICULO_FIN { get; set; }
        public string LP_RELACION_C_LINEA { get; set; }
        public int LP_MODALIDAD { get; set; }
        public string LP_OP_INI { get; set; }
        public string LP_OP_FIN { get; set; }
        public int LP_CONSIDERAR_OP { get; set; }
        public int LP_QUIEBRE { get; set; }
        public int LP_SALTO_OP_X_PAGINA { get; set; }
        public string  LP_RELACION_LOTE { get; set; }
        public string LP_RELACION_ID_TRANSACCION { get; set; }
        public string LP_NUM_OT { get; set; }

    }
}