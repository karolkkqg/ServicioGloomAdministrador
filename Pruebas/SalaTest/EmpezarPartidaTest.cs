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
    public class EmpezarPartidaTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            ServicioJuego.salaJugadoresPorSala.Clear();
            ServicioJuego.jugadoresConectadosListos.Clear();

            ServicioJuego.salaJugadoresPorSala.Add("Sala1", new Dictionary<string, ISalaCallback>
            {
                { "Jugador1", Mock.Of<ISalaCallback>() },
                { "Jugador2", Mock.Of<ISalaCallback>() },
                { "Jugador3", Mock.Of<ISalaCallback>() }
            });

            ServicioJuego.jugadoresConectadosListos.Add("Sala1", new List<string>
            {
                "Jugador1", "Jugador2", "Jugador3"
            });
        }

        [TestMethod]
        public void EmpezarPartidaIniciaCorrectamenteParaTodosLosJugadores()
        {
            string idSala = "Sala1";

            foreach (var jugador in ServicioJuego.salaJugadoresPorSala[idSala])
            {
                var mockCallback = Mock.Get(jugador.Value);
                mockCallback.Setup(c => c.EmpezarJuego()).Verifiable();
            }

            servicioJuego.EmpezarPartida(idSala);

            foreach (var jugador in ServicioJuego.salaJugadoresPorSala[idSala])
            {
                var mockCallback = Mock.Get(jugador.Value);
                mockCallback.Verify(c => c.EmpezarJuego(), Times.Once, $"El jugador {jugador.Key} no fue notificado para empezar el juego.");
            }
        }

        [TestMethod]
        public void EmpezarPartidaManejaErroresDeComunicacion()
        {
           
            string idSala = "Sala1";

            var mockCallback = Mock.Get(ServicioJuego.salaJugadoresPorSala[idSala]["Jugador2"]);
            mockCallback.Setup(c => c.EmpezarJuego()).Throws(new CommunicationException());

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.EmpezarPartida(idSala);
            });

            Assert.AreEqual("16", exception.Detail.codigo, "El código de la excepción no es el esperado.");

            Assert.IsFalse(ServicioJuego.salaJugadoresPorSala[idSala].ContainsKey("Jugador2"), "El jugador no fue eliminado tras el fallo de comunicación.");
        }

        [TestMethod]
        public void EmpezarPartidaManejaErroresDeTimeout()
        {
            string idSala = "Sala1";

            var mockCallback = Mock.Get(ServicioJuego.salaJugadoresPorSala[idSala]["Jugador3"]);
            mockCallback.Setup(c => c.EmpezarJuego()).Throws(new TimeoutException());

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.EmpezarPartida(idSala);
            });

            Assert.AreEqual("18", exception.Detail.codigo, "El código de la excepción no es el esperado.");

            Assert.IsFalse(ServicioJuego.salaJugadoresPorSala[idSala].ContainsKey("Jugador3"), "El jugador no fue eliminado tras el fallo de tiempo de espera.");
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            ServicioJuego.salaJugadoresPorSala.Clear();
        }
    }
}
