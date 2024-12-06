using ServicioGlomm;
using System;
using System.ServiceModel;
using log4net;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(typeof(Program));
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                using (ServiceHost host = new ServiceHost(typeof(ServicioGloomm.ServicioJuego)))
                {
                    host.Open();
                    Console.WriteLine("Server is running");
                    Console.ReadLine();
                }
            }
            catch (AddressAccessDeniedException ex)
            {
                administradorLogger.RegistroError(ex);
                Console.ReadLine();
            }
            catch (CommunicationException ex)
            {
                administradorLogger.RegistroError(ex);
                Console.ReadLine();
            }
            catch (Exception ex) 
            {
                administradorLogger.RegistroError(ex);
                Console.ReadLine();
            }

            
        }

    }
}
