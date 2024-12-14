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
        public BibliotecaClases.Sala BuscarSalaExistente(String nombreSala, String codigo)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                AccesoDatos.Sala SalaDb;
                BibliotecaClases.Sala SalaBiblioteca = new BibliotecaClases.Sala();

                SalaDb = AccesoSala.BuscarPartida(nombreSala, codigo);
                SalaBiblioteca.fecha = SalaDb.Fecha;
                SalaBiblioteca.idSala = SalaDb.IdSala;
                SalaBiblioteca.tipoSala = SalaDb.TipoSala;
                SalaBiblioteca.tipoPartida = SalaDb.TipoPartida;
                SalaBiblioteca.ganador = SalaDb.Ganador;
                SalaBiblioteca.codigo = codigo;
                SalaBiblioteca.nombreSala = nombreSala;
                SalaBiblioteca.noJugadores = SalaDb.NoJugadores;
                SalaBiblioteca.idAdministrador = SalaDb.IdAdministrador;

                return SalaBiblioteca;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }

        }

        public BibliotecaClases.Sala ActualizarGanador(String idSala, String codigo)
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }

        }

        public void ValidarCantidadJugadoresEnSala(string numeroSala, int cantidadJugadores)
        {
            
            if (salaJugadores[numeroSala].Count() == cantidadJugadores)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("34", "La sala ya está llena, no puede ingresar"));
            }
        }

        public void ValidarPartidaNoIniciada(string numeroSala)
        {
            if (partidaYaIniciada[numeroSala] == true)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("39", "La partida ya se encuentra iniciada, no puede ingresar"));
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
            if (!jugadoresConectadosListos.ContainsKey(numeroSala))
            {
                jugadoresConectadosListos[numeroSala] = new List<string>();
            }
            if (!turnosPorSala.ContainsKey(numeroSala))
            {
                turnosPorSala[numeroSala] = new List<string>();
            }


        }

        public string ObtenerCodigoSala(string idAdminsitrador, string nombreSala)
        {
            try
            {
                string codigoSala;

                codigoSala = AccesoSala.BuscarCodigoSala(idAdminsitrador, nombreSala);

                return codigoSala.Trim();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }
        }

    }
}
