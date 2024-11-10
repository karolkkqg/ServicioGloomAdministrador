using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IServicioCarta
    {
        private ServicioJuego servicioJuego;
        private static readonly Dictionary<string, List<Carta>> barajaJugadores = new Dictionary<string, List<Carta>>();
        /*
        private List<Carta> CreateCharacterCards()
        {
            Carta card1 = new Carta { ID = "DocOchCard.png", Type = "Character" };
            Carta card2 = new Carta { ID = "ElectroJuanCard.png", Type = "Character" };
            Carta card3 = new Carta { ID = "MysteRevoCard.png", Type = "Character" };
            Carta card4 = new Carta { ID = "RhinosidroCard.png", Type = "Character" };
            Carta card5 = new Carta { ID = "VernomCard.png", Type = "Character" };
            Carta card6 = new Carta { ID = "XandManCard.png", Type = "Character" };
        }
        */

        public List<Carta> BarajearMazo(string numeroSala)
        {
            List<Carta> primerMazo = CrearCartasDeMuerte();
            List<Carta> segundoMazo = CrearCartasModificador();
            servicioJuego = new ServicioJuego();
            var cartasSobrantes = RepartirCartas(servicioJuego.obtenerJugadores(numeroSala), CombinarCartas((List<Carta>)primerMazo.Concat(segundoMazo).ToList()));
            return cartasSobrantes;
        }

        private List<Carta> CrearCartasDeMuerte()
        {
            Carta cartaMuerte1 = new Carta { identificador = "MuerteInoportuna.png", valor = 200 };
            Carta cartaMuerte2 = new Carta { identificador = "BebeCenteno.png", valor = 200 };
            Carta cartaMuerte3 = new Carta { identificador = "HorneadoEnTarta.png", valor = 200 };
            Carta cartaMuerte4 = new Carta { identificador = "AtragantarHueso.png", valor = 200 };
            Carta cartaMuerte5 = new Carta { identificador = "MuerteViejo.png", valor = 200 };
            Carta cartaMuerte6 = new Carta { identificador = "MuerteSarampio.png", valor = 200 };
            Carta cartaMuerte7 = new Carta { identificador = "DevoradoPorComdrejas.png", valor = 200 };
            Carta cartaMuerte8 = new Carta { identificador = "ConsumidoPorFuego.png", valor = 200 };
            Carta cartaMuerte9 = new Carta { identificador = "Desmembrado.png", valor = 200 };
            Carta cartaMuerte10 = new Carta { identificador = "ComidoPorOsos.png", valor = 200 };
            Carta cartaMuerte11 = new Carta { identificador = "MuerteSinPreocupacion.png", valor = 200 };
            Carta cartaMuerte12 = new Carta { identificador = "EmpujadoPorLasEscaleras.png", valor = 200 };
            Carta cartaMuerte13 = new Carta { identificador = "AsesionadoPorHeredero.png", valor = 200 };
            Carta cartaMuerte14 = new Carta { identificador = "AhogadoEnPantano.png", valor = 200 };
            Carta cartaMuerte15 = new Carta { identificador = "QurmadoPorTurbia.png", valor = 200 };
            Carta cartaMuerte16 = new Carta { identificador = "NoRegreso.png", valor = 200 };
            Carta cartaMuerte17 = new Carta { identificador = "SeveramenteQuemado.png", valor = 200 };
            Carta cartaMuerte18 = new Carta { identificador = "MuertePorDesesperacion.png", valor = 200 };
            Carta cartaMuerte19 = new Carta { identificador = "SinAire.png", valor = 200 };
            Carta cartaMuerte20 = new Carta { identificador = "DesaparecioEnNiebla.png", valor = 200 };
            Carta cartaMuerte21 = new Carta { identificador = "CayoDesdeAlto.png", valor = 200 };

            List<Carta> PrimerMazo = new List<Carta>();
            PrimerMazo.Add(cartaMuerte1);
            PrimerMazo.Add(cartaMuerte2);
            PrimerMazo.Add(cartaMuerte3);
            PrimerMazo.Add(cartaMuerte4);
            PrimerMazo.Add(cartaMuerte5);
            PrimerMazo.Add(cartaMuerte6);
            PrimerMazo.Add(cartaMuerte7);
            PrimerMazo.Add(cartaMuerte8);
            PrimerMazo.Add(cartaMuerte9);
            PrimerMazo.Add(cartaMuerte10);
            PrimerMazo.Add(cartaMuerte11);
            PrimerMazo.Add(cartaMuerte12);
            PrimerMazo.Add(cartaMuerte13);
            PrimerMazo.Add(cartaMuerte14);
            PrimerMazo.Add(cartaMuerte15);
            PrimerMazo.Add(cartaMuerte16);
            PrimerMazo.Add(cartaMuerte17);
            PrimerMazo.Add(cartaMuerte18);
            PrimerMazo.Add(cartaMuerte19);
            PrimerMazo.Add(cartaMuerte20);
            PrimerMazo.Add(cartaMuerte21);

            return PrimerMazo;

        }

        private List<Carta> RepartirCartas(List<string> jugadores, List<Carta> cards)
        {
            int cartasPorJugador = 5;
            Random random = new Random();

            List<Carta> cartasCombinadas = CombinarCartas(cards);

            foreach (string jugador in jugadores)
            {
                List<Carta> mazoDelJugador = new List<Carta>();

                for (int i = 0; i < cartasPorJugador; i++)
                {
                    int numeroCarta = random.Next(cartasCombinadas.Count);
                    Carta cartaSeleciconada = cartasCombinadas[numeroCarta];

                    mazoDelJugador.Add(cartaSeleciconada);
                    cartasCombinadas.RemoveAt(numeroCarta);
                }

                // Verificar si el jugador ya existe en el diccionario
                if (!barajaJugadores.ContainsKey(jugador))
                {
                    barajaJugadores.Add(jugador, mazoDelJugador);
                }
                else
                {
                    barajaJugadores[jugador] = mazoDelJugador; // Actualiza el mazo si ya existe
                }
            }
            return cartasCombinadas;
        }

        private List<Carta> CombinarCartas(List<Carta> cartas)
        {
            Random random = new Random();
            int numeroCartas = cartas.Count;
            int limiteMinimo = 1;

            while (numeroCartas > limiteMinimo)
            {
                numeroCartas--;
                int cambio = random.Next(numeroCartas + limiteMinimo);
                Carta cartaSeleccionada = cartas[cambio];
                cartas[cambio] = cartas[numeroCartas];
                cartas[numeroCartas] = cartaSeleccionada;
            }
            return cartas;
        }

        private List<Carta> CrearCartasModificador()
        {
            Carta cartaMuerte1 = new Carta { identificador = "MuerteInoportuna.png", valor = 200 };
            Carta cartaMuerte2 = new Carta { identificador = "BebeCenteno.png", valor = 200 };
            Carta cartaMuerte3 = new Carta { identificador = "HorneadoEnTarta.png", valor = 200 };
            Carta cartaMuerte4 = new Carta { identificador = "AtragantarHueso.png", valor = 200 };
            Carta cartaMuerte5 = new Carta { identificador = "MuerteViejo.png", valor = 200 };
            Carta cartaMuerte6 = new Carta { identificador = "MuerteSarampio.png", valor = 200 };
            Carta cartaMuerte7 = new Carta { identificador = "DevoradoPorComdrejas.png", valor = 200 };
            Carta cartaMuerte8 = new Carta { identificador = "ConsumidoPorFuego.png", valor = 200 };
            Carta cartaMuerte9 = new Carta { identificador = "Desmembrado.png", valor = 200 };
            Carta cartaMuerte10 = new Carta { identificador = "ComidoPorOsos.png", valor = 200 };
            Carta cartaMuerte11 = new Carta { identificador = "MuerteSinPreocupacion.png", valor = 200 };
            Carta cartaMuerte12 = new Carta { identificador = "EmpujadoPorLasEscaleras.png", valor = 200 };
            Carta cartaMuerte13 = new Carta { identificador = "AsesionadoPorHeredero.png", valor = 200 };
            Carta cartaMuerte14 = new Carta { identificador = "AhogadoEnPantano.png", valor = 200 };
            Carta cartaMuerte15 = new Carta { identificador = "QurmadoPorTurbia.png", valor = 200 };
            Carta cartaMuerte16 = new Carta { identificador = "NoRegreso.png", valor = 200 };
            Carta cartaMuerte17 = new Carta { identificador = "SeveramenteQuemado.png", valor = 200 };
            Carta cartaMuerte18 = new Carta { identificador = "MuertePorDesesperacion.png", valor = 200 };
            Carta cartaMuerte19 = new Carta { identificador = "SinAire.png", valor = 200 };
            Carta cartaMuerte20 = new Carta { identificador = "DesaparecioEnNiebla.png", valor = 200 };
            Carta cartaMuerte21 = new Carta { identificador = "CayoDesdeAlto.png", valor = 200 };

            List<Carta> PrimerMazo = new List<Carta>();
            PrimerMazo.Add(cartaMuerte1);
            PrimerMazo.Add(cartaMuerte2);
            PrimerMazo.Add(cartaMuerte3);
            PrimerMazo.Add(cartaMuerte4);
            PrimerMazo.Add(cartaMuerte5);
            PrimerMazo.Add(cartaMuerte6);
            PrimerMazo.Add(cartaMuerte7);
            PrimerMazo.Add(cartaMuerte8);
            PrimerMazo.Add(cartaMuerte9);
            PrimerMazo.Add(cartaMuerte10);
            PrimerMazo.Add(cartaMuerte11);
            PrimerMazo.Add(cartaMuerte12);
            PrimerMazo.Add(cartaMuerte13);
            PrimerMazo.Add(cartaMuerte14);
            PrimerMazo.Add(cartaMuerte15);
            PrimerMazo.Add(cartaMuerte16);
            PrimerMazo.Add(cartaMuerte17);
            PrimerMazo.Add(cartaMuerte18);
            PrimerMazo.Add(cartaMuerte19);
            PrimerMazo.Add(cartaMuerte20);
            PrimerMazo.Add(cartaMuerte21);

            return PrimerMazo;

        }

        public List<Carta> ObtenerMazoJugador(string nombreJugador)
        {
            return barajaJugadores.TryGetValue(nombreJugador, out List<Carta> mazo) ? mazo : new List<Carta>();
        }

    }
}
