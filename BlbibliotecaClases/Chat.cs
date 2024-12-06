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

        public Chat(string nombreDeUsuario, string mensajeEscrito)
        {
            nombreUsuario = nombreDeUsuario;
            mensaje = mensajeEscrito;
        }

        public override string ToString()
        {
            return $"{nombreUsuario} : {mensaje}";
        }
    }
}
