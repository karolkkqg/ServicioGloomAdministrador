using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : IServicioHistorialPartida
    {
        public List<BibliotecaClases.Sala> ObtenerDatosHistorial(string nombreUsuario)
        {
            var historial = AccesoSala.ObtenerHistorialPartidas(nombreUsuario);

            var listaSalas = historial.Select(s => new BibliotecaClases.Sala
            {
                idSala = s.IdSala,
                nombreSala = s.NombreSala,
                tipoSala = s.TipoSala,
                tipoPartida = s.TipoPartida,
                noJugadores = s.NoJugadores,
                codigo = s.Codigo,
                idAdministrador = s.IdAdministrador,
                fecha = s.Fecha,
                ganador = s.Ganador,
                jugador = nombreUsuario
            }).ToList();

            return listaSalas;
        }

        public List<String> ObtenerParticipantesDeJuego(string identificadorSala)
        {
            var participantes = AccesoSala.ObtenerParticipantesDeSala(identificadorSala);

            return participantes;
        }
    }
}
