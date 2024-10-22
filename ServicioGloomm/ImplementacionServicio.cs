using AccesoDatos;
using BlbibliotecaClases;
using ServicioGlomm;
using System;
using System.Data.SqlClient;
using System.ServiceModel;

namespace ServicioGloomm
{
   
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ImplementacionServicio : IServicioAdministrador, IJugador, IPartida
    {  
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

        public BlbibliotecaClases.Jugador ObtenerJugador(string nombreUsuario)
        {
            try
            {
                AccesoDatos.Jugador jugadorDb;
                BlbibliotecaClases.Jugador jugadorBiblioteca = new BlbibliotecaClases.Jugador();

                jugadorDb = AccesoBaseDeDatos.BuscarJugadorPorNombreUsuario(nombreUsuario);
                jugadorBiblioteca.nombreUsuario = jugadorDb.NombreUsuario;
                jugadorBiblioteca.nombre = jugadorDb.Nombre;
                jugadorBiblioteca.correo = jugadorDb.Correo;
                jugadorBiblioteca.contraseña = jugadorDb.Contraseña;
                jugadorBiblioteca.apellidos = jugadorDb.Apellidos;
                jugadorBiblioteca.tipo = jugadorDb.Tipo;
                jugadorBiblioteca.icono = jugadorDb.Icono;

                return jugadorBiblioteca;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        public int CrearPartida(BibliotecaClases.Partida partida)
        {
            try
            {
                var nuevaPartida = new AccesoDatos.Partida
                {
                    IdPartida = partida.idPartida,
                    Ganador = partida.ganador,
                    Apellidos = partida.fecha,
                    IdAdministrador = partida.idAdministrador,
                    Tipo = partida.tipo,
                    IdSala = partida.idSala,
                }
        

                int resultado = AccesoBaseDeDatos.AgregarPartidaABaseDeDatos(nuevaPartida);
                String mensaje = "Partida creada " + partida.idPartida;
                return resultado;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }

        }
    }
}
