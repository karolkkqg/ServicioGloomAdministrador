using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ObtenerSalasActivasConEstadoTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.salasActivasEnMemoria.Clear();
        }

        [TestMethod]
        public void ObtenerSalasActivasConEstado_SinSalasActivas_RetornaListaVacia()
        {
            var resultado = new ServicioJuego().ObtenerSalasActivasConEstado();

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(0, resultado.Count, "El resultado debería ser una lista vacía.");
        }

        [TestMethod]
        public void ObtenerSalasActivasConEstado_ConSalasActivas_RetornaSalasConEstadoActualizado()
        {
            string idSala1 = "Sala1";
            string idSala2 = "Sala2";

            ServicioJuego.salasActivasEnMemoria[idSala1] = new Sala
            {
                idSala = idSala1,
                nombreSala = "Primera Sala",
                noJugadores = 0
            };

            ServicioJuego.salasActivasEnMemoria[idSala2] = new Sala
            {
                idSala = idSala2,
                nombreSala = "Segunda Sala",
                noJugadores = 0
            };

            ServicioJuego.jugadoresEnSala[idSala1] = new HashSet<string> { "Jugador1", "Jugador2" };
            ServicioJuego.jugadoresEnSala[idSala2] = new HashSet<string> { "JugadorA" };

            var resultado = new ServicioJuego().ObtenerSalasActivasConEstado();

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(2, resultado.Count, "El resultado debería contener 2 salas activas.");

            var sala1 = resultado.Find(s => s.idSala == idSala1);
            Assert.IsNotNull(sala1, "La sala con ID Sala1 debería existir en el resultado.");
            Assert.AreEqual(2, sala1.noJugadores, "La cantidad de jugadores en Sala1 debería ser 2.");

            var sala2 = resultado.Find(s => s.idSala == idSala2);
            Assert.IsNotNull(sala2, "La sala con ID Sala2 debería existir en el resultado.");
            Assert.AreEqual(1, sala2.noJugadores, "La cantidad de jugadores en Sala2 debería ser 1.");
        }

        [TestMethod]
        public void ObtenerSalasActivasConEstado_SalaSinJugadores_RetornaSalasSinActualizarEstado()
        {
            string idSala = "Sala1";

            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                nombreSala = "Sala Sin Jugadores",
                noJugadores = 0
            };

            var resultado = new ServicioJuego().ObtenerSalasActivasConEstado();

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(1, resultado.Count, "El resultado debería contener 1 sala activa.");
            Assert.AreEqual(0, resultado[0].noJugadores, "La cantidad de jugadores en la sala debería permanecer como 0.");
        }
    }
}
