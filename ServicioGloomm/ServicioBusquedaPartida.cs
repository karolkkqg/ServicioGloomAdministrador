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
    public partial class ServicioJuego : IServicioBusquedaPartida
    {
        public void UnirseASalaPublicaNormal(string idSala, string idUsuario)
        {
            if (salasActivasEnMemoria.TryGetValue(idSala, out var sala) && sala.tipoPartida == "Pública" && sala.tipoSala == "Normal")
            {
                if (!jugadoresEnSala.ContainsKey(idSala))
                {
                    jugadoresEnSala[idSala] = new HashSet<string>();
                }
                jugadoresEnSala[idSala].Add(idUsuario);
                usuariosSalaCallback[idUsuario] = OperationContext.Current.GetCallbackChannel<ISalaCallback>();

            }
            else
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("23", "La sala no es pública o no existe."), new FaultReason("La sala no es pública o no existe."));
            }
        }

        public void UnirseASalaPrivadaNormal(string idUsuario, string idSala, string codigoAcceso)
        {
            if (!salasActivasEnMemoria.TryGetValue(idSala, out var sala) || sala.tipoSala != "Normal" || sala.codigo != codigoAcceso)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("24", "Verifique el código de la sala."), new FaultReason("Verifique el código de la sala."));
            }

            if (!jugadoresEnSala.ContainsKey(idSala))
            {
                jugadoresEnSala[idSala] = new HashSet<string>();
            }
            jugadoresEnSala[idSala].Add(idUsuario);
            usuariosSalaCallback[idUsuario] = OperationContext.Current.GetCallbackChannel<ISalaCallback>();

        }



        public void UnirseASalaPrivadaMiniHistoria(string idUsuario, string idSala, string codigoAcceso)
        {
            if (!salasActivasEnMemoria.TryGetValue(idSala, out var sala) || sala.tipoSala != "Mini historia" || sala.codigo != codigoAcceso)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("24", "Verifique el código de la sala."), new FaultReason("Verifique el código de la sala."));
            }

            if (!jugadoresEnSala.ContainsKey(idSala))
            {
                jugadoresEnSala[idSala] = new HashSet<string>();
            }
            jugadoresEnSala[idSala].Add(idUsuario);
            usuariosSalaCallback[idUsuario] = OperationContext.Current.GetCallbackChannel<ISalaCallback>();

        }
    }
}
