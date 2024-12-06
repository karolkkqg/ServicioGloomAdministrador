using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioJuegoTableroTest
{

    [TestClass]
    public class ConectarConTableroTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresConectadosTablero.Clear();
            ServicioJuego.jugadoresVivos.Clear();
            ServicioJuego.jugadoresVivos["Sala1"] = new List<string>();
        }

        [TestMethod]
        public void ConectarConTableroAgregaJugadorExitosamente()
        {
            string nombreUsuario = "Jugador1";
            string numeroSala = "Sala1";

            var callback = new MockCallback();
            using (var factory = new DuplexChannelFactory<IServicioJuegoTablero>(
                new InstanceContext(callback),
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba")))
            {
                var client = factory.CreateChannel();
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    servicioJuego.ConectarConTablero(nombreUsuario, numeroSala);
                }
            }

            Assert.IsTrue(ServicioJuego.jugadoresConectadosTableroCallback.ContainsKey(nombreUsuario), "El callback no fue agregado correctamente.");

            Assert.IsTrue(ServicioJuego.jugadoresConectadosTablero.ContainsKey(nombreUsuario), "El jugador no fue agregado correctamente a la lista de conectados al tablero.");
            Assert.AreEqual(numeroSala, ServicioJuego.jugadoresConectadosTablero[nombreUsuario], "El número de sala no coincide.");

            Assert.IsTrue(ServicioJuego.jugadoresVivos[numeroSala].Contains(nombreUsuario), "El jugador no fue agregado correctamente a la lista de jugadores vivos.");
        }

        [TestMethod]
        public void ConectarConTableroNoAgregaJugadorSiNombreUsuarioIgualANumeroSala()
        {
            string nombreUsuario = "Sala1";
            string numeroSala = "Sala1";

            var callback = new MockCallback();
            using (var factory = new DuplexChannelFactory<IServicioJuegoTablero>(
                new InstanceContext(callback),
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ServicioPrueba")))
            {
                var client = factory.CreateChannel();
                using (OperationContextScope scope = new OperationContextScope((IContextChannel)client))
                {
                    servicioJuego.ConectarConTablero(nombreUsuario, numeroSala);
                }
            }

            Assert.IsFalse(ServicioJuego.jugadoresConectadosTableroCallback.ContainsKey(nombreUsuario), "El callback fue agregado incorrectamente.");

            Assert.IsFalse(ServicioJuego.jugadoresConectadosTablero.ContainsKey(nombreUsuario), "El jugador fue agregado incorrectamente a la lista de conectados al tablero.");

            Assert.IsFalse(ServicioJuego.jugadoresVivos[numeroSala].Contains(nombreUsuario), "El jugador fue agregado incorrectamente a la lista de jugadores vivos.");
        }

        [ClassCleanup]
        public static void LimpiarDiccionarios()
        {
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresConectadosTablero.Clear();
            ServicioJuego.jugadoresVivos.Clear();
        }

        private class MockCallback : IJuegoAdministradorCallback
        {
            public void ActualizarImagenMazoCartaBonus()
            {
                throw new NotImplementedException();
            }

            public void ActualizarImagenMazoCartaSobrante()
            {
                throw new NotImplementedException();
            }

            public void ActualizarInterfazExpulsion(string jugadorExpulsado)
            {
                throw new NotImplementedException();
            }

            public void ActualizarJugadorMuerto(string jugadorMuerto)
            {
                throw new NotImplementedException();
            }

            public void ActualizarMazoJugador()
            {
                throw new NotImplementedException();
            }

            public void ActualizarTurno(string nombreDelUsuarioEnTurno)
            {
                throw new NotImplementedException();
            }

            public void EnviarGanador(string jugador)
            {
                throw new NotImplementedException();
            }

            public void EnviarTurno(string nombreDelUsuarioEnTurno)
            {
                throw new NotImplementedException();
            }

            public void IniciarVotacion(string jugadorObjetivo)
            {
                throw new NotImplementedException();
            }

            public void NotificarExpulsion(string jugadorExpulsado)
            {
                throw new NotImplementedException();
            }

            public void NotificarVotacionExpulsion(string jugadorPropuesto)
            {
                throw new NotImplementedException();
            }

            public void RecibirExpulsion(string jugadorObjetivo)
            {
                throw new NotImplementedException();
            }
        }
    }
}
