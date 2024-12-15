using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Pruebas.ServicioJuegoTest
{
    [TestClass]
    public class ObtenerFamiliasTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
            ServicioJuego.familias.Clear();

            ServicioJuego.jugadoresEnSala["Sala1"] = new HashSet<string> { "Jugador1", "Jugador2" };
            ServicioJuego.familiasSeleccionadasPorSala["Sala1"] = new HashSet<string> { "FamiliaA", "FamiliaB" };
            ServicioJuego.familias = new Dictionary<string, List<(string nombrePersonaje, int vida)>>
            {
                { "FamiliaA", new List<(string, int)> { ("Personaje1", 100), ("Personaje2", 90) } },
                { "FamiliaB", new List<(string, int)> { ("Personaje3", 80), ("Personaje4", 70) } }
            };
        }

        [TestMethod]
        public void ObtenerFamiliasSeleccionadasPorSala_DevuelveDatosCorrectos()
        {
            var servicio = new ServicioJuego();

            var resultado = servicio.ObtenerFamiliasSeleccionadasPorSala();

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(1, resultado.Count, "Debería haber una sala registrada.");
            Assert.IsTrue(resultado.ContainsKey("Sala1"), "El resultado debería incluir 'Sala1'.");
            CollectionAssert.AreEqual(
                new List<string> { "FamiliaA", "FamiliaB" },
                resultado["Sala1"].ToList(),
                "Las familias seleccionadas no coinciden con las esperadas."
            );
        }

        [TestMethod]
        public void ObtenerFamiliasYPersonajes_SalaConFamilias_RetornaDatosCorrectos()
        {
            var servicio = new ServicioJuego();

            var resultado = servicio.ObtenerFamiliasYPersonajes("Sala1");

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(2, resultado.Count, "Debería haber datos de 2 familias.");
            Assert.IsTrue(resultado.ContainsKey("FamiliaA"), "El resultado debería incluir 'FamiliaA'.");
            Assert.IsTrue(resultado.ContainsKey("FamiliaB"), "El resultado debería incluir 'FamiliaB'.");
            CollectionAssert.AreEqual(
                new List<(string, int)> { ("Personaje1", 100), ("Personaje2", 90) },
                resultado["FamiliaA"],
                "Los personajes de 'FamiliaA' no coinciden con los esperados."
            );
            CollectionAssert.AreEqual(
                new List<(string, int)> { ("Personaje3", 80), ("Personaje4", 70) },
                resultado["FamiliaB"],
                "Los personajes de 'FamiliaB' no coinciden con los esperados."
            );
        }

        [TestMethod]
        public void ObtenerFamiliasYPersonajes_SalaNoExistente_LanzaExcepcion()
        {
            var servicio = new ServicioJuego();

            Assert.ThrowsException<FaultException<ManejadorExcepciones>>(
                () => servicio.ObtenerFamiliasYPersonajes("SalaInvalida"),
                "Debería lanzarse una excepción para una sala no existente."
            );
        }
    }
}
