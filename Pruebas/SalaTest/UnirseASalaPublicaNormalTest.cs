using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.SalaTest
{
    /*[TestClass]
    public class UnirseASalaPublicaNormalTest
    {
        private Mock<ISalaCallback> callbackMock;

        [TestInitialize]
        public void TestInitialize()
        {
            // Limpiar y reinicializar los diccionarios estáticos antes de cada prueba
            ServicioJuego.salasActivasEnMemoria.Clear();
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.usuariosSalaCallback.Clear();

            // Crear un mock para ISalaCallback
            callbackMock = new Mock<ISalaCallback>();

            // Simular el contexto de operación para el callback
            var mockOperationContext = new Mock<OperationContext>(MockBehavior.Strict);
            OperationContext.Current = mockOperationContext.Object;
            mockOperationContext.Setup(x => x.GetCallbackChannel<ISalaCallback>()).Returns(callbackMock.Object);
        }

        [TestMethod]
        public void UnirseASalaPublicaNormal_SalaValida_AgregaJugadorYNotifica()
        {
            // Arrange
            string idSala = "Sala1";
            string idUsuario = "Usuario1";

            // Configurar una sala activa en memoria
            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Normal",
                tipoPartida = "Pública"
            };

            // Act
            var servicio = new ServicioJuego();
            servicio.UnirseASalaPublicaNormal(idSala, idUsuario);

            // Assert
            // Verificar que el usuario fue agregado a la sala
            Assert.IsTrue(ServicioJuego.jugadoresEnSala.ContainsKey(idSala), "La sala no fue creada en jugadoresEnSala.");
            Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario), "El usuario no fue agregado a la sala.");

            // Verificar que el callback fue registrado
            Assert.IsTrue(ServicioJuego.usuariosSalaCallback.ContainsKey(idUsuario), "El callback del usuario no fue registrado.");

            // Verificar que se llamó al método de notificación
            callbackMock.Verify(x => x.ActualizarSalasActivas(It.IsAny<List<Sala>>()), Times.Once, "No se notificó a los clientes sobre la actualización de salas.");
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void UnirseASalaPublicaNormal_SalaNoValida_LanzaExcepcion()
        {
            // Arrange
            string idSala = "SalaInvalida";
            string idUsuario = "Usuario1";

            // Act
            var servicio = new ServicioJuego();
            servicio.UnirseASalaPublicaNormal(idSala, idUsuario);

            // Assert (manejado por ExpectedException)
        }

        [TestMethod]
        public void UnirseASalaPublicaNormal_SalaValidaConJugadores_AgregaJugador()
        {
            // Arrange
            string idSala = "Sala1";
            string idUsuario1 = "Usuario1";
            string idUsuario2 = "Usuario2";

            // Configurar una sala activa en memoria con un jugador existente
            ServicioJuego.salasActivasEnMemoria[idSala] = new Sala
            {
                idSala = idSala,
                tipoSala = "Normal",
                tipoPartida = "Pública"
            };

            ServicioJuego.jugadoresEnSala[idSala] = new HashSet<string> { idUsuario1 };

            // Act
            var servicio = new ServicioJuego();
            servicio.UnirseASalaPublicaNormal(idSala, idUsuario2);

            // Assert
            // Verificar que ambos usuarios están en la sala
            Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario1), "El usuario 1 no está en la sala.");
            Assert.IsTrue(ServicioJuego.jugadoresEnSala[idSala].Contains(idUsuario2), "El usuario 2 no fue agregado a la sala.");
        }
    }*/
}
