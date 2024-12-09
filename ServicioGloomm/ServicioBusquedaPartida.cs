using AccesoDatos;
using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGlomm
{
    public partial class ServicioJuego : IServicioBusquedaPartida
    {
        public List<BibliotecaClases.Sala> ObtenerSalasActivas()
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                var salasAccesoDatos = AccesoSala.ObtenerSalasEnPartida();

                var salasBibliotecaClases = salasAccesoDatos.Select(s => new BibliotecaClases.Sala
                {
                    idSala = s.IdSala,
                    nombreSala = s.NombreSala,
                    tipoSala = s.TipoSala,
                    tipoPartida = s.TipoPartida,
                    noJugadores = s.NoJugadores,
                    codigo = s.Codigo,
                    idAdministrador = s.IdAdministrador,
                    fecha = s.Fecha,
                    ganador = s.Ganador
                }).ToList();

                return salasBibliotecaClases;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Message));
            }

        }

        public void ActualizarSalasParaTodos()
        {
            var listaActualizada = ObtenerSalasActivasConEstado();
            foreach (var callback in usuariosSalaCallback.Values)
            {
                callback.ActualizarSalasActivas(listaActualizada);
            }
        }

        public void NotificarResultadoUnirseASala(string idUsuario, string idSala, bool esExitoso)
        {
            if (usuariosSalaCallback.TryGetValue(idUsuario, out var callback))
            {
                callback.ResultadoUnirseASala(idSala, salasActivasEnMemoria[idSala].codigo, esExitoso);
            }
        }

    }
}
