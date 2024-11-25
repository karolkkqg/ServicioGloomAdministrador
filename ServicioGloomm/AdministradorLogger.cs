using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGlomm
{
    public class AdministradorLogger
    {
        public ILog Registro { get; set; }

        public AdministradorLogger(Type tipo)
        {
            Registro = LogManager.GetLogger(tipo);
        }

        public ILog GetRegistro(Type tipo)
        {
            return LogManager.GetLogger(tipo);
        }

        public void iInformacionRegistro(string mensaje)
        {
            Registro.Info(mensaje);
        }

        public void RegistroError(string message, Exception ex)
        {
            Registro.Error(message, ex);
        }

        public void RegistroError(Exception ex)
        {
            Registro.Error(ex);
        }
        public void RegistroFatal(Exception ex)
        {
            Registro.Fatal(ex);
        }

        public void RegistroAdvertencia(Exception ex)
        {
            Registro.Warn(ex);
        }

        public void RegistroDebug(Exception ex)
        {
            Registro.Debug(ex);
        }
    }
}
