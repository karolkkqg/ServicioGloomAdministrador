using BibliotecaClases;
using ServicioGlomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IServicioSalaMiniHistoria
    {
        public static readonly Dictionary<string, Dictionary<string, IServicioSalaMiniHistoriaCallback>> salaJugadoresPorSalaMiniHisotria = new Dictionary<string, Dictionary<string, IServicioSalaMiniHistoriaCallback>>();
        public static readonly Dictionary<string, List<string>> personajesUsadosPorSala = new Dictionary<string, List<string>>();
        public void ConectarConSalaMiniPartida(string numeroSala, string nombreUsuario)
        {
            if (!salaJugadoresPorSalaMiniHisotria.ContainsKey(numeroSala))
            {
                salaJugadoresPorSalaMiniHisotria[numeroSala] = new Dictionary<string, IServicioSalaMiniHistoriaCallback>();
            }
            if (!salaJugadoresPorSalaMiniHisotria[numeroSala].ContainsKey(nombreUsuario))
            {
                IServicioSalaMiniHistoriaCallback callback = OperationContext.Current.GetCallbackChannel<IServicioSalaMiniHistoriaCallback>();
                salaJugadoresPorSalaMiniHisotria[numeroSala].Add(nombreUsuario, callback);
            }
        }

        public void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            EstaSiendoUtilizado(numeroSala, nombrePersonaje);

            string personajeAnterior = AgregarJugadorYPersonaje(nombreUsuario, nombrePersonaje, numeroSala);

            ValidarJugadorNoListo(nombreUsuario);

            if (salaJugadoresPorSalaMiniHisotria.ContainsKey(numeroSala))
            {
                foreach (var jugador in salaJugadoresPorSalaMiniHisotria[numeroSala])
                {
                    try
                    {
                        jugador.Value.ActualizarImagenPersonaje(nombrePersonaje, personajeAnterior);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[numeroSala].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }
        }

        public void ValidarJugadorNoListo(string nombreUsuario)
        {
            if (jugadoresConectadosListos.ContainsKey(nombreUsuario))
            {
                jugadoresConectadosListos.Remove(nombreUsuario);
            }

        }

        public void EstaSiendoUtilizado(string numeroSala, string nombrePersonaje)
        {
            if (personajesUsadosPorSala[numeroSala].Contains(nombrePersonaje))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("14", "Personaje utilizado"));
            }
            else
            {
                personajesUsadosPorSala[numeroSala].Add(nombrePersonaje);
            }
        }

        public string AgregarJugadorYPersonaje(string nombreUsuario, string nombrePersonaje, string numeroSala)
        {
            InicializarSalaSiNoExiste(numeroSala);

            string personajeAnterior = ActualizarPersonajeAnterior(nombreUsuario, nombrePersonaje, numeroSala);
            AgregarPersonajeUsado(nombrePersonaje, personajesUsadosPorSala[numeroSala]);

            return personajeAnterior;
        }

        public void InicializarSalaSiNoExiste(string numeroSala)
        {
            if (!personajesPorSala.ContainsKey(numeroSala))
            {
                personajesPorSala[numeroSala] = new Dictionary<string, (string nombrePersonaje, int vida)>();
            }

            if (!personajesUsadosPorSala.ContainsKey(numeroSala))
            {
                personajesUsadosPorSala[numeroSala] = new List<string>();
            }
        }

        public string ActualizarPersonajeAnterior(string nombreUsuario, string nombrePersonaje, string numeroSala)
        {
            var personajesEnSala = personajesPorSala[numeroSala];
            var personajesUsados = personajesUsadosPorSala[numeroSala];

            string personajeAnterior = "sin personaje";

            if (personajesEnSala.ContainsKey(nombreUsuario))
            {
                personajeAnterior = personajesEnSala[nombreUsuario].nombrePersonaje;
                personajesEnSala[nombreUsuario] = (nombrePersonaje, 0);

                if (personajesUsados.Contains(personajeAnterior))
                {
                    personajesUsados.Remove(personajeAnterior);
                }
            }
            else
            {
                personajesEnSala[nombreUsuario] = (nombrePersonaje, 0);
            }

            return personajeAnterior;
        }

        private void AgregarPersonajeUsado(string nombrePersonaje, List<string> personajesUsados)
        {
            if (!personajesUsados.Contains(nombrePersonaje))
            {
                personajesUsados.Add(nombrePersonaje);
            }
        }

        public void EliminarPersonajeUsado(string numeroSala, string nombrePersonaje)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (personajesUsadosPorSala.ContainsKey(numeroSala))
            {
                if (personajesUsadosPorSala[numeroSala].Contains(nombrePersonaje))
                {
                    personajesUsadosPorSala[numeroSala].Remove(nombrePersonaje);
                }

                if (personajesUsadosPorSala[numeroSala].Count == 0)
                {
                    personajesUsadosPorSala.Remove(numeroSala);
                }

                if (salaJugadoresPorSalaMiniHisotria.ContainsKey(numeroSala))
                {
                    foreach (var jugador in salaJugadoresPorSalaMiniHisotria[numeroSala])
                    {
                        try
                        {
                            jugador.Value.ActualizarImagenPersonaje("sin personaje", nombrePersonaje);
                        }
                        catch (CommunicationException ex)
                        {
                            administradorLogger.RegistroError(ex);

                            salaJugadoresPorSala[numeroSala].Remove(jugador.Key);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                        }
                        catch (TimeoutException ex)
                        {
                            administradorLogger.RegistroError(ex);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                        }
                    }
                }
            }
        }
    }


}
