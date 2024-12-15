using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class UnirseASalaPrivadaMiniHistoriaTest
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
        public void UnirseASalaPrivadaMiniHistoriaSalaValidaConCodigoCorrectoAgregaJugador()
        {
            string idSala = "SalaPrivada1";
            string idUsuario = "Usuario1";
            string codigoAcceso = "ABC123";

            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Mini historia",
                codigo = codigoAcceso
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
                    servicioJuego.UnirseASalaPrivadaMiniHistoria(idUsuario, idSala, codigoAcceso);

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
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void UnirseASalaPrivadaMiniHistoriaSalaConCodigoIncorrectoLanzaExcepcion()
        {
            string idSala = "SalaPrivada1";
            string idUsuario = "Usuario1";
            string codigoAccesoIncorrecto = "XYZ789";

            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Mini historia",
                codigo = "ABC123"
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
                    servicioJuego.UnirseASalaPrivadaMiniHistoria(idUsuario, idSala, codigoAccesoIncorrecto);

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