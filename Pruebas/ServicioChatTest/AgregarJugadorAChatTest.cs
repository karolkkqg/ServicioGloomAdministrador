using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System.ServiceModel;

namespace Pruebas.ServicioChatTest
{
    [TestClass]
    public class AgregarJugadorAChatTest
    {
        private ServicioJuego servicioChat;

        [TestInitialize]
        public void SetUp()
        {
            servicioChat = new ServicioJuego();
            ServicioJuego.jugadoresPartida.Clear();
        }
        [TestMethod]
        public void AgregarJugadorAChat_AgregaNuevoJugador()
        {
            string nombreUsuario = "Usuario1";

            var callbackMock = new Mock<IChatCallback>();
            var callbackInstance = new InstanceContext(callbackMock.Object);
            var factory = new DuplexChannelFactory<IChat>(
                callbackInstance,
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba"));

            var client = factory.CreateChannel();

            try
            {
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {

                    servicioChat.AgregarJugadorAChat(nombreUsuario);

                    Assert.IsTrue(ServicioJuego.jugadoresPartida.ContainsKey(nombreUsuario), "El jugador no fue agregado al chat.");
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