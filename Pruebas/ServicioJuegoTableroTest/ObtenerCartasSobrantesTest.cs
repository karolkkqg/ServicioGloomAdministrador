using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioJuegoTableroTest
{
    [TestClass]
    public class ObtenerCartasSobrantesTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            ServicioJuego.cartasSobrantesGlobal.Clear();
        }

        [TestMethod]
        public void ObtenerCartasSobrantesExitoso()
        {
            ServicioJuego.cartasSobrantesGlobal.Clear();

            ServicioJuego.cartasSobrantesGlobal.Add(new Carta { identificador = "Carta1", valor = 10 });
            ServicioJuego.cartasSobrantesGlobal.Add(new Carta { identificador = "Carta2", valor = 20 });

            List<Carta> resultado = servicioJuego.ObtenerCartasSobrantes();

            Assert.AreEqual(2, resultado.Count, "El número de cartas devuelto no es correcto.");
        }

        [TestMethod]
        public void ObtenerCartasSobrantesSinCartas()
        {

            List<Carta> resultado = servicioJuego.ObtenerCartasSobrantes();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
        }
    }
}

