using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public static class AdministradorDeComportamiento
    {
        public static void cambiarModoComportamientoSolo()
        {
            var servicio = (ServiceHost)OperationContext.Current.Host;
            var comportamiento = servicio.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            comportamiento.ConcurrencyMode = ConcurrencyMode.Single;
        }

        public static void cambiarModoComportamientoReentrante()
        {
            var servicio = (ServiceHost)OperationContext.Current.Host;
            var comportamiento = servicio.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            comportamiento.ConcurrencyMode = ConcurrencyMode.Reentrant;
        }


    }
}
