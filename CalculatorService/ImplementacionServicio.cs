using AccesoDatos;
using ServicioAdministrador;
using System;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading;

namespace CalculatorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CalculatorService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode= ConcurrencyMode.Reentrant)]
    public partial class ImplementacionServicio : IJugador
    {

        public void AgregarJugador(Jugador jugador)
        {
            try
            {
                var nuevoJugador = new Jugador
                {
                    nombreUsuario = jugador.nombreUsuario,
                    nombre = jugador.nombre,
                    apellidos = jugador.apellidos,
                    correo = jugador.correo,
                    contraseña = jugador.contraseña,
                    tipo = jugador.tipo,
                };

                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador agregado " + jugador.nombreUsuario;
                OperationContext.Current.GetCallbackChannel<IJugadorCallback>().RespuestaJugador(mensaje);


            }
            catch (SqlException ex)
            {
                throw ManejadorExcepciones.CrearSqlException(ex);
            }

        }

        public void ActualizarJugador(Jugador jugador)
        {
            try
            {
                var nuevoJugador = new Jugador
                {
                    nombreUsuario = jugador.nombreUsuario,
                    nombre = jugador.nombre,
                    apellidos = jugador.apellidos,
                    correo = jugador.correo,
                    contraseña = jugador.contraseña,
                    tipo = jugador.tipo,
                };

                AccesoBaseDeDatos.ActualizarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador actualizado " + jugador.nombreUsuario;
                OperationContext.Current.GetCallbackChannel<IJugadorCallback>().RespuestaJugador(mensaje);


            }
            catch (SqlException ex)
            {
                throw ManejadorExcepciones.CrearSqlException(ex);
            }
        }

        void IJugador.AgregarJugador(BlbibliotecaClases.Jugador jugador)
        {
            throw new NotImplementedException();
        }

        void IJugador.ActualizarJugador(BlbibliotecaClases.Jugador jugador)
        {
            throw new NotImplementedException();
        }
    }

}
