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
        private ServicioCarta servicioCarta;
        private static readonly Dictionary<string, IJuegoAdministradorCallback> JugadoresConectadosCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        private static readonly Dictionary<string, string> JugadoresConectados = new Dictionary<string, string>();
        private static readonly Dictionary<string, List<PosicionesJugador>> direccionJugadorEnJuego = new Dictionary<string, List<PosicionesJugador>>();
        private static readonly Dictionary<string, string> TurnsInGameboard = new Dictionary<string, string>();
        private static readonly List<Carta> CartasSobrantes = new List<Carta>();

        public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            HostBehaviorManager.ChangeToReentrant();
            var callback = OperationContext.Current.GetCallbackChannel<IJuegoAdministradorCallback>();
            if (!JugadoresConectadosCallback.ContainsKey(numeroSala))
            {
                JugadoresConectadosCallback.Add(nombreUsuario, callback);
                JugadoresConectados.Add(nombreUsuario, numeroSala);
                List<Carta> cartasSobrantes = EmpezarJuego(numeroSala, numeroJugadores);
                CartasSobrantes.Clear();
                CartasSobrantes.AddRange(cartasSobrantes);
            }
        }

        private List<Carta> EmpezarJuego(string numeroSala, int numeroJugadores)
        {
            VerificarparticipantesConectados(numeroSala, numeroJugadores);
            
                servicioCarta = new ServicioCarta();
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
            List<string> jugadorPartida = obtenerJugadores(numeroSala);
            int cantidadJugadores = jugadorPartida.Count;

            List<PosicionesJugador> posicionesJugador = new List<PosicionesJugador>();

            for (int i = 0; i < cantidadJugadores; i++)
            {
                string vecinoIzquierdo = jugadorPartida[(i - 1 + cantidadJugadores) % cantidadJugadores];
                string vecinoDerecho = jugadorPartida[(i + 1) % cantidadJugadores];

                posicionesJugador.Add(new PosicionesJugador
                {
                    nombreUsuario = jugadorPartida[i],
                    izquierda = vecinoIzquierdo,
                    derecha = vecinoDerecho
                });
            }

            direccionJugadorEnJuego.Add(numeroSala, posicionesJugador);
        }

        public List<string> obtenerJugadores(string numeroSala)
        {
            return JugadoresConectados.Where(gamer => gamer.Value == numeroSala).Select(gamer => gamer.Key).ToList();
        }

        private void asignarPrimerTurno(string numeroSala)
        {
         
            List<PosicionesJugador> turnsList = direccionJugadorEnJuego[numeroSala];
            string nombreUsuario = turnsList[0].nombreUsuario;
            TurnsInGameboard.Add(numeroSala, nombreUsuario);

            try
            {
                JugadoresConectadosCallback[nombreUsuario].RecibirTurno(true);
            }
            catch (CommunicationException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Message));
            }
            catch (TimeoutException ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Message));
            }

        }

        public List<Carta> ObtenerCartasSobrantes()
        {
            return new List<Carta>(CartasSobrantes);
        }

        public void RecibirTurno(bool validarTurno)
        {
            throw new NotImplementedException();
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