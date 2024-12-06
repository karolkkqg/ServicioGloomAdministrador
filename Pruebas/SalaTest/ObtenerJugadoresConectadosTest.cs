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
            ServicioJuego.salaJugadoresPorSala.Clear();

            ServicioJuego.salaJugadoresPorSala["Sala1"] = new Dictionary<string, ServicioGloomm.ISalaCallback>
            {
                { "jugador1", null },
                { "jugador2", null },
                { "jugador3", null }
            };

            var servicio = new ServicioJuego();
            var resultado = servicio.ObtenerJugadoresConectados("Sala1");

            Assert.AreEqual(3, resultado.Count, "El número de jugadores conectados no es correcto.");
            CollectionAssert.Contains(resultado, "jugador1");
            CollectionAssert.Contains(resultado, "jugador2");
            CollectionAssert.Contains(resultado, "jugador3");
        }
    }
}