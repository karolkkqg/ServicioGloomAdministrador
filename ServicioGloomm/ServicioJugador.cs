using AccesoDatos;
using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ServicioGloomm
{
   
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServicioJuego : IJugador
    {  
        public int AgregarJugador(BibliotecaClases.Jugador jugador)
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

                resultado= AccesoJugador.AgregarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador agregado " + jugador.nombreUsuario;
                return resultado;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
            
        }

        public int ActualizarJugador(BibliotecaClases.Jugador jugador)
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

                int resultado = AccesoJugador.ActualizarJugadorABaseDeDatos(nuevoJugador);
                String mensaje = "Jugador actualizado " + jugador.nombreUsuario;
                return resultado;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        public int AutenticarJugador(BibliotecaClases.Jugador jugador)
        {
            try
            {
                var nuevoJugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = jugador.nombreUsuario,
                    Contraseña = jugador.contraseña,

                };

                int resultado = AccesoJugador.ValidarJugadorParaAutenticacion(nuevoJugador);
                String mensaje = "Jugador actualizado " + jugador.nombreUsuario;
                return resultado;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        public BibliotecaClases.Jugador ObtenerJugador(string nombreUsuario)
        {
            try
            {
                AccesoDatos.Jugador jugadorDb;
                BibliotecaClases.Jugador jugadorBiblioteca = new BibliotecaClases.Jugador();

                jugadorDb = AccesoJugador.BuscarJugadorPorNombreUsuario(nombreUsuario);
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

        public List<BibliotecaClases.Jugador> BuscarJugadoresPorNombreUsuario(string nombreUsuarioParcial)
        {
            var jugadores = AccesoJugador.BuscarJugadoresPorNombreUsuario(nombreUsuarioParcial);

            var listaJugadores = jugadores.Select(j => new BibliotecaClases.Jugador
            {
                nombreUsuario = j.nombreUsuario,
                nombre = j.nombre,
                apellidos = j.apellidos,
                correo = j.correo,
                tipo = j.tipo,
                icono = j.icono
            }).ToList();

            return listaJugadores;
        }
    }
}
