using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChat
    {
        [OperationContract(IsOneWay = true)]
        void EnviarMensaje(string nombreUsuario, string mensaje);

        [OperationContract]
        List<Chat> ObtenerHistorialMensajes();

        [OperationContract]
        void AgregarJugadorAChat(string nombreUsuario);
    }
}
