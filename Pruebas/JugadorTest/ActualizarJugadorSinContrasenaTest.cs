using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.JugadorTest
{
    [TestClass]
    public class ActualizarJugadorSinCambiarContraseñaTest
    {
        private AccesoDatos.Jugador jugador;
        private AccesoDatos.Jugador jugadorValido;

        [TestInitialize]
        public void TestInitialize()
        {
            // Crear un jugador inicial en la base de datos
            jugador = new AccesoDatos.Jugador
            {
                NombreUsuario = "PatoHeroico",
                Nombre = "Carlos",
                Apellidos = "Sánchez",
                Correo = "carlos.pato@correo.com",
                Contraseña = "secreta123",
                Tipo = "Registrado",
                Icono = "Icono1",
            };
            AccesoJugador.AgregarJugadorABaseDeDatos(jugador);
        }

        [TestMethod]
        public void TestActualizarJugadorSinCambiarContraseña_Exitoso()
        {
            // Arrange: Actualizar propiedades excepto contraseña
            var jugadorModificado = new AccesoDatos.Jugador
            {
                NombreUsuario = "PatoHeroico",
                Nombre = "Carlos Modificado",
                Apellidos = "Sánchez Modificado",
                Correo = "carlos.modificado@correo.com",
                Contraseña = "nuevaContraseña123", // Este valor no debería afectar la base de datos
                Tipo = "Premium",
                Icono = "IconoNuevo",
            };

            // Act: Llamar al método para actualizar en la base de datos
            int filasAfectadas = AccesoJugador.ActualizarJugadorSinContrasenaABaseDeDatos(jugadorModificado);

            // Assert: Verificar que una fila fue afectada
            Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide");

            // Assert: Verificar en la base de datos
            using (var contexto = new EntidadesGloom())
            {
                var jugadorActualizado = contexto.Jugador
                    .FirstOrDefault(j => j.NombreUsuario == "PatoHeroico");

                Assert.IsNotNull(jugadorActualizado, "El jugador no existe en la base de datos");
                Assert.AreEqual("Carlos Modificado", jugadorActualizado.Nombre, "El nombre no se actualizó correctamente");
                Assert.AreEqual("Sánchez Modificado", jugadorActualizado.Apellidos, "Los apellidos no se actualizaron correctamente");
                Assert.AreEqual("carlos.modificado@correo.com", jugadorActualizado.Correo, "El correo no se actualizó correctamente");
                Assert.AreEqual("Premium", jugadorActualizado.Tipo, "El tipo no se actualizó correctamente");
                Assert.AreEqual("IconoNuevo", jugadorActualizado.Icono, "El icono no se actualizó correctamente");
                jugadorValido = new AccesoDatos.Jugador
                {
                    NombreUsuario = "PatoHeroico",
                    Contraseña = "secreta123"
                };
                int jugadorEncontrado = AccesoJugador.ValidarJugadorParaAutenticacion(jugadorValido);
                Assert.AreEqual(1, jugadorEncontrado);
            }
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugador = contexto.Jugador
                    .FirstOrDefault(j => j.NombreUsuario == "PatoHeroico");

                if (jugador != null)
                {
                    contexto.Jugador.Remove(jugador);
                    contexto.SaveChanges();
                }
            }
        }
    }
}
