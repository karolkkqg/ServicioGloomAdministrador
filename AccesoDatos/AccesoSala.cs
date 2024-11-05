using BibliotecaClases;
using System;
using System.Collections.Generic;
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
            }
        }

        private static Sala ValidarNombreSala(Sala sala)
        {
            using (var contexto = new EntidadesGloom())
            {
                var nombreSalaEncontrada = contexto.Sala.FirstOrDefault(j => j.NombreSala == sala.NombreSala);
                if (nombreSalaEncontrada != null)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("8"));
                }
                return sala;
            }
        }

        public static List<Sala> ObtenerHistorialPartidas(string nombreJugador)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var salas = contexto.Sala
                .Where(s => s.Ganador != "no hay ganador" &&
                            contexto.Participantes.Any(p => p.IdPartida == s.IdSala && p.NombreUsuario == nombreJugador))
                .ToList();

                    return salas;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString()));
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
                    new ManejadorExcepciones(ex.Number.ToString()));
            }
        }

        public static Sala BuscarPartida(String idSala, String codigo)
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var sala = contexto.Sala.FirstOrDefault(p => p.IdSala == idSala && p.Codigo == codigo);

                    if (sala == null)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("10"));
                    }

                    return sala;
                }
            }
            catch (SqlException ex)
            {
                throw new FaultException<ManejadorExcepciones>(
                    new ManejadorExcepciones(ex.Number.ToString()));
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
    }
}
