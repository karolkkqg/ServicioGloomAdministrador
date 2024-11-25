using AccesoDatos;
using BibliotecaClases;
using log4net.Core;
using ServicioGlomm;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IAmigos
    {
        public int EnviarSolcitudAmistad(Amistad solicitud)
        {
            AdministradorLogger administradorLogger= new AdministradorLogger(this.GetType());
            try
            {

                int resultado = AccesoAmigos.AgregarSolcitudAmistad(solicitud);
                return resultado;

            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        public int ValidarSolcitudAmistad(Amistad solicitud)
        {
            int resultado = AccesoAmigos.CambiarEstadoSolicitud(solicitud);
            return resultado;
        }

        public int ArchivarAmistad(Amistad solicitud)
        {
            int resultado = AccesoAmigos.EliminarAmigo(solicitud.nombreUsuario.nombreUsuario, solicitud.jugadorAmigo.nombreUsuario);
            return resultado;
        }

        public List<BibliotecaClases.Amistad> ObtenerListaAmigos(String solicitud)
        {
            var amigos = AccesoAmigos.ObtenerAmigosDelJugador(solicitud);

            var listaAmistades = amigos.Select(a => new BibliotecaClases.Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador { nombreUsuario = a.NombreUsuario },
                jugadorAmigo = new BibliotecaClases.Jugador { nombreUsuario = a.JugadorAmigo },
                estado = a.Estado
            }).ToList();

            return listaAmistades;
        }
        public List<Amistad> ObtenerSolicitudesDeAmistadPorJugador(String nombreUsuario)
        {
            var amigos = AccesoAmigos.ObtenerSolicitudesJugador(nombreUsuario);

            var listaAmistades = amigos.Select(a => new BibliotecaClases.Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador { nombreUsuario = a.NombreUsuario },
                jugadorAmigo = new BibliotecaClases.Jugador { nombreUsuario = a.JugadorAmigo },
                estado = a.Estado
            }).ToList();

            return listaAmistades;
        }
    }
}
