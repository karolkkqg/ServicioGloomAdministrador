/* using System;
using System.Data.SqlClient;
using System.ServiceModel;

namespace ServicioGloom
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CalculatorService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ImplementacionServicio : IServicioAdministrador, IJugador
    {
        public void AgregarJugador(Jugador jugador)
        {
            try
            {
                var nuevoJugador = new Jugador
                {
                    NombreUsuario = jugador.NombreUsuario,
                    Nombre = jugador.Nombre,
                    Apellidos = jugador.Apellidos,
                    Correo = jugador.Correo,
                    Contraseña = jugador.Contraseña,
                    Tipo = jugador.Tipo,
                    Icono = jugador.Icono,
                };

                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador agregado " + jugador.NombreUsuario;
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
                    NombreUsuario = jugador.NombreUsuario,
                    Nombre = jugador.Nombre,
                    Apellidos = jugador.Apellidos,
                    Correo = jugador.Correo,
                    Contraseña = jugador.Contraseña,
                    Tipo = jugador.Tipo,
                    Icono = jugador.Icono,
                };

                AccesoBaseDeDatos.ActualizarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador actualizado " + jugador.NombreUsuario;
                OperationContext.Current.GetCallbackChannel<IJugadorCallback>().RespuestaJugador(mensaje);


            }
            catch (SqlException ex)
            {
                throw ManejadorExcepciones.CrearSqlException(ex);
            }
        }

        public void AutenticarJugador(Jugador jugador)
        {
            try
            {
                Jugador jugadorValido = CrearJugadorValidoParaInicio(jugador.Correo, jugador.Contraseña);

                AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(jugadorValido);
                String mensaje = "Jugador autenticado" + jugador.NombreUsuario;
                OperationContext.Current.GetCallbackChannel<IJugadorCallback>().RespuestaJugador(mensaje);


            }
            catch (SqlException ex)
            {
                throw ManejadorExcepciones.CrearSqlException(ex);
            }
        }

        private Jugador CrearJugadorValidoParaInicio(string correo, string contrasena)
        {
            Jugador jugador = new Jugador();
            //Jugador jugador = Jugador.GetInstancia();
            //jugador.LimpiarSesion();
            jugador.Correo = correo;
            jugador.Contraseña = contrasena;
            return jugador;
        }

        void IJugador.AgregarJugador(BlbibliotecaClases.Jugador jugador)
        {
            throw new NotImplementedException();
        }

        void IJugador.ActualizarJugador(BlbibliotecaClases.Jugador jugador)
        {
            throw new NotImplementedException();
        }

        void IJugador.AutenticarJugador(BlbibliotecaClases.Jugador jugador)
        {
            throw new NotImplementedException();
        }
    }
}
*/