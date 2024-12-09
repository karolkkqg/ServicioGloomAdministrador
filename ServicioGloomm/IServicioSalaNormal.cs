using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGlomm
{
    public interface IServicioSalaNormal
    {
    }

    public interface IServicioSalaNormalCallback{

        [OperationContract(IsOneWay = true)]
        void ActualizarSeleccionFamilia(string nombreFamilia, string nombreFamiliaAnterior);
    }
}
