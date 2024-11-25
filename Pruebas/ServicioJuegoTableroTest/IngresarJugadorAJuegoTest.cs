using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.ServicioJuegoTableroTest
{
    public class ServicioJuegoPrueba
    {
        private readonly ServicioJuego _servicioJuego = new ServicioJuego();

        public void IngresarJugadorAJuegoSinCambiarModo(string nombreUsuario, string numeroSala, int numeroJugadores)
        {
            _servicioJuego.IngresarJugadorAJuego(nombreUsuario, numeroSala, numeroJugadores);
        }
    }

    //[TestClass]
    public class IngresarJugadorAJuegoTest
    {
        private ServicioJuegoPrueba servicioJuegoPrueba;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuegoPrueba = new ServicioJuegoPrueba();

            typeof(ServicioJuego).GetField("jugadoresConectadosCallback", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new Dictionary<string, IJuegoAdministradorCallback>());

            var jugadoresConectadosField = typeof(ServicioJuego)
                .GetField("jugadoresConectados", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectados = (Dictionary<string, string>)jugadoresConectadosField.GetValue(null);
            jugadoresConectados.Clear();
        }

        [TestMethod]
        public void IngresarJugadorAJuegoTestExitoso()
        {
            var callbackMock = new Mock<IJuegoAdministradorCallback>();

            string nombreUsuario = "JugadorNuevo";
            string numeroSala = "Sala1";
            int numeroJugadores = 3;

            var jugadoresConectadosCallbackField = typeof(ServicioJuego)
                .GetField("jugadoresConectadosCallback", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectadosCallback = (Dictionary<string, IJuegoAdministradorCallback>)jugadoresConectadosCallbackField.GetValue(null);

            jugadoresConectadosCallback[nombreUsuario] = callbackMock.Object;

            servicioJuegoPrueba.IngresarJugadorAJuegoSinCambiarModo(nombreUsuario, numeroSala, numeroJugadores);

            var jugadoresConectadosField = typeof(ServicioJuego)
                .GetField("jugadoresConectados", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectados = (Dictionary<string, string>)jugadoresConectadosField.GetValue(null);

            Assert.IsTrue(jugadoresConectados.ContainsKey(nombreUsuario));
            Assert.AreEqual(numeroSala, jugadoresConectados[nombreUsuario]);

            Assert.IsTrue(jugadoresConectadosCallback.ContainsKey(nombreUsuario));
            Assert.AreEqual(callbackMock.Object, jugadoresConectadosCallback[nombreUsuario]);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void IngresarJugadorAJuegoTestFallido()
        {
            var callbackMock = new Mock<IJuegoAdministradorCallback>();

            string nombreUsuario = "JugadorExistente";
            string numeroSala = "Sala1";
            int numeroJugadores = 3;

            var jugadoresConectadosField = typeof(ServicioJuego)
                .GetField("jugadoresConectados", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectados = (Dictionary<string, string>)jugadoresConectadosField.GetValue(null);

            jugadoresConectados.Clear();  
            jugadoresConectados[nombreUsuario] = numeroSala;

            var jugadoresConectadosCallbackField = typeof(ServicioJuego)
                .GetField("jugadoresConectadosCallback", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectadosCallback = (Dictionary<string, IJuegoAdministradorCallback>)jugadoresConectadosCallbackField.GetValue(null);
            jugadoresConectadosCallback.Clear();
            jugadoresConectadosCallback[nombreUsuario] = callbackMock.Object;

            servicioJuegoPrueba.IngresarJugadorAJuegoSinCambiarModo(nombreUsuario, numeroSala, numeroJugadores);

        }
    }
}
