using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BlbibliotecaClases
{
    [DataContract]

    public class Jugador
    {
        private static Jugador instancia;

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


        private Jugador()
        {
            nombreUsuario = "Usuario0000";
            nombre = "UsuarioSinNombre";
            apellidos = "UsuarioSinApellidos";
            correo = "correo@delUsuario";
            contraseña = "si@ScoNtrasa4";
            tipo = "Invitado";
            icono = "default_icon.png";
        }

        [OperationContract]
        public static Jugador GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new Jugador();
                instancia.nombreUsuario = "UsuarioIncial0000";
            }
            return instancia;
        }

        [OperationContract]
        public void LimpiarSesion()
        {
            nombreUsuario = "AC00000";
            nombre = "Anonimo";
            apellidos = "SinApellidos";
            correo = "anonimo@uv.mx";
            contraseña = "si@ScoNtrasa4";
            tipo = "sin_usuario";
            icono = "default_icon.png";
        }

    }

}
