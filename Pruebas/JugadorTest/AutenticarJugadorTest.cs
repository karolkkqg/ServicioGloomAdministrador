using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
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
           Jugador jugadorEncontrado = AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(jugador);   
           Assert.IsNotNull(jugadorEncontrado, "El jugador no fue encontrado en la base de datos");
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestAuteticarUsuarioCorreoNoenonctradoFallido()
        {
            jugador.NombreUsuario = "TacoDoradoDePato";
            jugador.Nombre = "Hector";
            jugador.Apellidos = "Juarez Castillo";
            jugador.Contraseña = "123456";
            jugador.Tipo = "Registrado";
            jugador.Icono = "Icono1";
            jugador.Correo = "usuarioNoRegistrado@hotmail.com";

            var exception = Assert.ThrowsException<ManejadorExcepciones>(() =>
            {
                AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(jugador);
            });
            Assert.AreEqual("El jugador no fue encontrado, verifique su correo o contraseña.", exception.Message);

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

            var exception = Assert.ThrowsException<ManejadorExcepciones>(() =>
            {
                AccesoBaseDeDatos.ValidarJugadorParaAutenticacion(jugador);
            });
            Assert.AreEqual("El jugador no fue encontrado, verifique su correo o contraseña.", exception.Message);

            LimpiarDatosDePrueba();
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugadores = contexto.Jugador.ToList();
                foreach (var jugador in jugadores)
                {
                    contexto.Jugador.Remove(jugador);
                }
                contexto.SaveChanges();
            }
        }
    }
}
