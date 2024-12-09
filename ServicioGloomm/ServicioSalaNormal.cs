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
    public partial class ServicioJuego : IServicioSalaNormal
    {
        public static readonly Dictionary<string, Dictionary<string, IServicioSalaNormalCallback>> salaJugadoresPorSalaNormal= new Dictionary<string, Dictionary<string, IServicioSalaNormalCallback>>();
        public void ConectarConSalaNormal(string numeroSala, string nombreUsuario)
        {
            if (!salaJugadoresPorSalaNormal.ContainsKey(numeroSala))
            {
                salaJugadoresPorSalaNormal[numeroSala] = new Dictionary<string, IServicioSalaNormalCallback>();
            }
            if (!salaJugadoresPorSalaNormal[numeroSala].ContainsKey(nombreUsuario))
            {
                IServicioSalaNormalCallback callback = OperationContext.Current.GetCallbackChannel<IServicioSalaNormalCallback>();
                salaJugadoresPorSalaNormal[numeroSala].Add(nombreUsuario, callback);
            }
        }
        public void SeleccionarFamilia(string nombreUsuario, string nombreFamilia, string salaId)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            string familiaAnterior = ObtenerFamiliaAnterior(nombreUsuario);
            if (familiaAnterior != "sin familia")
            {
                familiasSeleccionadasPorSala[salaId].Remove(familiaAnterior);
            }

            FamiliaEnSeleccion(salaId, nombreFamilia);

            if (!familiasSeleccionadasPorSala.ContainsKey(salaId))
            {
                familiasSeleccionadasPorSala[salaId] = new HashSet<string>();
            }
            familiasSeleccionadasPorSala[salaId].Add(nombreFamilia);

            if (!personajesFamiliaDeUsuario.ContainsKey(nombreUsuario))
            {
                personajesFamiliaDeUsuario[nombreUsuario] = familias[nombreFamilia];
            }
            else
            {
                personajesFamiliaDeUsuario[nombreUsuario] = familias[nombreFamilia];
            }

            ValidarJugadorNoListo(nombreUsuario);

            if (salaJugadoresPorSalaNormal.ContainsKey(salaId))
            {
                foreach (var jugador in salaJugadoresPorSalaNormal[salaId])
                {
                    try
                    {
                        jugador.Value.ActualizarSeleccionFamilia(nombreFamilia, familiaAnterior);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        salaJugadoresPorSala[salaId].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }
        }

    }
}
