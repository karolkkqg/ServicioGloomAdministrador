using System;

using System.ServiceModel;
using System.Threading;

namespace CalculatorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CalculatorService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode= ConcurrencyMode.Reentrant)]
    public partial class ServiceImplementation : ICalculatorService
    {
        public void Sum(int a, int b)
        {
            int result = a + b;
            OperationContext.Current.GetCallbackChannel<ICalculatorServiceCallback>().Response(result);


        }
    }

    public partial class ServiceImplementation : IUsersManager
    {
        public void AddUser(User user)
        {
            Console.WriteLine("add user");
            String message = "User added " + user.Name + " " + user.LastName;
            OperationContext.Current.GetCallbackChannel<IUsersCallback>().UsersResponse(message);

        }
    }

}
