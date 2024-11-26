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
        void UnirseASalaPublicaNormal(string idSala, string idUsuario);

        [OperationContract]
        void UnirseASalaPrivadaNormal(string idUsuario, string idSala, string codigoAcceso);

        [OperationContract]
        void UnirseASalaPrivadaMiniHistoria(string idUsuario, string idSala, string codigoAcceso);

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
        Dictionary<string, string> ObtenerFamiliaPorJugador();

        [OperationContract]
        Dictionary<string, List<(string nombrePersonaje, int vida)>> ObtenerFamiliasYPersonajes();


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

        [OperationContract(IsOneWay = true)]
        void ActualizarSeleccionFamilia(string nombreUsuario, string nombreFamilia);

    }
}
