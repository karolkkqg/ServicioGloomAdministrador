using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioJuegoTableroTest
{
    [TestClass]
    public class EliminarJugadorDeJuegoTest
    {
        
        private ServicioJuego juego;

        [TestInitialize]
        public void SetUp()
        {
            juego = new ServicioJuego();

            ServicioJuego.jugadoresConectadosListos.Add("Jugador1", new List<string> { "DatosJugador1" });
            ServicioJuego.jugadoresConectadosListos.Add("Jugador2", new List<string> { "DatosJugador2" });
        }

        [TestMethod]
        public void EliminarJugadorExistente_Exitoso()
        {
            juego.EliminarJugadorDeJuego("Jugador1");

            Assert.IsFalse(ServicioJuego.jugadoresConectadosListos.ContainsKey("Jugador1"), "El jugador no fue eliminado correctamente.");
        }
        
    }
}
