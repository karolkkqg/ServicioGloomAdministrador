
using BlbibliotecaClases;
using System.ServiceModel;


namespace ServicioGloomm
{
    
    [ServiceContract(CallbackContract =typeof(IAdministradorServiceCallback))]
    public interface IServicioAdministrador
    {
        
    }
    [ServiceContract]
    public interface IAdministradorServiceCallback
    {
        [OperationContract]
        void Response(int result);
    }
}
