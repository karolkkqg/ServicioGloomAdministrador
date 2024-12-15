using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ConectarConSalaTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            servicioJuego.AsegurarSalaExistente("Sala1");
        }

        [TestMethod]
        public void ConectarConSalaAgregaJugadorExitosamente()
        {
            string numeroSala = "Sala1";
            string nombreUsuario = "Usuario1";

            var callback = new MockCallback();

            ServicioJuego.salaJugadoresPorSala[numeroSala] = new Dictionary<string, ISalaCallback>
        {
            { "Usuario1", Mock.Of<ISalaCallback>() }
        };

            using (var factory = new DuplexChannelFactory<ISala>(
                new InstanceContext(callback),
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba")))
            {
                var client = factory.CreateChannel();
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    var mockJugadorCallback = Mock.Get(ServicioJuego.salaJugadoresPorSala[numeroSala]["Usuario1"]);
                    mockJugadorCallback.Setup(c => c.ActualizarNumeroJugadores()).Throws(new CommunicationException());
                    servicioJuego.ConectarConSala(numeroSala, nombreUsuario);

                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSala.ContainsKey(numeroSala), "La sala no fue creada.");
                    Assert.IsTrue(ServicioJuego.salaJugadoresPorSala[numeroSala].ContainsKey(nombreUsuario), "El usuario no fue agregado correctamente a la sala.");
                }
            }
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            ServicioJuego.salaJugadoresPorSala.Clear();
        }

        private class MockCallback : ISalaCallback
        {
            public void ActualizarImagenPersonaje(string personaje, string personajeAnterior)
            {
                throw new NotImplementedException();
            }

            public void ActualizarNumeroJugadores()
            {
            }

            public void ActualizarSalasActivas(List<Sala> salasActivas)
            {
                throw new NotImplementedException();
            }

            public void ActualizarSeleccionFamilia(string nombreFamilia, string nombreFamiliaAnterior)
            {
                throw new NotImplementedException();
            }

            public void EmpezarJuego()
            {
                throw new NotImplementedException();
            }

            public void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso)
            {
                throw new NotImplementedException();
            }

            public void SacarDeSalaATodosJugadores()
            {
                throw new NotImplementedException();
            }
        }
    }
}