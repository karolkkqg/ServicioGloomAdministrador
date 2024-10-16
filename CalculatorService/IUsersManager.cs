using System;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace CalculatorService
{
    [ServiceContract(CallbackContract = typeof(IUsersCallback))]
    interface IUsersManager
    {
        [OperationContract]
        void AddUser(User user);
    }

    [ServiceContract]
    interface IUsersCallback
    {
        [OperationContract]
        void UsersResponse(String response);
    }
    [DataContract]
    public class User
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]    
        public String LastName { get; set; }   
    }
}
