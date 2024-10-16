
using System.ServiceModel;


namespace CalculatorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICalculatorService" in both code and config file together.
    [ServiceContract(CallbackContract =typeof(ICalculatorServiceCallback))]
    public interface IServicioAdministrador
    {
        [OperationContract(IsOneWay = true)]
        void Sum(int a, int b);
    }
    [ServiceContract]
    public interface ICalculatorServiceCallback
    {
        [OperationContract]
        void Response(int result);
    }
}
