using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.ServicioSalaMiniPartidaTest
{
    [TestClass]
    public class ConectarConSalaMiniPartidaTest
    {
        private Mock<IServicioSalaMiniHistoriaCallback> callbackMock;
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuego = new ServicioJuego();

            callbackMock = new Mock<IServicioSalaMiniHistoriaCallback>();
        }

        [TestMethod]
        public void ConectarConSalaMiniPartida_SalaNoExiste_CreaSalaYAgregaUsuario()
        {
            string numeroSala = "SalaMini1";
            string nombreUsuario = "Usuario1";

            var callbackInstance = new InstanceContext(callbackMock.Object);
            var factory = new DuplexChannelFactory<IServicioSalaMiniHistoria>(
                callbackInstance,
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba"));

            var client = factory.CreateChannel();

            try
            {
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    servicioJuego.ConectarConSalaMiniPartida(numeroSala, nombreUsuario);

                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaMiniHisotria.ContainsKey(numeroSala), "La sala no fue creada en salaJugadoresPorSalaMiniHisotria.");
                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaMiniHisotria[numeroSala].ContainsKey(nombreUsuario), "El usuario no fue agregado a la sala.");
                }
            }
            finally
            {
                if (client is IClientChannel channel)
                {
                    channel.Close();
                }
            }
        }

        [TestMethod]
        public void ConectarConSalaMiniPartida_SalaYaExiste_AgregaUsuario()
        {
            string numeroSala = "SalaMini1";
            string nombreUsuario1 = "Usuario1";
            string nombreUsuario2 = "Usuario2";

            ServicioJuego.salaJugadoresPorSalaMiniHisotria[numeroSala] = new Dictionary<string, IServicioSalaMiniHistoriaCallback>
            {
                { nombreUsuario1, Mock.Of<IServicioSalaMiniHistoriaCallback>() }
            };

            var callbackInstance = new InstanceContext(callbackMock.Object);
            var factory = new DuplexChannelFactory<IServicioSalaMiniHistoria>(
                callbackInstance,
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba"));

            var client = factory.CreateChannel();

            try
            {
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    servicioJuego.ConectarConSalaMiniPartida(numeroSala, nombreUsuario2);

                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaMiniHisotria.ContainsKey(numeroSala), "La sala no fue encontrada en salaJugadoresPorSalaMiniHisotria.");
                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaMiniHisotria[numeroSala].ContainsKey(nombreUsuario1), "El usuario 1 no está en la sala.");
                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaMiniHisotria[numeroSala].ContainsKey(nombreUsuario2), "El usuario 2 no fue agregado a la sala.");
                }
            }
            finally
            {
                if (client is IClientChannel channel)
                {
                    channel.Close();
                }
            }
        }
    }
}
