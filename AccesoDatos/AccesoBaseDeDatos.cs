using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class AccesoBaseDeDatos
    {
        public static Jugador ConvertirAJugador(Jugador jugador)
        {
            return new Jugador
            {
                NombreUsuario = jugador.NombreUsuario,
                Nombre = jugador.Nombre,
                Apellidos = jugador.Apellidos,
                Correo = jugador.Correo,
                Contraseña = jugador.Contraseña,
                Tipo = jugador.Tipo,
                Icono = jugador.Icono,
            };
        }

        public static int AgregarJugadorABaseDeDatos(Jugador jugador)
        {
            try
            {
                Jugador jugadrovalidoCorreo = ValidarCorreoJugador(jugador);
                Jugador jugadorValidoNombre = ValidarUsuarioJugador(jugadrovalidoCorreo);
                var filasAfectadas = EjecutarAgregarJugadorABaseDeDatos(jugadorValidoNombre);
                return filasAfectadas;
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
        }

        public static int EjecutarAgregarJugadorABaseDeDatos(Jugador jugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorEntidad = ConvertirAJugador(jugador);
                    contexto.Jugador.Add(jugadorEntidad);
                    int filasAfectadas = contexto.SaveChanges();
                    return filasAfectadas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
        }
        public static int ActualizarJugadorABaseDeDatos(Jugador jugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorEntidad = ConvertirAJugador(jugador);
                    contexto.Jugador.Attach(jugadorEntidad);
                    contexto.Entry(jugadorEntidad).State = EntityState.Modified;
                    int filasAfectadas = contexto.SaveChanges();
                    return filasAfectadas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
        }

        private static Jugador ValidarCorreoJugador(Jugador jugador)
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugadorConCorreo = contexto.Jugador.FirstOrDefault(j => j.Correo == jugador.Correo);
                if (jugadorConCorreo != null)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("2"));
                }
                return jugador;
            }
        }
        private static Jugador ValidarUsuarioJugador(Jugador jugador)
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugadorConNombreUsuario = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == jugador.NombreUsuario);
                if (jugadorConNombreUsuario != null)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("1"));
                }
                return jugador;
            }
        }
        public static int ValidarJugadorParaAutenticacion(Jugador jugador)
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugadorEncontrado = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == jugador.NombreUsuario && j.Contraseña == jugador.Contraseña);
                if (jugadorEncontrado == null)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("3"));
                }
                return 1;
            }
        }
    }
}
