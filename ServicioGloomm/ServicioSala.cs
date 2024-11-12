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
        public static readonly Dictionary<string, ISalaCallback> salaJugadoresCallback = new Dictionary<string, ISalaCallback>();
        public static readonly Dictionary<string, (string nombrePersonaje, int vida)> personajesPorUsuario = new Dictionary<string, (string, int)>();
        public static readonly List<string> personajesUsados = new List<string>();

        public int AgregarParticipantesAPartida(BibliotecaClases.Sala sala)
        {
            int resultado = AccesoSala.AgregarParticipante(sala);
            return resultado;
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
            AdministradorDeComportamiento.cambiarModoComportamientoReentrante();
            if (!salaJugadoresCallback.ContainsKey(nombreUsuario))
            {
                salaJugadoresCallback.Add(nombreUsuario, OperationContext.Current.GetCallbackChannel<ISalaCallback>());
                foreach (var jugador in salaJugadoresCallback)
                {
                    if (salaJugadoresCallback.ContainsKey(jugador.Key))
                    {
                        try
                        {
                            salaJugadoresCallback[jugador.Key].ActualizarNumeroJugadores();
                        }
                        catch (CommunicationException ex)
                        {
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                        }
                        catch (TimeoutException ex)
                        {
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                        }
                    }
                }
            }
        }

        public List<string> ObtenerJugadoresConectados(String nombreUsuario)
        {
            return salaJugadoresCallback.Keys.ToList();
        }

        public void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, int vida)
        {
            string personajeAnterior="sin personaje";

            EstaSiendoUtilizado(nombrePersonaje);
           
            if (personajesPorUsuario.ContainsKey(nombreUsuario))
            {
                personajeAnterior = personajesPorUsuario[nombreUsuario].Item1;
                personajesPorUsuario[nombreUsuario] = (nombrePersonaje, vida);
                personajesUsados.Remove(personajeAnterior);
            }
            else
            {
                personajesPorUsuario.Add(nombreUsuario, (nombrePersonaje, vida));
            }
            AdministradorDeComportamiento.cambiarModoComportamientoReentrante();
            foreach (var jugador in salaJugadoresCallback)
            {
                if (salaJugadoresCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        salaJugadoresCallback[jugador.Key].ActualizarImagenPersonaje(nombrePersonaje, personajeAnterior);
                    }
                    catch (CommunicationException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
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

        public void EmpezarPartida(string idSala)
        {
            AdministradorDeComportamiento.cambiarModoComportamientoReentrante();
            foreach (var jugador in salaJugadoresCallback)
            {
                if (salaJugadoresCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        salaJugadoresCallback[jugador.Key].EmpezarJuego();
                    }
                    catch (CommunicationException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        public void SacarDeSala(string nombreUsuario)
        {
            EliminarJugadorDeSala(nombreUsuario);
            string personaje = EliminarJugadorYPersonaje(nombreUsuario);
            EliminarPersonajeUsado(personaje);
        }

        public void EliminarJugadorDeSala(string nombreUsuario)
        {
            AdministradorDeComportamiento.cambiarModoComportamientoReentrante();

            salaJugadoresCallback.Remove(nombreUsuario);
            foreach (var jugador in salaJugadoresCallback)
            {
                if (salaJugadoresCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        salaJugadoresCallback[jugador.Key].ActualizarNumeroJugadores();
                    }
                    catch (CommunicationException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        private void EliminarPersonajeUsado(string nombrePersonaje)
        {
            personajesUsados.Remove(nombrePersonaje);
        }

        private string EliminarJugadorYPersonaje(string nombreUsuario)
        {
            personajesPorUsuario.TryGetValue(nombreUsuario, out var InformacionPersonaje);
            personajesPorUsuario.Remove(nombreUsuario);
            personajesUsados.Remove(InformacionPersonaje.nombrePersonaje);
            return InformacionPersonaje.nombrePersonaje;
        }

        public List<string> ObtenerPersonajesUsados()
        {
            return new List<string>(personajesUsados);
        }

    }
}
