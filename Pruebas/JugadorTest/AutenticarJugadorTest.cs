using AccesoDatos;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.JugadorTest
{
    [TestClass()]
    public class AutenticarJugadorTest
    {
        private AccesoDatos.Jugador jugador;

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
            AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
        }

        [TestMethod()]
        public void TestAuteticarUsuarioExitoso()
        {
           int jugadorEncontrado = AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(jugador);
            Assert.AreEqual(1, jugadorEncontrado);
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestAuteticarUsuarioUsuarioNoenonctradoFallido()
        {
            jugador.NombreUsuario = "TacoDoradoDePapa";
            jugador.Nombre = "Hector";
            jugador.Apellidos = "Juarez Castillo";
            jugador.Contraseña = "123456";
            jugador.Tipo = "Registrado";
            jugador.Icono = "Icono1";
            jugador.Correo = "usuarioNoRegistrado@hotmail.com";

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(jugador);
            });

            Assert.AreEqual("3", exception.Detail.mensaje);

            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestAuteticarUsuarioContraseñaIncorrectaFallido()
        {
            jugador.NombreUsuario = "TacoDoradoDePato";
            jugador.Nombre = "Hector";
            jugador.Apellidos = "Juarez Castillo";
            jugador.Contraseña = "12345600000";
            jugador.Tipo = "Registrado";
            jugador.Icono = "Icono1";
            jugador.Correo = "hectJuarPato@gmail.com";

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(jugador);
            });
            Assert.AreEqual("3", exception.Detail.mensaje);

            LimpiarDatosDePrueba();
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
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
