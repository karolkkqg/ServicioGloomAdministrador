using AccesoDatos;
using BibliotecaClases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    
    public partial class ServicioJuego : ISala
    {
        private static readonly Dictionary<string, ISalaCallback> salaJugadoresCallback = new Dictionary<string, ISalaCallback>();
        private static readonly Dictionary<string, (string nombrePersonaje, int vida)> personajesPorUsuario = new Dictionary<string, (string, int)>();
        private static readonly List<string> personajesUsados = new List<string>();

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

        public BibliotecaClases.Sala BuscarSalaExistente(String idSala, String codigo)
        {
            try
            {
                AccesoDatos.Sala SalaDb;
                BibliotecaClases.Sala SalaBiblioteca = new BibliotecaClases.Sala();

                SalaDb = AccesoSala.BuscarPartida(idSala, codigo);
                SalaBiblioteca.fecha = SalaDb.Fecha;
                SalaBiblioteca.idSala = idSala;
                SalaBiblioteca.tipoSala = SalaDb.TipoSala;
                SalaBiblioteca.tipoPartida = SalaDb.TipoPartida;
                SalaBiblioteca.ganador = SalaDb.Ganador;
                SalaBiblioteca.codigo = codigo;
                SalaBiblioteca.nombreSala = SalaDb.NombreSala;
                SalaBiblioteca.noJugadores = SalaDb.NoJugadores;
                SalaBiblioteca.idAdministrador = SalaDb.IdAdministrador;

                return SalaBiblioteca;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }

        }

        public void ConectarConSala(string nombreUsuario)
        {
            if (!salaJugadoresCallback.ContainsKey(nombreUsuario))
            {
                salaJugadoresCallback.Add(nombreUsuario, OperationContext.Current.GetCallbackChannel<ISalaCallback>());
            }
        }

        public List<string> ObtenerJugadoresConectados(String nombreUsuario)
        {
            return salaJugadoresCallback.Keys.ToList();
        }

        public void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, int vida)
        {
            EstaSiendoUtilizado(nombrePersonaje);
           
            if (personajesPorUsuario.ContainsKey(nombreUsuario))
            {
                var personajeAnterior = personajesPorUsuario[nombreUsuario].Item1;
                personajesPorUsuario[nombreUsuario] = (nombrePersonaje, vida);
                personajesUsados.Remove(personajeAnterior);
            }
            else
            {
                personajesPorUsuario.Add(nombreUsuario, (nombrePersonaje, vida));
            }
        }
        private void EstaSiendoUtilizado(string nombrePersonaje)
        {
            if (personajesUsados.Contains(nombrePersonaje))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("14"));
            }
            else
            {
                personajesUsados.Add(nombrePersonaje);
            }
        }

        public void validarPersonajesSeleccionados(int cantidadJugadores)
        {
            if (cantidadJugadores != personajesUsados.Count())
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("15"));
            }
        }

        public Dictionary<string, (string nombrePersonaje, int vida)> ObtenerUsuariosYPersonajes()
        {
            return new Dictionary<string, (string nombrePersonaje, int vida)>(personajesPorUsuario);
        }

        public void LimpiarListaJugadores()
        {
            personajesPorUsuario.Clear();
        }

        public void LimpiarListaPersonajes()
        {
            personajesUsados.Clear();
        }
    }
}
