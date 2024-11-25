using AccesoDatos;
using BibliotecaClases;
using BlbibliotecaClases;
using ServicioGlomm;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{

    public partial class ServicioJuego : IServicioJuegoTablero
    {

        private ServicioJuego servicioCarta;
        public static readonly List<Carta> CartasSobrantes = new List<Carta>();
        public static readonly Dictionary<string, List<string>> turnosPorSala = new Dictionary<string, List<string>>();
        public static readonly Dictionary<string, int> indiceTurnoActual = new Dictionary<string, int>();
        public static readonly Dictionary<string, bool> partidaYaIniciada = new Dictionary<string, bool>();
        public static readonly Dictionary<string, string> jugadoresConectadosListos = new Dictionary<string, string>();
        public static readonly List<Carta> cartasSobrantes = new List<Carta>();
        public static readonly Dictionary<string, IJuegoAdministradorCallback> jugadoresConectadosTableroCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        public static readonly Dictionary<string, string> jugadoresConectadosTablero = new Dictionary<string, string>();
        public static readonly Dictionary<string, int> jugadoresConCastigos = new Dictionary<string, int>();

        public List<string> ObtenerJugadoresConectados(string numeroSala)
        {
            return salaJugadoresPorSala[numeroSala].Keys.ToList();
        }

            public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        private static readonly Dictionary<string, string> administradoresDeSala = new Dictionary<string, string>();
        private static readonly Dictionary<string, List<string>> votosExpulsion = new Dictionary<string, List<string>>();



        public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            if (!jugadoresConectadosListos.ContainsKey(numeroSala))
            {
                jugadoresConectadosListos.Add(nombreUsuario, numeroSala);

            }
        }

        public void IniciarPartidaPorAdministrador(string nombreAdministrador, string numeroSala, int numeroJugadores)
        {
            if (!partidaYaIniciada.ContainsKey(numeroSala) || !partidaYaIniciada[numeroSala])
            {
                partidaYaIniciada[numeroSala] = true;
                VerificarParticipantesConectados(numeroSala, numeroJugadores, nombreAdministrador);
                List<Carta> cartasSobrantes = EmpezarJuego(numeroSala);

                CartasSobrantes.Clear();
                CartasSobrantes.AddRange(cartasSobrantes);

            }
        }

        private List<Carta> EmpezarJuego(string numeroSala)
        {
            servicioCarta = new ServicioJuego();
            AsignarTurnos(numeroSala);
            var cartasSobrantes = servicioCarta.BarajearMazo(numeroSala);
            return cartasSobrantes;
        }

        private void VerificarParticipantesConectados(string numeroSala, int numeroJugadores, string nombreAdministrador)
        {
            List<string> jugadoresEnSala = ObtenerJugadores(numeroSala);
            int conteoJugadores = jugadoresEnSala.Count(jugador => jugadoresConectadosListos.ContainsKey(jugador));

            if (conteoJugadores != numeroJugadores)
            {
                jugadoresConectadosListos.Remove(nombreAdministrador);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("17"));
            }
        }

        private void AsignarTurnos(string numeroSala)
        {
            List<string> jugadores = ObtenerJugadores(numeroSala).OrderBy(j => Guid.NewGuid()).ToList();
            turnosPorSala[numeroSala] = jugadores;
            indiceTurnoActual[numeroSala] = 0;
        }

        public string AsignarPrimerTurno(string numeroSala)
        {
            string jugadorActual = turnosPorSala[numeroSala][indiceTurnoActual[numeroSala]];
            return jugadorActual;
        }

        public string ObtenerJugadorActual(string numeroSala)
        {
            int indiceActual = indiceTurnoActual[numeroSala];
            return turnosPorSala[numeroSala][indiceActual];
        }

        public void CambiarTurno(string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            var jugadores = turnosPorSala[numeroSala];
            int totalJugadores = jugadores.Count;

            int indiceActual = indiceTurnoActual[numeroSala];
            bool turnoAsignado = false;

            while (!turnoAsignado)
            {
                int siguienteIndice = (indiceActual + 1) % totalJugadores;
                indiceActual = siguienteIndice;

                string jugadorSiguiente = jugadores[siguienteIndice];

                if (!jugadoresConCastigos.ContainsKey(jugadorSiguiente))
                {
                    indiceTurnoActual[numeroSala] = siguienteIndice;
                    turnoAsignado = true;

                    foreach (var jugador in jugadoresConectadosTableroCallback)
                    {
                        if (jugadoresConectadosTableroCallback.ContainsKey(jugador.Key) && jugadoresConectadosTableroCallback[jugador.Key] != null)
                        {
                            try
                            {
                                jugadoresConectadosTableroCallback[jugador.Key].ActualizarTurno(jugadorSiguiente);
                            }
                            catch (CommunicationException ex)
                            {
                                administradorLogger.RegistroError(ex);
                                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                            }
                            catch (TimeoutException ex)
                            {
                                administradorLogger.RegistroError(ex);
                                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                            }
                        }
                    }
                }
                else
                {
                    DisminuirCastigo(jugadorSiguiente);
                }
            }
        }


        private void ValidarJugadorIndice(string numeroSala, int totalJugadores)
        {
            if (!indiceTurnoActual.ContainsKey(numeroSala))
            {
                indiceTurnoActual[numeroSala] = 0;
            }
            else
            {
                indiceTurnoActual[numeroSala] = (indiceTurnoActual[numeroSala] + 1) % totalJugadores;
            }
        }

        private void DisminuirCastigo(string jugador)
        {
            if (jugadoresConCastigos.TryGetValue(jugador, out int castigos) && castigos > 0)
            {
                if (castigos > 1)
                {
                    jugadoresConCastigos[jugador]--;
                }
                else
                {
                    jugadoresConCastigos.Remove(jugador);
                }
            }
        }

        public List<Carta> ObtenerCartasSobrantes()
        {
            return new List<Carta>(cartasSobrantes);
        }


        public void EliminarJugadorDeJuego(string nombreUsuario)
        {
            jugadoresConectadosListos.Remove(nombreUsuario);
        }

        public void ConectarConTablero(string nombreUsuario, string numeroSala)
        {
            jugadoresConectadosTableroCallback.Add(nombreUsuario, OperationContext.Current.GetCallbackChannel<IJuegoAdministradorCallback>());
            jugadoresConectadosTablero.Add(nombreUsuario, numeroSala);
        }

        public List<string> ObtenerJugadores(string numeroSala)
        {
            return jugadoresConectadosListos.Where(gamer => gamer.Value == numeroSala).Select(gamer => gamer.Key).ToList();
        }

        public void AgregarCastigo(string nombreJugador)
        {
            if (jugadoresConCastigos.ContainsKey(nombreJugador))
            {
                jugadoresConCastigos[nombreJugador]++;
            }
            else
            {
                jugadoresConCastigos.Add(nombreJugador, 1);
            }
        }

        public void TerminarPartidaMiniJuego(string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            string jugadorGanador = ObtenerGanador(numeroSala);
            foreach (var jugador in jugadoresConectadosTableroCallback)
            {
                if (jugadoresConectadosTableroCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        jugadoresConectadosTableroCallback[jugador.Key].EnviarGanador(jugadorGanador);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        Dictionary<string, Dictionary<string, int>> familiasConPersonajes = new Dictionary<string, Dictionary<string, int>>
        {
            {
                "Ramfez", new Dictionary<string, int>
                {
                    { "Seti", 0 },
                    { "Merit", 0 },
                    { "Neferu", 0 },
                    { "Sobek", 0 }
                }
            },
            {
                "Garlo", new Dictionary<string, int>
                {
                    { "Tucani", 0 },
                    { "Lusiel", 0 },
                    { "Angelus", 0 },
                    { "Luan", 0 }
                }
            },
            {
                "Corbat", new Dictionary<string, int>
                {
                    { "Gaia", 0 },
                    { "Arialyn", 0 },
                    { "Aris", 0 },
                    { "Abelith", 0 }
                }
            },
            {
                "Ores", new Dictionary<string, int>
                {
                    { "Didorian", 0 },
                    { "Zael", 0 },
                    { "Pablian", 0 },
                    { "Lorenzeo", 0 }
                }
            }
        };
            
        /*
        public void SumarVidaPersonajeJuegoNormal(string nombreJugador, string nombrePersonaje, int cantidadVida)
        {          
            if (jugadoresConFamilias.TryGetValue(nombreJugador, out string nombreFamilia))
            {
              
              var personajes = familiasConPersonajes[nombreFamilia];
        public void SolicitarExpulsion(string solicitante, string jugadorObjetivo, string numeroSala)
        {
            if (EsAdministrador(solicitante, numeroSala))
            {
                ExpulsarJugador(jugadorObjetivo, numeroSala);
            }
            else
            {
                IniciarVotacionExpulsion(solicitante, jugadorObjetivo, numeroSala);
            }
        }

        private void ExpulsarJugador(string jugadorObjetivo, string numeroSala)
        {
            if (jugadoresConectados.ContainsKey(jugadorObjetivo))
            {
                jugadoresConectados.Remove(jugadorObjetivo);
                jugadoresConectadosCallback.Remove(jugadorObjetivo);

                foreach (var jugador in ObtenerJugadores(numeroSala))
                {
                    if (jugadoresConectadosCallback.TryGetValue(jugador, out var callback))
                    {
                        callback.NotificarExpulsion(jugadorObjetivo);
                    }
                }
            }
        }

        private void IniciarVotacionExpulsion(string solicitante, string jugadorObjetivo, string numeroSala)
        {
            if (!votosExpulsion.ContainsKey(numeroSala))
            {
                votosExpulsion[numeroSala] = new List<string>();
            }

            foreach (var jugador in ObtenerJugadores(numeroSala))
            {
                if (jugadoresConectadosCallback.TryGetValue(jugador, out var callback))
                {
                    callback.IniciarVotacion(jugadorObjetivo);
                }
            }
        }

        public void VotarExpulsion(string votante, string jugadorObjetivo, string numeroSala)
        {
            if (votosExpulsion.ContainsKey(numeroSala) && !votosExpulsion[numeroSala].Contains(votante))
            {
                votosExpulsion[numeroSala].Add(votante);
            }

              if (personajes.ContainsKey(nombrePersonaje))
               {
                 personajes[nombrePersonaje] += cantidadVida;
               }
            }
        }*/
            if (votosExpulsion[numeroSala].Count >= ObtenerJugadores(numeroSala).Count / 2)
            {
                ExpulsarJugador(jugadorObjetivo, numeroSala);
                votosExpulsion.Remove(numeroSala);
            }
        }





        private bool EsAdministrador(string nombreUsuario, string numeroSala)
        {
            return administradoresDeSala.TryGetValue(numeroSala, out var administrador) && administrador == nombreUsuario;
        }


    }
}