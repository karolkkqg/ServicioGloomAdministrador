using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

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

            ServicioJuego.CartasSobrantes.Clear();
            ServicioJuego.indiceTurnoActual.Clear();
            ServicioJuego.partidaYaIniciada.Clear();
            ServicioJuego.jugadoresConectadosListos.Clear();
            ServicioJuego.turnosPorSala.Clear();

            ServicioJuego.jugadoresConectadosListos["Sala1"] = new List<string> { "Jugador1", "Jugador2", "Jugador3" };
            ServicioJuego.partidaYaIniciada["Sala1"] = false;

            ServicioJuego.salaJugadoresPorSala["Sala1"] = new Dictionary<string, ISalaCallback>
            {
                { "Jugador1", null },
                { "Jugador2", null },
                { "Jugador3", null }
            };
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

            Assert.IsTrue(ServicioJuego.turnosPorSala.ContainsKey(numeroSala));
            Assert.AreEqual(numeroJugadores, ServicioJuego.turnosPorSala[numeroSala].Count);

            Assert.IsTrue(ServicioJuego.indiceTurnoActual.ContainsKey(numeroSala));
            Assert.AreEqual(0, ServicioJuego.indiceTurnoActual[numeroSala]);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ServicioJuego.CartasSobrantes.Clear();
            ServicioJuego.indiceTurnoActual.Clear();
            ServicioJuego.partidaYaIniciada.Clear();
            ServicioJuego.jugadoresConectadosListos.Clear();
            ServicioJuego.turnosPorSala.Clear();
            ServicioJuego.salaJugadoresPorSala.Clear();
        }
    }
}