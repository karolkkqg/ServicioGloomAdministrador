using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGlomm
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ServicioChat:IChat
    {

        private Queue<Chat> mensajes = new Queue<Chat>();
        private Dictionary<string, IChatCallback> jugadoresPartida = new Dictionary<string, IChatCallback>();
        private IChatCallback respuesta;


        public List<Chat> ObtenerHistorialMensajes()
        {
            return mensajes.ToList();

        }


        public void agregarJugador(string nombreUsuario)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
            if (!jugadoresPartida.ContainsKey(nombreUsuario))
            {
                jugadoresPartida.Add(nombreUsuario, callback);
            }
        }




        public void enviarMensaje(string nomberUsuario, string message)

        {

            if (nomberUsuario.Length > 15)
            {
                nomberUsuario = nomberUsuario.Substring(0, 15);
            }

            respuesta = OperationContext.Current.GetCallbackChannel<IChatCallback>();


            Chat mensajeChat = new Chat(nomberUsuario, message);


            agregarMensaje(mensajeChat);
            mandarMensajeAJugadores(mensajeChat);
            Console.WriteLine($"{nomberUsuario} : {message}");


        }
        private void agregarMensaje(Chat mensajesChat)
        {
            mensajes?.Enqueue(mensajesChat);
        }

        private void mandarMensajeAJugadores(Chat mensajesChat)
        {
            foreach (var jugador in jugadoresPartida.Values)
            {
                try
                {
                    jugador.enviarMensajeCliente(mensajesChat);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al enviar mensaje a cliente: " + ex.Message);
                }
            }

        }
    }
}
