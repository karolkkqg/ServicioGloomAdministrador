using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioJuegoTableroTest
{
    [TestClass]
    public class IniciarPartidaPorAdministradorTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.JugadoresConectadosCallback.Clear();
            ServicioJuego.JugadoresConectados.Clear();
            ServicioJuego.direccionJugadorEnJuego.Clear();
            ServicioJuego.TurnsInGameboard.Clear();
            ServicioJuego.CartasSobrantes.Clear();
            ServicioJuego.indiceTurnoActual.Clear();
            ServicioJuego.partidaYaIniciada.Clear();

            ServicioJuego.JugadoresConectados.Add("Jugador1", "Sala1");
            ServicioJuego.JugadoresConectados.Add("Jugador2", "Sala1");
            ServicioJuego.JugadoresConectados.Add("Jugador3", "Sala1");

            ServicioJuego.partidaYaIniciada.Add("Sala1", false);
        }

        [TestMethod]
        public void IniciarPartidaPorAdministradorTestExitoso()
        {
            string nombreAdministrador = "Admin1";
            string numeroSala = "Sala1";
            int numeroJugadores = 3;

            servicioJuego.IniciarPartidaPorAdministrador(nombreAdministrador, numeroSala, numeroJugadores);

            Assert.IsTrue(ServicioJuego.partidaYaIniciada.ContainsKey(numeroSala));
            Assert.IsTrue(ServicioJuego.partidaYaIniciada[numeroSala]);

            Assert.IsNotNull(ServicioJuego.CartasSobrantes);
            Assert.IsTrue(ServicioJuego.CartasSobrantes.Count > 0);

            Assert.IsTrue(ServicioJuego.direccionJugadorEnJuego.ContainsKey(numeroSala));
            Assert.AreEqual(numeroJugadores, ServicioJuego.direccionJugadorEnJuego[numeroSala].Count);

            Assert.IsTrue(ServicioJuego.TurnsInGameboard.ContainsKey(numeroSala));
            Assert.IsNotNull(ServicioJuego.TurnsInGameboard[numeroSala]);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void IniciarPartidaPorAdministradorJugadoresIncorrectosTestFallido()
        {
            string nombreAdministrador = "Admin1";
            string numeroSala = "Sala1";
            int numeroJugadores = 4;

            servicioJuego.IniciarPartidaPorAdministrador(nombreAdministrador, numeroSala, numeroJugadores);
        }
    }
}
