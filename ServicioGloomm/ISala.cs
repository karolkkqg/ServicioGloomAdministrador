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
        List<BibliotecaClases.Sala> ObtenerDatosHistorial(String nombreUsuario);


        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<String> ObtenrParticipantesDeJuego(String identificadorSala);
    }
}
