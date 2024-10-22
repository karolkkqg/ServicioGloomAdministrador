using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BlbibliotecaClases
{
    [DataContract]
    public class ManejadorExcepciones
    {
        [DataMember]
        public string mensaje { get; set; }
        

        public ManejadorExcepciones(string mensajeError)
        {
            mensaje = mensajeError;
        }

        public static string ObtenerMensajeError(int codigoError)
        {
            var mensajesDeError = new Dictionary<int, string>
            {
                { 53, "No se puede encontrar el servidor SQL." },
                { 4060, "No se pudo abrir la base de datos." },
                { 10054, "Se interrumpió la conexión de red." },
                { 10060, "Tiempo de espera agotado al conectar." },
                { 11001, "Nombre del servidor SQL incorrecto." },
                { 18456, "Error de autenticación." }
            };

            return mensajesDeError.ContainsKey(codigoError)
                ? mensajesDeError[codigoError]
                : "Error desconocido.";
        }

        public static bool EsErrorDeConexion(SqlException sqlEx)
        {
            int[] codigosDeErrorConexion = { 53, 4060, 10054, 10060, 11001, 18456 };
            return codigosDeErrorConexion.Contains(sqlEx.Number);
        }

        public static ManejadorExcepciones CrearSqlException(SqlException sqlEx)
        {
            string mensaje = ObtenerMensajeError(sqlEx.Number);
            return new ManejadorExcepciones(mensaje);
        }

        public static ManejadorExcepciones PropagarExcepcion(ManejadorExcepciones ex)
        {
            return new ManejadorExcepciones(ex.mensaje);
        }
    }
}
