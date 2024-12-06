using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;

namespace Pruebas.ServicioJuegoTableroTest
{
    /*[TestClass]
    public class SolicitarExpulsionTest
    {
        private ServicioJuego servicio;
        private const string NumeroSala = "SalaTest";

        [TestInitialize]
        public void TestInitialize()
        {
            servicio = new ServicioJuego();

            // Inicializar los diccionarios
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresConectadosTablero.Clear();
            ServicioJuego.salaJugadoresPorSala.Clear();

            // Agregar datos de prueba
            ServicioJuego.jugadoresConectadosTableroCallback["Jugador1"] = new MockCallback();
            ServicioJuego.jugadoresConectadosTableroCallback["Jugador2"] = new MockCallback();

            ServicioJuego.jugadoresConectadosTablero["Jugador1"] = NumeroSala;
            ServicioJuego.jugadoresConectadosTablero["Jugador2"] = NumeroSala;

            ServicioJuego.salaJugadoresPorSala[NumeroSala] = new Dictionary<string, ISalaCallback>
        {
            { "Jugador1", new MockCallback() },
            { "Jugador2", new MockCallback() }
        };

            // Asignar administrador para la sala
            ServicioJuego.administradoresDeSala[NumeroSala] = "Administrador";
        }

        [TestMethod]
        public void SolicitarExpulsion_JugadorNoExiste_NoHaceNada()
        {
            // Arrange
            string solicitante = "Jugador1";
            string jugadorObjetivo = "JugadorInexistente";

            // Act
            servicio.SolicitarExpulsion(solicitante, jugadorObjetivo, NumeroSala);

            // Assert
            Assert.IsFalse(ServicioJuego.jugadoresConectadosTableroCallback.ContainsKey(jugadorObjetivo),
                "El jugador inexistente no debería estar en los callbacks.");
        }

        [TestMethod]
        public void SolicitarExpulsion_SolicitanteEsAdministrador_ExpulsaJugador()
        {
            // Arrange
            string solicitante = "Administrador";
            string jugadorObjetivo = "Jugador1";

            // Act
            servicio.SolicitarExpulsion(solicitante, jugadorObjetivo, NumeroSala);

            // Assert
            Assert.IsFalse(ServicioJuego.jugadoresConectadosTableroCallback.ContainsKey(jugadorObjetivo),
                "El jugador debería haber sido expulsado.");
            Assert.IsFalse(ServicioJuego.jugadoresConectadosTablero.ContainsKey(jugadorObjetivo),
                "El jugador debería haber sido removido de los datos de tablero.");
        }

        [TestMethod]
        public void SolicitarExpulsion_SolicitanteNoEsAdministrador_IniciaVotacion()
        {
            // Arrange
            string solicitante = "Jugador1";
            string jugadorObjetivo = "Jugador2";

            // Act
            servicio.SolicitarExpulsion(solicitante, jugadorObjetivo, NumeroSala);

            // Assert
            Assert.IsTrue(servicio.votosExpulsion.ContainsKey(NumeroSala),
                "Debería haberse iniciado una votación para la sala.");
            Assert.IsTrue(servicio.votosExpulsion[NumeroSala].Contains(solicitante),
                "El solicitante debería haber votado automáticamente.");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Limpiar los datos de prueba
            ServicioJuego.jugadoresConectadosTableroCallback.Clear();
            ServicioJuego.jugadoresConectadosTablero.Clear();
            ServicioJuego.salaJugadoresPorSala.Clear();
            ServicioJuego.administradoresDeSala.Clear();
            servicio.votosExpulsion.Clear();
        }

        // Mock callback para simular los callbacks en los tests
        private class MockCallback : IJuegoAdministradorCallback
        {
            public void RecibirExpulsion(string jugadorObjetivo)
            {
                // Simula el método del callback
            }

            public void NotificarVotacionExpulsion(string jugadorObjetivo)
            {
                // Simula el método del callback
            }

            public void ActualizarTurno(string jugadorSiguiente)
            {
                // Simula el método del callback
            }

            void IJuegoAdministradorCallback.EnviarTurno(string nombreDelUsuarioEnTurno)
            {
                throw new NotImplementedException();
            }

            void IJuegoAdministradorCallback.ActualizarImagenMazoCartaSobrante()
            {
                throw new NotImplementedException();
            }

            void IJuegoAdministradorCallback.ActualizarImagenMazoCartaBonus()
            {
                throw new NotImplementedException();
            }

            void IJuegoAdministradorCallback.ActualizarMazoJugador()
            {
                throw new NotImplementedException();
            }

            void IJuegoAdministradorCallback.EnviarGanador(string jugador)
            {
                throw new NotImplementedException();
            }

            void IJuegoAdministradorCallback.NotificarResultadoExpulsion(string jugadorExpulsado, bool expulsado)
            {
                throw new NotImplementedException();
            }
        }
    }*/
}
