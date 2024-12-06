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
    public class CambiarTurnoTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.turnosPorSala["Sala1"] = new List<string> { "Jugador1", "Jugador2", "Jugador3" };
            ServicioJuego.indiceTurnoActual["Sala1"] = 0;

            ServicioJuego.jugadoresConCastigos.Clear();
            ServicioJuego.jugadoresVivos["Sala1"] = new List<string> { "Jugador1", "Jugador2", "Jugador3" };
        }

        [TestMethod]
        public void CambiarTurnoSinCastigosExitoso()
        {
            servicioJuego.CambiarTurno("Sala1");

            Assert.AreEqual(1, ServicioJuego.indiceTurnoActual["Sala1"], "El turno no cambió correctamente.");
            Assert.AreEqual("Jugador2", ServicioJuego.turnosPorSala["Sala1"][ServicioJuego.indiceTurnoActual["Sala1"]], "El jugador con el turno actual no es el esperado.");
        }

        [TestMethod]
        public void CambiarTurnoConCastigosExitoso()
        {
            ServicioJuego.jugadoresConCastigos["Jugador2"] = 1;
            servicioJuego.CambiarTurno("Sala1");

            Assert.AreEqual(2, ServicioJuego.indiceTurnoActual["Sala1"], "El turno no se saltó al jugador con castigo.");
            Assert.AreEqual("Jugador3", ServicioJuego.turnosPorSala["Sala1"][ServicioJuego.indiceTurnoActual["Sala1"]], "El jugador con el turno actual no es el esperado.");
        }

        [TestMethod]
        public void CambiarTurnoCastigosReducidosExitoso()
        {
            ServicioJuego.jugadoresConCastigos["Jugador2"] = 1;

            servicioJuego.CambiarTurno("Sala1");
            Assert.AreEqual(2, ServicioJuego.indiceTurnoActual["Sala1"], "El turno no se saltó al jugador con castigo.");
            Assert.AreEqual("Jugador3", ServicioJuego.turnosPorSala["Sala1"][ServicioJuego.indiceTurnoActual["Sala1"]], "El jugador con el turno actual no es el esperado.");

            servicioJuego.CambiarTurno("Sala1");
            servicioJuego.CambiarTurno("Sala1");
            Assert.IsFalse(ServicioJuego.jugadoresConCastigos.ContainsKey("Jugador2"), "El castigo del jugador no se eliminó correctamente.");
            Assert.AreEqual(1, ServicioJuego.indiceTurnoActual["Sala1"], "El turno no volvió al jugador esperado.");
            Assert.AreEqual("Jugador2", ServicioJuego.turnosPorSala["Sala1"][ServicioJuego.indiceTurnoActual["Sala1"]], "El jugador con el turno actual no es el esperado.");
        }

        [TestMethod]
        public void CambiarTurnoJugadorMuertoNoRecibeTurno()
        {
            ServicioJuego.jugadoresVivos["Sala1"].Remove("Jugador2");

            servicioJuego.CambiarTurno("Sala1");

            Assert.AreEqual(2, ServicioJuego.indiceTurnoActual["Sala1"], "El turno no se saltó al jugador muerto.");
            Assert.AreEqual("Jugador3", ServicioJuego.turnosPorSala["Sala1"][ServicioJuego.indiceTurnoActual["Sala1"]], "El jugador con el turno actual no es el esperado.");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No hay jugadores vivos en la sala.")]
        public void CambiarTurnoSinJugadoresVivosLanzaError()
        {
            ServicioJuego.jugadoresVivos["Sala1"].Clear();

            servicioJuego.CambiarTurno("Sala1");
        }
    }



 class MockCallback : IJuegoAdministradorCallback
    {
        public string UltimoJugadorEnTurno { get; private set; }

        public void ActualizarImagenMazoCartaBonus()
        {
            throw new NotImplementedException();
        }

        public void ActualizarImagenMazoCartaSobrante()
        {
            throw new NotImplementedException();
        }

        public void ActualizarJugadorMuerto(string jugadorMuerto)
        {
            throw new NotImplementedException();
        }

        public void ActualizarMazoJugador()
        {
            throw new NotImplementedException();
        }

        public void ActualizarTurno(string nombreDelUsuarioEnTurno)
        {
            UltimoJugadorEnTurno = nombreDelUsuarioEnTurno; 
        }

        public void EnviarGanador(string jugador)
        {
            throw new NotImplementedException();
        }

        public void EnviarTurno(string nombreDelUsuarioEnTurno)
        {
            throw new NotImplementedException();
        }

        public void IniciarVotacion(string jugadorObjetivo)
        {
            throw new NotImplementedException();
        }

        public void NotificarExpulsion(string jugadorExpulsado)
        {
            throw new NotImplementedException();
        }
    }
}
