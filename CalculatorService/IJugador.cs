/*using BlbibliotecaClases;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace ServicioGloom
{
    [ServiceContract(CallbackContract = typeof(IJugadorCallback))]
    public interface IJugador
    {
        [OperationContract(IsOneWay = true)]
        void AgregarJugador(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void ActualizarJugador(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void AutenticarJugador(Jugador jugador);

        //[OperationContract(IsOneWay = true)]
        //void SendMessage(string message);
    }

    [ServiceContract]
    interface IJugadorCallback
    {
        // Método asíncrono para recibir mensajes (no espera respuesta).
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string message);

        [OperationContract]
        void RespuestaJugador(string response);
    }
}
*/