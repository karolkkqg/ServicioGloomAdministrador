using BibliotecaClases;
using System;
using System.Collections.Generic;
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
        int ActualizarJugadorSinContrasena(BibliotecaClases.Jugador jugador);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int AutenticarJugador(Jugador jugador);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Jugador ObtenerJugador(String nombreUsuario);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<BibliotecaClases.Jugador> BuscarJugadoresPorNombreUsuario(String nombreUsuarioParcial);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Jugador AgregarJugadorInvitado();

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        bool EliminarJugadorInvitado(string nombreUsuario);

        [OperationContract(IsOneWay = true)]
        void CerrarSesionJugador(string nombreJugador);
    }

}
