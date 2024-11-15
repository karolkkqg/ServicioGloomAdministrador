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

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Sala BuscarSalaExistente(String idSala, String codigo);

        [OperationContract(IsOneWay = true)]
        void ConectarConSala(string nombreUsuario);
        [OperationContract]
        List<string> ObtenerJugadoresConectados(string nombreUsuario);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, int vida);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void ValidarPersonajesSeleccionados(int cantidadJugadores);
        
        [OperationContract]
        Dictionary<string, (string nombrePersonaje, int vida)> ObtenerUsuariosYPersonajes();

        [OperationContract]
        void SacarDeSala(string nombreUsuario);

        [OperationContract]
        void EmpezarPartida(string idSala);

        [OperationContract]
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
        void ActualizarSalasActivas(List<Sala> salasActivas);

        [OperationContract(IsOneWay = true)]
        void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso);

    }
}
