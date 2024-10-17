using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
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

        public static void AgregarJugadorABaseDeDatos(Jugador jugador)
        {
            try
            {
                ValidarCorreoJugador(jugador);
                ValidarUsuarioJugador(jugador);
                EjecutarAgregarJugadorABaseDeDatos(jugador);

            }
            catch (ManejadorExcepciones ex)
            {
                throw ManejadorExcepciones.PropagarExpcecion(ex);
            }
        }

        public static void EjecutarAgregarJugadorABaseDeDatos(Jugador jugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorEntidad = ConvertirAJugador(jugador);
                    contexto.Jugador.Add(jugadorEntidad);
                    contexto.SaveChanges();
                }
            }
            catch (SqlException ex)
            {
                throw ManejadorExcepciones.CrearSqlException(ex);
            }
        }
        public static void ActualizarJugadorABaseDeDatos(Jugador jugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorEntidad = ConvertirAJugador(jugador);
                    contexto.Jugador.Attach(jugadorEntidad);
                    contexto.Entry(jugadorEntidad).State = EntityState.Modified;
                    contexto.SaveChanges();
                }
            }
            catch (SqlException ex)
            {
                throw ManejadorExcepciones.CrearSqlException(ex);
            }
        }

        private static void ValidarCorreoJugador(Jugador jugador)
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugadorConCorreo = contexto.Jugador.FirstOrDefault(j => j.Correo == jugador.Correo);
                if (jugadorConCorreo != null)
                {
                    throw new ManejadorExcepciones(TipoErrorJugador.DatosInvalidos, "Este correo ya está registrado en el sistema.");
                }
            }
        }
        private static void ValidarUsuarioJugador(Jugador jugador)
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugadorConNombreUsuario = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == jugador.NombreUsuario);
                if (jugadorConNombreUsuario != null)
                {
                    throw new ManejadorExcepciones(TipoErrorJugador.DatosInvalidos, "Este nombre de usuario ya está registrado en el sistema.");
                }
            }
        }
    }
}
