using System.Runtime.Serialization;

namespace BibliotecaClases
{
    [DataContract]
    public class Sala
    {
        [DataMember]
        public string idSala { get; set; }

        [DataMember]
        public string nombreSala { get; set; }

        [DataMember]
        public string tipoSala { get; set; }

        [DataMember]
        public string tipoPartida { get; set; }

        [DataMember]
        public int noJugadores { get; set; }

        [DataMember]
        public string codigo { get; set; }

        [DataMember]
        public string idAdministrador { get; set; }

        [DataMember]
        public string fecha { get; set; }

        [DataMember]
        public string ganador { get; set; }

        [DataMember]
        public string jugador { get; set; }

        [DataMember]
        public int noJugadoresActuales { get; set; }
    }
}
