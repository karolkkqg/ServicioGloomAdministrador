using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Pruebas.ServicioChatTest
{
    [TestClass]
    public class EnviarMensajeTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.mensajes.Clear();
            ServicioJuego.jugadoresPartida.Clear();
        }

        [TestMethod]
        public void ObtenerHistorialMensajes_DevuelveTodosLosMensajes()
        {
            ServicioJuego.mensajes.Enqueue(new Chat("Jugador1", "Mensaje 1"));
            ServicioJuego.mensajes.Enqueue(new Chat("Jugador2", "Mensaje 2"));


            var historial = servicioJuego.ObtenerHistorialMensajes();

            Assert.AreEqual(2, historial.Count, "El historial de mensajes no contiene la cantidad correcta de mensajes.");
            Assert.AreEqual("Mensaje 1", historial[0].mensaje, "El primer mensaje no coincide.");
            Assert.AreEqual("Mensaje 2", historial[1].mensaje, "El segundo mensaje no coincide.");
        }

        [TestCleanup]
        public void CleanUp()
        {

            ServicioJuego.mensajes.Clear();
            ServicioJuego.jugadoresPartida.Clear();
        }
    }
}