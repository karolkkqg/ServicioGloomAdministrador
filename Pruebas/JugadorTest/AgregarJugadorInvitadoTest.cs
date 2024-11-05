using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.JugadorTest
{
    [TestClass]
    public class AgregarJugadorInvitadoTest
    {
        private ServicioGloomm.ServicioJuego juego = new ServicioGloomm.ServicioJuego();

        [TestMethod]
        public void AgregarUnInvitadoExitoso()
        {
            juego.AgregarJugadorInvitado();

            var jugador = juego.ObtenerJugadorInvitado("Invitado1");
            Assert.IsNotNull(jugador, "El jugador debería haber sido creado.");
            Assert.AreEqual("Jugador invitado anónimo", jugador.nombre, "El nombre del jugador es incorrecto.");
            Assert.AreEqual("Jugador invitado anónimo", jugador.apellidos, "Los apellidos del jugador son incorrectos.");
            Assert.AreEqual("sin correo", jugador.correo, "El correo del jugador es incorrecto.");
            Assert.AreEqual("sin contraseña", jugador.contraseña, "La contraseña del jugador es incorrecta.");
            Assert.AreEqual("Invitado", jugador.tipo, "El tipo de jugador es incorrecto.");
            Assert.AreEqual("Imagenes/PerfilUnicornio.png", jugador.icono, "El icono del jugador es incorrecto.");
        }

        [TestMethod]
        public void AgregarDosJugadoresInvitadosExitoso()
        {
            juego.AgregarJugadorInvitado(); 
            juego.AgregarJugadorInvitado();

            var jugador1 = juego.ObtenerJugadorInvitado("Invitado1");
            var jugador2 = juego.ObtenerJugadorInvitado("Invitado2");

            Assert.IsNotNull(jugador1, "El jugador 'Invitado1' debería haber sido creado.");
            Assert.IsNotNull(jugador2, "El jugador 'Invitado2' debería haber sido creado.");

            Assert.AreEqual("Jugador invitado anónimo", jugador1.nombre);
            Assert.AreEqual("Jugador invitado anónimo", jugador2.nombre);
            Assert.AreEqual("Invitado1", jugador1.nombreUsuario);
            Assert.AreEqual("Invitado2", jugador2.nombreUsuario);
        }

        [TestCleanup]
        public void TestCleanup()
        {
           juego.LimpiarListaNumeroJugadores();
          juego.LimpiarListaJugadoresInvitados();
        }
    }
}
