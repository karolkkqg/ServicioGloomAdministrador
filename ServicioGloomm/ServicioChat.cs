using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IChat
    {
        private readonly Dictionary<string, Queue<Chat>> mensajesPorSala = new Dictionary<string, Queue<Chat>>();
        private readonly Dictionary<string, Dictionary<string, IChatCallback>> jugadoresPorSala = new Dictionary<string, Dictionary<string, IChatCallback>>();

        public void AgregarJugadorAChat(string nombreUsuario, string idSala)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();

            if (!jugadoresPorSala.ContainsKey(idSala))
            {
                jugadoresPorSala[idSala] = new Dictionary<string, IChatCallback>();
                mensajesPorSala[idSala] = new Queue<Chat>();
            }

            if (!jugadoresPorSala[idSala].ContainsKey(nombreUsuario))
            {
                jugadoresPorSala[idSala].Add(nombreUsuario, callback);
                Console.WriteLine($"Jugador {nombreUsuario} agregado a la sala {idSala}");
            }
        }

        public void EnviarMensaje(string nombreUsuario, string mensaje, string idSala)
        {
            if (!jugadoresPorSala.ContainsKey(idSala)) return;

            if (nombreUsuario.Length > 15)
            {
                nombreUsuario = nombreUsuario.Substring(0, 15);
            }

            var chatMensaje = new Chat(nombreUsuario, mensaje);
            mensajesPorSala[idSala].Enqueue(chatMensaje);

            foreach (var jugador in jugadoresPorSala[idSala].Values)
            {
                try
                {
                    Console.WriteLine($"Llamando a RecibirMensaje para: {nombreUsuario}");
                    jugador.RecibirMensaje(chatMensaje);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar mensaje a cliente: {ex.Message}");
                }
            }
        }


        public List<Chat> ObtenerHistorialMensajes(string idSala)
        {
            return mensajesPorSala.ContainsKey(idSala) ? mensajesPorSala[idSala].ToList() : new List<Chat>();
        }
    }
}
