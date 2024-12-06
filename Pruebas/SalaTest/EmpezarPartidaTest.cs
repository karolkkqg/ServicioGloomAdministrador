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
   /*[TestClass]
    public class ServicioTest
    {
        [TestMethod]
        public void EmpezarPartida_DebeLlamarEmpezarJuegoParaCadaJugador()
        {
            var jugador1Mock = new Mock<ISalaCallback>();
            var jugador2Mock = new Mock<ISalaCallback>();
            jugador1Mock.Setup(j => j.EmpezarJuego()).Verifiable();
            jugador2Mock.Setup(j => j.EmpezarJuego()).Verifiable();

            ServicioJuego.salaJugadoresCallback.Clear();
            ServicioJuego.salaJugadoresCallback.Add("jugador1", jugador1Mock.Object);
            ServicioJuego.salaJugadoresCallback.Add("jugador2", jugador2Mock.Object);

            var servicio = new ServicioJuego();

            var channelMock = new Mock<IContextChannel>();
            using (new OperationContextScope(channelMock.Object))
            {
                servicio.EmpezarPartida("idSala");
                jugador1Mock.Verify(j => j.EmpezarJuego(), Times.Once);
                jugador2Mock.Verify(j => j.EmpezarJuego(), Times.Once);
            }
        }
    }*/
}
