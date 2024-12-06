using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceContract(CallbackContract = typeof(IJuegoAdministradorCallback))]
    public interface IServicioJuegoTablero
    {
        [OperationContract]
        List<Carta> ObtenerCartasSobrantes();

        [OperationContract]
        void IniciarPartidaPorAdministrador(string nombreAdministrador, string numeroSala, int numeroJugadores);

        [OperationContract]
        void EliminarJugadorDeJuego(string nombreUsuario);

        [OperationContract]
        void CambiarTurno(string numeroSala);

        [OperationContract]
        string AsignarPrimerTurno(string numeroSala);

        [OperationContract]
        string ObtenerJugadorActual(string numeroSala);

        [OperationContract(IsOneWay = true)]
        void ConectarConTablero(string nombreUsuario, string numeroSala);

        [OperationContract(IsOneWay = true)]
        void SumarVidaPersonaje(string numeroSala, string nombreUsuario, int cantidadVida);

        [OperationContract(IsOneWay = true)]
        void AgregarCastigo(string nombreJugador);

        [OperationContract(IsOneWay = true)]
        void MatarJugador(string numeroSala, string jugadorAMatar, string jugadorPropietario);

        [OperationContract(IsOneWay = true)]
        void AplicarModificadorPositivo(Carta carta, string usuarioObjetivo, string personajeObjetivo);

        [OperationContract(IsOneWay = true)]
        void AplicarModificadorNegativo(Carta carta, string nombreUsuario, string personajeObjetivo);

        [OperationContract(IsOneWay = true)]
        void AplicarCartaMuerte(string numeroSala, string nombreUsuario, string personajeObjetivo);


        [OperationContract(IsOneWay = true)]
        void SolicitarExpulsion(string solicitante, string jugadorObjetivo, string numeroSala);

        [OperationContract(IsOneWay = true)]
        void TerminarPartidaNormal(string numeroSala);

        [OperationContract]
        bool EsSalaActiva(string numeroSala);

        [OperationContract(IsOneWay = true)]
        void IncrementarTurnos(string numeroSala);

        [OperationContract]
        Dictionary<string, (string familia, int vidaTotal)> ObtenerResumenFamiliasPorSala(string numeroSala);

        [OperationContract]
        void BorrarEstructurasPorSala(string numeroSala);
    }

    [ServiceContract]
    public interface IJuegoAdministradorCallback
    {
        [OperationContract(IsOneWay = true)]
        void EnviarTurno(string nombreDelUsuarioEnTurno);

        [OperationContract(IsOneWay = true)]
        void ActualizarTurno(string nombreDelUsuarioEnTurno);

        [OperationContract(IsOneWay = true)]
        void ActualizarImagenMazoCartaSobrante();

        [OperationContract(IsOneWay = true)]
        void ActualizarImagenMazoCartaBonus();

        [OperationContract(IsOneWay = true)]
        void ActualizarMazoJugador();

        [OperationContract(IsOneWay = true)]
        void EnviarGanador(string jugador);

        [OperationContract(IsOneWay = true)]
        void ActualizarJugadorMuerto(string jugadorMuerto);

        [OperationContract(IsOneWay = true)]
        void NotificarVotacionExpulsion(string jugadorPropuesto);

        [OperationContract(IsOneWay = true)]
        void RecibirExpulsion(string jugadorObjetivo);

        [OperationContract(IsOneWay = true)]
        void ActualizarInterfazExpulsion(string jugadorExpulsado);
    }
}
