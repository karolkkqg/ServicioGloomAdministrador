using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlbibliotecaClases
{
    [DataContract]
    public class Partida
    {
        [DataMember]
        public string idPartida {  get; set; }
        [DataMember]
        public string ganador { get; set; }
        [DataMember]
        public string fecha { get; set; }
        [DataMember]
        public string idAdministrador { get; set; }
        [DataMember]
        public string tipo { get; set; }
        [DataMember]
        public string idSala {  get; set; }
    }
}
