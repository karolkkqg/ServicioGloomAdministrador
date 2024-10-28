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
    public interface IAmigos
    {
        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int EnviarSolcitudAmistad(Amistad solicitud);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int ValidarSolcitudAmistad(Amistad solicitud);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int ArchivarAmistad(Amistad solicitud);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<BibliotecaClases.Amistad> ObtenerListaAmigos(String nombreUsuario);


        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        List<BibliotecaClases.Amistad> ObtenerSolicitudesDeAmistadPorJugador(String nombreUsuario);
        
    }
}
