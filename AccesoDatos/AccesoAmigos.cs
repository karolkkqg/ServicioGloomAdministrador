using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class AccesoAmigos
    {
        public static int AgregarSolcitudAmistad(Amistad solicitud)
        {
            
            try
            {
                ValidarSiEsMismoJugadorSolcitud(solicitud.nombreUsuario.nombreUsuario, solicitud.jugadorAmigo.nombreUsuario);
                ValidarSiSonAmigos(solicitud.nombreUsuario.nombreUsuario, solicitud.jugadorAmigo.nombreUsuario);
                ValidarSiExisteSolcitud(solicitud.nombreUsuario.nombreUsuario, solicitud.jugadorAmigo.nombreUsuario);

                int filasAfectadas = EjecutarSolictudAmistad(solicitud);
                return filasAfectadas;
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
        }

        private static int EjecutarSolictudAmistad(Amistad solicitud)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var solictudEntidad = ConvertirASolicitud(solicitud);
                    contexto.Amigos.Add(solictudEntidad);
                    int filasAfectadas = contexto.SaveChanges();
                    return filasAfectadas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
        }

        public static void ValidarSiSonAmigos(string nombreUsuario1, string nombreUsuario2)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var amistad = contexto.Amigos
                        .Where(a =>
                            (a.NombreUsuario == nombreUsuario1 && a.JugadorAmigo == nombreUsuario2 && a.Estado == "Aceptado") ||
                            (a.NombreUsuario == nombreUsuario2 && a.JugadorAmigo == nombreUsuario1 && a.Estado == "Aceptado"))
                        .FirstOrDefault();

                    if (amistad != null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("4"));
                    }
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
            
        }

        public static void ValidarSiExisteSolcitud(string nombreUsuario1, string nombreUsuario2)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var amistad = contexto.Amigos
                        .Where(a =>
                            (a.NombreUsuario == nombreUsuario1 && a.JugadorAmigo == nombreUsuario2 && a.Estado == "Pendiente") ||
                            (a.NombreUsuario == nombreUsuario2 && a.JugadorAmigo == nombreUsuario1 && a.Estado == "Pendiente"))
                        .FirstOrDefault();

                    if (amistad != null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("5"));
                    }
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
           
        }

        public static void ValidarSiEsMismoJugadorSolcitud(string nombreUsuario1, string nombreUsuario2)
        {
            if (nombreUsuario1.Equals(nombreUsuario2, StringComparison.OrdinalIgnoreCase))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("6"));
            }

        }

        public static int CambiarEstadoSolicitud(Amistad solicitud)
        {

            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var solicitudOriginal = contexto.Amigos.FirstOrDefault(a => a.NombreUsuario == solicitud.nombreUsuario.nombreUsuario &&
                              a.JugadorAmigo == solicitud.jugadorAmigo.nombreUsuario);

                    solicitudOriginal.Estado = solicitud.estado;

                    int filasAfectadas = solicitudOriginal != null ? contexto.SaveChanges() : 0;

                    return filasAfectadas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
        }
        public static int EliminarAmigo(string nombreUsuario, string nombreAmigo)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var amigoAEliminar = contexto.Amigos.FirstOrDefault(a => a.NombreUsuario == nombreUsuario && a.JugadorAmigo == nombreAmigo);

                        contexto.Amigos.Remove(amigoAEliminar);
                        int filasAfectadas = contexto.SaveChanges();
                    return filasAfectadas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
        }

        public static List<Amigos> ObtenerAmigosDelJugador(string nombreUsuario)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var amigosAceptados = contexto.Amigos.Where(a => (a.NombreUsuario == nombreUsuario || a.JugadorAmigo == nombreUsuario)
                && a.Estado == "Aceptado").ToList();
                    return amigosAceptados;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
        }

        public static List<Amigos> ObtenerSolicitudesJugador(string nombreUsuario)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var amigosAceptados = contexto.Amigos.Where(a => a.JugadorAmigo == nombreUsuario && a.Estado == "Pendiente").ToList();
                    return amigosAceptados;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
        }


        private static BibliotecaClases.Amistad ConvertirAClasesBiblioteca(Amistad solicitudDb)
        {
            var solicitud = new BibliotecaClases.Amistad();
            {
                solicitud.nombreUsuario= solicitudDb.nombreUsuario;
                solicitud.jugadorAmigo = solicitudDb.jugadorAmigo;
                solicitud.estado = solicitudDb.estado;

            };

            return solicitud;
        }

        public static AccesoDatos.Amigos ConvertirASolicitud(BibliotecaClases.Amistad solicitud)
        {
            return new AccesoDatos.Amigos
            {
                NombreUsuario = solicitud.nombreUsuario.nombreUsuario,
                JugadorAmigo = solicitud.jugadorAmigo.nombreUsuario,
                Estado = solicitud.estado,
            };
        }

        public static string BuscarCorreoAmigo(string nombreUsurioAmigo)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {

                    var correoJugadorAmigo = contexto.Amigos
                .Where(a => a.JugadorAmigo == nombreUsurioAmigo)
                .Join(contexto.Jugador,
                      a => a.JugadorAmigo,
                      j => j.NombreUsuario,
                      (a, j) => j.Correo)
                .FirstOrDefault();

                    if (correoJugadorAmigo == null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("42"));
                    }

                    return correoJugadorAmigo;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41"));
            }
        }
    }
  

}
