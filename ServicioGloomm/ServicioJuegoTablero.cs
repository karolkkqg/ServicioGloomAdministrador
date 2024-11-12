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
<<<<<<< HEAD
        private ServicioJuego servicioCarta;
        public static readonly Dictionary<string, IJuegoAdministradorCallback> JugadoresConectadosCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        public static readonly Dictionary<string, string> JugadoresConectados = new Dictionary<string, string>();
        public static readonly Dictionary<string, List<PosicionesJugador>> direccionJugadorEnJuego = new Dictionary<string, List<PosicionesJugador>>();
        public static readonly Dictionary<string, string> TurnsInGameboard = new Dictionary<string, string>();
        public static readonly List<Carta> CartasSobrantes = new List<Carta>();
        public static readonly Dictionary<string, int> indiceTurnoActual = new Dictionary<string, int>();
        public static readonly Dictionary<string, bool> partidaYaIniciada = new Dictionary<string, bool>();
=======
        private ServicioCarta servicioCarta;
        private static readonly Dictionary<string, IJuegoAdministradorCallback> jugadoresConectadosCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        private static readonly Dictionary<string, string> jugadoresConectados = new Dictionary<string, string>();
        private static readonly Dictionary<string, List<PosicionesJugador>> direccionJugadorEnJuego = new Dictionary<string, List<PosicionesJugador>>();
        private static readonly Dictionary<string, string> TurnsInGameboard = new Dictionary<string, string>();
        private static readonly List<Carta> cartasSobrantes = new List<Carta>();
>>>>>>> 351c70a0508036a7c93ce7726f275ea9c1dd7cda

        public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            AdministradorDeComportamiento.cambiarModoComportamientoReentrante();
            var callback = OperationContext.Current.GetCallbackChannel<IJuegoAdministradorCallback>();
            if (!jugadoresConectadosCallback.ContainsKey(numeroSala))
            {
<<<<<<< HEAD
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
=======
                jugadoresConectadosCallback.Add(nombreUsuario, callback);
                jugadoresConectados.Add(nombreUsuario, numeroSala);
                List<Carta> cartasSobrantes = EmpezarJuego(numeroSala, numeroJugadores);
                ServicioJuego.cartasSobrantes.Clear();
                ServicioJuego.cartasSobrantes.AddRange(cartasSobrantes);
>>>>>>> 351c70a0508036a7c93ce7726f275ea9c1dd7cda
            }
        }

        private List<Carta> EmpezarJuego(string numeroSala)
        {
<<<<<<< HEAD

            servicioCarta = new ServicioJuego();
            asignarTurnos(numeroSala);
            var cartasSobrantes = servicioCarta.BarajearMazo(numeroSala);
            asignarPrimerTurno(numeroSala);
            return cartasSobrantes;
=======
            VerificarparticipantesConectados(numeroSala, numeroJugadores);
            
                servicioCarta = new ServicioCarta();
                AsignarTurnos(numeroSala);
                var cartasSobrantes = servicioCarta.BarajearMazo(numeroSala);
                AsignarPrimerTurno(numeroSala);
                return cartasSobrantes;
>>>>>>> 351c70a0508036a7c93ce7726f275ea9c1dd7cda
            
        }

        private void VerificarparticipantesConectados(string numeroSala, int numeroJugadores)
        {
            List<string> jugadoresEnSala= ObtenerJugadores(numeroSala);
            int conteoJugadores = jugadoresEnSala.Count(jugador => jugadoresConectados.ContainsKey(jugador));

            if (conteoJugadores != numeroJugadores)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("17"));
            }
        }

        private void AsignarTurnos(string numeroSala)
        {
<<<<<<< HEAD
=======
            List<string> jugadorPartida = ObtenerJugadores(numeroSala);
            int cantidadJugadores = jugadorPartida.Count;
>>>>>>> 351c70a0508036a7c93ce7726f275ea9c1dd7cda

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

        public List<string> ObtenerJugadores(string numeroSala)
        {
            return jugadoresConectados.Where(gamer => gamer.Value == numeroSala).Select(gamer => gamer.Key).ToList();
        }

        private void AsignarPrimerTurno(string numeroSala)
        {

<<<<<<< HEAD
                List<PosicionesJugador> posiciones = direccionJugadorEnJuego[numeroSala];
                int totalJugadores = posiciones.Count;
=======
            try
            {
                jugadoresConectadosCallback[nombreUsuario].RecibirTurno(true);
            }
            catch (CommunicationException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Message));
            }
            catch (TimeoutException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Message));
            }
>>>>>>> 351c70a0508036a7c93ce7726f275ea9c1dd7cda

            ValidarJugadorIndice(numeroSala, totalJugadores);

                int indiceActual = indiceTurnoActual[numeroSala];
                string jugadorActual = posiciones[indiceActual].nombreUsuario;
                TurnsInGameboard[numeroSala] = jugadorActual;

                if (JugadoresConectadosCallback.TryGetValue(jugadorActual, out var callback) && callback != null)
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
            JugadoresConectadosCallback.Remove(nombreUsuario);
            JugadoresConectados.Remove(nombreUsuario);
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