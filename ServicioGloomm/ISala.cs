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
    [ServiceContract(CallbackContract = typeof(ISalaCallback))]
    public interface ISala
    {
        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int AgregarParticipantesAPartida(Sala sala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int CrearPartida(Sala sala);

        [OperationContract(IsOneWay = true)]
        void ConectarConSala(string numeroSala, string nombreUsuario);
        [OperationContract]
        List<string> ObtenerJugadoresConectados(string nombreUsuario);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, string numeroSala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void ValidarPersonajesSeleccionados(string numeroSala, int cantidadJugadores);

        [OperationContract]
        void SacarDeSala(string numeroSala, string nombreUsuario);

        [OperationContract]
        void EmpezarPartida(string idSala);

        [OperationContract]
        List<string> ObtenerPersonajesUsados(string numeroSala);

        [OperationContract]
        void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores);
        [OperationContract(IsOneWay = true)]
        void SacarATodosLosJugadoresDeSala(string numeroSala);
        List<string> ObtenerPersonajesUsados();

        [OperationContract]
        List<Sala> ObtenerSalasActivas();

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<Sala> ObtenerSalasActivasConEstado();

        [OperationContract]
        void UnirseASalaPublica(string idSala, string idUsuario);

        [OperationContract]
        void UnirseASalaPrivada(string idUsuario, string idSala, string codigoAcceso);

        [OperationContract]
        void SalirDeSala(string idSala, string idUsuario);

    }

    [ServiceContract]
    public interface ISalaCallback
    {
        [OperationContract(IsOneWay =true)]
        void EmpezarJuego();

        [OperationContract(IsOneWay = true)]
        void ActualizarNumeroJugadores();
        [OperationContract(IsOneWay = true)]
        void ActualizarImagenPersonaje(string personaje, string personajeAnterior);

        [OperationContract(IsOneWay = true)]
        void SacarDeSalaATodosJugadores();
        

        [OperationContract(IsOneWay = true)]
        void ActualizarSalasActivas(List<Sala> salasActivas);

        [OperationContract(IsOneWay = true)]
        void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso);

    }
}
