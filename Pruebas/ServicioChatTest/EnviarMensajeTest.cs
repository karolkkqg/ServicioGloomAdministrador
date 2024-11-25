using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Pruebas.ServicioJuegoTest
{
    /*[TestClass]
    public class ChatTest
    {
        private static ServicioGloomJuego chatProxy;
        private static ChatCallbackImplementation chatCallbackImplementation;

        [TestInitialize]
        public void Setup()
        {
            chatCallbackImplementation = new ChatCallbackImplementation();
            chatProxy = new ServicioGloomJuego(new InstanceContext(chatCallbackImplementation));
        }

        [TestCleanup]
        public void Cleanup()
        {
            chatProxy.Close();
        }

        [TestMethod]
        public async Task AgregarJugadorTestExitoso()
        {
            string nombreUsuario = "Jugador1";
            chatProxy.AgregarJugador(nombreUsuario);

            await Task.Delay(2000);

            Assert.IsTrue(chatCallbackImplementation.MensajeRecibido, "No se recibió el mensaje de bienvenida.");

            chatProxy.DesconectarJugador(nombreUsuario);  
        }

        [TestMethod]
        public async Task EnviarMensajeTestExitoso()
        {
            string nombreUsuario = "Jugador1";
            chatProxy.AgregarJugador(nombreUsuario);

            string mensajeTexto = "Hola, este es un mensaje de prueba.";
            chatProxy.EnviarMensaje(nombreUsuario, mensajeTexto);

            await Task.Delay(2000);

            Assert.IsTrue(chatCallbackImplementation.MensajeRecibido, "No se recibió el mensaje en el callback.");
            Assert.AreEqual(mensajeTexto, chatCallbackImplementation.UltimoMensaje.mensaje);

            chatProxy.DesconectarJugador(nombreUsuario);  
        }
    }

    public class ChatCallbackImplementation : IChatCallback
    {
        public bool MensajeRecibido { get; private set; }
        public Chat UltimoMensaje { get; private set; }

        public void EnviarMensajeCliente(Chat mensaje)
        {
            MensajeRecibido = true;
            UltimoMensaje = mensaje;
            Console.WriteLine("Mensaje recibido en callback: ");
        }
    }*/
}
