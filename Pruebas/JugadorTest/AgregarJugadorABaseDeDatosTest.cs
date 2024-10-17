using AccesoDatos;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloom.Tests
{
    [TestClass()]
    public class AgregarJugadorABaseDeDatosTest
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
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosExitoso()
        {

            try
            {
                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
            }
            catch (DbEntityValidationException ex)
            {
                Assert.Fail("Se produjeron errores de validación al insertar el jugador.");
            }

            using (var contexto = new EntidadesGloom())
            {
                var jugadorInsertado = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");

                Assert.IsNotNull(jugadorInsertado, "El jugador no fue encontrado en la base de datos");

                Assert.AreEqual("Hector", jugadorInsertado.Nombre, "El nombre del jugador no coincide");
                Assert.AreEqual("Juarez Castillo", jugadorInsertado.Apellidos, "Los apellidos del jugador no coinciden");
                Assert.AreEqual("hectJuarPato@gmail.com", jugadorInsertado.Correo, "El correo del jugador no coincide");
                Assert.AreEqual("Registrado", jugadorInsertado.Tipo, "El tipo de jugador no coincide");
                Assert.AreEqual("Icono1", jugadorInsertado.Icono, "El iconoc del jugador no coincide");
            }
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosFallidoCorreoRepetido()
        {
            AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
            var exception = Assert.ThrowsException<ManejadorExcepciones>(() =>
            {
                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
            });
            Assert.AreEqual("Este correo ya está registrado en el sistema.", exception.Message);
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosFallidoNombreRepetido()
        {
            var jugadorConCorreoUnico = new AccesoDatos.Jugador
            {
                NombreUsuario = "Jugador1",
                Nombre = "Nombre1",
                Apellidos = "Apellido1",
                Correo = "correo1@example.com",
                Contraseña = "Contraseña1",
                Tipo = "Tipo1",
                Icono = "Icono1"
            };

            AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugadorConCorreoUnico);

            var jugadorConNombreRepetido = new AccesoDatos.Jugador
            {
                NombreUsuario = "Jugador1",
                Nombre = "Nombre2",
                Apellidos = "Apellido2",
                Correo = "correo2@example.com",
                Contraseña = "Contraseña2",
                Tipo = "Tipo2",
                Icono = "Icono2"
            };

            var exception = Assert.ThrowsException<ManejadorExcepciones>(() =>
            {
                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugadorConNombreRepetido);
            });
            Assert.AreEqual("Este nombre de usuario ya está registrado en el sistema.", exception.Message);
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

