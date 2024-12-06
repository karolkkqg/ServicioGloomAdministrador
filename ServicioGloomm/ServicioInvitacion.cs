using BibliotecaClases;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IInvitacion
    {
        public bool EnviarInvitacion(string correo, string codigo, string administrador)
        {
            bool resultado = false;
            try
            {
                string rutaPlantilla = ObtenerDireccionPlantilla("PlantillaInvitacionCorreo.html");
                string plantillaCorreo = File.ReadAllText(rutaPlantilla);
                string cuerpoCorreo = plantillaCorreo.Replace("{administrador}", administrador).Replace("{codigo}", codigo);

                string asunto = "Invitacion a Gloom";

                resultado = EnviarCorreo(correo, asunto, cuerpoCorreo);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("25"));
            }
            catch (IOException ioException)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("26"));
            }

                return resultado;
        }

        private string ObtenerDireccionPlantilla(String nombrePlantilla)
        {
            string rutaDirectorio = AppDomain.CurrentDomain.BaseDirectory;
            string rutaDirectorioServidor = Path.GetFullPath(Path.Combine(rutaDirectorio, "../../../"));
            return Path.Combine(rutaDirectorioServidor, "ServicioGloomm", nombrePlantilla);

        }

        private bool EnviarCorreo(String correoDestinatario, String asunto, String cuerpoCorreo)
        {
            bool resultadoEnvioCorreo = false;

            try
            {
                string remitenteCorreo = "gloom.oficial.94@gmail.com";
                string contraseñaCorreo = Environment.GetEnvironmentVariable("CONTRASEÑA_GLOOM");

                MailMessage mensaje = new MailMessage();

                mensaje.From = new MailAddress(remitenteCorreo);
                mensaje.Subject = asunto;
                mensaje.To.Add(new MailAddress(correoDestinatario));
                mensaje.Body = cuerpoCorreo;
                mensaje.IsBodyHtml = true;

                var smtpCliente = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(remitenteCorreo, contraseñaCorreo),
                    EnableSsl = true
                };

                smtpCliente.Send(mensaje);

                resultadoEnvioCorreo = true;
            }

            catch (FormatException formatException)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("27"));
            }

            catch (SmtpFailedRecipientException failedRecipientException)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("28"));
            }
            catch (SmtpException smtpException)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("29"));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("30"));
            }

            return resultadoEnvioCorreo;
        }
    }
}