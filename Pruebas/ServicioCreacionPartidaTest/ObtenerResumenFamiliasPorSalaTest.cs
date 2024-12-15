using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

namespace Pruebas.ServicioJuegoTest
{
    [TestClass]
    public class ObtenerResumenFamiliasPorSalaTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
            ServicioJuego.personajesFamiliaDeUsuario.Clear();
            ServicioJuego.familias = new Dictionary<string, List<(string nombrePersonaje, int vida)>>
            {
                { "Familia1", new List<(string, int)> { ("Personaje1", 100), ("Personaje2", 100), ("Personaje3", 100), ("Personaje4", 100) } },
                { "Familia2", new List<(string, int)> { ("PersonajeA", 90), ("PersonajeB", 90), ("PersonajeC", 90), ("PersonajeD", 90) } }
            };
        }

        [TestMethod]
        public void ObtenerResumenFamiliasPorSala_SalaConFamilias_RetornaResumenCorrecto()
        {
            string numeroSala = "1";

            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { "Jugador1", "Jugador2" };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string>
    {
        "Familia1",
        "Familia2"
    };
            ServicioJuego.personajesFamiliaDeUsuario["Jugador1"] = ServicioJuego.familias["Familia1"];
            ServicioJuego.personajesFamiliaDeUsuario["Jugador2"] = ServicioJuego.familias["Familia2"];
            

            var servicio = new ServicioJuego();
            var resultado = servicio.ObtenerResumenFamiliasPorSala(numeroSala);

            Assert.IsNotNull(resultado, "El resultado no debería ser nulo.");
            Assert.AreEqual(2, resultado.Count, "El resultado debería contener datos de 2 jugadores.");
            Assert.IsTrue(resultado.ContainsKey("Jugador1"), "El resumen debería incluir 'Jugador1'.");
            Assert.IsTrue(resultado.ContainsKey("Jugador2"), "El resumen debería incluir 'Jugador2'.");
        }
    }
}
