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
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
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
            }
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosFallidoNombreRepetido()
        {
            Assert.ThrowsException<DbUpdateException>(() => AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador));
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugador = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");
                if (jugador != null)
                {
                    contexto.Jugador.Remove(jugador);
                    contexto.SaveChanges();
                }
            }
        }
    }
}
