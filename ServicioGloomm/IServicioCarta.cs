using BlbibliotecaClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{
    [ServiceContract]
    public interface IServicioCarta
    {
        [OperationContract]
        List<Carta> ObtenerMazoJugador(string nombreJugador);

        [OperationContract(IsOneWay =true)]
        void AgregarCartaAMazoJugador(string nombreUsuario);

        [OperationContract(IsOneWay = true)]
        void QuitarCartaDeMazoJugador(string nombreUsuario, Carta cartaAEliminar);

        [OperationContract]
        Carta ObtenerCartasBonus();

        [OperationContract(IsOneWay = true)]
        void QuitarCartaDeMazoJugadorExterno(string nombreUsuario);
    }

}
