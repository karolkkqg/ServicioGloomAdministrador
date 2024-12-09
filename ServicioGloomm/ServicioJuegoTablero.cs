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
        public static readonly Dictionary<string, List<string>> jugadoresConectadosListos = new Dictionary<string, List<string>>();
        public static readonly List<Carta> cartasSobrantes = new List<Carta>();
        public static readonly Dictionary<string, IJuegoAdministradorCallback> jugadoresConectadosTableroCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        public static readonly Dictionary<string, string> jugadoresConectadosTablero = new Dictionary<string, string>();
        public static readonly Dictionary<string, int> jugadoresConCastigos = new Dictionary<string, int>();
        public static readonly Dictionary<string, List<string>> jugadoresVivos = new Dictionary<string, List<string>>();
        private BibliotecaClases.Sala nuevaParticipante;
        private Dictionary<string, int> turnosJugados = new Dictionary<string, int>();
        public readonly Dictionary<string, List<string>> votosExpulsion = new Dictionary<string, List<string>>();

        public List<string> ObtenerJugadoresConectados(string numeroSala)
        {
            return salaJugadoresPorSala[numeroSala].Keys.ToList();
        }


        public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            if (!jugadoresConectadosListos[numeroSala].Contains(nombreUsuario))
            {
                jugadoresConectadosListos[numeroSala].Add(nombreUsuario);
            }
            nuevaParticipante = new BibliotecaClases.Sala
            {
                idSala = numeroSala,
                jugador = nombreUsuario
            };
            AccesoSala.AgregarParticipante(nuevaParticipante);
        }

        public void IniciarPartidaPorAdministrador(string nombreAdministrador, string numeroSala, int numeroJugadores)
        {
            AsegurarEstructurasPorSala(numeroSala);
            if (!partidaYaIniciada.ContainsKey(numeroSala) || !partidaYaIniciada[numeroSala])
            {
                try
                {
                    partidaYaIniciada[numeroSala] = true;
                    VerificarParticipantesConectados(numeroSala, numeroJugadores, nombreAdministrador);
                    List<Carta> cartasSobrantes = EmpezarJuego(numeroSala);

                    CartasSobrantes.Clear();
                    CartasSobrantes.AddRange(cartasSobrantes);
                }
                catch (FaultException<ManejadorExcepciones> ex)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
                }


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
            int conteoJugadores = salaJugadoresPorSala[numeroSala].Count;

            if (conteoJugadores != numeroJugadores)
            {
                if (jugadoresConectadosListos.ContainsKey(numeroSala))
                {
                    jugadoresConectadosListos[numeroSala].Remove(nombreAdministrador);
                }

                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("17", "La sala aún no está llena, espere a los demás jugadores"));
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

                if (!jugadoresConCastigos.ContainsKey(jugadorSiguiente) && jugadoresVivos[numeroSala].Contains(jugadorSiguiente))
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
            return new List<Carta>(cartasSobrantesGlobal);
        }


        public void EliminarJugadorDeJuego(string nombreUsuario)
        {
            jugadoresConectadosListos.Remove(nombreUsuario);
        }

        public void ConectarConTablero(string nombreUsuario, string numeroSala)
        {
            if (nombreUsuario != numeroSala)
            {
                jugadoresConectadosTableroCallback.Add(nombreUsuario, OperationContext.Current.GetCallbackChannel<IJuegoAdministradorCallback>());
                jugadoresConectadosTablero.Add(nombreUsuario, numeroSala);
                jugadoresVivos[numeroSala].Add(nombreUsuario);
            }

        }

        public List<string> ObtenerJugadores(string numeroSala)
        {
            return jugadoresConectadosListos[numeroSala];
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

        public void MatarJugador(string numeroSala, string jugadorAMatar, string jugadorPropietario)
        {

            ValidarAutoIntentoDeMuerte(jugadorAMatar, jugadorPropietario);
            ValidarAutoestimaJugador(numeroSala, jugadorAMatar);

            if (jugadoresVivos[numeroSala].Count == 2)
            {
                TerminarPartidaMiniJuego(numeroSala, jugadorPropietario);
                return;
            }
            EliminarJugadorDeLista(numeroSala, jugadorAMatar);

            NotificarJugadorMuerto(jugadorAMatar);
        }


        private void ValidarAutoIntentoDeMuerte(string jugadorAMatar, string jugadorPropietario)
        {
            if (jugadorAMatar.Equals(jugadorPropietario))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("45", "No se puede automatar así mismo"));
            }
        }
        private void ValidarAutoestimaJugador(string numeroSala, string jugadorAMatar)
        {
            if (!personajesPorSala.ContainsKey(numeroSala) ||
                !personajesPorSala[numeroSala].ContainsKey(jugadorAMatar) ||
                personajesPorSala[numeroSala][jugadorAMatar].vida > -400)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("44", "Ya ha iniciado sesión anteriormente, cerre la sesión de ese dispositivo si desea continuar aquí "));
            }
        }

        private void EliminarJugadorDeLista(string numeroSala, string jugadorAMatar)
        {
            if (jugadoresVivos[numeroSala].Contains(jugadorAMatar))
            {
                jugadoresVivos[numeroSala].Remove(jugadorAMatar);
            }
        }

        private void NotificarJugadorMuerto(string jugadorAMatar)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            foreach (var jugador in jugadoresConectadosTableroCallback)
            {
                if (jugadoresConectadosTableroCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        jugadoresConectadosTableroCallback[jugador.Key].ActualizarJugadorMuerto(jugadorAMatar);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
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

        public void TerminarPartidaMiniJuego(string numeroSala, string jugadorGanador)
        {

            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            AsignarGanadorASala(numeroSala, jugadorGanador);
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
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }
            jugadoresVivos.Remove(numeroSala);
        }

        private void AsignarGanadorASala(string numeroSala, string ganador)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                AccesoSala.ActualizarEstadoPartida(numeroSala, ganador);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Message));
            }
        }
        private void AsegurarEstructurasPorSala(string numeroSala)
        {
            if (!turnosJugados.ContainsKey(numeroSala))
            {
                turnosJugados[numeroSala] = 0;
            }

            if (!turnosPorSala.ContainsKey(numeroSala))
            {
                turnosPorSala[numeroSala] = new List<string>();
            }

            if (!indiceTurnoActual.ContainsKey(numeroSala))
            {
                indiceTurnoActual[numeroSala] = 0;
            }

            if (!partidaYaIniciada.ContainsKey(numeroSala))
            {
                partidaYaIniciada[numeroSala] = false;
            }

            if (!jugadoresConectadosListos.ContainsKey(numeroSala))
            {
                jugadoresConectadosListos[numeroSala] = new List<string>();
            }

            if (!jugadoresConectadosTablero.ContainsKey(numeroSala))
            {
                jugadoresConectadosTablero[numeroSala] = string.Empty;
            }

            if (!jugadoresConCastigos.ContainsKey(numeroSala))
            {
                jugadoresConCastigos[numeroSala] = 0;
            }
            if (!jugadoresVivos.ContainsKey(numeroSala))
            {
                jugadoresVivos[numeroSala] = new List<string>();
            }
        }

        public void BorrarEstructurasPorSala(string numeroSala)
        {
            if (turnosPorSala.ContainsKey(numeroSala))
            {
                turnosPorSala.Remove(numeroSala);
            }

            if (indiceTurnoActual.ContainsKey(numeroSala))
            {
                indiceTurnoActual.Remove(numeroSala);
            }

            if (partidaYaIniciada.ContainsKey(numeroSala))
            {
                partidaYaIniciada.Remove(numeroSala);
            }

            if (jugadoresConectadosListos.ContainsKey(numeroSala))
            {
                jugadoresConectadosListos.Remove(numeroSala);
            }

            if (jugadoresConectadosTablero.ContainsKey(numeroSala))
            {
                jugadoresConectadosTablero.Remove(numeroSala);
            }

            if (jugadoresConCastigos.ContainsKey(numeroSala))
            {
                jugadoresConCastigos.Remove(numeroSala);
            }
            if (jugadoresVivos.ContainsKey(numeroSala))
            {
                jugadoresConCastigos.Remove(numeroSala);
            }

        }

        public void TerminarPartidaNormal(string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            if (!turnosPorSala.TryGetValue(numeroSala, out var jugadores) || jugadores.Count == 0)
            {
                NotificarFinPartidaSinGanador(numeroSala);
                return;
            }

            string jugadorGanador = "Sin ganador";
            foreach (var jugador in jugadores)
            {
                if (!TienePersonajesRestantes(jugador))
                {
                    jugadorGanador = jugador;
                    break;
                }
            }

            if (jugadorGanador != null)
            {
                foreach (var jugador in jugadoresConectadosTableroCallback.Keys)
                {
                    if (jugadoresConectadosTablero[jugador] == numeroSala)
                    {
                        try
                        {
                            string mensajeGanador = jugador.Equals(jugadorGanador) ? jugadorGanador : "Sin ganador";
                            jugadoresConectadosTableroCallback[jugador].EnviarGanador(mensajeGanador);
                        }
                        catch (CommunicationException ex)
                        {
                            administradorLogger.RegistroError(ex);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                        }
                        catch (TimeoutException ex)
                        {
                            administradorLogger.RegistroError(ex);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                        }
                    }
                }

                LimpiarEstructurasDeSala(numeroSala);
                return;
            }

            var jugadoresParaExpulsar = new List<string>();
            foreach (var jugador in jugadores)
            {
                if (!TienePersonajesRestantes(jugador))
                {
                    jugadoresParaExpulsar.Add(jugador);
                }
            }

            foreach (var jugador in jugadoresParaExpulsar)
            {
                jugadores.Remove(jugador);
            }


            turnosPorSala[numeroSala] = jugadores;

        }


        public void NotificarGanador(string jugadorGanador, string numeroSala)
        {

            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            if (turnosPorSala.TryGetValue(numeroSala, out var jugadores))
            {
                foreach (var jugador in jugadores)
                {
                    if (jugadoresConectadosTableroCallback.TryGetValue(jugador, out var callback))
                    {
                        try
                        {
                            callback.EnviarGanador(jugadorGanador);
                        }
                        catch (CommunicationException ex)
                        {
                            administradorLogger.RegistroError(ex);
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



        public void IncrementarTurnos(string numeroSala)
        {

            if (!turnosJugados.ContainsKey(numeroSala))
            {
                turnosJugados[numeroSala] = 0;
            }
            turnosJugados[numeroSala]++;
        }




        private void NotificarFinPartidaSinGanador(string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            foreach (var jugador in jugadoresConectadosTableroCallback.Keys)
            {
                if (jugadoresConectadosTablero[jugador] == numeroSala)
                {
                    try
                    {
                        jugadoresConectadosTableroCallback[jugador].EnviarGanador("Sin ganador");
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }

            LimpiarEstructurasDeSala(numeroSala);
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


        public void SolicitarExpulsion(string solicitante, string jugadorObjetivo, string numeroSala)
        {
            if (!jugadoresConectadosTableroCallback.ContainsKey(jugadorObjetivo))
            {
                Console.WriteLine($"El jugador {jugadorObjetivo} no está conectado o no existe.");
                return;
            }

            if (EsAdministrador(solicitante, numeroSala))
            {
                ExpulsarJugador(jugadorObjetivo, numeroSala);
            }
            else
            {
                IniciarVotacionExpulsion(solicitante, jugadorObjetivo, numeroSala);
            }
        }


        public void ExpulsarJugador(string jugadorObjetivo, string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (jugadoresConectadosTableroCallback.TryGetValue(jugadorObjetivo, out var callback))
            {
                try
                {
                    string mensaje = $"Has sido expulsado de la sala {numeroSala}.";
                    callback.RecibirExpulsion(mensaje);
                }
                catch (CommunicationException ex)
                {
                    administradorLogger.RegistroError(ex);
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                }
                catch (TimeoutException ex)
                {
                    administradorLogger.RegistroError(ex);
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                }
            }

            jugadoresConectadosTableroCallback.Remove(jugadorObjetivo);
            jugadoresConectadosTablero.Remove(jugadorObjetivo);

            if (turnosPorSala.TryGetValue(numeroSala, out var jugadores))
            {
                jugadores.Remove(jugadorObjetivo);

                if (jugadores.Count == 1)
                {
                    string jugadorRestante = jugadores.First();
                    TerminarPartidaConGanador(jugadorRestante, numeroSala);
                    return;
                }

                if (jugadores.Count == 0)
                {
                    NotificarFinPartidaSinGanador(numeroSala);
                    return;
                }

                CambiarTurno(numeroSala);
            }
            NotificarExpulsionATodos(jugadorObjetivo, numeroSala);
        }

        private void NotificarExpulsionATodos(string jugadorExpulsado, string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            foreach (var jugador in jugadoresConectadosTableroCallback.Keys)
            {
                if (jugadoresConectadosTablero[jugador] == numeroSala)
                {
                    try
                    {

                        jugadoresConectadosTableroCallback[jugador].ActualizarInterfazExpulsion(jugadorExpulsado);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
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



        private void TerminarPartidaConGanador(string ganador, string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            foreach (var jugador in jugadoresConectadosTableroCallback.Keys)
            {
                if (jugadoresConectadosTablero[jugador] == numeroSala)
                {
                    try
                    {
                        jugadoresConectadosTableroCallback[jugador].EnviarGanador(ganador);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }
            LimpiarEstructurasDeSala(numeroSala);
        }

        private void LimpiarEstructurasDeSala(string numeroSala)
        {
            if (turnosJugados.ContainsKey(numeroSala))
            {
                turnosJugados.Remove(numeroSala);
            }

            if (turnosPorSala.ContainsKey(numeroSala))
            {
                turnosPorSala.Remove(numeroSala);
            }

            if (indiceTurnoActual.ContainsKey(numeroSala))
            {
                indiceTurnoActual.Remove(numeroSala);
            }

            if (partidaYaIniciada.ContainsKey(numeroSala))
            {
                partidaYaIniciada.Remove(numeroSala);
            }

            var jugadoresEnSala = jugadoresConectadosTablero
                .Where(j => j.Value == numeroSala)
                .Select(j => j.Key)
                .ToList();

            foreach (var jugador in jugadoresEnSala)
            {
                jugadoresConectadosTablero.Remove(jugador);
                jugadoresConectadosTableroCallback.Remove(jugador);
            }

            if (votosExpulsion.ContainsKey(numeroSala))
            {
                votosExpulsion.Remove(numeroSala);
            }

            if (jugadoresConectadosListos.ContainsKey(numeroSala))
            {
                jugadoresConectadosListos.Remove(numeroSala);
            }

            var jugadoresCastigados = jugadoresConCastigos
                .Where(j => turnosPorSala.ContainsKey(numeroSala) && turnosPorSala[numeroSala].Contains(j.Key))
                .Select(j => j.Key)
                .ToList();

            foreach (var jugador in jugadoresCastigados)
            {
                jugadoresConCastigos.Remove(jugador);
            }

            if (cartasSobrantes != null && cartasSobrantes.Count > 0)
            {
                cartasSobrantes.Clear();
            }
        }


        private void IniciarVotacionExpulsion(string solicitante, string jugadorObjetivo, string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (!turnosPorSala.TryGetValue(numeroSala, out var jugadores) || jugadores.Count <= 2)
            {
                ExpulsarJugador(jugadorObjetivo, numeroSala);
                return;
            }

            if (!votosExpulsion.ContainsKey(numeroSala))
            {
                votosExpulsion[numeroSala] = new List<string>();
            }

            votosExpulsion[numeroSala].Clear();
            votosExpulsion[numeroSala].Add(solicitante);

            foreach (var jugador in jugadoresConectadosTableroCallback.Keys)
            {
                if (jugador != jugadorObjetivo && jugador != solicitante && jugadoresConectadosTablero[jugador] == numeroSala)
                {
                    if (jugadoresConectadosTableroCallback.TryGetValue(jugador, out var callback))
                    {
                        try
                        {
                            callback.NotificarVotacionExpulsion(jugadorObjetivo);
                        }
                        catch (CommunicationException ex)
                        {
                            administradorLogger.RegistroError(ex);
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


        public void RegistrarVotoExpulsion(string numeroSala, string jugadorQueVota, bool votoAFavor)
        {
            if (!votosExpulsion.ContainsKey(numeroSala))
            {
                return;
            }

            if (!votoAFavor || votosExpulsion[numeroSala].Contains(jugadorQueVota))
            {
                return;
            }

            votosExpulsion[numeroSala].Add(jugadorQueVota);

            var jugadoresSala = ObtenerJugadores(numeroSala);
            int votosRequeridos = (int)Math.Ceiling(jugadoresSala.Count / 2.0);

            if (votosExpulsion[numeroSala].Count >= votosRequeridos)
            {
                string jugadorObjetivo = votosExpulsion[numeroSala][0];
                ExpulsarJugador(jugadorObjetivo, numeroSala);
                votosExpulsion[numeroSala].Clear();

            }
        }

        private bool EsAdministrador(string nombreUsuario, string numeroSala)
        {
            return administradoresDeSala.TryGetValue(numeroSala, out var administrador) && administrador == nombreUsuario;
        }

        public bool EsSalaActiva(string numeroSala)
        {
            return turnosPorSala.ContainsKey(numeroSala);
        }


    }
}