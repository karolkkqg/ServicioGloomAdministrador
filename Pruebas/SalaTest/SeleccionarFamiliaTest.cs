using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Pruebas.ServicioJuegoTest
{
    [TestClass]
    public class SeleccionarFamiliaTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
            ServicioJuego.personajesFamiliaDeUsuario.Clear();
            ServicioJuego.salaJugadoresPorSalaNormal.Clear();
            ServicioJuego.familias = new Dictionary<string, List<(string nombrePersonaje, int vida)>>
            {
                { "FamiliaA", new List<(string, int)> { ("Personaje1", 100), ("Personaje2", 90) } },
                { "FamiliaB", new List<(string, int)> { ("Personaje3", 80), ("Personaje4", 70) } }
            };
        }

        [TestMethod]
        public void SeleccionarFamilia_CambioFamilia_ActualizaFamiliasCorrectamente()
        {
            string salaId = "Sala1";
            string nombreUsuario = "Jugador1";
            string nuevaFamilia = "FamiliaA";

            ServicioJuego.familiasSeleccionadasPorSala[salaId] = new HashSet<string> { "FamiliaB" };
            ServicioJuego.personajesFamiliaDeUsuario[nombreUsuario] = ServicioJuego.familias["FamiliaB"];

            var servicio = new ServicioJuego();

            servicio.SeleccionarFamilia(nombreUsuario, nuevaFamilia, salaId);

            Assert.IsTrue(ServicioJuego.familiasSeleccionadasPorSala[salaId].Contains("FamiliaA"), "La nueva familia no se agregó correctamente.");
            Assert.IsFalse(ServicioJuego.familiasSeleccionadasPorSala[salaId].Contains("FamiliaB"), "La familia anterior no se eliminó correctamente.");
            Assert.AreEqual(
                ServicioJuego.familias["FamiliaA"],
                ServicioJuego.personajesFamiliaDeUsuario[nombreUsuario],
                "Los personajes asignados no coinciden con la nueva familia seleccionada."
            );
        }

        [TestMethod]
        public void SeleccionarFamilia_PrimeraSeleccion_AgregaFamiliaCorrectamente()
        {
            string salaId = "Sala2";
            string nombreUsuario = "Jugador2";
            string nuevaFamilia = "FamiliaB";

            ServicioJuego.familiasSeleccionadasPorSala[salaId] = new HashSet<string>();

            var servicio = new ServicioJuego();

            servicio.SeleccionarFamilia(nombreUsuario, nuevaFamilia, salaId);

            Assert.IsTrue(ServicioJuego.familiasSeleccionadasPorSala[salaId].Contains("FamiliaB"), "La familia no se agregó correctamente a la sala.");
            Assert.AreEqual(
                ServicioJuego.familias["FamiliaB"],
                ServicioJuego.personajesFamiliaDeUsuario[nombreUsuario],
                "Los personajes asignados no coinciden con la nueva familia seleccionada."
            );
        }
    }
}
