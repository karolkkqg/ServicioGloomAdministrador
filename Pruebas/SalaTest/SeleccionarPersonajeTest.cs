using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class SeleciconarPersonajeTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.salaJugadoresPorSala.Clear();
            ServicioJuego.personajesUsadosPorSala.Clear();
            ServicioJuego.personajesPorSala.Clear();

            var mockCallback = new Mock<ISalaCallback>();

            ServicioJuego.salaJugadoresPorSala.Add("Sala1", new Dictionary<string, ISalaCallback>
        {
            { "Jugador1", mockCallback.Object }
        });

            ServicioJuego.personajesUsadosPorSala.Add("Sala1", new List<string>());
            ServicioJuego.personajesPorSala.Add("Sala1", new Dictionary<string, (string, int)>());
        }

        [TestMethod]
        public void SeleccionarPersonaje_AsignaPersonajeCorrectamente()
        {
            string numeroSala = "Sala1";
            string nombreUsuario = "Jugador1";
            string nombrePersonaje = "Personaje1";

            servicioJuego.SeleccionarPersonaje(nombreUsuario, nombrePersonaje, numeroSala);

            Assert.IsTrue(ServicioJuego.personajesUsadosPorSala[numeroSala].Contains(nombrePersonaje), "El personaje no fue agregado correctamente a personajesUsadosPorSala.");
            Assert.IsTrue(ServicioJuego.personajesPorSala[numeroSala].ContainsKey(nombreUsuario), "El personaje no fue asignado correctamente al jugador.");
            Assert.AreEqual(nombrePersonaje, ServicioJuego.personajesPorSala[numeroSala][nombreUsuario].Item1, "El personaje asignado no es correcto.");
        }

        [TestMethod]
        public void SeleccionarPersonaje_LanzaExcepcionSiPersonajeYaSeleccionado()
        {
            string numeroSala = "Sala1";
            string nombreUsuario = "Jugador1";
            string nombrePersonaje = "Personaje1";

            ServicioJuego.personajesUsadosPorSala[numeroSala].Add(nombrePersonaje);

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.SeleccionarPersonaje(nombreUsuario, nombrePersonaje, numeroSala);
            });

            Assert.AreEqual("14", exception.Detail.codigo, "El mensaje de la excepción no es el esperado.");
        }

        [TestCleanup]
        public void LimpiarDiccionarios()
        {
            ServicioJuego.salaJugadoresPorSala.Clear();
            ServicioJuego.personajesUsadosPorSala.Clear();
            ServicioJuego.personajesPorSala.Clear();
        }
    }
}