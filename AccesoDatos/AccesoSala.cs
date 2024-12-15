using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class AccesoSala
    {
        public static int AgregarPartidaABaseDeDatos(BibliotecaClases.Sala sala)
        {
            try
            {
                var salaLista = ConvertirASala(sala);
                Sala nombreSalaValida = ValidarNombreSala(salaLista);
                var filasAfectadas = EjecutarRegistroPartida(nombreSalaValida);
                return filasAfectadas;
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.Message);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", ex.Message));
                

            }
        }

        public static int EjecutarRegistroPartida(Sala sala)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    contexto.Sala.Add(sala);
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

        private static Sala ValidarNombreSala(Sala sala)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var nombreSalaEncontrada = contexto.Sala.FirstOrDefault(j => j.NombreSala == sala.NombreSala);
                    if (nombreSalaEncontrada != null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("8", "Nombre de sala ocupado"));
                    }
                    return sala;
                }
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
            
        }

        public static List<Sala> ObtenerHistorialPartidas(string nombreJugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var salas = contexto.Sala
        .Where(s => s.Ganador != "Sin ganador" &&
                    s.Ganador != "En partida" &&
                    contexto.Participantes.Any(p => p.IdPartida == s.IdSala && p.NombreUsuario == nombreJugador)).ToList();

                    return salas;
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

        public static List<string> ObtenerParticipantesDeSala(string idSala)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var participantes = contexto.Participantes
                        .Where(p => p.IdPartida == idSala)
                        .Select(p => p.NombreUsuario)
                        .ToList();

                    return participantes;
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

        public static int AgregarParticipante(BibliotecaClases.Sala participanteSala)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var particpanteSalaCorrecto = ConvertirAParticpante(participanteSala);
                    contexto.Participantes.Add(particpanteSalaCorrecto);
                    int filasAfectadas = contexto.SaveChanges();
                    return filasAfectadas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(
                    new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }

        public static Sala BuscarPartida(String nombreSala, String codigo)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var sala = contexto.Sala.FirstOrDefault(p => p.NombreSala == nombreSala && p.Codigo == codigo);

                    if (sala == null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("10", "No se encontró ninguna sala con ese código o id"));
                    }

                    return sala;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(
                    new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }

        private static AccesoDatos.Sala ConvertirASala(BibliotecaClases.Sala sala)
        {
            return new AccesoDatos.Sala
            {
                IdSala = sala.idSala,
                NombreSala = sala.nombreSala,
                TipoSala = sala.tipoSala,
                TipoPartida = sala.tipoPartida,
                NoJugadores = sala.noJugadores,
                Codigo = sala.codigo,
                IdAdministrador = sala.idAdministrador,
                Fecha = sala.fecha,
                Ganador = sala.ganador
            };
        }

        private static AccesoDatos.Participantes ConvertirAParticpante(BibliotecaClases.Sala participante)
        {
            return new AccesoDatos.Participantes
            {
                IdPartida = participante.idSala,
                NombreUsuario = participante.jugador
            };
        }

        public static int ActualizarEstadoPartida(string idPartida, string ganador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var partida = contexto.Sala.FirstOrDefault(p => p.IdSala == idPartida);
                    partida.Ganador = ganador;
                    int filasAfectadas = contexto.SaveChanges();
                    return filasAfectadas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }

        public static string BuscarCodigoSala(string administrador, string nombreSala)
        {
            using (var contexto = new EntidadesGloom())
            {
                var codigoSala = contexto.Sala.Where(c => c.IdAdministrador == administrador && c.NombreSala == nombreSala).Select(c => c.Codigo).FirstOrDefault();
                if (codigoSala == null)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("10", "No se encontró ninguna sala con ese código o id"));
                }

                return codigoSala;

            }
        }

        public static List<Sala> ObtenerSalasEnPartida()
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var salasEnPartida = contexto.Sala
                        .Where(s => s.Ganador == "Sin ganador")
                        .ToList();

                    return salasEnPartida;
                }

            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(
                    new ManejadorExcepciones(ex.Number.ToString(), ex.Message));
            }
            catch (EntityException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("41", "Error con la base de datos"));
            }
        }
    }
}
