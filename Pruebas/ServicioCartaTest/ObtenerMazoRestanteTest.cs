using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;

namespace Pruebas.ServicioCartaTest
{
    [TestClass]
    public class ObtenerMazoRestanteTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // Limpiar las propiedades estáticas antes de cada prueba
            ServicioJuego.cartasSobrantesGlobal.Clear();
        }

        [TestMethod]
        public void ObtenerMazoRestante_CartasDisponibles_DevuelveListaCorrecta()
        {
            // Arrange
            var cartasEsperadas = new List<Carta>
        {
            new Carta { identificador = "Carta1.png", valor = 10, tipo = "modificador" },
            new Carta { identificador = "Carta2.png", valor = 20, tipo = "modificador" }
        };

            ServicioJuego.cartasSobrantesGlobal.AddRange(cartasEsperadas);

            var servicio = new ServicioJuego();

            // Act
            var resultado = servicio.ObtenerMazoRestante();

            // Assert
            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(cartasEsperadas.Count, resultado.Count, "El número de cartas no coincide.");
            CollectionAssert.AreEqual(cartasEsperadas, resultado, "Las cartas devueltas no coinciden con las esperadas.");
        }

        [TestMethod]
        public void ObtenerMazoRestante_SinCartas_DevuelveListaVacia()
        {
            // Arrange
            var servicio = new ServicioJuego();

            // Act
            var resultado = servicio.ObtenerMazoRestante();

            // Assert
            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(0, resultado.Count, "El número de cartas debería ser 0.");
        }

        [TestMethod]
        public void ObtenerMazoRestante_AgregarCartasDespues_DevuelveListaActualizada()
        {
            // Arrange
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

            // Act - Antes de agregar cartas adicionales
            var resultadoInicial = servicio.ObtenerMazoRestante();

            // Assert - Antes de agregar cartas adicionales
            Assert.AreEqual(cartasIniciales.Count, resultadoInicial.Count, "El número de cartas inicial no coincide.");

            // Act - Después de agregar cartas adicionales
            ServicioJuego.cartasSobrantesGlobal.AddRange(cartasAdicionales);
            var resultadoActualizado = servicio.ObtenerMazoRestante();

            // Assert - Después de agregar cartas adicionales
            Assert.AreEqual(cartasIniciales.Count + cartasAdicionales.Count, resultadoActualizado.Count, "El número de cartas actualizado no coincide.");
        }
    }
}
