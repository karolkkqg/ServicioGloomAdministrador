using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.ServicioBusquedaPartidaTest
{
    [TestClass]
    public class UnirseASalaPublicaNormalTest
    {
        private Mock<ISalaCallback> callbackMock;
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuego = new ServicioJuego();

            servicioJuego.AsegurarSalaExistente("Sala1");

            callbackMock = new Mock<ISalaCallback>();
        }
        [TestMethod]
        public void UnirseASalaPublicaNormalSalaValidaAgregaJugadorYNotifica()
        {
            string idSala = "Sala1";
            string idUsuario = "Usuario1";

            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Normal",
                tipoPartida = "Pública"
            };

            var callbackInstance = new InstanceContext(callbackMock.Object);
            var factory = new DuplexChannelFactory<ISala>(
                callbackInstance,
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba"));

            var client = factory.CreateChannel();

            try
            {
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    var servicio = new ServicioJuego();
                    servicio.UnirseASalaPublicaNormal(idSala, idUsuario);

                    Assert.IsTrue(ServicioJuego.jugadoresEnSala.ContainsKey(idSala), "La sala no fue creada en jugadoresEnSala.");
                    Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario), "El usuario no fue agregado a la sala.");
                    Assert.IsTrue(ServicioJuego.usuariosSalaCallback.ContainsKey(idUsuario), "El callback del usuario no fue registrado.");
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
        public void UnirseASalaPublicaNormalSalaValidaConJugadoresAgregaJugador()
        {
            string idSala = "Sala1";
            string idUsuario1 = "Usuario1";
            string idUsuario2 = "Usuario2";

            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Normal",
                tipoPartida = "Pública"
            };

            ServicioJuego.jugadoresEnSala[idSala] = new HashSet<string> { idUsuario1 };

            var callbackInstance = new InstanceContext(callbackMock.Object);
            var factory = new DuplexChannelFactory<ISala>(
                callbackInstance,
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba"));

            var client = factory.CreateChannel();

            try
            {
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    servicioJuego.UnirseASalaPublicaNormal(idSala, idUsuario2);

                    Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario1), "El usuario 1 no está en la sala.");
                    Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario2), "El usuario 2 no fue agregado a la sala.");
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
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void UnirseASalaPublicaNormalSalaNoExisteLanzaExcepcion()
        {
            string idSala = "SalaInexistente";
            string idUsuario = "Usuario1";

            var servicio = new ServicioJuego();
            servicio.UnirseASalaPublicaNormal(idSala, idUsuario);
        }
    }
}
