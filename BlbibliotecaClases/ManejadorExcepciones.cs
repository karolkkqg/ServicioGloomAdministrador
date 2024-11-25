using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    [DataContract]
    public class ManejadorExcepciones
    {
        [DataMember]
        public string mensaje {  get; set; }

        public ManejadorExcepciones(string mensajeError)
        {
            mensaje = mensajeError;
        }
        public string Mensaje
        {
            get { return mensaje; }
        }
    }
}
