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
    public class QuitarCartaDeMazoJugadorTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            ServicioJuego.barajaJugadores.Clear();
        }

        [TestMethod]
        public void QuitarCartaDeMazoJugadorCartaExistenteTest()
        {
            var cartasJugador = new List<Carta>
        {
            new Carta { identificador = "Carta1.png", valor = 100, tipo = "muerte" },
            new Carta { identificador = "Carta2.png", valor = 150, tipo = "modificador" }
        };
            ServicioJuego.barajaJugadores["Jugador1"] = cartasJugador;

            Assert.AreEqual(2, ServicioJuego.barajaJugadores["Jugador1"].Count, "El número inicial de cartas no es correcto.");

            var cartaAEliminar = new Carta { identificador = "Carta1.png", valor = 100, tipo = "muerte" };
            servicioJuego.QuitarCartaDeMazoJugador("Jugador1", cartaAEliminar);

            Assert.AreEqual(1, ServicioJuego.barajaJugadores["Jugador1"].Count, "La carta no fue eliminada correctamente.");
            Assert.AreEqual("Carta2.png", ServicioJuego.barajaJugadores["Jugador1"][0].identificador, "La carta restante no es la esperada.");
        }
    }
}