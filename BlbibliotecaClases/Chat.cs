using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlbibliotecaClases
{
    [DataContract]
    public class Chat
    {
        [DataMember]
        public string nombreUsuario { get; set; }
        [DataMember]
        public string mensaje { get; set; }

        public Chat()
        {

        }

        public Chat(string NombreUsuario, string Mensaje)
        {
            nombreUsuario = NombreUsuario;
            mensaje = Mensaje;
        }

        public override string ToString()
        {
            return $"{nombreUsuario} : {mensaje}";
        }
    }
}
