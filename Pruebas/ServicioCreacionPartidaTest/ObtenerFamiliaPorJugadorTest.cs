using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.ServicioCreacionPartidaTest
{
    [TestClass]
    public class ObtenerFamiliaPorJugadorTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
            ServicioJuego.personajesFamiliaDeUsuario.Clear();
            ServicioJuego.familias = new Dictionary<string, List<(string nombrePersonaje, int vida)>>
        {
            {"Familia1", new List<(string, int)> { ("Personaje1", 0), ("Personaje2", 0), ("Personaje3", 0), ("Personaje4", 0) }},
            {"Familia2", new List<(string, int)> { ("PersonajeA", 0), ("PersonajeB", 0), ("PersonajeC", 0), ("PersonajeD", 0) }}
        };
        }

        [TestMethod]
        public void ObtenerFamiliaPorJugador_SalaConDatosValidos_RetornaFamilias()
        {
            string numeroSala = "1";
            string jugador1 = "Jugador1";
            string jugador2 = "Jugador2";

            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { jugador1, jugador2 };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string> { "Familia1", "Familia2" };
            ServicioJuego.personajesFamiliaDeUsuario[jugador1] = ServicioJuego.familias["Familia1"];
            ServicioJuego.personajesFamiliaDeUsuario[jugador2] = ServicioJuego.familias["Familia2"];

            var resultado = new ServicioJuego().ObtenerFamiliaPorJugador(numeroSala);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count, "El resultado debería contener datos para 2 jugadores.");
            Assert.AreEqual("Familia1", resultado[jugador1], "Jugador1 debería estar asociado con Familia1.");
            Assert.AreEqual("Familia2", resultado[jugador2], "Jugador2 debería estar asociado con Familia2.");
        }

        [TestMethod]
        public void ObtenerFamiliaPorJugador_UsuarioSinPersonajes_RetornaFamiliaNoAsignada()
        {
            string numeroSala = "1";
            string jugador1 = "Jugador1";

            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { jugador1 };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string> { "Familia1" };

            var resultado = new ServicioJuego().ObtenerFamiliaPorJugador(numeroSala);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count, "El resultado debería contener datos para 1 jugador.");
            Assert.AreEqual("Familia no asignada", resultado[jugador1], "Jugador1 debería tener 'Familia no asignada'.");
        }

        [TestMethod]
        public void ObtenerFamiliaPorJugador_UsuarioConPersonajesNoCoinciden_RetornaFamiliaNoAsignada()
        {
            string numeroSala = "1";
            string jugador1 = "Jugador1";

            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { jugador1 };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string> { "Familia1" };

            ServicioJuego.personajesFamiliaDeUsuario[jugador1] = new List<(string, int)>
        {
            ("PersonajeX", 0),
            ("PersonajeY", 0)
        };

            var resultado = new ServicioJuego().ObtenerFamiliaPorJugador(numeroSala);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count, "El resultado debería contener datos para 1 jugador.");
            Assert.AreEqual("Familia no asignada", resultado[jugador1], "Jugador1 debería tener 'Familia no asignada'.");
        }
    }
}
