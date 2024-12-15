using BibliotecaClases;
using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IChat
    {

        public static Queue<Chat> mensajes = new Queue<Chat>();
        public static Dictionary<string, IChatCallback> jugadoresPartida = new Dictionary<string, IChatCallback>();
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
            else
            {
                jugadoresPartida[nombreUsuario] = callback;
            }
        }


        public void EnviarMensaje(string nombreUsuario, string mensaje)

        {

            if (nombreUsuario.Length > 15)
            {
                nombreUsuario = nombreUsuario.Substring(0, 15);
            }

            respuesta = OperationContext.Current.GetCallbackChannel<IChatCallback>();


            Chat mensajeChat = new Chat(nombreUsuario, mensaje);

            AgregarMensaje(mensajeChat);
            MandarMensajeAJugadores(mensajeChat);
        }
        private void AgregarMensaje(Chat mensajesChat)
        {
            mensajes?.Enqueue(mensajesChat);
        }

        private void MandarMensajeAJugadores(Chat mensajesChat)
        {
            foreach (var jugador in jugadoresPartida)
            {
                try
                {
                    jugador.Value.EnviarMensajeCliente(mensajesChat);
                }
                catch (CommunicationException ex)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("29", "Problema de enviar el correo"), new FaultReason("Problema de enviar el correo"));
                }
                catch (TimeoutException ex)
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("29", "Problema de enviar el correo"), new FaultReason("Problema de enviar el correo"));
                }
            }

        }
    }
}