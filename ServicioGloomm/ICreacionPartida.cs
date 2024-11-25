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

    }
}
