using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

namespace Pruebas.ServicioCreacionPartidaTest
{
    [TestClass]
    public class ObtenerFamiliasYPersonajesTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // Limpiar y reinicializar los diccionarios estáticos antes de cada prueba
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
            ServicioJuego.familias = new Dictionary<string, List<(string nombrePersonaje, int vida)>>
        {
            {"Familia1", new List<(string, int)> { ("Personaje1", 100), ("Personaje2", 100), ("Personaje3", 100), ("Personaje4", 100) }},
            {"Familia2", new List<(string, int)> { ("PersonajeA", 90), ("PersonajeB", 90), ("PersonajeC", 90), ("PersonajeD", 90) }}
        };
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void ObtenerFamiliasYPersonajes_SalaNoExistente_LanzaExcepcion()
        {
            // Arrange
            string numeroSala = "1";

            // Act
            var resultado = new ServicioJuego().ObtenerFamiliasYPersonajes(numeroSala);

            // Assert (se maneja con el atributo ExpectedException)
        }

        [TestMethod]
        public void ObtenerFamiliasYPersonajes_SalaConFamiliasValidas_RetornaDatos()
        {
            // Arrange
            string numeroSala = "1";
            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { "Jugador1", "Jugador2" };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string> { "Familia1", "Familia2" };

            // Act
            var resultado = new ServicioJuego().ObtenerFamiliasYPersonajes(numeroSala);

            // Assert
            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(2, resultado.Count, "El resultado debería contener datos de 2 familias.");
            Assert.IsTrue(resultado.ContainsKey("Familia1"), "El resultado debería incluir 'Familia1'.");
            Assert.IsTrue(resultado.ContainsKey("Familia2"), "El resultado debería incluir 'Familia2'.");
            CollectionAssert.AreEqual(
                ServicioJuego.familias["Familia1"],
                resultado["Familia1"].ToList(),
                "Los personajes de 'Familia1' no coinciden con los esperados."
            );
            CollectionAssert.AreEqual(
                ServicioJuego.familias["Familia2"],
                resultado["Familia2"].ToList(),
                "Los personajes de 'Familia2' no coinciden con los esperados."
            );
        }

        [TestMethod]
        public void ObtenerFamiliasYPersonajes_FamiliaNoSeleccionada_RetornaVacio()
        {
            // Arrange
            string numeroSala = "1";
            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { "Jugador1" };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string>(); // Sin familias seleccionadas

            // Act
            var resultado = new ServicioJuego().ObtenerFamiliasYPersonajes(numeroSala);

            // Assert
            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(0, resultado.Count, "El resultado debería estar vacío si no hay familias seleccionadas.");
        }

        [TestMethod]
        public void ObtenerFamiliasYPersonajes_FamiliaSeleccionadaNoValida_RetornaVacio()
        {
            // Arrange
            string numeroSala = "1";
            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { "Jugador1" };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string> { "FamiliaInexistente" }; // Familia no válida

            // Act
            var resultado = new ServicioJuego().ObtenerFamiliasYPersonajes(numeroSala);

            // Assert
            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(0, resultado.Count, "El resultado debería estar vacío si las familias seleccionadas no son válidas.");
        }
    }
}
