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
            var personajes = servicioSala.ObtenerUsuariosYPersonajes();
            Assert.AreEqual(0, personajes.Count, "El diccionario debería estar vacío al inicio.");
        }

        [TestMethod]
        public void ObtenerUsuariosYPersonajes_ContieneUsuariosYPersonajesCorrectos()
        {
            ServicioGloomm.ServicioJuego.personajesPorUsuario.Add("Usuario1", ("Personaje1", 100));
            ServicioGloomm.ServicioJuego.personajesPorUsuario.Add("Usuario2", ("Personaje2", 80));
            var personajes = servicioSala.ObtenerUsuariosYPersonajes();

            Assert.AreEqual(2, personajes.Count, "El diccionario debería contener dos entradas.");
        }

        public void LimpiarDatos()
        {
            servicioSala.LimpiarListaPersonajes();
            servicioSala.LimpiarListaJugadores();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LimpiarDatos();
        }
    }
}
