using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class ObtenerCartasBonusTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.cartasbonus.Clear();

            ServicioJuego.cartasbonus.Add(new Carta { identificador = "Bonus1", valor = 0, tipo = "saltarJugador" });
            ServicioJuego.cartasbonus.Add(new Carta { identificador = "Bonus2", valor = 0, tipo = "robar2Cartas" });
        }

        [TestMethod]
        public void ObtenerCartasBonusExitoso()
        {
            var carta = servicioJuego.ObtenerCartasBonus();

            Assert.AreEqual("Bonus1", carta.identificador, "La carta devuelta no es la esperada.");
            Assert.AreEqual(1, ServicioJuego.cartasbonus.Count, "La carta no fue removida correctamente del mazo de cartas bonus.");
        }

        [TestMethod]
        public void ObtenerCartasBonusExitosoSegundaCarta()
        {
            servicioJuego.ObtenerCartasBonus();
            var carta = servicioJuego.ObtenerCartasBonus();

            Assert.AreEqual("Bonus2", carta.identificador, "La segunda carta devuelta no es la esperada.");
            Assert.AreEqual(0, ServicioJuego.cartasbonus.Count, "La segunda carta no fue removida correctamente del mazo de cartas bonus.");
        }

        [TestMethod]
        public void ObtenerCartasBonusFallaPorCartasAgotadas()
        {
            ServicioJuego.cartasbonus.Clear();

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.ObtenerCartasBonus();
            });

            Assert.AreEqual("21", excepcion.Detail.mensaje, "El mensaje de error no coincide con el esperado.");
        }
    }
}
