using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System.Collections.Generic;

namespace Pruebas.ServicioCartaTest
{
    [TestClass]
    public class ObtenerMazoRestanteTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.cartasSobrantesGlobal.Clear();
        }

        [TestMethod]
        public void ObtenerMazoRestante_CartasDisponibles_DevuelveTrue()
        {

            var cartasEsperadas = new List<Carta>
            {
                new Carta { identificador = "Carta1.png", valor = 10, tipo = "modificador" },
                new Carta { identificador = "Carta2.png", valor = 20, tipo = "modificador" }
            };

            ServicioJuego.cartasSobrantesGlobal.AddRange(cartasEsperadas);

            var servicio = new ServicioJuego();


            var resultado = servicio.ObtenerMazoRestante();


            Assert.IsTrue(resultado, "El método debería devolver true porque hay cartas disponibles.");
        }

        [TestMethod]
        public void ObtenerMazoRestante_SinCartas_DevuelveFalse()
        {

            var servicio = new ServicioJuego();


            var resultado = servicio.ObtenerMazoRestante();


            Assert.IsFalse(resultado, "El método debería devolver false porque no hay cartas disponibles.");
        }

        [TestMethod]
        public void ObtenerMazoRestante_AgregarCartasDespues_DevuelveTrueCuandoSeAgreganCartas()
        {

            var cartasIniciales = new List<Carta>
            {
                new Carta { identificador = "Carta1.png", valor = 10, tipo = "modificador" }
            };

            var cartasAdicionales = new List<Carta>
            {
                new Carta { identificador = "Carta2.png", valor = 20, tipo = "modificador" },
                new Carta { identificador = "Carta3.png", valor = 30, tipo = "modificador" }
            };

            ServicioJuego.cartasSobrantesGlobal.AddRange(cartasIniciales);

            var servicio = new ServicioJuego();

            var resultadoInicial = servicio.ObtenerMazoRestante();

            Assert.IsTrue(resultadoInicial, "El método debería devolver true porque hay cartas disponibles.");

            ServicioJuego.cartasSobrantesGlobal.AddRange(cartasAdicionales);
            var resultadoActualizado = servicio.ObtenerMazoRestante();

            Assert.IsTrue(resultadoActualizado, "El método debería devolver true porque se agregaron más cartas.");
        }

        [TestMethod]
        public void ObtenerMazoRestante_EliminarTodasLasCartas_DevuelveFalse()
        {
            var cartasIniciales = new List<Carta>
            {
                new Carta { identificador = "Carta1.png", valor = 10, tipo = "modificador" }
            };

            ServicioJuego.cartasSobrantesGlobal.AddRange(cartasIniciales);

            var servicio = new ServicioJuego();

            var resultadoConCartas = servicio.ObtenerMazoRestante();
            ServicioJuego.cartasSobrantesGlobal.Clear();
            var resultadoSinCartas = servicio.ObtenerMazoRestante();

            Assert.IsTrue(resultadoConCartas, "El método debería devolver true porque había cartas al inicio.");
            Assert.IsFalse(resultadoSinCartas, "El método debería devolver false porque se eliminaron todas las cartas.");
        }
    }
}