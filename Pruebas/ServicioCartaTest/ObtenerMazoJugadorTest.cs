using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioCartaTest
{
    [TestClass]
    public class ObtenerMazoJugadorTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            ServicioJuego.barajaJugadores.Clear();
        }

        [TestMethod]
        public void ObtenerMazoJugadorTestExitos()
        {
            var cartasJugador = new List<Carta>
        {
            new Carta { identificador = "Carta1.png", valor = 100 },
            new Carta { identificador = "Carta2.png", valor = 150 }
        };

            ServicioJuego.barajaJugadores["Jugador1"] = cartasJugador;
            List<Carta> mazoObtenido = servicioJuego.ObtenerMazoJugador("Jugador1");

            Assert.AreEqual(cartasJugador.Count, mazoObtenido.Count, "La cantidad de cartas no coincide.");
            Assert.AreEqual(cartasJugador[0].identificador, mazoObtenido[0].identificador, "La primera carta no coincide.");
            Assert.AreEqual(cartasJugador[1].identificador, mazoObtenido[1].identificador, "La segunda carta no coincide.");
        }

        [TestMethod]
        public void ObtenerMazoJugadorJugadorInexistenteTestExitoso()
        {
            List<Carta> mazoObtenido = servicioJuego.ObtenerMazoJugador("JugadorInexistente");

            Assert.IsNotNull(mazoObtenido, "El mazo devuelto es nulo.");
            Assert.AreEqual(0, mazoObtenido.Count, "El mazo devuelto no está vacío.");
        }
    }
}
