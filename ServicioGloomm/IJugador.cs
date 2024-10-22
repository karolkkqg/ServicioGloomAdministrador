using BlbibliotecaClases;
using System;
using System.ServiceModel;


namespace ServicioGloomm
{
    [ServiceContract]
    public interface IJugador
    {
        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int AgregarJugador(Jugador jugador);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int ActualizarJugador(Jugador jugador);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int AutenticarJugador(Jugador jugador);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Jugador ObtenerJugador(String nombreUsuario);

    }

}
