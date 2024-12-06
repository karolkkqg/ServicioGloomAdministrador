using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ValidarFamiliaSeleccionadaTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
        }

        [TestMethod]
        public void ValidarFamiliaSeleccionada_CantidadCorrecta_NoLanzaExcepcion()
        {
            string idSala = "Sala1";
            ServicioJuego.familiasSeleccionadasPorSala[idSala] = new HashSet<string> { "Familia1", "Familia2" };
            int cantidadJugadores = 2;

            var servicio = new ServicioJuego();

            try
            {
                servicio.ValidarFamiliaSeleccionada(cantidadJugadores, idSala);
            }
            catch (Exception ex)
            {
                Assert.Fail($"No se esperaba ninguna excepción, pero se lanzó: {ex.Message}");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void ValidarFamiliaSeleccionada_CantidadIncorrecta_LanzaExcepcion()
        {
            string idSala = "Sala1";
            ServicioJuego.familiasSeleccionadasPorSala[idSala] = new HashSet<string> { "Familia1" }; // Solo 1 familia seleccionada
            int cantidadJugadores = 2;

            var servicio = new ServicioJuego();

            servicio.ValidarFamiliaSeleccionada(cantidadJugadores, idSala);

        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void ValidarFamiliaSeleccionada_SalaSinFamiliasSeleccionadas_LanzaExcepcion()
        {
            string idSala = "SalaInexistente";
            int cantidadJugadores = 2;

            var servicio = new ServicioJuego();

            servicio.ValidarFamiliaSeleccionada(cantidadJugadores, idSala);

        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void ValidarFamiliaSeleccionada_ErrorInesperado_LanzaExcepcion()
        {
            string idSala = "Sala1";
            ServicioJuego.familiasSeleccionadasPorSala[idSala] = null;
            int cantidadJugadores = 2;

            var servicio = new ServicioJuego();

            servicio.ValidarFamiliaSeleccionada(cantidadJugadores, idSala);

        }

        [TestMethod]
        public void ValidarFamiliaSeleccionada_CantidadIncorrecta_LanzaExcepcionConMensajeCorrecto()
        {
            string idSala = "Sala1";
            ServicioJuego.familiasSeleccionadasPorSala[idSala] = new HashSet<string> { "Familia1" };
            int cantidadJugadores = 2;

            var servicio = new ServicioJuego();

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicio.ValidarFamiliaSeleccionada(cantidadJugadores, idSala);
            });

            Assert.AreEqual("No se han seleccionado todas las familias. Faltan jugadores por seleccionar una familia.", excepcion.Detail.mensaje, "El mensaje de error no coincide.");
        }

    }
}
