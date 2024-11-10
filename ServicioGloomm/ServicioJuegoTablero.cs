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
        private static readonly Dictionary<string, IJuegoAdministradorCallback> JugadoresConectadosCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        private static readonly Dictionary<string, string> JugadoresConectados = new Dictionary<string, string>();
        private static readonly Dictionary<string, List<PosicionesJugador>> direccionJugadorEnJuego = new Dictionary<string, List<PosicionesJugador>>();
        private static readonly Dictionary<string, string> TurnsInGameboard = new Dictionary<string, string>();
        private static readonly List<Carta> CartasSobrantes = new List<Carta>();
        private static readonly Dictionary<string, int> indiceTurnoActual = new Dictionary<string, int>();
        private static readonly Dictionary<string, bool> partidaYaIniciada = new Dictionary<string, bool>();

        public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            AdministradorDeComportamiento.cambiarModoComportamientoReentrante();
            var callback = OperationContext.Current.GetCallbackChannel<IJuegoAdministradorCallback>();
            if (!JugadoresConectadosCallback.ContainsKey(numeroSala))
            {
                JugadoresConectadosCallback.Add(nombreUsuario, callback);
                JugadoresConectados.Add(nombreUsuario, numeroSala);

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
            asignarTurnos(numeroSala);
            var cartasSobrantes = servicioCarta.BarajearMazo(numeroSala);
            asignarPrimerTurno(numeroSala);
            return cartasSobrantes;
            
        }

        private void VerificarparticipantesConectados(string numeroSala, int numeroJugadores)
        {
            List<string> jugadoresEnSala= obtenerJugadores(numeroSala);
            int conteoJugadores = jugadoresEnSala.Count(jugador => JugadoresConectados.ContainsKey(jugador));

            if (conteoJugadores != numeroJugadores)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("17"));
            }
        }

        private void asignarTurnos(string numeroSala)
        {

            List<string> jugadores = obtenerJugadores(numeroSala).OrderBy(j => Guid.NewGuid()).ToList();
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

        public List<string> obtenerJugadores(string numeroSala)
        {
            return JugadoresConectados.Where(gamer => gamer.Value == numeroSala).Select(gamer => gamer.Key).ToList();
        }

        private void asignarPrimerTurno(string numeroSala)
        {

                List<PosicionesJugador> posiciones = direccionJugadorEnJuego[numeroSala];
                int totalJugadores = posiciones.Count;

            ValidarJugadorIndice(numeroSala, totalJugadores);

                int indiceActual = indiceTurnoActual[numeroSala];
                string jugadorActual = posiciones[indiceActual].nombreUsuario;
                TurnsInGameboard[numeroSala] = jugadorActual;

                if (JugadoresConectadosCallback.TryGetValue(jugadorActual, out var callback) && callback != null)
                {
                    try
                    {
                        callback.EnviarTurno(jugadorActual);
                        Console.WriteLine($"Turno asignado a {jugadorActual} con éxito.");
                    }
                    catch (CommunicationException ex)
                    {
                        Console.WriteLine("Excepción de comunicación: " + ex.Message);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Message));
                    }
                    catch (TimeoutException ex)
                    {
                        Console.WriteLine("Excepción de tiempo de espera: " + ex.Message);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Message));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Excepción inesperada en el callback: " + ex.Message);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("Error inesperado al enviar el turno: " + ex.Message));
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
            return new List<Carta>(CartasSobrantes);
        }

        /*
private void RemoveFromGameboard(string gamertag)
{
   decks.Remove(gamertag);
   GamersInGameBoard.Remove(gamertag);
   GamersInGameBoardCallback.Remove(gamertag);
}
*/
    }
}