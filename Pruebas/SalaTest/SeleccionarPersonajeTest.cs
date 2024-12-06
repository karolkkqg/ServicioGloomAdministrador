using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    //[TestClass]
    public class SeleccionarPersonajeTest
    {
        private ServicioGloomm.ServicioJuego servicioSala;
        private List<string> personajesUsados;
        private Dictionary<string, (string nombrePersonaje, int vida)> personajesPorUsuario;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioSala = new ServicioGloomm.ServicioJuego();
            personajesUsados = new List<string>();
            personajesPorUsuario = new Dictionary<string, (string nombrePersonaje, int vida)>();
            LimpiarDatos();
        }

        [TestMethod]
        public void SeleccionarPersonajeExitosoPersonajeNoUsado()
        {
            servicioSala.SeleccionarPersonaje("Usuario1", "Personaje1", "Sala1");

            var personajes = servicioSala.ObtenerUsuariosYPersonajes("Sala1");
            Assert.AreEqual(1, personajes.Count);
        }

        [TestMethod]
        public void SeleccionarPersonajeCambioDePersonaje()
        {
            servicioSala.SeleccionarPersonaje("Usuario1", "Personaje1", "Sala1");
            servicioSala.SeleccionarPersonaje("Usuario1", "Personaje2", "Sala1");

            //var personajes = servicioSala.ObtenerUsuariosYPersonajes();
            //Assert.AreEqual(1, personajes.Count);
        }

        [TestMethod]
        public void SeleccionarPersonajeFallaPorPersonajeYaUsado()
        {
            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioSala.SeleccionarPersonaje("Usuario1", "Personaje1", "Sala1");
                servicioSala.SeleccionarPersonaje("Usuario2", "Personaje1", "Sala1");
            });
            Assert.AreEqual("14", excepcion.Detail.mensaje);

        }

        public void LimpiarDatos()
        {
            personajesUsados.Clear();
            personajesPorUsuario.Clear();
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
