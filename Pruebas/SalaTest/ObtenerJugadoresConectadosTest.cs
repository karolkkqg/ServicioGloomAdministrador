using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ObtenerJugadoresConectadosTest
    {
        [TestMethod]
        public void ObtenerJugadoresConectadosTestExitoso()
        {
            ServicioJuego.salaJugadoresCallback.Clear();
            ServicioJuego.salaJugadoresCallback.Add("jugador1", null);
            ServicioJuego.salaJugadoresCallback.Add("jugador2", null);
            ServicioJuego.salaJugadoresCallback.Add("jugador3", null);

            var servicio = new ServicioJuego();
            var resultado = servicio.ObtenerJugadoresConectados("jugador1");

            Assert.AreEqual(3, resultado.Count);
            CollectionAssert.Contains(resultado, "jugador1");
            CollectionAssert.Contains(resultado, "jugador2");
            CollectionAssert.Contains(resultado, "jugador3");
        }
    }
}