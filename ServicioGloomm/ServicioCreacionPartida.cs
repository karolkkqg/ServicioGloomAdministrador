using AccesoDatos;
using BibliotecaClases;
using ServicioGlomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    public partial class ServicioJuego : ICreacionPartida
    {
        public BibliotecaClases.Sala BuscarSalaExistente(String idSala, String codigo)
        {
            AsegurarSalaExistente(idSala);
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                AccesoDatos.Sala SalaDb;
                BibliotecaClases.Sala SalaBiblioteca = new BibliotecaClases.Sala();

                SalaDb = AccesoSala.BuscarPartida(idSala, codigo);
                SalaBiblioteca.fecha = SalaDb.Fecha;
                SalaBiblioteca.idSala = idSala;
                SalaBiblioteca.tipoSala = SalaDb.TipoSala;
                SalaBiblioteca.tipoPartida = SalaDb.TipoPartida;
                SalaBiblioteca.ganador = SalaDb.Ganador;
                SalaBiblioteca.codigo = codigo;
                SalaBiblioteca.nombreSala = SalaDb.NombreSala;
                SalaBiblioteca.noJugadores = SalaDb.NoJugadores;
                SalaBiblioteca.idAdministrador = SalaDb.IdAdministrador;

                return SalaBiblioteca;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }

        }

        public void ValidarCantidadJugadoresEnSala(string numeroSala, int cantidadJugadores)
        {
            
            if (salaJugadores[numeroSala].Count() == cantidadJugadores)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("34"));
            }
        }

        public void ValidarPartidaNoIniciada(string numeroSala)
        {
            if (partidaYaIniciada[numeroSala] == true)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("39"));
            }
        }

        private void AsegurarSalaExistente(string numeroSala)
        {

            if (!salaJugadores.ContainsKey(numeroSala))
            {
                salaJugadores[numeroSala] = new List<string>();
            }
            if (!personajesUsadosPorSala.ContainsKey(numeroSala))
            {
                personajesUsadosPorSala[numeroSala] = new List<string>();
            }
            if (!personajesPorSala.ContainsKey(numeroSala))
            {
                personajesPorSala[numeroSala] = new Dictionary<string, (string nombrePersonaje, int vida)>();
            }
        }

    }
}
