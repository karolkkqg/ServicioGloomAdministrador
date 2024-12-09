using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class SalirDeSalaTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // Limpiar y reinicializar las propiedades estáticas antes de cada prueba
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.salasActivasEnMemoria.Clear();
        }

        [TestMethod]
        public void SalirDeSala_UsuarioSaleSalaConMasJugadores()
        {
            // Arrange
            string idSala = "Sala1";
            string idUsuario1 = "Usuario1";
            string idUsuario2 = "Usuario2";

            ServicioJuego.jugadoresEnSala[idSala] = new HashSet<string> { idUsuario1, idUsuario2 };
            ServicioJuego.salasActivasEnMemoria[idSala] = new BibliotecaClases.Sala
            {
                idSala = idSala,
                nombreSala = "Sala Test",
                noJugadores = 2
            };

            var servicio = new ServicioJuego();

            // Act
            //servicio.SalirDeSala(idSala, idUsuario1);

            // Assert
            Assert.IsTrue(ServicioJuego.jugadoresEnSala.ContainsKey(idSala), "La sala debería seguir activa.");
            Assert.AreEqual(1, ServicioJuego.jugadoresEnSala[idSala].Count, "La sala debería contener un jugador.");
            Assert.IsFalse(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario1), "El usuario 1 debería haber salido de la sala.");
        }

        [TestMethod]
        public void SalirDeSala_UsuarioSaleYEliminaSala()
        {
            // Arrange
            string idSala = "Sala1";
            string idUsuario = "Usuario1";

            ServicioJuego.jugadoresEnSala[idSala] = new HashSet<string> { idUsuario };
            ServicioJuego.salasActivasEnMemoria[idSala] = new BibliotecaClases.Sala
            {
                idSala = idSala,
                nombreSala = "Sala Test",
                noJugadores = 1
            };

            var servicio = new ServicioJuego();

            // Act
            //servicio.SalirDeSala(idSala, idUsuario);

            // Assert
            Assert.IsFalse(ServicioJuego.jugadoresEnSala.ContainsKey(idSala), "La sala debería haber sido eliminada.");
            Assert.IsFalse(ServicioJuego.salasActivasEnMemoria.ContainsKey(idSala), "La sala activa debería haber sido eliminada de la memoria.");
        }

        [TestMethod]
        public void SalirDeSala_UsuarioNoExiste_NoCambiaEstado()
        {
            // Arrange
            string idSala = "Sala1";
            string idUsuarioExistente = "Usuario1";
            string idUsuarioInexistente = "UsuarioNoExistente";

            ServicioJuego.jugadoresEnSala[idSala] = new HashSet<string> { idUsuarioExistente };
            ServicioJuego.salasActivasEnMemoria[idSala] = new BibliotecaClases.Sala
            {
                idSala = idSala,
                nombreSala = "Sala Test",
                noJugadores = 1
            };

            var servicio = new ServicioJuego();

            // Act
            //servicio.SalirDeSala(idSala, idUsuarioInexistente);

            // Assert
            Assert.IsTrue(ServicioJuego.jugadoresEnSala.ContainsKey(idSala), "La sala debería seguir activa.");
            Assert.AreEqual(1, ServicioJuego.jugadoresEnSala[idSala].Count, "La cantidad de jugadores debería permanecer sin cambios.");
            Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuarioExistente), "El usuario existente debería seguir en la sala.");
        }

        [TestMethod]
        public void SalirDeSala_SalaNoExiste_NoGeneraErrores()
        {
            // Arrange
            string idSalaInexistente = "SalaInexistente";
            string idUsuario = "Usuario1";

            var servicio = new ServicioJuego();

            // Act
            //servicio.SalirDeSala(idSalaInexistente, idUsuario);

            // Assert
            Assert.IsFalse(ServicioJuego.jugadoresEnSala.ContainsKey(idSalaInexistente), "No debería crearse una sala inexistente.");
            Assert.IsFalse(ServicioJuego.salasActivasEnMemoria.ContainsKey(idSalaInexistente), "No debería existir una sala en memoria.");
        }
    }
}
