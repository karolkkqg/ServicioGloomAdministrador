using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceContract]
    public interface IInvitacion
    {
        [OperationContract]
        bool EnviarInvitacion(String correo, string codigo, string administrador);
    }

}
