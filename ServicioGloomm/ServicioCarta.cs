using BibliotecaClases;
using BlbibliotecaClases;
using ServicioGlomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IServicioCarta
    {
        private ServicioJuego servicioJuego;
        public static readonly Dictionary<string, List<Carta>> barajaJugadores = new Dictionary<string, List<Carta>>();
        public static readonly List<Carta> cartasSobrantesGlobal = new List<Carta>();
        public static readonly List<Carta> cartasBonus = new List<Carta>();

        public List<Carta> BarajearMazo(string numeroSala)
        {
            CrearCartasBonus();
            List<Carta> primerMazo = CrearCartasDeMuerte();
            List<Carta> segundoMazo = CrearCartasModificador();
            servicioJuego = new ServicioJuego();

            cartasSobrantesGlobal.AddRange(RepartirCartas(
            servicioJuego.ObtenerJugadores(numeroSala),
            CombinarCartas(primerMazo.Concat(segundoMazo).ToList())
            ));

            return cartasSobrantesGlobal;
        }

        private List<Carta> CrearCartasDeMuerte()
        {
            Carta cartaMuerte1 = new Carta { identificador = "MuerteInoportuna.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte2 = new Carta { identificador = "BebeCenteno.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte3 = new Carta { identificador = "HorneadoEnTarta.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte4 = new Carta { identificador = "AtragantarHueso.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte5 = new Carta { identificador = "MuerteViejo.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte6 = new Carta { identificador = "MuerteSarampio.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte7 = new Carta { identificador = "DevoradoPorComdrejas.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte8 = new Carta { identificador = "ConsumidoPorFuego.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte9 = new Carta { identificador = "Desmembrado.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte10 = new Carta { identificador = "ComidoPorOsos.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte11 = new Carta { identificador = "MuerteSinPreocupacion.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte12 = new Carta { identificador = "EmpujadoPorLasEscaleras.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte13 = new Carta { identificador = "AsesionadoPorHeredero.png", valor = 200, tipo = "muerte" };
            Carta cartaMuerte14 = new Carta { identificador = "AhogadoEnPantano.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte15 = new Carta { identificador = "QurmadoPorTurbia.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte16 = new Carta { identificador = "NoRegreso.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte17 = new Carta { identificador = "SeveramenteQuemado.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte18 = new Carta { identificador = "MuertePorDesesperacion.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte19 = new Carta { identificador = "SinAire.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte20 = new Carta { identificador = "DesaparecioEnNiebla.png", valor = 200 , tipo = "muerte" };
            Carta cartaMuerte21 = new Carta { identificador = "CayoDesdeAlto.png", valor = 200 , tipo = "muerte" };

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
                if (!barajaJugadores.ContainsKey(jugador))
                {
                    barajaJugadores.Add(jugador, mazoDelJugador);
                }
                else
                {
                    barajaJugadores[jugador] = mazoDelJugador;
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
            Carta cartaModificadora1 = new Carta { identificador = "Accidente1.png", valor = -160, tipo = "modificador"};
            Carta cartaModificadora2 = new Carta { identificador = "Accidente2.png", valor = -120, tipo = "modificador" };
            Carta cartaModificadora3 = new Carta { identificador = "Accidente3.png", valor = -100 , tipo = "modificador" };
            Carta cartaModificadora4 = new Carta { identificador = "Accidente4.png", valor = -110 , tipo = "modificador" };
            Carta cartaModificadora5 = new Carta { identificador = "Accidente5.png", valor = 35, tipo = "modificador" };
            Carta cartaModificadora6 = new Carta { identificador = "Accidente6.png", valor = 45 , tipo = "modificador" };
            Carta cartaModificadora7 = new Carta { identificador = "Accidente7.png", valor = 20 , tipo = "modificador" };
            Carta cartaModificadora8 = new Carta { identificador = "Desamor1.png", valor = -130 , tipo = "modificador" };
            Carta cartaModificadora9 = new Carta { identificador = "Desamor2.png", valor = -125 , tipo = "modificador" };
            Carta cartaModificadora10 = new Carta { identificador = "Desamor3.png", valor = -160 , tipo = "modificador" };
            Carta cartaModificadora11 = new Carta { identificador = "Desamor4.png", valor = -145 , tipo = "modificador" };
            Carta cartaModificadora12 = new Carta { identificador = "Desamor5.png", valor = 35 , tipo = "modificador" };
            Carta cartaModificadora13 = new Carta { identificador = "Desamor6.png", valor = 10 , tipo = "modificador" };
            Carta cartaModificadora14 = new Carta { identificador = "Desamor7.png", valor = 50 , tipo = "modificador" };
            Carta cartaModificadora15 = new Carta { identificador = "Desgracia1.png", valor = -120 , tipo = "modificador" };
            Carta cartaModificadora16 = new Carta { identificador = "Desgracia2.png", valor = -115 , tipo = "modificador" };
            Carta cartaModificadora17 = new Carta { identificador = "Desgracia3.png", valor = -140 , tipo = "modificador" };
            Carta cartaModificadora18 = new Carta { identificador = "Desgracia4.png", valor = -110 , tipo = "modificador" };
            Carta cartaModificadora19 = new Carta { identificador = "Desgracia5.png", valor = 40 , tipo = "modificador" };
            Carta cartaModificadora20 = new Carta { identificador = "Desgracia6.png", valor = 30 , tipo = "modificador" };
            Carta cartaModificadora21 = new Carta { identificador = "Desgracia7.png", valor = 50 , tipo = "modificador" };
            Carta cartaModificadora22 = new Carta { identificador = "Enfermedad1.png", valor = -145 , tipo = "modificador" };
            Carta cartaModificadora23 = new Carta { identificador = "Enfermedad2.png", valor = -180 , tipo = "modificador" };
            Carta cartaModificadora24 = new Carta { identificador = "Enfermedad3.png", valor = -120 , tipo = "modificador" };
            Carta cartaModificadora25 = new Carta { identificador = "Enfermedad4.png", valor = -130, tipo = "modificador" };
            Carta cartaModificadora26 = new Carta { identificador = "Enfermedad5.png", valor = 40, tipo = "modificador" };
            Carta cartaModificadora27 = new Carta { identificador = "Enfermedad6.png", valor = 25, tipo = "modificador" };
            Carta cartaModificadora28 = new Carta { identificador = "Enfermedad7.png", valor = 30, tipo = "modificador" };
            Carta cartaModificadora29 = new Carta { identificador = "Escándalo1.png", valor = -150 , tipo = "modificador" };
            Carta cartaModificadora30 = new Carta { identificador = "Escándalo2.png", valor = -170 , tipo = "modificador" };
            Carta cartaModificadora31 = new Carta { identificador = "Escándalo3.png", valor = -145 , tipo = "modificador" };
            Carta cartaModificadora32 = new Carta { identificador = "Escándalo4.png", valor = -110 , tipo = "modificador" };
            Carta cartaModificadora33 = new Carta { identificador = "Escándalo5.png", valor = 20 , tipo = "modificador" };
            Carta cartaModificadora34 = new Carta { identificador = "Escándalo6.png", valor = 10 , tipo = "modificador" };
            Carta cartaModificadora35 = new Carta { identificador = "Escándalo7.png", valor = 40 , tipo = "modificador" };
            Carta cartaModificadora36 = new Carta { identificador = "Fraude1.png", valor = -150 , tipo = "modificador" };
            Carta cartaModificadora37 = new Carta { identificador = "Fraude2.png", valor = -160 , tipo = "modificador" };
            Carta cartaModificadora38 = new Carta { identificador = "Fraude3.png", valor = -160 , tipo = "modificador" };
            Carta cartaModificadora39 = new Carta { identificador = "Fraude4.png", valor = -115 , tipo = "modificador" };
            Carta cartaModificadora40 = new Carta { identificador = "Fraude5.png", valor = 40 , tipo = "modificador" };
            Carta cartaModificadora41 = new Carta { identificador = "Fraude6.png", valor = 25 , tipo = "modificador" };
            Carta cartaModificadora42 = new Carta { identificador = "Fraude7.png", valor = 35 , tipo = "modificador" };
            Carta cartaModificadora43 = new Carta { identificador = "Pérdida1.png", valor = -140 , tipo = "modificador" };
            Carta cartaModificadora44 = new Carta { identificador = "Pérdida2.png", valor = -90 , tipo = "modificador" };
            Carta cartaModificadora45 = new Carta { identificador = "Pérdida3.png", valor = -125 , tipo = "modificador" };
            Carta cartaModificadora46 = new Carta { identificador = "Pérdida4.png", valor = -150 , tipo = "modificador" };
            Carta cartaModificadora47 = new Carta { identificador = "Pérdida5.png", valor = 50 , tipo = "modificador" };
            Carta cartaModificadora48 = new Carta { identificador = "Pérdida6.png", valor = 30 , tipo = "modificador" };
            Carta cartaModificadora49 = new Carta { identificador = "Pérdida7.png", valor = 40 , tipo = "modificador" };
            Carta cartaModificadora50 = new Carta { identificador = "Ruina1.png", valor = -100 , tipo = "modificador" };
            Carta cartaModificadora51 = new Carta { identificador = "Ruina2.png", valor = -150 , tipo = "modificador" };
            Carta cartaModificadora52 = new Carta { identificador = "Ruina3.png", valor = -120 , tipo = "modificador" };
            Carta cartaModificadora53 = new Carta { identificador = "Ruina4.png", valor = -135 , tipo = "modificador" };
            Carta cartaModificadora54 = new Carta { identificador = "Ruina5.png", valor = 50 , tipo = "modificador" };
            Carta cartaModificadora55 = new Carta { identificador = "Ruina6.png", valor = 40 , tipo = "modificador" };
            Carta cartaModificadora56 = new Carta { identificador = "Ruina7.png", valor = 35 , tipo = "modificador" };

            List<Carta> segundoMazo = new List<Carta>();

            segundoMazo.Add(cartaModificadora1);
            segundoMazo.Add(cartaModificadora2);
            segundoMazo.Add(cartaModificadora3);
            segundoMazo.Add(cartaModificadora4);
            segundoMazo.Add(cartaModificadora5);
            segundoMazo.Add(cartaModificadora6);
            segundoMazo.Add(cartaModificadora7);
            segundoMazo.Add(cartaModificadora8);
            segundoMazo.Add(cartaModificadora9);
            segundoMazo.Add(cartaModificadora10);
            segundoMazo.Add(cartaModificadora11);
            segundoMazo.Add(cartaModificadora12);
            segundoMazo.Add(cartaModificadora13);
            segundoMazo.Add(cartaModificadora14);
            segundoMazo.Add(cartaModificadora15);
            segundoMazo.Add(cartaModificadora16);
            segundoMazo.Add(cartaModificadora17);
            segundoMazo.Add(cartaModificadora18);
            segundoMazo.Add(cartaModificadora19);
            segundoMazo.Add(cartaModificadora20);
            segundoMazo.Add(cartaModificadora21);
            segundoMazo.Add(cartaModificadora22);
            segundoMazo.Add(cartaModificadora23);
            segundoMazo.Add(cartaModificadora24);
            segundoMazo.Add(cartaModificadora25);
            segundoMazo.Add(cartaModificadora26);
            segundoMazo.Add(cartaModificadora27);
            segundoMazo.Add(cartaModificadora28);
            segundoMazo.Add(cartaModificadora29);
            segundoMazo.Add(cartaModificadora30);
            segundoMazo.Add(cartaModificadora31);
            segundoMazo.Add(cartaModificadora32);
            segundoMazo.Add(cartaModificadora33);
            segundoMazo.Add(cartaModificadora34);
            segundoMazo.Add(cartaModificadora35);
            segundoMazo.Add(cartaModificadora36);
            segundoMazo.Add(cartaModificadora37);
            segundoMazo.Add(cartaModificadora38);
            segundoMazo.Add(cartaModificadora39);
            segundoMazo.Add(cartaModificadora40);
            segundoMazo.Add(cartaModificadora41);
            segundoMazo.Add(cartaModificadora42);
            segundoMazo.Add(cartaModificadora43);
            segundoMazo.Add(cartaModificadora44);
            segundoMazo.Add(cartaModificadora45);
            segundoMazo.Add(cartaModificadora46);
            segundoMazo.Add(cartaModificadora47);
            segundoMazo.Add(cartaModificadora48);
            segundoMazo.Add(cartaModificadora49);
            segundoMazo.Add(cartaModificadora50);
            segundoMazo.Add(cartaModificadora51);
            segundoMazo.Add(cartaModificadora52);
            segundoMazo.Add(cartaModificadora53);
            segundoMazo.Add(cartaModificadora54);
            segundoMazo.Add(cartaModificadora55);
            segundoMazo.Add(cartaModificadora56);

            return segundoMazo;

        }

        public List<Carta> ObtenerMazoJugador(string nombreJugador)
        {
            return barajaJugadores.TryGetValue(nombreJugador, out List<Carta> mazo) ? mazo : new List<Carta>();
        }

        public void AgregarCartaAMazoJugador(string nombreUsuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            List<Carta> mazoDelJugador = barajaJugadores[nombreUsuario];
            try
            {
                ValidarExistenciaCartasSobrantes();
                ValidarCantidadDeCartasJugador(nombreUsuario);
                Carta cartaNueva = cartasSobrantesGlobal[0];
                mazoDelJugador.Add(cartaNueva);
                cartasSobrantesGlobal.RemoveAt(0);
            } catch (FaultException<ManejadorExcepciones> ex) 
            {
                administradorLogger.RegistroError(ex); 
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
            }  
        }

        public bool ObtenerMazoRestante()
        {
            return cartasSobrantesGlobal.Any();
        }

        private void ValidarCantidadDeCartasJugador(string nombreUsuario)
        {
            List<Carta> mazoDelJugador = barajaJugadores[nombreUsuario];
            if (mazoDelJugador.Count >= 7)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("20", "No puede agarrar más de 7 cartas"));
            }
        }

        private void ValidarExistenciaCartasSobrantes()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (cartasSobrantesGlobal.Count == 0)
            {
                foreach (var jugador in jugadoresConectadosTableroCallback)
                {
                    if (jugadoresConectadosTableroCallback.ContainsKey(jugador.Key) && jugadoresConectadosTableroCallback[jugador.Key] != null)
                    {
                        try
                        {
                            jugadoresConectadosTableroCallback[jugador.Key].ActualizarImagenMazoCartaSobrante();
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

        public void QuitarCartaDeMazoJugador(string nombreUsuario, Carta cartaAEliminar)
        {
            var mazoDelJugadorPropietario = barajaJugadores[nombreUsuario];
            int indiceCarta = mazoDelJugadorPropietario.FindIndex(c =>
            c.identificador == cartaAEliminar.identificador &&
            c.valor == cartaAEliminar.valor &&
            c.tipo == cartaAEliminar.tipo);
            mazoDelJugadorPropietario.RemoveAt(indiceCarta);
        }

        public void QuitarCartaDeMazoJugadorExterno(string nombreUsuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (barajaJugadores.ContainsKey(nombreUsuario))
            {
                ValidarExistenciaDeCartasJugador(nombreUsuario);
                var mazoDelJugador = barajaJugadores[nombreUsuario];

                if (mazoDelJugador.Count > 0)
                {
                    mazoDelJugador.RemoveAt(0);

                   
                    if (jugadoresConectadosTableroCallback.TryGetValue(nombreUsuario, out IJuegoAdministradorCallback callback))
                    {
                        try
                        {
                            callback.ActualizarMazoJugador();
                        }
                        catch (FaultException<ManejadorExcepciones> ex)
                        {
                            administradorLogger.RegistroError(ex);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));       
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

        private void ValidarExistenciaDeCartasJugador(string nombreUsuario)
        {
            if (barajaJugadores[nombreUsuario].Count == 0)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("38", "El jugador ya no tiene cartas para quitar"));
            }
        }

        private void CrearCartasBonus()
        {
            Carta cartaBonus1 = new Carta { identificador = "SaltarJugador.png", valor = 0, tipo = "saltarJugador" };
            Carta cartaBonus2 = new Carta { identificador = "RobarCarta2.png", valor = 0, tipo = "robar2Cartas" };
            Carta cartaBonus3 = new Carta { identificador = "RobarCarta1.png", valor = 0, tipo = "robar1Cartas" };
            Carta cartaBonus4 = new Carta { identificador = "QuitarCarta.png", valor = 0, tipo = "QuitarCarta" };
            Carta cartaBonus5 = new Carta { identificador = "PerderTurno.png", valor = 0, tipo = "PerderTurno" };

            cartasBonus.Add(cartaBonus1);
            cartasBonus.Add(cartaBonus2);
            cartasBonus.Add(cartaBonus3);
            cartasBonus.Add(cartaBonus4);
            cartasBonus.Add(cartaBonus5);
            cartasBonus.Add(cartaBonus1);
            cartasBonus.Add(cartaBonus2);
            cartasBonus.Add(cartaBonus3);
            cartasBonus.Add(cartaBonus4);
            cartasBonus.Add(cartaBonus5);

        }

        public Carta ObtenerCartasBonus()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                Carta carta = cartasBonus[0];
                cartasBonus.RemoveAt(0);
                ValidarExistenciaCartasSobrantesBonus();
                return carta;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
            }
            
        }

        private void ValidarExistenciaCartasSobrantesBonus()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (cartasBonus.Count == 0)
            {

                foreach (var jugador in jugadoresConectadosTableroCallback)
                {
                    if (jugadoresConectadosTableroCallback.ContainsKey(jugador.Key) && jugadoresConectadosTableroCallback[jugador.Key] != null)
                    {
                        try
                        {
                            jugadoresConectadosTableroCallback[jugador.Key].ActualizarImagenMazoCartaBonus();
                            
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
    }
}
