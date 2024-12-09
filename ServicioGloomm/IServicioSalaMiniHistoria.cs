using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGlomm
{
    internal interface IServicioSalaMiniHistoria
    {
    }

    public interface IServicioSalaMiniHistoriaCallback
    {
        [OperationContract(IsOneWay = true)]
        void ActualizarImagenPersonaje(string personaje, string personajeAnterior);
    }
}
