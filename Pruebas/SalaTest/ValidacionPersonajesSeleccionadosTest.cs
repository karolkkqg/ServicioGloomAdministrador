using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ServicioGloomm;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ValidacionPersonajesSeleccionadosTest
    {
        private ServicioGloomm.ServicioJuego servicioSala;
         List<string> personajesUsados;
        Dictionary<string, (string nombrePersonaje, int vida)> personajesPorUsuario;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioSala = new ServicioGloomm.ServicioJuego();
            personajesUsados = new List<string>();
            personajesPorUsuario = new Dictionary<string, (string nombrePersonaje, int vida)>();
            LimpiarDatos();
        }

        [TestMethod]
        public void ValidarPersonajesSeleccionadosExitoso()
        {
            int cantidadJugadores = 3;
            servicioSala.SeleccionarPersonaje("Usuario1", "Personaje1", 100);
            servicioSala.SeleccionarPersonaje("Usuario2", "Personaje2", 100);
            servicioSala.SeleccionarPersonaje("Usuario3", "Personaje3", 100);

            try
            {
                servicioSala.validarPersonajesSeleccionados(cantidadJugadores);
            }
            catch (FaultException<ManejadorExcepciones>)
            {
                Assert.Fail("Se lanzó una excepción inesperada.");
            }
        }

        [TestMethod]
        public void ValidarPersonajesSeleccionadosFallaPorCantidadIncorrecta()
        {
            int cantidadJugadores = 2;
            servicioSala.SeleccionarPersonaje("Usuario1", "Personaje1", 100);
            servicioSala.SeleccionarPersonaje("Usuario2", "Personaje2", 100);
            servicioSala.SeleccionarPersonaje("Usuario3", "Personaje3", 100);
            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioSala.validarPersonajesSeleccionados(cantidadJugadores);
            });
            Assert.AreEqual("15", excepcion.Detail.mensaje);
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
