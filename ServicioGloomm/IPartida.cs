using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGlomm
{
    [ServiceContract]
    public interface IPartida
    {
        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        int CrearPartida(Partida partida);
    }
}
