using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioCreacionPartidaTest
{
    [TestClass]
    public class ValidarCantidadJugadoresEnSalaTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            if (!ServicioJuego.salaJugadores.ContainsKey("SAL001"))
            {
                ServicioJuego.salaJugadores["SAL001"] = new List<string>();
            }

            ServicioJuego.salaJugadores["SAL001"].Add("Jugador1");
            ServicioJuego.salaJugadores["SAL001"].Add("Jugador2");
        }

        [TestMethod]
        public void ValidarCantidadJugadoresEnSalaLanzaExcepcionCuandoCantidadCoincide()
        {
            ServicioJuego.salaJugadores.Clear();
            ServicioJuego.salaJugadores["SAL001"] = new List<string> { "Jugador1", "Jugador2" };

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.ValidarCantidadJugadoresEnSala("SAL001", 2);
            });

            Assert.AreEqual("34", excepcion.Detail.codigo, "El mensaje de error no coincide con el esperado.");
        }

        [TestMethod]
        public void ValidarCantidadJugadoresEnSalaNoLanzaExcepcionCuandoCantidadNoCoincide()
        {
            servicioJuego.ValidarCantidadJugadoresEnSala("SAL001", 1);
            ServicioJuego.salaJugadores.Clear();
        }
    }
}
