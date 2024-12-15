using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System.Collections.Generic;

namespace Pruebas.ServicioJuegoTableroTests
{
    [TestClass]
    public class ObtenerJugadoresVivosTests
    {
        ServicioJuego servicioJuego;


        [TestInitialize]
        public void TestInitialize()
        {
            string numeroSala = "Sala1";
            servicioJuego = new ServicioJuego();
            ServicioJuego.jugadoresVivos[numeroSala] = new List<string>();
        }

        [TestMethod]
        public void ObtenerJugadoresVivos_SalaExiste_DevuelveListaJugadores()
        {
            string numeroSala = "Sala1";
            List<string> jugadoresEsperados = new List<string> { "Jugador1", "Jugador2", "Jugador3" };

            ServicioJuego.jugadoresVivos[numeroSala] = new List<string>(jugadoresEsperados);

            var resultado = servicioJuego.ObtenerJugadoresVivos(numeroSala);

            CollectionAssert.AreEqual(jugadoresEsperados, resultado);
        }


        [TestMethod]
        public void ObtenerJugadoresVivos_SalaConJugadoresVacios_DevuelveListaVacia()
        {
            string numeroSala = "SalaVacia";
            ServicioJuego.jugadoresVivos[numeroSala] = new List<string>();

            var resultado = servicioJuego.ObtenerJugadoresVivos(numeroSala);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
        }

    }
}
