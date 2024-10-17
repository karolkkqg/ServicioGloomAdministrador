using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlbibliotecaClases
{
    [DataContract]

    public class Jugador
    {
        [DataMember]
        public String nombreUsuario { get; set; }
        [DataMember]
        public String nombre { get; set; }
        [DataMember]
        public String apellidos { get; set; }
        [DataMember]
        public String correo { get; set; }
        [DataMember]
        public String contraseña { get; set; }
        [DataMember]
        public String tipo { get; set; }
        [DataMember]
        public String icono { get; set; }
    }
}
