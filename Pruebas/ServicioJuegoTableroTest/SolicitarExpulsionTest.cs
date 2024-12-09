using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class SolicitarExpulsionTests
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresConectadosTablero.Clear();
            ServicioJuego.turnosPorSala.Clear();
            ServicioJuego.votosExpulsion.Clear();
            ServicioJuego.administradoresDeSala.Clear();
            var mockCallback1 = new Mock<IJuegoAdministradorCallback>();
            var mockCallback2 = new Mock<IJuegoAdministradorCallback>();

            ServicioJuego.jugadoresConectadosTableroCallback.Add("Pinku", mockCallback1.Object);
            ServicioJuego.jugadoresConectadosTableroCallback.Add("Jugador2", mockCallback2.Object);

            ServicioJuego.jugadoresConectadosTablero.Add("Pinku", "12345");
            ServicioJuego.jugadoresConectadosTablero.Add("Jugador2", "12345");

            ServicioJuego.turnosPorSala.Add("12345", new List<string> { "Pinku", "Jugador2" });

            ServicioJuego.administradoresDeSala.Add("12345", "Pinku");
        }

        [TestMethod]
        public void SolicitarExpulsion_ExpulsaJugadorDirectamente_SiEsAdministrador()
        {
            servicioJuego.SolicitarExpulsion("Pinku", "Jugador2", "12345");

            Assert.IsFalse(ServicioJuego.jugadoresConectadosTableroCallback.ContainsKey("Jugador2"), "El jugador no fue expulsado correctamente.");
            Assert.IsFalse(ServicioJuego.jugadoresConectadosTablero.ContainsKey("Jugador2"), "El jugador no fue removido correctamente del tablero.");
        }

        [TestMethod]
        public void SolicitarExpulsion_NoHaceNada_SiJugadorNoConectado()
        {
          
            servicioJuego.SolicitarExpulsion("Pinku", "JugadorDesconectado", "12345");

            Assert.IsFalse(ServicioJuego.votosExpulsion.ContainsKey("12345"), "No debería haberse iniciado una votación.");
            Assert.IsTrue(ServicioJuego.jugadoresConectadosTableroCallback.ContainsKey("Pinku"), "El solicitante debería permanecer conectado.");
        }

        [TestMethod]
        public void ExpulsarJugador_NotificaExpulsion()
        {
            var mockCallback = new Mock<IJuegoAdministradorCallback>();
            ServicioJuego.jugadoresConectadosTableroCallback["Jugador2"] = mockCallback.Object;

            servicioJuego.ExpulsarJugador("Jugador2", "12345");

            mockCallback.Verify(m => m.RecibirExpulsion(It.IsAny<string>()), Times.Once, "El jugador expulsado no recibió la notificación.");
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresConectadosTablero.Clear();
            ServicioJuego.turnosPorSala.Clear();
            ServicioJuego.votosExpulsion.Clear();
            ServicioJuego.administradoresDeSala.Clear();
        }
    }
}
