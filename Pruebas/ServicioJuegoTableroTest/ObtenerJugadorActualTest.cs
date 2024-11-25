using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioJuegoTableroTest
{
    [TestClass]
    public class ObtenerJugadorActualTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.turnosPorSala["Sala1"] = new List<string> { "Jugador1", "Jugador2", "Jugador3" };
            ServicioJuego.indiceTurnoActual["Sala1"] = 0;
        }

        [TestMethod]
        public void ObtenerJugadorActualExitoso()
        {
            string jugadorActual = servicioJuego.ObtenerJugadorActual("Sala1");

            Assert.AreEqual("Jugador1", jugadorActual, "El jugador actual no es el esperado.");
        }

        [TestMethod]
        public void ObtenerJugadorActualCambiaTurnoExitoso()
        {
            ServicioJuego.indiceTurnoActual["Sala1"] = 1;

            string jugadorActual = servicioJuego.ObtenerJugadorActual("Sala1");

            Assert.AreEqual("Jugador2", jugadorActual, "El jugador actual no es el esperado.");
        }
    }
}
