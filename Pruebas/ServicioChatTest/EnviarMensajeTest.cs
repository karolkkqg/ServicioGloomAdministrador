using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System.ServiceModel;
using System;
using BlbibliotecaClases;

[TestClass]
public class EnviarMensajeTest
{
    private ServicioJuego servicioJuego;
    private Mock<IChatCallback> mockCallback;
    private ServiceHost serviceHost;

    [TestInitialize]
    public void SetUp()
    {
        // Crear el servicio de manera real para habilitar OperationContext
        servicioJuego = new ServicioJuego();

        // Inicializar el Mock del callback
        mockCallback = new Mock<IChatCallback>();

        // Crear y abrir un host para el servicio con una instancia única
        serviceHost = new ServiceHost(servicioJuego, new Uri("net.pipe://localhost"));
        serviceHost.AddServiceEndpoint(typeof(IChat), new NetNamedPipeBinding(), "Servicio");
        serviceHost.Open();

        // Simular OperationContext.Current asignando un contexto válido
        var factory = new ChannelFactory<IChat>(new NetNamedPipeBinding(), "net.pipe://localhost/Servicio");
        var proxy = factory.CreateChannel();

        OperationContext.Current = new OperationContext((IContextChannel)proxy);

        // Limpiar datos previos
        ServicioJuego.jugadoresPartida.Clear();
    }

    [TestMethod]
    public void EnviarMensaje_AgregaMensajeAColaYNotificaJugadores()
    {
        // Arrange
        string nombreUsuario = "JugadorPrueba";
        string mensaje = "Mensaje de prueba";
        servicioJuego.AgregarJugadorAChat(nombreUsuario);

        // Act
        servicioJuego.EnviarMensaje(nombreUsuario, mensaje);

        // Assert
        Assert.AreEqual(1, ServicioJuego.jugadoresPartida.Count, "El jugador no fue agregado correctamente.");
        Assert.AreEqual(1, servicioJuego.ObtenerHistorialMensajes().Count, "El mensaje no fue agregado al historial.");

        var mensajeEnviado = servicioJuego.ObtenerHistorialMensajes()[0];
        Assert.AreEqual(nombreUsuario, mensajeEnviado.nombreUsuario, "El nombre del usuario no coincide.");
        Assert.AreEqual(mensaje, mensajeEnviado.mensaje, "El contenido del mensaje no coincide.");

        mockCallback.Verify(callback => callback.EnviarMensajeCliente(It.Is<Chat>(
            m => m.nombreUsuario == nombreUsuario && m.mensaje == mensaje)), Times.Once, "El mensaje no fue enviado al callback correctamente.");
    }

    [TestCleanup]
    public void TearDown()
    {
        if (serviceHost != null)
        {
            try
            {
                if (serviceHost.State == CommunicationState.Faulted)
                {
                    serviceHost.Abort();
                }
                else
                {
                    serviceHost.Close();
                }
            }
            catch
            {
                serviceHost.Abort();
            }
        }

        ServicioJuego.jugadoresPartida.Clear();
    }
}
