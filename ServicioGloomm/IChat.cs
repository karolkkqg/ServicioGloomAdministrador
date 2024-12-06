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
        [OperationContract]
        void AgregarJugadorAChat(string nombreUsuario, string idSala);

        [OperationContract]
        void EnviarMensaje(string nombreUsuario, string mensaje, string idSala);

        [OperationContract]
        List<Chat> ObtenerHistorialMensajes(string idSala);
    }
}
