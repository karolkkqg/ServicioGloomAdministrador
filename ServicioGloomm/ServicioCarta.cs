using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioCarta
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
            var cartasSobrantes = RepartirCartas(servicioJuego.ObtenerJugadores(numeroSala), CombinarCartas((List<Carta>)primerMazo.Concat(segundoMazo)));
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

            List<Carta> primerMazo = new List<Carta>();
            primerMazo.Add(cartaMuerte1);
            primerMazo.Add(cartaMuerte2);
            primerMazo.Add(cartaMuerte3);
            primerMazo.Add(cartaMuerte4);
            primerMazo.Add(cartaMuerte5);
            primerMazo.Add(cartaMuerte6);
            primerMazo.Add(cartaMuerte7);
            primerMazo.Add(cartaMuerte8);
            primerMazo.Add(cartaMuerte9);
            primerMazo.Add(cartaMuerte10);
            primerMazo.Add(cartaMuerte11);
            primerMazo.Add(cartaMuerte12);
            primerMazo.Add(cartaMuerte13);
            primerMazo.Add(cartaMuerte14);
            primerMazo.Add(cartaMuerte15);
            primerMazo.Add(cartaMuerte16);
            primerMazo.Add(cartaMuerte17);
            primerMazo.Add(cartaMuerte18);
            primerMazo.Add(cartaMuerte19);
            primerMazo.Add(cartaMuerte20);
            primerMazo.Add(cartaMuerte21);

            return primerMazo;

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

                barajaJugadores.Add(jugador, mazoDelJugador);
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
            Carta cartaModificadora1 = new Carta { identificador = "Accidente1.png", valor = -160 };
            Carta cartaModificadora2 = new Carta { identificador = "Accidente2.png", valor = -120 };
            Carta cartaModificadora3 = new Carta { identificador = "Accidente3.png", valor = -100 };
            Carta cartaModificadora4 = new Carta { identificador = "Accidente4.png", valor = -110 };
            Carta cartaModificadora5 = new Carta { identificador = "Accidente5.png", valor = 35 };
            Carta cartaModificadora6 = new Carta { identificador = "Accidente6.png", valor = 45 };
            Carta cartaModificadora7 = new Carta { identificador = "Accidente7.png", valor = 20 };
            Carta cartaModificadora8 = new Carta { identificador = "Desamor1.png", valor = -130 };
            Carta cartaModificadora9 = new Carta { identificador = "Desamor2.png", valor = -125 };
            Carta cartaModificadora10 = new Carta { identificador = "Desamor3.png", valor = -160 };
            Carta cartaModificadora11 = new Carta { identificador = "Desamor4.png", valor = -145 };
            Carta cartaModificadora12 = new Carta { identificador = "Desamor5.png", valor = 35 };
            Carta cartaModificadora13 = new Carta { identificador = "Desamor6.png", valor = 10 };
            Carta cartaModificadora14 = new Carta { identificador = "Desamor7.png", valor = 50 };
            Carta cartaModificadora15 = new Carta { identificador = "Desgracia1.png", valor = -120 };
            Carta cartaModificadora16 = new Carta { identificador = "Desgracia2.png", valor = -115 };
            Carta cartaModificadora17 = new Carta { identificador = "Desgracia3.png", valor = -140 };
            Carta cartaModificadora18 = new Carta { identificador = "Desgracia4.png", valor = -110 };
            Carta cartaModificadora19 = new Carta { identificador = "Desgracia5.png", valor = 40 };
            Carta cartaModificadora20 = new Carta { identificador = "Desgracia6.png", valor = 30 };
            Carta cartaModificadora21 = new Carta { identificador = "Desgracia7.png", valor = 50 };
            Carta cartaModificadora22 = new Carta { identificador = "Enfermedad1.png", valor = -145 };
            Carta cartaModificadora23 = new Carta { identificador = "Enfermedad2.png", valor = -180 };
            Carta cartaModificadora24 = new Carta { identificador = "Enfermedad3.png", valor = -120 };
            Carta cartaModificadora25 = new Carta { identificador = "Enfermedad4.png", valor = -130 };
            Carta cartaModificadora26 = new Carta { identificador = "Enfermedad5.png", valor = 40 };
            Carta cartaModificadora27 = new Carta { identificador = "Enfermedad6.png", valor = 25 };
            Carta cartaModificadora28 = new Carta { identificador = "Enfermedad7.png", valor = 30 };
            Carta cartaModificadora29 = new Carta { identificador = "Escándalo1.png", valor = -150 };
            Carta cartaModificadora30 = new Carta { identificador = "Escándalo2.png", valor = -170 };
            Carta cartaModificadora31 = new Carta { identificador = "Escándalo3.png", valor = -145 };
            Carta cartaModificadora32 = new Carta { identificador = "Escándalo4.png", valor = -110 };
            Carta cartaModificadora33 = new Carta { identificador = "Escándalo5.png", valor = 20 };
            Carta cartaModificadora34 = new Carta { identificador = "Escándalo6.png", valor = 10 };
            Carta cartaModificadora35 = new Carta { identificador = "Escándalo7.png", valor = 40 };
            Carta cartaModificadora36 = new Carta { identificador = "Fraude1.png", valor = -150 };
            Carta cartaModificadora37 = new Carta { identificador = "Fraude2.png", valor = -160 };
            Carta cartaModificadora38 = new Carta { identificador = "Fraude3.png", valor = -160 };
            Carta cartaModificadora39 = new Carta { identificador = "Fraude4.png", valor = -115 };
            Carta cartaModificadora40 = new Carta { identificador = "Fraude5.png", valor = 40 };
            Carta cartaModificadora41 = new Carta { identificador = "Fraude6.png", valor = 25 };
            Carta cartaModificadora42 = new Carta { identificador = "Fraude7.png", valor = 35 };
            Carta cartaModificadora43 = new Carta { identificador = "Pérdida1.png", valor = -140 };
            Carta cartaModificadora44 = new Carta { identificador = "Pérdida2.png", valor = -90 };
            Carta cartaModificadora45 = new Carta { identificador = "Pérdida3.png", valor = -125 };
            Carta cartaModificadora46 = new Carta { identificador = "Pérdida4.png", valor = -150 };
            Carta cartaModificadora47 = new Carta { identificador = "Pérdida5.png", valor = 50 };
            Carta cartaModificadora48 = new Carta { identificador = "Pérdida6.png", valor = 30 };
            Carta cartaModificadora49 = new Carta { identificador = "Pérdida7.png", valor = 40 };
            Carta cartaModificadora50 = new Carta { identificador = "Ruina1.png", valor = -100 };
            Carta cartaModificadora51 = new Carta { identificador = "Ruina2.png", valor = -150 };
            Carta cartaModificadora52 = new Carta { identificador = "Ruina3.png", valor = -120 };
            Carta cartaModificadora53 = new Carta { identificador = "Ruina4.png", valor = -135 };
            Carta cartaModificadora54 = new Carta { identificador = "Ruina5.png", valor = 50 };
            Carta cartaModificadora55 = new Carta { identificador = "Ruina6.png", valor = 40 };
            Carta cartaModificadora56 = new Carta { identificador = "Ruina7.png", valor = 35 };

            List<Carta> primerMazo = new List<Carta>();
            primerMazo.Add(cartaModificadora1);
            primerMazo.Add(cartaModificadora2);
            primerMazo.Add(cartaModificadora3);
            primerMazo.Add(cartaModificadora4);
            primerMazo.Add(cartaModificadora5);
            primerMazo.Add(cartaModificadora6);
            primerMazo.Add(cartaModificadora7);
            primerMazo.Add(cartaModificadora8);
            primerMazo.Add(cartaModificadora9);
            primerMazo.Add(cartaModificadora10);
            primerMazo.Add(cartaModificadora11);
            primerMazo.Add(cartaModificadora12);
            primerMazo.Add(cartaModificadora13);
            primerMazo.Add(cartaModificadora14);
            primerMazo.Add(cartaModificadora15);
            primerMazo.Add(cartaModificadora16);
            primerMazo.Add(cartaModificadora17);
            primerMazo.Add(cartaModificadora18);
            primerMazo.Add(cartaModificadora19);
            primerMazo.Add(cartaModificadora20);
            primerMazo.Add(cartaModificadora21);
            primerMazo.Add(cartaModificadora22);
            primerMazo.Add(cartaModificadora23);
            primerMazo.Add(cartaModificadora24);
            primerMazo.Add(cartaModificadora25);
            primerMazo.Add(cartaModificadora26);
            primerMazo.Add(cartaModificadora27);
            primerMazo.Add(cartaModificadora28);
            primerMazo.Add(cartaModificadora29);
            primerMazo.Add(cartaModificadora30);
            primerMazo.Add(cartaModificadora31);
            primerMazo.Add(cartaModificadora32);
            primerMazo.Add(cartaModificadora33);
            primerMazo.Add(cartaModificadora34);
            primerMazo.Add(cartaModificadora35);
            primerMazo.Add(cartaModificadora36);
            primerMazo.Add(cartaModificadora37);
            primerMazo.Add(cartaModificadora38);
            primerMazo.Add(cartaModificadora39);
            primerMazo.Add(cartaModificadora40);
            primerMazo.Add(cartaModificadora41);
            primerMazo.Add(cartaModificadora42);
            primerMazo.Add(cartaModificadora43);
            primerMazo.Add(cartaModificadora44);
            primerMazo.Add(cartaModificadora45);
            primerMazo.Add(cartaModificadora46);
            primerMazo.Add(cartaModificadora47);
            primerMazo.Add(cartaModificadora48);
            primerMazo.Add(cartaModificadora49);
            primerMazo.Add(cartaModificadora50);
            primerMazo.Add(cartaModificadora51);
            primerMazo.Add(cartaModificadora52);
            primerMazo.Add(cartaModificadora53);
            primerMazo.Add(cartaModificadora54);
            primerMazo.Add(cartaModificadora55);
            primerMazo.Add(cartaModificadora56);

            return primerMazo;

        }

    }
}
