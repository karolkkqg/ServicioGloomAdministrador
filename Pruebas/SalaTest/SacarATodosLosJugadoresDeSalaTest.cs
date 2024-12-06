using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class SacarATodosLosJugadoresDeSalaTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.salaJugadoresPorSala.Clear();

            ServicioJuego.salaJugadoresPorSala.Add("Sala1", new Dictionary<string, ISalaCallback>
        {
            { "Jugador1", Mock.Of<ISalaCallback>() },
            { "Jugador2", Mock.Of<ISalaCallback>() },
            { "Jugador3", Mock.Of<ISalaCallback>() }
        });
        }

        [TestMethod]
        public void SacarATodosLosJugadoresDeSalaEliminaTodosLosJugadoresYSala()
        {
            string numeroSala = "Sala1";

            servicioJuego.SacarATodosLosJugadoresDeSala(numeroSala);

            Assert.IsFalse(ServicioJuego.salaJugadoresPorSala.ContainsKey(numeroSala), "La sala no fue eliminada correctamente.");
        }

        [TestMethod]
        public void SacarATodosLosJugadoresDeSalaManejaErroresDeComunicacion()
        {
            string numeroSala = "Sala1";

            var mockCallback = Mock.Get(ServicioJuego.salaJugadoresPorSala["Sala1"]["Jugador2"]);
            mockCallback.Setup(c => c.SacarDeSalaATodosJugadores()).Throws(new CommunicationException());

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.SacarATodosLosJugadoresDeSala(numeroSala);
            });

            Assert.AreEqual("16", exception.Detail.mensaje, "El código de la excepción no es el esperado.");

            Assert.IsFalse(ServicioJuego.salaJugadoresPorSala["Sala1"].ContainsKey("Jugador2"), "El jugador no fue eliminado tras el fallo de comunicación.");
        }

        [TestMethod]
        public void SacarATodosLosJugadoresDeSalaManejaErroresDeTimeout()
        {
            string numeroSala = "Sala1";

            var mockCallback = Mock.Get(ServicioJuego.salaJugadoresPorSala["Sala1"]["Jugador3"]);
            mockCallback.Setup(c => c.SacarDeSalaATodosJugadores()).Throws(new TimeoutException());

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.SacarATodosLosJugadoresDeSala(numeroSala);
            });

            Assert.AreEqual("18", exception.Detail.mensaje, "El código de la excepción no es el esperado.");

            Assert.IsFalse(ServicioJuego.salaJugadoresPorSala["Sala1"].ContainsKey("Jugador3"), "El jugador no fue eliminado tras el fallo de timeout.");
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            ServicioJuego.salaJugadoresPorSala.Clear();
        }
    }
}
