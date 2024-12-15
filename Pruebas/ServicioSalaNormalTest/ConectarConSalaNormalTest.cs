using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.ServicioSalaNormalTest

{
    [TestClass]
    public class ConectarConSalaNormalTest
    {
        private Mock<IServicioSalaNormalCallback> callbackMock;
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuego = new ServicioJuego();

            callbackMock = new Mock<IServicioSalaNormalCallback>();
        }

        [TestMethod]
        public void ConectarConSalaNormal_SalaYaExiste_AgregaUsuario()
        {
            string numeroSala = "Sala1";
            string nombreUsuario1 = "Usuario1";
            string nombreUsuario2 = "Usuario2";

            ServicioJuego.salaJugadoresPorSalaNormal[numeroSala] = new Dictionary<string, IServicioSalaNormalCallback>
    {
        { nombreUsuario1, Mock.Of<IServicioSalaNormalCallback>() }
    };

            var callbackInstance = new InstanceContext(callbackMock.Object);
            var factory = new DuplexChannelFactory<IServicioSalaNormal>(
                callbackInstance,
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba"));

            var client = factory.CreateChannel();

            try
            {
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    servicioJuego.ConectarConSalaNormal(numeroSala, nombreUsuario2);

                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaNormal.ContainsKey(numeroSala), "La sala no fue encontrada en salaJugadoresPorSalaNormal.");
                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaNormal[numeroSala].ContainsKey(nombreUsuario1), "El usuario 1 no está en la sala.");
                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSalaNormal[numeroSala].ContainsKey(nombreUsuario2), "El usuario 2 no fue agregado a la sala.");
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