using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ObtenerPersonajesUsadosTest
    {
        private ServicioJuego servicioSala;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioSala = new ServicioJuego();
        }

        [TestMethod]
        public void ObtenerPersonajesUsados_ContienePersonajesCorrectos()
        {
            ServicioGloomm.ServicioJuego.personajesUsados.Add("Personaje1");
            ServicioGloomm.ServicioJuego.personajesUsados.Add("Personaje2");

            var personajes = servicioSala.ObtenerPersonajesUsados();

            Assert.AreEqual(2, personajes.Count, "La lista debería contener dos personajes.");
            Assert.IsTrue(personajes.Contains("Personaje1"), "La lista debería contener 'Personaje1'.");
            Assert.IsTrue(personajes.Contains("Personaje2"), "La lista debería contener 'Personaje2'.");
        }

        public void LimpiarDatos()
        {
            servicioSala.LimpiarListaPersonajes();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LimpiarDatos();
        }
    }

   
}

