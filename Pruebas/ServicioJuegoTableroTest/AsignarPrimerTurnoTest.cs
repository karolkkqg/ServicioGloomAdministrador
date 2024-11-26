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
    public class AsignarPrimerTurnoTest
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
        public void AsignarPrimerTurnoExitoso()
        {
            string jugadorActual = servicioJuego.AsignarPrimerTurno("Sala1");

            Assert.AreEqual("Jugador1", jugadorActual, "El primer turno no fue asignado al jugador esperado.");
        }

    }
}
