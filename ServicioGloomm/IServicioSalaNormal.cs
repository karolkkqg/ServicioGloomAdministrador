using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceContract(CallbackContract = typeof(IServicioSalaNormalCallback))]
    public interface IServicioSalaNormal
    {
        [OperationContract]
        void ConectarConSalaNormal(string numeroSala, string nombreUsuario);
    }

    public interface IServicioSalaNormalCallback{

        [OperationContract(IsOneWay = true)]
        void ActualizarSeleccionFamilia(string nombreFamilia, string nombreFamiliaAnterior);
    }
}
