using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    //[TestClass]
    public class ConectarConTableroTest : IJuegoAdministradorCallback
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
        }

        [TestMethod]
        public void ConectarConTablero_Exitoso()
        {
            var mockCallback = this as IJuegoAdministradorCallback;
            var mockChannel = new MockContextChannel();

            using (new OperationContextScope(new OperationContext(mockChannel)))
            {
                OperationContext.Current.Extensions.Add(new MockOperationContextExtension(mockCallback));

                servicioJuego.ConectarConTablero("Jugador1", "Sala1");
                Assert.IsTrue(ServicioJuego.jugadoresConectadosTableroCallback.ContainsKey("Jugador1"), "El usuario no fue registrado en jugadoresConectadosTableroCallback.");
                Assert.AreEqual(this, ServicioJuego.jugadoresConectadosTableroCallback["Jugador1"], "El callback registrado no coincide.");

                Assert.IsTrue(ServicioJuego.jugadoresConectadosTablero.ContainsKey("Jugador1"), "El usuario no fue registrado en jugadoresConectadosTablero.");
                Assert.AreEqual("Sala1", ServicioJuego.jugadoresConectadosTablero["Jugador1"], "La sala registrada no coincide.");
            }
        }

        public void ActualizarTurno(string nombreJugador)
        {
        }

        public void EnviarTurno(string nombreDelUsuarioEnTurno)
        {
            throw new NotImplementedException();
        }

        public void ActualizarImagenMazoCartaSobrante()
        {
            throw new NotImplementedException();
        }

        public void ActualizarImagenMazoCartaBonus()
        {
            throw new NotImplementedException();
        }

        public void ActualizarMazoJugador()
        {
            throw new NotImplementedException();
        }

        public void EnviarGanador(string jugador)
        {
            throw new NotImplementedException();
        }
    }

    public class MockContextChannel : IContextChannel
    {

        public event EventHandler Closed;
        public event EventHandler Closing;
        public event EventHandler Faulted;
        public event EventHandler Opened;
        public event EventHandler Opening;

        public T GetProperty<T>() where T : class
        {
            return null;
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Close(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public void EndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Open(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public void EndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IExtensionCollection<IContextChannel> Extensions => new ExtensionCollection<IContextChannel>(this);
        //x|public OperationTimeout Timeout { get => timeout; set => timeout = value; }
        public string SessionId => "MockSession";

        public bool AllowOutputBatching { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IInputSession InputSession => throw new NotImplementedException();

        public EndpointAddress LocalAddress => throw new NotImplementedException();

        public TimeSpan OperationTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IOutputSession OutputSession => throw new NotImplementedException();

        public EndpointAddress RemoteAddress => throw new NotImplementedException();

        public CommunicationState State => throw new NotImplementedException();
    }
    public class MockOperationContextExtension : IExtension<OperationContext>
    {
        private readonly IJuegoAdministradorCallback mockCallback;

        public MockOperationContextExtension(IJuegoAdministradorCallback mockCallback)
        {
            this.mockCallback = mockCallback;
        }

        public void Attach(OperationContext owner)
        {
        }

        public void Detach(OperationContext owner)
        {
        }

        public IJuegoAdministradorCallback GetCallbackChannel<T>() where T : class
        {
            return mockCallback;
        }
    }
}

