using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System.Collections.Generic;

namespace Pruebas.ServicioJuegoTableroTest
{
    [TestClass]
    public class ObtenerCartasSobrantesTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuego = new ServicioJuego();

            var cartasSobrantesField = typeof(ServicioJuego).GetField("cartasSobrantes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var cartasSobrantes = (List<Carta>)cartasSobrantesField.GetValue(null);
            cartasSobrantes.Clear();
        }

        [TestMethod]
        public void ObtenerCartasSobrantesTestExitoso()
        {
            var cartasSobrantesField = typeof(ServicioJuego).GetField("cartasSobrantes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var cartasSobrantes = (List<Carta>)cartasSobrantesField.GetValue(null);

            cartasSobrantes.Add(new Carta { identificador = "Carta1", valor = 10 });
            cartasSobrantes.Add(new Carta { identificador = "Carta2", valor = 20 });

            List<Carta> resultado = servicioJuego.ObtenerCartasSobrantes();

            Assert.AreEqual(2, resultado.Count, "El número de cartas en el resultado no es el esperado.");
            Assert.AreEqual("Carta1", resultado[0].identificador);
            Assert.AreEqual(10, resultado[0].valor);
            Assert.AreEqual("Carta2", resultado[1].identificador);
            Assert.AreEqual(20, resultado[1].valor);
        }

        [TestMethod]
        public void ObtenerCartasSobrantesTestFallido()
        {
            List<Carta> resultado = servicioJuego.ObtenerCartasSobrantes();

            Assert.AreEqual(0, resultado.Count, "Se esperaba una lista vacía, pero el resultado contiene elementos.");
        }
    }
}
