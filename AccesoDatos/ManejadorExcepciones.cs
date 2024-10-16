using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public enum TipoErrorJugador
    {
        Conexion,
        DatosInvalidos,
        NoEncontrado,
        Desconocido
    }

    public class ManejadorExcepciones : Exception
    {
        public TipoErrorJugador TipoError { get; }

        public ManejadorExcepciones(TipoErrorJugador tipoError, string message)
            : base(message)
        {
            TipoError = tipoError;
        }

        public ManejadorExcepciones(TipoErrorJugador tipoError, string message, Exception innerException)
            : base(message, innerException)
        {
            TipoError = tipoError;
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

            return mensajesDeError.TryGetValue(codigoError, out string mensaje)
                ? mensaje
                : "Error SQL desconocido.";
        }

        public static bool EsErrorDeConexion(SqlException sqlEx)
        {
            int[] codigosDeErrorConexion = { 53, 4060, 10054, 10060, 11001, 18456 };
            return codigosDeErrorConexion.Contains(sqlEx.Number);
        }

        public static ManejadorExcepciones CrearSqlException(SqlException sqlEx)
        {
            var tipoError = EsErrorDeConexion(sqlEx)
                ? TipoErrorJugador.Conexion
                : TipoErrorJugador.Desconocido;

            string mensaje = ObtenerMensajeError(sqlEx.Number);
            return new ManejadorExcepciones(tipoError, mensaje, sqlEx);
        }
    }
}
