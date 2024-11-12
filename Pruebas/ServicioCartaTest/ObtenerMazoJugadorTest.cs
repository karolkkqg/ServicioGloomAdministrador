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
            typeof(ServicioJuego).GetField("barajaJugadores", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, new Dictionary<string, List<Carta>>());
        }

        [TestMethod]
        public void ObtenerMazoJugadorTestExitos()
        {
            var cartasJugador = new List<Carta>
        {
            new Carta { identificador = "Carta1.png", valor = 100 },
            new Carta { identificador = "Carta2.png", valor = 150 }
        };

            var barajaJugadores = (Dictionary<string, List<Carta>>)typeof(ServicioJuego)
                .GetField("barajaJugadores", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                .GetValue(null);

            barajaJugadores["Jugador1"] = cartasJugador;

            List<Carta> mazoObtenido = servicioJuego.ObtenerMazoJugador("Jugador1");
            Assert.AreEqual(cartasJugador.Count, mazoObtenido.Count);
            Assert.AreEqual(cartasJugador[0].identificador, mazoObtenido[0].identificador);
            Assert.AreEqual(cartasJugador[1].identificador, mazoObtenido[1].identificador);
        }

        [TestMethod]
        public void ObtenerMazoJugadorJugadorInexistenteTestExitoso()
        {
            List<Carta> mazoObtenido = servicioJuego.ObtenerMazoJugador("JugadorInexistente");
            Assert.IsNotNull(mazoObtenido);
            Assert.AreEqual(0, mazoObtenido.Count);
        }
    }
}
