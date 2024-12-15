using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class UnirseASalaPrivadaNormalTest
    {
        private Mock<IBusquedaPartidaCallback> callbackMock;

        [TestInitialize]
        public void TestInitialize()
        {
            
            ServicioJuego.salasActivasEnMemoria.Clear();
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.usuariosSalaCallback.Clear();

            
            callbackMock = new Mock<IBusquedaPartidaCallback>();
        }

        [TestMethod]
        public void UnirseASalaPrivadaNormal_SalaValida_AgregaJugadorYNotifica()
        {
            
            string idSala = "Sala1";
            string idUsuario = "Usuario1";
            string codigoAcceso = "ABC123";

            
            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Normal",
                codigo = codigoAcceso
            };

            
            using (var scope = new OperationContextScope(CreateChannel()))
            {
                
                var servicio = new ServicioJuego();
                servicio.UnirseASalaPrivadaNormal(idUsuario, idSala, codigoAcceso);


                Assert.IsTrue(ServicioJuego.jugadoresEnSala.ContainsKey(idSala), "La sala no fue creada en jugadoresEnSala.");
                Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario), "El usuario no fue agregado a la sala.");
                Assert.IsTrue(ServicioJuego.usuariosSalaCallback.ContainsKey(idUsuario), "El callback del usuario no fue registrado.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void UnirseASalaPrivadaNormal_SalaNoValida_LanzaExcepcion()
        {

            string idSala = "SalaInvalida";
            string idUsuario = "Usuario1";
            string codigoAcceso = "ABC123";

            using (var scope = new OperationContextScope(CreateChannel()))
            {
                
                var servicio = new ServicioJuego();
                servicio.UnirseASalaPrivadaNormal(idUsuario, idSala, codigoAcceso);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void UnirseASalaPrivadaNormal_CodigoIncorrecto_LanzaExcepcion()
        {
            string idSala = "Sala1";
            string idUsuario = "Usuario1";
            string codigoAcceso = "CodigoIncorrecto";
            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Normal",
                codigo = "ABC123"
            };

            using (var scope = new OperationContextScope(CreateChannel()))
            {
                var servicio = new ServicioJuego();
                servicio.UnirseASalaPrivadaNormal(idUsuario, idSala, codigoAcceso);

            }
        }

        [TestMethod]
        public void UnirseASalaPrivadaNormal_SalaValidaConJugadores_AgregaJugador()
        {
            string idSala = "Sala1";
            string idUsuario1 = "Usuario1";
            string idUsuario2 = "Usuario2";
            string codigoAcceso = "ABC123";

            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Normal",
                codigo = codigoAcceso
            };

            ServicioJuego.jugadoresEnSala[idSala] = new HashSet<string> { idUsuario1 };

            using (var scope = new OperationContextScope(CreateChannel()))
            {
                var servicio = new ServicioJuego();
                servicio.UnirseASalaPrivadaNormal(idUsuario2, idSala, codigoAcceso);

                Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario1), "El usuario 1 no está en la sala.");
                Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario2), "El usuario 2 no fue agregado a la sala.");
            }
        }

        private IContextChannel CreateChannel()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://tempuri.org");
            var factory = new ChannelFactory<IBusquedaPartidaCallback>(binding, endpoint);

            return factory.CreateChannel() as IContextChannel;
        }
    }
}
