using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceContract(CallbackContract = typeof(IServicioSalaMiniHistoriaCallback))]
    public interface IServicioSalaMiniHistoria
    {
        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, string numeroSala);

        [OperationContract]
        [FaultContract(typeof(ManejadorExcepciones))]
        void ConectarConSalaMiniPartida(string numeroSala, string nombreUsuario);
    }

    public interface IServicioSalaMiniHistoriaCallback
    {
        [OperationContract(IsOneWay = true)]
        void ActualizarImagenPersonaje(string personaje, string personajeAnterior);
    }
}
