using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ObtenerFamiliaSeleccionadaTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
        }

        [TestMethod]
        public void ObtenerFamiliaSeleccionada_SalaConFamiliasDevuelveLista()
        {
            string idSala = "Sala1";
            List<string> familiasEsperadas = new List<string> { "Familia1", "Familia2" };

            ServicioJuego.familiasSeleccionadasPorSala[idSala] = new HashSet<string>(familiasEsperadas);

            var servicio = new ServicioJuego();

            var resultado = servicio.ObtenerFamiliaSeleccionada(idSala);

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            CollectionAssert.AreEqual(familiasEsperadas, resultado, "Las familias obtenidas no coinciden con las esperadas.");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ObtenerFamiliaSeleccionada_SalaInexistente_LanzaExcepcion()
        {
            string idSalaInexistente = "SalaInexistente";

            var servicio = new ServicioJuego();

            servicio.ObtenerFamiliaSeleccionada(idSalaInexistente);

        }

        [TestMethod]
        public void ObtenerFamiliaSeleccionada_SalaSinFamiliasDevuelveListaVacia()
        {
            string idSala = "Sala2";

            ServicioJuego.familiasSeleccionadasPorSala[idSala] = new HashSet<string>();

            var servicio = new ServicioJuego();

            var resultado = servicio.ObtenerFamiliaSeleccionada(idSala);

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(0, resultado.Count, "La lista de familias debería estar vacía.");
        }
    }
}
