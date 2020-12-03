using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseDatos
{
    public class ClaseConexion
    {
        public string DB_ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DB_DIMCEF"].ConnectionString;
            }
        }
    }
}
