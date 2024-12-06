using BibliotecaClases;
using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServicioJuego : IChat
    {

        private Queue<Chat> mensajes = new Queue<Chat>();
        private Dictionary<string, IChatCallback> jugadoresPartida = new Dictionary<string, IChatCallback>();
        private IChatCallback respuesta;


        public List<Chat> ObtenerHistorialMensajes()
        {
            return mensajes.ToList();

        }


        public void AgregarJugadorAChat(string nombreUsuario)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
            if (!jugadoresPartida.ContainsKey(nombreUsuario))
            {
                jugadoresPartida.Add(nombreUsuario, callback);
            }
        }




        public void EnviarMensaje(string nomberUsuario, string message)

        {

            if (nomberUsuario.Length > 15)
            {
                nomberUsuario = nomberUsuario.Substring(0, 15);
            }

            respuesta = OperationContext.Current.GetCallbackChannel<IChatCallback>();


            Chat mensajeChat = new Chat(nomberUsuario, message);


            AgregarMensaje(mensajeChat);
            MandarMensajeAJugadores(mensajeChat);
            


        }
        private void AgregarMensaje(Chat mensajesChat)
        {
            mensajes?.Enqueue(mensajesChat);
        }

        private void MandarMensajeAJugadores(Chat mensajesChat)
        {
            foreach (var jugador in jugadoresPartida.Values)
            {
                try
                {
                    jugador.EnviarMensajeCliente(mensajesChat);
                }
                catch (CommunicationException ex)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("29"));
                }
                catch (TimeoutException ex)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("29"));
                }
            }

        }
    }
}
