using AccesoDatos;
using BibliotecaClases;
using BlbibliotecaClases;
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
    public class PosicionesJugador
    {
        public string nombreUsuario { get; set; }
        public string izquierda { get; set; }
        public string derecha { get; set; }
    }

    public partial class ServicioJuego : IServicioJuegoTablero
    {

        private ServicioJuego servicioCarta;
        public static readonly Dictionary<string, List<PosicionesJugador>> direccionJugadorEnJuego = new Dictionary<string, List<PosicionesJugador>>();
        public static readonly Dictionary<string, string> TurnsInGameboard = new Dictionary<string, string>();
        public static readonly List<Carta> CartasSobrantes = new List<Carta>();
        public static readonly Dictionary<string, int> indiceTurnoActual = new Dictionary<string, int>();
        public static readonly Dictionary<string, bool> partidaYaIniciada = new Dictionary<string, bool>();
        private static readonly Dictionary<string, IJuegoAdministradorCallback> jugadoresConectadosCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        private static readonly Dictionary<string, string> jugadoresConectados = new Dictionary<string, string>();
        private static readonly List<Carta> cartasSobrantes = new List<Carta>();

        private static readonly Dictionary<string, string> administradoresDeSala = new Dictionary<string, string>();
        private static readonly Dictionary<string, List<string>> votosExpulsion = new Dictionary<string, List<string>>();



        public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            var callback = OperationContext.Current.GetCallbackChannel<IJuegoAdministradorCallback>();
            if (!jugadoresConectadosCallback.ContainsKey(numeroSala))
            {

                jugadoresConectadosCallback.Add(nombreUsuario, callback);
                jugadoresConectados.Add(nombreUsuario, numeroSala);

            }
        }

        public void IniciarPartidaPorAdministrador(string nombreAdministrador, string numeroSala, int numeroJugadores)
        {
            if (!partidaYaIniciada.ContainsKey(numeroSala) || !partidaYaIniciada[numeroSala])
            {
                partidaYaIniciada[numeroSala] = true;
                VerificarparticipantesConectados(numeroSala, numeroJugadores);
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
            AsignarPrimerTurno(numeroSala);
            return cartasSobrantes;
        }

        private void VerificarparticipantesConectados(string numeroSala, int numeroJugadores)
        {
            List<string> jugadoresEnSala = ObtenerJugadores(numeroSala);
            int conteoJugadores = jugadoresEnSala.Count(jugador => jugadoresConectados.ContainsKey(jugador));

            if (conteoJugadores != numeroJugadores)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("17"));
            }
        }

        private void AsignarTurnos(string numeroSala)
        {

            List<string> jugadorPartida = ObtenerJugadores(numeroSala);
            int cantidadJugadores = jugadorPartida.Count;


            List<string> jugadores = ObtenerJugadores(numeroSala).OrderBy(j => Guid.NewGuid()).ToList();
            int totalJugadores = jugadores.Count;

            List<PosicionesJugador> posiciones = new List<PosicionesJugador>();
            for (int i = 0; i < totalJugadores; i++)
            {
                string vecinoIzquierdo = jugadores[(i - 1 + totalJugadores) % totalJugadores];
                string vecinoDerecho = jugadores[(i + 1) % totalJugadores];

                posiciones.Add(new PosicionesJugador
                {
                    nombreUsuario = jugadores[i],
                    izquierda = vecinoIzquierdo,
                    derecha = vecinoDerecho
                });
            }
            direccionJugadorEnJuego[numeroSala] = posiciones;
        }

        public List<string> ObtenerJugadores(string numeroSala)
        {
            return jugadoresConectados.Where(gamer => gamer.Value == numeroSala).Select(gamer => gamer.Key).ToList();
        }

        private void AsignarPrimerTurno(string numeroSala)
        {
            List<PosicionesJugador> posiciones = direccionJugadorEnJuego[numeroSala];
            int totalJugadores = posiciones.Count;
            ValidarJugadorIndice(numeroSala, totalJugadores);
            int indiceActual = indiceTurnoActual[numeroSala];
            string jugadorActual = posiciones[indiceActual].nombreUsuario;
            TurnsInGameboard[numeroSala] = jugadorActual;

            if (jugadoresConectadosCallback.TryGetValue(jugadorActual, out var callback) && callback != null)
            {
                try
                {
                    callback.EnviarTurno(jugadorActual);
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

        public List<Carta> ObtenerCartasSobrantes()
        {
            return new List<Carta>(cartasSobrantes);
        }


        public void EliminarJugadorDeJuego(string nombreUsuario)
        {
            jugadoresConectadosCallback.Remove(nombreUsuario);
            jugadoresConectados.Remove(nombreUsuario);
        }

        public void CambiarTurno(string numeroSala)
        {
            List<PosicionesJugador> posiciones = direccionJugadorEnJuego[numeroSala];
            int totalJugadores = posiciones.Count;

            int indiceActual = indiceTurnoActual[numeroSala];
            int siguienteIndice = (indiceActual + 1) % totalJugadores;
            indiceTurnoActual[numeroSala] = siguienteIndice;

            string jugadorSiguiente = posiciones[siguienteIndice].nombreUsuario;

            TurnsInGameboard[numeroSala] = jugadorSiguiente;

            if (jugadoresConectadosCallback.TryGetValue(jugadorSiguiente, out var callback) && callback != null)
            {
                try
                {
                    callback.EnviarTurno(jugadorSiguiente);
                }
                catch (CommunicationException)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                }
                catch (TimeoutException)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                }
            }
        }

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