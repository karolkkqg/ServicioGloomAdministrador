using AccesoDatos;
using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServicioJuego : ISala
    {
        public int AgregarParticipantesAPartida(BibliotecaClases.Sala sala)
        {
            int resultado = AccesoSala.AgregarParticipante(sala);
            return resultado;
        }

        public List<BibliotecaClases.Sala> ObtenerDatosHistorial(string nombreUsuario)
        {
            var historial = AccesoSala.ObtenerHistorialPartidas(nombreUsuario);

            var listaSalas = historial.Select(s => new BibliotecaClases.Sala
            {
                idSala = s.IdSala,
                nombreSala = s.NombreSala,
                tipoSala = s.TipoSala,
                tipoPartida = s.TipoPartida,
                noJugadores = s.NoJugadores,
                codigo = s.Codigo,
                idAdministrador = s.IdAdministrador,
                fecha = s.Fecha,
                ganador = s.Ganador,
                jugador = nombreUsuario
            }).ToList();

            return listaSalas;
        }

        public List<String> ObtenrParticipantesDeJuego(string identificadorSala)
        {
            var participantes = AccesoSala.ObtenerParticipantesDeSala(identificadorSala);

            return participantes;
        }

        public int CrearPartida(BibliotecaClases.Sala sala)
        {

            try
            {
                String codigoGenerado = generarCodigo();
                sala.codigo = codigoGenerado;
                sala.idSala = codigoGenerado;

                var nuevaPartida = new BibliotecaClases.Sala
                {
                    nombreSala = sala.nombreSala,
                    tipoSala = sala.tipoSala,
                    tipoPartida = sala.tipoPartida,
                    noJugadores = sala.noJugadores,
                    codigo = sala.codigo,
                    idAdministrador = sala.idAdministrador,
                    fecha = sala.fecha,
                    ganador = sala.ganador,
                    idSala = sala.idSala,

                };


                int resultado = AccesoSala.AgregarPartidaABaseDeDatos(nuevaPartida);
                String mensaje = "Partida creada " + sala.nombreSala;
                return resultado;
            }


            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        private string generarCodigo()
        {
            string codigoGenerado;

            do
            {
                string caracteresPermitidos = "0123456789";
                Random random = new Random();

                codigoGenerado = new string(Enumerable.Repeat(caracteresPermitidos, 5)
                    .Select(selection => selection[random.Next(selection.Length)]).ToArray());


            } while (!codigoValido(codigoGenerado));
            return codigoGenerado;
        }

        private bool codigoValido(String codigo)
        {
            bool valido = false;
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var codigoExistente = contexto.Sala.FirstOrDefault(salaCodigo => salaCodigo.Codigo == codigo);
                    if (codigoExistente == null)
                    {
                        valido = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                valido = false;
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("9"));

            }

            return valido;
        }
    }
}
