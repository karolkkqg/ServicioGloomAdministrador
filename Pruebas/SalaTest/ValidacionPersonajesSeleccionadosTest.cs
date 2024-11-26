using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            if (!ServicioGloomm.ServicioJuego.personajesUsadosPorSala.ContainsKey("Sala1"))
            {
                ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"] = new List<string>();
            }

            ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"].Add("Personaje1");
            ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"].Add("Personaje2");
            ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"].Add("Personaje3");

            try
            {
                servicioSala.ValidarPersonajesSeleccionados("Sala1", cantidadJugadores);
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                Assert.Fail(ex.Detail.mensaje);
            }
        }

        [TestMethod]
        public void ValidarPersonajesSeleccionadosFallaPorCantidadIncorrecta()
        {
            int cantidadJugadores = 2;
            if (!ServicioGloomm.ServicioJuego.personajesUsadosPorSala.ContainsKey("Sala1"))
            {
                ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"] = new List<string>();
            }

            ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"].Add("Personaje1");
            ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"].Add("Personaje2");
            ServicioGloomm.ServicioJuego.personajesUsadosPorSala["Sala1"].Add("Personaje3");
            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioSala.ValidarPersonajesSeleccionados("Sala1", cantidadJugadores);
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
