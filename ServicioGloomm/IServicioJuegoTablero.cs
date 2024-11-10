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
        void IngresarJugadorAJuego(string nombreUsuario, string numeroSala, int numeroJugadores);

        [OperationContract]
        List<Carta> ObtenerCartasSobrantes();

        [OperationContract]
        void IniciarPartidaPorAdministrador(string nombreAdministrador, string numeroSala, int numeroJugadores);
    }

    [ServiceContract]
    public interface IJuegoAdministradorCallback
    {
        [OperationContract(IsOneWay = true)]
        void EnviarTurno(string nombreDelUsusarioEnTurno);
    }
}
