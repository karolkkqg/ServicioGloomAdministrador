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
    public class ActualizarJugadorABaseDeDatosTest
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
        public void TestActualizarJugadorABaseDeDatosMismaInformacionExitoso()
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
                int filasAfectadas = AccesoBaseDeDatos.ActualizarJugadorABaseDeDatos(jugador);

            using (var contexto = new EntidadesGloom())
            {
                Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide");
                /*
                var jugadorInsertado = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");

                Assert.IsNotNull(jugadorInsertado, "El jugador no fue encontrado en la base de datos");

                Assert.AreEqual("Hector", jugadorInsertado.Nombre, "El nombre del jugador no coincide");
                Assert.AreEqual("Juarez Castillo", jugadorInsertado.Apellidos, "Los apellidos del jugador no coinciden");
                Assert.AreEqual("hectJuarPato@gmail.com", jugadorInsertado.Correo, "El correo del jugador no coincide");
                Assert.AreEqual("Registrado", jugadorInsertado.Tipo, "El tipo de jugador no coincide");
                Assert.AreEqual("Icono1", jugadorInsertado.Icono, "El iconoc del jugador no coincide");
                */
            }
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestActualizarJugadorABaseDeDatosDiferenteInformacionExitoso()
        {
           
                jugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = "TacoDoradoDePato",
                    Nombre = "Gloria",
                    Apellidos = "Trevi",
                    Correo = "gloriaADIos@hotmail.com",
                    Contraseña = "123456",
                    Tipo = "Registrado",
                    Icono = "Icono3",
                };
                int filasAfectadas = AccesoBaseDeDatos.ActualizarJugadorABaseDeDatos(jugador);

            Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide");
            /*
            using (var contexto = new EntidadesGloom())
            {
                var jugadorInsertado = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");

                Assert.IsNotNull(jugadorInsertado, "El jugador no fue encontrado en la base de datos");

                Assert.AreEqual("Gloria", jugadorInsertado.Nombre, "El nombre del jugador no coincide");
                Assert.AreEqual("Trevi", jugadorInsertado.Apellidos, "Los apellidos del jugador no coinciden");
                Assert.AreEqual("gloriaADIos@hotmail.com", jugadorInsertado.Correo, "El correo del jugador no coincide");
                Assert.AreEqual("Registrado", jugadorInsertado.Tipo, "El tipo de jugador no coincide");
                Assert.AreEqual("Icono3", jugadorInsertado.Icono, "El iconoc del jugador no coincide");
            }
            */
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
