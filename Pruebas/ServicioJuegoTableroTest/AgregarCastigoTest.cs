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
    public class AgregarCastigoTest
    {
        [TestInitialize]
        public void SetUp()
        {
            ServicioJuego.jugadoresConCastigos.Clear();
        }

        [TestMethod]
        public void AgregarCastigoAJugadorExistente()
        {
            var servicioJuego = new ServicioJuego();
            ServicioJuego.jugadoresConCastigos.Add("Jugador1", 2);

            servicioJuego.AgregarCastigo("Jugador1");

            Assert.AreEqual(3, ServicioJuego.jugadoresConCastigos["Jugador1"], "El castigo no se incrementó correctamente para el jugador existente.");
        }

        [TestMethod]
        public void AgregarCastigoANuevoJugador()
        {
            var servicioJuego = new ServicioJuego();

            servicioJuego.AgregarCastigo("JugadorNuevo");

            Assert.IsTrue(ServicioJuego.jugadoresConCastigos.ContainsKey("JugadorNuevo"), "El nuevo jugador no fue añadido correctamente.");
            Assert.AreEqual(1, ServicioJuego.jugadoresConCastigos["JugadorNuevo"], "El castigo inicial para el nuevo jugador no es correcto.");
        }
    }
}
