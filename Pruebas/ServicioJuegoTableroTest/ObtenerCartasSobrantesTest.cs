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

            var cartasSobrantesField = typeof(ServicioJuego).GetField("cartasSobrantes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var cartasSobrantes = (List<Carta>)cartasSobrantesField.GetValue(null);
            cartasSobrantes.Clear();
        }

        [TestMethod]
        public void ObtenerCartasSobrantesExitoso()
        {
            ServicioJuego.cartasSobrantes.Clear();
            ServicioJuego.cartasSobrantes.AddRange(new List<Carta>
            {
                new Carta { identificador = "Carta1.png", valor = 10 },
                new Carta { identificador = "Carta2.png", valor = 20 }
            });

            cartasSobrantes.Add(new Carta { identificador = "Carta1", valor = 10 });
            cartasSobrantes.Add(new Carta { identificador = "Carta2", valor = 20 });

            List<Carta> resultado = servicioJuego.ObtenerCartasSobrantes();

            Assert.AreEqual(2, resultado.Count, "El número de cartas devuelto no es correcto.");
            Assert.AreEqual("Carta1.png", resultado[0].identificador);
            Assert.AreEqual(10, resultado[0].valor);
            Assert.AreEqual("Carta2.png", resultado[1].identificador);
            Assert.AreEqual(20, resultado[1].valor);
            ServicioJuego.cartasSobrantes.Clear();
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

