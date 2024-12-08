using BibliotecaClases;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class AccesoJugador
    {
        public static Jugador ConvertirAJugador(Jugador jugador)
        {
            return new Jugador
            {
                NombreUsuario = jugador.NombreUsuario,
                Nombre = jugador.Nombre,
                Apellidos = jugador.Apellidos,
                Correo = jugador.Correo,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(jugador.Contraseña),
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }
        public static int ActualizarJugadorABaseDeDatos(Jugador jugador)
        {
            try
            {
                ValidarCorreoActualizacionJugador(jugador);
                int resultado = EjecutarActualizacionABaseDeDatos(jugador);
                return resultado;
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }

        private static int EjecutarActualizacionABaseDeDatos(Jugador jugador)
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }
        private static Jugador ValidarCorreoActualizacionJugador(Jugador jugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorConCorreo = contexto.Jugador
                        .FirstOrDefault(j => j.Correo == jugador.Correo && j.NombreUsuario != jugador.NombreUsuario);

                    if (jugadorConCorreo != null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("2", "Correo registrado"));
                    }

                    return jugador;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
            
        }

        private static Jugador ValidarCorreoJugador(Jugador jugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorConCorreo = contexto.Jugador.FirstOrDefault(j => j.Correo == jugador.Correo);
                    if (jugadorConCorreo != null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("2", "Correo registrado"));
                    }
                    return jugador;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
          
        }
        private static Jugador ValidarUsuarioJugador(Jugador jugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorConNombreUsuario = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == jugador.NombreUsuario);
                    if (jugadorConNombreUsuario != null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("1", "Nombre de usuario registrado"));
                    }
                    return jugador;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }
        public static int ValidarJugadorParaAutenticacion(Jugador jugador)
        {
            jugador.Contraseña = jugador.Contraseña.Trim();
            bool contrasenaValida = false;
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                  
                        var jugadorEncontrado = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == jugador.NombreUsuario);
                   
                    if (jugadorEncontrado == null)
                        {
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("3", "Nombre usuario no encontrado en registro"));
                        }
                    jugadorEncontrado.Contraseña = jugadorEncontrado.Contraseña.Trim();
                    contrasenaValida = BCrypt.Net.BCrypt.Verify(jugador.Contraseña, jugadorEncontrado.Contraseña);
                    if (!contrasenaValida)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("3", "Contraseña incorrecta"));
                    }
                    return 1;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
            
        }

        public static Jugador BuscarJugadorPorNombreUsuario(string nombreUsuario)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadorConNombreUsuario = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == nombreUsuario);
                    if (jugadorConNombreUsuario == null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("57", "El jugador no está registrado"));
                    }
                    BibliotecaClases.Jugador jugadorBiblioteca = ConvertirAClasesBiblioteca(jugadorConNombreUsuario);
                    return jugadorConNombreUsuario;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }

        public static List<BibliotecaClases.Jugador> BuscarJugadoresPorNombreUsuario(string nombreParcial)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugadoresConCoincidencia = contexto.Jugador
                        .Where(j => j.NombreUsuario.Contains(nombreParcial))
                        .ToList();

                    var jugadoresBiblioteca = jugadoresConCoincidencia
                        .Select(j => ConvertirAClasesBiblioteca(j))
                        .ToList();

                    return jugadoresBiblioteca;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }

        private static BibliotecaClases.Jugador ConvertirAClasesBiblioteca(Jugador jugadorDb)
        {
            var jugador = new BibliotecaClases.Jugador
            {
                nombreUsuario = jugadorDb.NombreUsuario,
                nombre = jugadorDb.Nombre,
                apellidos = jugadorDb.Apellidos,
                correo = jugadorDb.Correo,
                contraseña = jugadorDb.Contraseña,
                tipo = jugadorDb.Tipo,
                icono = jugadorDb.Icono,

            };

            return jugador;
        }
    }
}
