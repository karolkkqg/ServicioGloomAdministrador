using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.ServiceModel;

namespace Pruebas.JugadorTest
{
    [TestClass()]
    public class AutenticarJugadorTest
    {
        private AccesoDatos.Jugador jugador;
        private AccesoDatos.Jugador jugadorValido;

        [TestInitialize]
        public void TestInitialize()
        {
            jugador = new AccesoDatos.Jugador
            {
                NombreUsuario = "TacoDoradoDePato",
                Nombre = "Hector",
                Apellidos = "Juarez Castillo",
                Correo = "hectJuarPato@gmail.com",
                Contraseña = "123456",
                Tipo = "Registrado",
                Icono = "Icono1",
            };
            AccesoJugador.AgregarJugadorABaseDeDatos(jugador);
        }

        [TestMethod()]
        public void TestAutenticarUsuarioExitoso()
        {

            jugadorValido = new AccesoDatos.Jugador
            {
                NombreUsuario = "TacoDoradoDePato",
                Contraseña = "123456"
            };
            int jugadorEncontrado = AccesoJugador.ValidarJugadorParaAutenticacion(jugadorValido);
            Assert.AreEqual(1, jugadorEncontrado);
        }

        [TestMethod()]
        public void TestAutenticarUsuarioUsuarioNoEncontradoFallido()
        {
            jugador = new AccesoDatos.Jugador
            {
                NombreUsuario = "TacoDePapa",
                Contraseña = "123456"
            };

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoJugador.ValidarJugadorParaAutenticacion(jugador);
            });
            Assert.AreEqual("3", exception.Detail.codigo);
        }

        [TestMethod()]
        public void TestAutenticarUsuarioContrasenaIncorrectaFallido()
        {
            jugador.Contraseña = "12345600000"; 
            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoJugador.ValidarJugadorParaAutenticacion(jugador);
            });
            Assert.AreEqual("3", exception.Detail.codigo);
        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugador = contexto.Jugador
                    .FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");

                if (jugador != null)
                {
                    contexto.Jugador.Remove(jugador);
                    contexto.SaveChanges();
                }
            }
        }
    }
}
