using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioJuegoTableroTest
{
    [TestClass]
    public class MatarJugadorTests
    {
        private ServicioJuego servicioJuego;

       

        [TestInitialize]
        public void SetUp()
        {
            var sala = new BibliotecaClases.Sala
            {
                idSala = "Sala1",
                nombreSala = "Sala1",
                tipoSala = "Normal",
                tipoPartida = "Publica",
                noJugadores = 2,
                codigo = "0000",
                idAdministrador = "Admin1",
                fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                ganador = "Ninguno"
            };
            AccesoSala.AgregarPartidaABaseDeDatos(sala);
            servicioJuego = new ServicioJuego();

            ServicioJuego.personajesPorSala.Clear();
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresVivos.Clear();

            ServicioJuego.jugadoresVivos.Add("Sala1", new List<string> { "Jugador1", "Jugador2", "Jugador3" });

            ServicioJuego.personajesPorSala.Add("Sala1", new Dictionary<string, (string nombrePersonaje, int vida)>
            {
                { "Jugador1", ("Personaje1", -500) },
                { "Jugador2", ("Personaje2", -500) },
                { "Jugador3", ("Personaje3", -500) }
            });

            var mockCallback1 = new Mock<IJuegoAdministradorCallback>();
            var mockCallback2 = new Mock<IJuegoAdministradorCallback>();
            var mockCallback3 = new Mock<IJuegoAdministradorCallback>();

            ServicioJuego.jugadoresConectadosTableroCallback.Add("Jugador1", mockCallback1.Object);
            ServicioJuego.jugadoresConectadosTableroCallback.Add("Jugador2", mockCallback2.Object);
            ServicioJuego.jugadoresConectadosTableroCallback.Add("Jugador3", mockCallback3.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void MatarJugador_AutoIntentoDeMuerte_LanzaExcepcion45()
        {
           
            try
            {
                servicioJuego.MatarJugador("Sala1", "Jugador1", "Jugador1");
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                Assert.AreEqual("45", ex.Detail.codigo, "No se lanzó la excepción 45 al intentar auto-eliminarse.");
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void MatarJugador_JugadorConVidaMayorAMenos400_LanzaExcepcion44()
        {
            ServicioJuego.personajesPorSala["Sala1"]["Jugador2"] = ("Personaje2", -300);

            try
            {
                servicioJuego.MatarJugador("Sala1", "Jugador2", "Jugador1");
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                Assert.AreEqual("44", ex.Detail.codigo, "No se lanzó la excepción 44 al intentar matar a un jugador con vida mayor a -400.");
                throw;
            }
        }

        /*[TestMethod]
        public void MatarJugador_JugadorValidoEliminado_NotificaCallback()
        {
            servicioJuego.MatarJugador("Sala1", "Jugador3", "Jugador1");

            Assert.IsFalse(ServicioJuego.jugadoresVivos["Sala1"].Contains("Jugador3"), "El jugador no fue removido de la lista de vivos.");

            foreach (var kvp in ServicioJuego.jugadoresConectadosTableroCallback)
            {
                var mockCallback = Mock.Get(kvp.Value);
                mockCallback.Verify(c => c.ActualizarJugadorMuerto("Jugador3"), Times.Once,
                    "No se llamó a ActualizarJugadorMuerto con el jugador eliminado.");
            }
        }*/

        [TestMethod]
        public void MatarJugador_Quedan2Jugadores_TerminaPartidaYNotificaGanador()
        {
            ServicioJuego.personajesPorSala.Clear();
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresVivos.Clear();

            ServicioJuego.jugadoresVivos.Add("Sala1", new List<string> { "Jugador1", "Jugador2" });

            ServicioJuego.personajesPorSala.Add("Sala1", new Dictionary<string, (string nombrePersonaje, int vida)>
    {
        { "Jugador1", ("Personaje1", -500) },
        { "Jugador2", ("Personaje2", -500) }
    });
            var mockCallback1 = new Mock<IJuegoAdministradorCallback>();
            var mockCallback2 = new Mock<IJuegoAdministradorCallback>();

            ServicioJuego.jugadoresConectadosTableroCallback.Add("Jugador1", mockCallback1.Object);
            ServicioJuego.jugadoresConectadosTableroCallback.Add("Jugador2", mockCallback2.Object);
           
            servicioJuego.MatarJugador("Sala1", "Jugador2", "Jugador1");

            string ganadorEsperado = "Jugador1";
            foreach (var kvp in ServicioJuego.jugadoresConectadosTableroCallback)
            {
                var mockCallback = Mock.Get(kvp.Value);
                mockCallback.Verify(c => c.EnviarGanador(ganadorEsperado), Times.Once,
                    "No se llamó a EnviarGanador con el jugador ganador esperado.");
            }
        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            ServicioJuego.personajesPorSala.Clear();
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresVivos.Clear();

            using (var contexto = new EntidadesGloom())
            {
                var sala = contexto.Sala
                    .FirstOrDefault(p => p.NombreSala == "Sala1");

                if (sala != null)
                {
                    contexto.Sala.Remove(sala);
                    contexto.SaveChanges();
                }

            }

        }

        [ClassCleanup]
        public static void LimpiarDatosDePruebaBasedeDatos()
        {
            using (var contexto = new EntidadesGloom())
            {
                var sala = contexto.Sala
                    .FirstOrDefault(p => p.NombreSala == "Sala1");

                if (sala != null)
                {
                    contexto.Sala.Remove(sala);
                    contexto.SaveChanges();
                }

            }
        }
    }

}