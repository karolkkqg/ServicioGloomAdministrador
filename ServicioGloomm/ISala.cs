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

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<Sala> ObtenerSalasActivasConEstado();

        [OperationContract]
        void SalirDeSala(string idSala, string idUsuario);


        [OperationContract]
        string ObtenerCodigoSala(string idAdminsitrador, string nombreSala);

        [OperationContract]
        List<string> ObtenerFamiliaSeleccionada(string idSala);

        [OperationContract]
        void SeleccionarFamilia(string nombreUsuario, string nombreFamilia, string salaId);

        [OperationContract]
        void ValidarFamiliaSeleccionada(int cantidadJugadores, string idSala);

        [OperationContract]
        Dictionary<string, HashSet<string>> ObtenerFamiliasSeleccionadasPorSala();

        [OperationContract]
        void CambiarEstadoParaPartida(string numeroSala, string ganador);

    }

    [ServiceContract]
    public interface ISalaCallback
    {
        [OperationContract(IsOneWay = true)]
        void EmpezarJuego();

        [OperationContract(IsOneWay = true)]
        void ActualizarNumeroJugadores();

        [OperationContract(IsOneWay = true)]
        void SacarDeSalaATodosJugadores();

    }



}
