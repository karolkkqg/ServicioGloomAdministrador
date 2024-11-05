using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ObtenerUsuariosYPersonajesTest
    {
        
        private ServicioGloomm.ServicioJuego servicioSala;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioSala = new ServicioGloomm.ServicioJuego();
            LimpiarDatos();
        }

        [TestMethod]
        public void ObtenerUsuariosYPersonajes_DiccionarioVacioSinPersonajesSeleccionados()
        {
            // Act
            var personajes = servicioSala.ObtenerUsuariosYPersonajes();

            // Assert
            Assert.AreEqual(0, personajes.Count, "El diccionario debería estar vacío al inicio.");
        }

        [TestMethod]
        public void ObtenerUsuariosYPersonajes_ContieneUsuariosYPersonajesCorrectos()
        {
            // Arrange
            servicioSala.SeleccionarPersonaje("Usuario1", "Personaje1", 100);
            servicioSala.SeleccionarPersonaje("Usuario2", "Personaje2", 80);

            // Act
            var personajes = servicioSala.ObtenerUsuariosYPersonajes();

            // Assert
            Assert.AreEqual(2, personajes.Count, "El diccionario debería contener dos entradas.");
            Assert.AreEqual(("Personaje1", 100), personajes["Usuario1"], "El personaje y vida de Usuario1 deberían coincidir.");
            Assert.AreEqual(("Personaje2", 80), personajes["Usuario2"], "El personaje y vida de Usuario2 deberían coincidir.");
        }

        [TestMethod]
        public void ObtenerUsuariosYPersonajes_DevuelveUnaCopiaNoReferencia()
        {
            // Arrange
            servicioSala.SeleccionarPersonaje("Usuario1", "Personaje1", 100);

            // Act
            var personajes = servicioSala.ObtenerUsuariosYPersonajes();

            // Modificación directa en el diccionario devuelto
            personajes["Usuario1"] = ("PersonajeModificado", 50);

            // Assert
            var personajesOriginal = servicioSala.ObtenerUsuariosYPersonajes();
            Assert.AreEqual(("Personaje1", 100), personajesOriginal["Usuario1"], "El diccionario original no debería verse afectado por cambios en la copia devuelta.");
        }

        public void LimpiarDatos()
        {
            servicioSala.LimpiarListaJugadores();
            servicioSala.LimpiarListaPersonajes();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LimpiarDatos();
        }
    }
}
