using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceContract]
    public interface ICreacionPartida
    {
        [OperationContract]
        Dictionary<string, (string nombrePersonaje, int vida)> ObtenerUsuariosYPersonajes(string numeroSala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Sala BuscarSalaExistente(String idSala, String codigo);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void ValidarCantidadJugadoresEnSala(string numeroSala, int cantidadJugadores);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void ValidarPartidaNoIniciada(string numeroSala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Dictionary<string, string> ObtenerFamiliaPorJugador(string numeroSala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Dictionary<string, (string familia, List<(string nombrePersonaje, int vida)> personajes)> ObtenerFamiliaYPersonajesPorUsuario(string numeroSala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        Dictionary<string, List<(string nombrePersonaje, int vida)>> ObtenerFamiliasYPersonajes(string numeroSala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int CrearPartida(Sala sala);

        [OperationContract]
        string ObtenerCodigoSala(string idAdminsitrador, string nombreSala);

        [OperationContract]
        Dictionary<string, (string familia, int vidaTotal)> ObtenerResumenFamiliasPorSala(string numeroSala);


    }
}
