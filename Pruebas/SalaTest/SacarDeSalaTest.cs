using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class SacarDeSalaTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.salaJugadoresPorSala.Clear();
            ServicioJuego.salaJugadores.Clear();
            ServicioJuego.personajesPorSala.Clear();
            ServicioJuego.personajesUsadosPorSala.Clear();

            var mockCallback = new Mock<ISalaCallback>();
            ServicioJuego.salaJugadoresPorSala.Add("Sala1", new Dictionary<string, ISalaCallback>
        {
            { "Jugador1", mockCallback.Object },
            { "Jugador2", mockCallback.Object }
        });

            ServicioJuego.salaJugadores.Add("Sala1", new List<string> { "Jugador1", "Jugador2" });

            ServicioJuego.personajesPorSala.Add("Sala1", new Dictionary<string, (string, int)>
        {
            { "Jugador1", ("Personaje1", 100) },
            { "Jugador2", ("Personaje2", 100) }
        });

            ServicioJuego.personajesUsadosPorSala.Add("Sala1", new List<string> { "Personaje1", "Personaje2" });
        }

        [TestMethod]
        public void SacarDeSala_RemueveJugadorExitosamente()
        {
            string numeroSala = "Sala1";
            string nombreUsuario = "Jugador1";

            servicioJuego.SacarDeSala(numeroSala, nombreUsuario);

            Assert.IsFalse(ServicioJuego.salaJugadoresPorSala[numeroSala].ContainsKey(nombreUsuario), "El jugador no fue removido correctamente de salaJugadoresPorSala.");
            Assert.IsFalse(ServicioJuego.salaJugadores[numeroSala].Contains(nombreUsuario), "El jugador no fue removido correctamente de salaJugadores.");
            Assert.IsFalse(ServicioJuego.personajesPorSala[numeroSala].ContainsKey(nombreUsuario), "El personaje asociado no fue removido correctamente de personajesPorSala.");
            Assert.IsFalse(ServicioJuego.personajesUsadosPorSala[numeroSala].Contains("Personaje1"), "El personaje usado no fue removido correctamente de personajesUsadosPorSala.");
        }

        [TestMethod]
        public void SacarDeSala_EliminaSalaCuandoQuedaVacia()
        {
            string numeroSala = "Sala1";
            string nombreUsuario1 = "Jugador1";
            string nombreUsuario2 = "Jugador2";

            servicioJuego.SacarDeSala(numeroSala, nombreUsuario1);
            servicioJuego.SacarDeSala(numeroSala, nombreUsuario2);

            Assert.IsFalse(ServicioJuego.salaJugadoresPorSala.ContainsKey(numeroSala), "La sala no fue eliminada correctamente de salaJugadoresPorSala.");
            Assert.IsFalse(ServicioJuego.salaJugadores.ContainsKey(numeroSala), "La sala no fue eliminada correctamente de salaJugadores.");
            Assert.IsFalse(ServicioJuego.personajesPorSala.ContainsKey(numeroSala), "La sala no fue eliminada correctamente de personajesPorSala.");
            Assert.IsFalse(ServicioJuego.personajesUsadosPorSala.ContainsKey(numeroSala), "La sala no fue eliminada correctamente de personajesUsadosPorSala.");
        }

        [TestCleanup]
        public void LimpiarDiccionarios()
        {
            ServicioJuego.salaJugadoresPorSala.Clear();
            ServicioJuego.salaJugadores.Clear();
            ServicioJuego.personajesPorSala.Clear();
            ServicioJuego.personajesUsadosPorSala.Clear();
        }
    }
}
