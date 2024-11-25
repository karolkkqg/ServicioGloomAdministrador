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
    public class EliminarJugadorDeJuegoTest
    {
        [TestInitialize]
        public void SetUp()
        {
            ServicioJuego.jugadoresConectadosListos.Clear();
            ServicioJuego.jugadoresConectadosListos.Add("Jugador1", "Sala1");
            ServicioJuego.jugadoresConectadosListos.Add("Jugador2", "Sala2");
            ServicioJuego.jugadoresConectadosListos.Add("Jugador3", "Sala3");
        }

        [TestMethod]
        public void EliminarJugadorExistenteDeJuego()
        {
            var servicioJuego = new ServicioJuego();
            servicioJuego.EliminarJugadorDeJuego("Jugador2");

            Assert.IsFalse(ServicioJuego.jugadoresConectadosListos.ContainsKey("Jugador2"), "El jugador no fue eliminado correctamente.");
            Assert.AreEqual(2, ServicioJuego.jugadoresConectadosListos.Count, "La cantidad de jugadores no es correcta.");
        }

        [TestMethod]
        public void EliminarJugadorInexistenteDeJuego()
        {
            var servicioJuego = new ServicioJuego();
            servicioJuego.EliminarJugadorDeJuego("JugadorInexistente");

            Assert.AreEqual(3, ServicioJuego.jugadoresConectadosListos.Count, "No debería haberse eliminado ningún jugador.");
        }
    }
}

