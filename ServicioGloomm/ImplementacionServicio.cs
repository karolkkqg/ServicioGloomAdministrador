using AccesoDatos;
using BlbibliotecaClases;
using System;
using System.Data.SqlClient;
using System.ServiceModel;

namespace ServicioGloomm
{
   
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ImplementacionServicio : IServicioAdministrador, IJugador
    {  

     /*   public void ActualizarJugador(Jugador jugador)
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
                OperationContext.Current.GetCallbackChannel<IJugadorCallback>().RespuestaJugador( ,mensaje);


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
        }*/

        public int AgregarJugador(BlbibliotecaClases.Jugador jugador)
        {
            int resultado;
            try
            {
                var nuevoJugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = jugador.nombreUsuario,
                    Nombre = jugador.nombre,
                    Apellidos = jugador.apellidos,
                    Correo = jugador.correo,
                    Contraseña = jugador.contraseña,
                    Tipo = jugador.tipo,
                    Icono = jugador.icono,
                };

                 resultado= AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador agregado " + jugador.nombreUsuario;
                return resultado;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
            
        }

        public int ActualizarJugador(BlbibliotecaClases.Jugador jugador)
        {
            try
            {
                var nuevoJugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = jugador.nombreUsuario,
                    Nombre = jugador.nombre,
                    Apellidos = jugador.apellidos,
                    Correo = jugador.correo,
                    Contraseña = jugador.contraseña,
                    Tipo = jugador.tipo,
                    Icono = jugador.icono,
                };

                int resultado = AccesoBaseDeDatos.ActualizarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador actualizado " + jugador.nombreUsuario;
                return resultado;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        public int AutenticarJugador(BlbibliotecaClases.Jugador jugador)
        {
            try
            {
                var nuevoJugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = jugador.nombreUsuario,
                    Contraseña = jugador.contraseña,

                };

                int resultado = AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(nuevoJugador);
                String mensaje = "Jugador actualizado " + jugador.nombreUsuario;
                return resultado;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }
    }
}
