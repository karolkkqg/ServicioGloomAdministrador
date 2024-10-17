
using AccesoDatos;
using System.ServiceModel;


namespace ServicioGloomm
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICalculatorService" in both code and config file together.
    [ServiceContract(CallbackContract =typeof(IAdministradorServiceCallback))]
    public interface IServicioAdministrador
    {
        void AgregarJugador(Jugador jugador);
    }
    [ServiceContract]
    public interface IAdministradorServiceCallback
    {
        [OperationContract]
        void Response(int result);
    }
}
