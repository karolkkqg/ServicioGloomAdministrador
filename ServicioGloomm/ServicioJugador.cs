using AccesoDatos;
using BibliotecaClases;
using ServicioGlomm;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ServicioGloomm
{

    public partial class ServicioJuego : IJugador
    {
        private static readonly Dictionary<string, BibliotecaClases.Jugador> jugadoresInvitados = new Dictionary<string, BibliotecaClases.Jugador>();
        private static readonly List<int> numeroJugadorInvitado = new List<int>();
        private static readonly List<string> usuarioConectado = new List<string>();

        public int AgregarJugador(BibliotecaClases.Jugador jugador)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }
            
        }

        public int ActualizarJugador(BibliotecaClases.Jugador jugador)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }
        }

        public int AutenticarJugador(BibliotecaClases.Jugador jugador)
        {
           
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                var nuevoJugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = jugador.nombreUsuario,
                    Contraseña = jugador.contraseña,

                };
                ValidarJugadorConectado(jugador.nombreUsuario);
                int resultado = AccesoJugador.ValidarJugadorParaAutenticacion(nuevoJugador);
                usuarioConectado.Add(jugador.nombreUsuario);
                return resultado;

            }
            catch (InvalidOperationException ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Message, ex.Message));
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }
        }

        private void ValidarJugadorConectado(string nombreUsuario)
        {
            if (usuarioConectado.Contains(nombreUsuario))
            {
                throw new InvalidOperationException("46");
            }
        }

        public BibliotecaClases.Jugador ObtenerJugador(string nombreUsuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
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

        public BibliotecaClases.Jugador AgregarJugadorInvitado()
        {
            int numeroInvitado = (numeroJugadorInvitado.Count > 0) ? numeroJugadorInvitado.Last() + 1 : 1;

            BibliotecaClases.Jugador jugadorInvitado = new BibliotecaClases.Jugador
            {
                nombreUsuario = "Invitado" + numeroInvitado,
                nombre = "Jugador invitado anónimo",
                apellidos = "Jugador invitado anónimo",
                correo = "sin correo",
                contraseña = "sin contraseña",
                tipo = "Invitado",
                icono = "Imagenes/PerfilUnicornio.png"
            }; 
                jugadoresInvitados[jugadorInvitado.nombreUsuario] = jugadorInvitado;
                numeroJugadorInvitado.Add(numeroInvitado);
            return jugadorInvitado;
        }
        
        public BibliotecaClases.Jugador ObtenerJugadorInvitado(string nombreUsuario)
        {
            if (jugadoresInvitados.TryGetValue(nombreUsuario, out var jugador))
            {
                return jugador;
            }
            throw new KeyNotFoundException("El jugador no fue encontrado.");
        }
        
        public bool EliminarJugadorInvitado(string nombreUsuario)
        {
            return jugadoresInvitados.Remove(nombreUsuario);
        }

        public void LimpiarListaNumeroJugadores()
        {
            numeroJugadorInvitado.Clear();
        }
        public void LimpiarListaJugadoresInvitados()
        {
            jugadoresInvitados.Clear();
        }

        public void CerrarSesionJugador(string nombreUsuario)
        {
            if (usuarioConectado.Contains(nombreUsuario))
            {
                usuarioConectado.Remove(nombreUsuario);
            }
        }
    }
}
