using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pruebas.ServicioCreacionPartidaTest
{
    [TestClass]
    public class ObtenerFamiliasYPersonajesPorUsuarioTest
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
        public void ObtenerFamiliaYPersonajesPorUsuario_SalaNoExistente_RetornaVacio()
        {
            
            string numeroSala = "1";

            var resultado = new ServicioJuego().ObtenerFamiliaYPersonajesPorUsuario(numeroSala);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count, "El resultado debería ser vacío si la sala no existe.");
        }

        [TestMethod]
        public void ObtenerFamiliaYPersonajesPorUsuario_SalaConFamiliaValida_RetornaDatos()
        {
            string numeroSala = "1";
            string usuario = "Usuario1";
            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { usuario };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string> { "Familia1" };
            ServicioJuego.personajesFamiliaDeUsuario[usuario] = ServicioJuego.familias["Familia1"];

            var resultado = new ServicioJuego().ObtenerFamiliaYPersonajesPorUsuario(numeroSala);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count, "El resultado debería contener datos para un usuario.");
            Assert.IsTrue(resultado.ContainsKey(usuario), "El resultado debería contener al usuario.");
            Assert.AreEqual("Familia1", resultado[usuario].familia, "La familia debería ser 'Familia1'.");
            CollectionAssert.AreEqual(
                ServicioJuego.familias["Familia1"],
                resultado[usuario].personajes.ToList(),
                "Los personajes no coinciden con los esperados."
            );
        }

        [TestMethod]
        public void ObtenerFamiliaYPersonajesPorUsuario_SalaConUsuarioSinPersonajes_RetornaVacio()
        {
            string numeroSala = "1";
            string usuario = "Usuario1";
            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string> { usuario };
            ServicioJuego.familiasSeleccionadasPorSala[numeroSala] = new HashSet<string> { "Familia1" };

            var resultado = new ServicioJuego().ObtenerFamiliaYPersonajesPorUsuario(numeroSala);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count, "El resultado debería ser vacío si el usuario no tiene personajes asignados.");
        }
    }
}
