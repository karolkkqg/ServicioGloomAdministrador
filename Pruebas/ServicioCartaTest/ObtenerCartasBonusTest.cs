using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioCartaTest
{
    [TestClass]
    public class ObtenerCartasBonusTest
    {
        private ServicioJuego servicioJuego;
        private MockCallback callback;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.cartasBonus.Clear();
            callback = new MockCallback();

            ServicioJuego.cartasBonus.Add(new Carta { identificador = "Bonus1", valor = 0, tipo = "saltarJugador" });
            ServicioJuego.cartasBonus.Add(new Carta { identificador = "Bonus2", valor = 0, tipo = "robar2Cartas" });

            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresConectadosTableroCallback["Usuario1"] = callback;
        }

        [TestMethod]
        public void ObtenerCartasBonusExitoso()
        {
            var carta = servicioJuego.ObtenerCartasBonus();

            Assert.AreEqual("Bonus1", carta.identificador, "La carta devuelta no es la esperada.");
            Assert.AreEqual(1, ServicioJuego.cartasBonus.Count, "La carta no fue removida correctamente del mazo de cartas bonus.");
        }

        [TestMethod]
        public void ObtenerCartasBonusExitosoSegundaCarta()
        {
            servicioJuego.ObtenerCartasBonus();
            var carta = servicioJuego.ObtenerCartasBonus();

            Assert.AreEqual("Bonus2", carta.identificador, "La segunda carta devuelta no es la esperada.");
            Assert.AreEqual(0, ServicioJuego.cartasBonus.Count, "La segunda carta no fue removida correctamente del mazo de cartas bonus.");
        }

        [TestMethod]
        public void ObtenerCartasBonusSinCartasInvocaCallback()
        {
            while (ServicioJuego.cartasBonus.Count > 0)
            {
                servicioJuego.ObtenerCartasBonus();
            }

            Assert.IsTrue(callback.ImagenMazoCartaBonusActualizada,
                "Se esperaba que ActualizarImagenMazoCartaBonus() fuera llamado exactamente una vez.");
        }

        private class MockCallback : IJuegoAdministradorCallback
        {
            public bool ImagenMazoCartaBonusActualizada { get; private set; }

            public void ActualizarImagenMazoCartaBonus()
            {
                ImagenMazoCartaBonusActualizada = true;
            }

            public void ActualizarImagenMazoCartaSobrante()
            {
                throw new NotImplementedException();
            }

            public void ActualizarImagenPersonaje(string personaje, string personajeAnterior) => throw new NotImplementedException();

            public void ActualizarInterfazExpulsion(string jugadorExpulsado)
            {
                throw new NotImplementedException();
            }

            public void ActualizarJugadorMuerto(string jugadorMuerto) => throw new NotImplementedException();
            public void ActualizarMazoJugador() => throw new NotImplementedException();
            public void ActualizarNumeroJugadores() => throw new NotImplementedException();
            public void ActualizarSalasActivas(List<Sala> salasActivas) => throw new NotImplementedException();
            public void ActualizarTurno(string nombreDelUsuarioEnTurno) => throw new NotImplementedException();
            public void EmpezarJuego() => throw new NotImplementedException();
            public void EnviarGanador(string jugador) => throw new NotImplementedException();
            public void EnviarTurno(string nombreDelUsuarioEnTurno) => throw new NotImplementedException();
            public void IniciarVotacion(string jugadorObjetivo) => throw new NotImplementedException();
            public void NotificarExpulsion(string jugadorExpulsado) => throw new NotImplementedException();

            public void NotificarVotacionExpulsion(string jugadorPropuesto)
            {
                throw new NotImplementedException();
            }

            public void RecibirExpulsion(string jugadorObjetivo)
            {
                throw new NotImplementedException();
            }

            public void ResultadoUnirseASala(string idSala, string codigo, bool esExitoso) => throw new NotImplementedException();
            public void SacarDeSalaATodosJugadores() => throw new NotImplementedException();
        }
    }
}
