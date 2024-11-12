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
    public interface IServicioHistorialPartida
    {
        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<BibliotecaClases.Sala> ObtenerDatosHistorial(String nombreUsuario);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<String> ObtenrParticipantesDeJuego(String identificadorSala);
    }
}
