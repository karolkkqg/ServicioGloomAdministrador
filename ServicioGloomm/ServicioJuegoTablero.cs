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
        private static readonly Dictionary<string, IJuegoAdministradorCallback> jugadoresConectadosCallback = new Dictionary<string, IJuegoAdministradorCallback>();
        private static readonly Dictionary<string, string> jugadoresConectados = new Dictionary<string, string>();
        private static readonly Dictionary<string, List<PosicionesJugador>> direccionJugadorEnJuego = new Dictionary<string, List<PosicionesJugador>>();
        private static readonly Dictionary<string, string> TurnsInGameboard = new Dictionary<string, string>();
        private static readonly List<Carta> cartasSobrantes = new List<Carta>();

        public void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            HostBehaviorManager.ChangeToReentrant();
            var callback = OperationContext.Current.GetCallbackChannel<IJuegoAdministradorCallback>();
            if (!jugadoresConectadosCallback.ContainsKey(numeroSala))
            {
                jugadoresConectadosCallback.Add(nombreUsuario, callback);
                jugadoresConectados.Add(nombreUsuario, numeroSala);
                List<Carta> cartasSobrantes = EmpezarJuego(numeroSala, numeroJugadores);
                ServicioJuego.cartasSobrantes.Clear();
                ServicioJuego.cartasSobrantes.AddRange(cartasSobrantes);
            }
        }

        private List<Carta> EmpezarJuego(string numeroSala, int numeroJugadores)
        {
            VerificarparticipantesConectados(numeroSala, numeroJugadores);
            
                servicioCarta = new ServicioCarta();
                AsignarTurnos(numeroSala);
                var cartasSobrantes = servicioCarta.BarajearMazo(numeroSala);
                AsignarPrimerTurno(numeroSala);
                return cartasSobrantes;
            
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
            List<string> jugadorPartida = ObtenerJugadores(numeroSala);
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

        public List<string> ObtenerJugadores(string numeroSala)
        {
            return jugadoresConectados.Where(gamer => gamer.Value == numeroSala).Select(gamer => gamer.Key).ToList();
        }

        private void AsignarPrimerTurno(string numeroSala)
        {
         
            List<PosicionesJugador> turnsList = direccionJugadorEnJuego[numeroSala];
            string nombreUsuario = turnsList[0].nombreUsuario;
            TurnsInGameboard.Add(numeroSala, nombreUsuario);

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

        }

        public List<Carta> ObtenerCartasSobrantes()
        {
            return new List<Carta>(cartasSobrantes);
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