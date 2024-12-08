using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGlomm;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioCartaTest
{
    [TestClass]
    public class QuitarCartaDeMazoJugadorExternoTest
    {
        private ServicioJuego servicioJuego;
        private Mock<IJuegoAdministradorCallback> mockCallback;
        private Mock<AdministradorLogger> mockLogger;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            mockCallback = new Mock<IJuegoAdministradorCallback>();
            mockLogger = new Mock<AdministradorLogger>(typeof(ServicioJuego));

            ServicioJuego.barajaJugadores["Jugador1"] = new List<Carta>();
            ServicioJuego.barajaJugadores["Jugador1"].Add(new Carta { identificador = "1", tipo= "Carta1" });
            ServicioJuego.barajaJugadores["Jugador1"].Add(new Carta {identificador = "2", tipo = "Carta2" });
            ServicioJuego.barajaJugadores["Jugador1"].Add(new Carta {identificador = "3", tipo = "Carta3" });

            ServicioJuego.jugadoresConectadosTableroCallback["Jugador1"] = mockCallback.Object;
        }

        [TestMethod]
        public void QuitarCartaDeMazoJugadorExterno_CartaRemovidaYCallbackEjecutado()
        {
            string nombreUsuario = "Jugador1";

            servicioJuego.QuitarCartaDeMazoJugadorExterno(nombreUsuario);
            Assert.AreEqual(2, ServicioJuego.barajaJugadores[nombreUsuario].Count, "No se eliminó una carta del mazo.");
        }

        [TestMethod]
        public void QuitarCartaDeMazoJugadorExterno_SinCartasNoHaceNada()
        {
            string nombreUsuario = "Jugador1";
            ServicioJuego.barajaJugadores[nombreUsuario].Clear();

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
                servicioJuego.QuitarCartaDeMazoJugadorExterno(nombreUsuario)
            );

            Assert.AreEqual("38", exception.Detail.codigo, "El mensaje de la excepción no coincide.");
        }

        [TestMethod]
        public void QuitarCartaDeMazoJugadorExternoJugadorNoExisteNoHaceNada()
        {
            string nombreUsuario = "JugadorInexistente";
            servicioJuego.QuitarCartaDeMazoJugadorExterno(nombreUsuario);
            Assert.IsFalse(ServicioJuego.barajaJugadores.ContainsKey(nombreUsuario), "El jugador no debería existir en el mazo.");
        }
    }
}
