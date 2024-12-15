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
    public class SumarVidaPersonajeTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            servicioJuego.LimpiarListaJugadores();
            servicioJuego.LimpiarListaPersonajes();

            ServicioJuego.personajesPorSala["Sala1"] = new Dictionary<string, (string nombrePersonaje, int vida)>
        {
            { "Usuario1", ("Personaje1", 100) }
        };
        }

        [TestMethod]
        public void SumarVidaPersonaje_IncrementoPositivo_Test()
        {
            servicioJuego.SumarVidaPersonaje("Sala1", "Usuario1", 20);

            Assert.AreEqual(120, ServicioJuego.personajesPorSala["Sala1"]["Usuario1"].vida, "La vida no se incrementó correctamente.");
        }

        [TestMethod]
        public void SumarVidaPersonaje_IncrementoNegativo_Test()
        {
            servicioJuego.SumarVidaPersonaje("Sala1", "Usuario1", -30);

            Assert.AreEqual(70, ServicioJuego.personajesPorSala["Sala1"]["Usuario1"].vida, "La vida no se decrementó correctamente.");
        }
    }
}