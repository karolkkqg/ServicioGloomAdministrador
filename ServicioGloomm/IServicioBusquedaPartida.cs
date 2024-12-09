using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BibliotecaClases;
using ServicioGloomm;

namespace ServicioGloomm
{
    [ServiceContract(CallbackContract = typeof(IBusquedaPartidaCallback))]
    public interface IServicioBusquedaPartida
    {
        [OperationContract]
        List<Sala> ObtenerSalasActivas();

        [OperationContract]
        void UnirseASalaPublicaNormal(string idSala, string idUsuario);

        [OperationContract]
        void UnirseASalaPrivadaNormal(string idUsuario, string idSala, string codigoAcceso);

        [OperationContract]
        void UnirseASalaPrivadaMiniHistoria(string idUsuario, string idSala, string codigoAcceso);

    }

    [ServiceContract]
    public interface IBusquedaPartidaCallback
    {
        [OperationContract(IsOneWay = true)]
        void ActualizarSalasActivas(List<Sala> salasActivas);

        [OperationContract(IsOneWay = true)]
        void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso);
    }
}
