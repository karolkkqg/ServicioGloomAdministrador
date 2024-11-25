using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class IniciarPartidaPorAdministradorTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuego = new ServicioJuego();
            ServicioJuego.CartasSobrantes.Clear();
            ServicioJuego.indiceTurnoActual.Clear();
            ServicioJuego.partidaYaIniciada.Clear();
            ServicioJuego.jugadoresConectadosListos.Clear();

            ServicioJuego.jugadoresConectadosListos.Add("Jugador1", "Sala1");
            ServicioJuego.jugadoresConectadosListos.Add("Jugador2", "Sala1");
            ServicioJuego.jugadoresConectadosListos.Add("Jugador3", "Sala1");

            ServicioJuego.partidaYaIniciada.Add("Sala1", false);
        }

        [TestMethod]
        public void IniciarPartidaPorAdministradorTestExitoso()
        {
            string nombreAdministrador = "Admin1";
            string numeroSala = "Sala1";
            int numeroJugadores = 3;

            servicioJuego.IniciarPartidaPorAdministrador(nombreAdministrador, numeroSala, numeroJugadores);

            Assert.IsTrue(ServicioJuego.partidaYaIniciada.ContainsKey(numeroSala));
            Assert.IsTrue(ServicioJuego.partidaYaIniciada[numeroSala]);

            Assert.IsNotNull(ServicioJuego.CartasSobrantes);
            Assert.IsTrue(ServicioJuego.CartasSobrantes.Count > 0);


        }

        [TestCleanup]
        public void Cleanup()
        {
            ServicioJuego.CartasSobrantes.Clear();
            ServicioJuego.indiceTurnoActual.Clear();
            ServicioJuego.partidaYaIniciada.Clear();
            ServicioJuego.jugadoresConectadosListos.Clear();
        }
    }
}
