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

            typeof(ServicioJuego).GetField("jugadoresConectadosCallback", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, new Dictionary<string, IJuegoAdministradorCallback>());
            

            ServicioJuego.direccionJugadorEnJuego.Clear();
            ServicioJuego.TurnsInGameboard.Clear();
            ServicioJuego.CartasSobrantes.Clear();
            ServicioJuego.indiceTurnoActual.Clear();
            ServicioJuego.partidaYaIniciada.Clear();

            var jugadoresConectadosField = typeof(ServicioJuego)
        .GetField("jugadoresConectados", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectados = (Dictionary<string, string>)jugadoresConectadosField.GetValue(null);
            jugadoresConectados.Clear();

            jugadoresConectados["Jugador1"] = "Sala1";
            jugadoresConectados["Jugador2"] = "Sala1";
            jugadoresConectados["Jugador3"] = "Sala1";

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
