using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    [DataContract]
    public class Amistad
    {
        [Key]
        public int Id { get; set; }

        [DataMember]
        public Jugador nombreUsuario { get; set; }
        [DataMember]
        public Jugador jugadorAmigo { get; set; }
        [DataMember]
        public String estado { get; set; }
    }
}
