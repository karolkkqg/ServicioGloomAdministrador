using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pruebas.ServicioJuegoTableroTest
{
    [TestClass]
    public class EliminarJugadorDeJuegoTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {
            servicioJuego = new ServicioJuego();

            // Limpiar los diccionarios estáticos antes de cada prueba para un estado controlado
            typeof(ServicioJuego).GetField("jugadoresConectadosCallback", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new Dictionary<string, IJuegoAdministradorCallback>());

            var jugadoresConectadosField = typeof(ServicioJuego)
                .GetField("jugadoresConectados", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectados = (Dictionary<string, string>)jugadoresConectadosField.GetValue(null);
            jugadoresConectados.Clear();
        }

        [TestMethod]
        public void EliminarJugadorDeJuegoTestExitoso()
        {
            // Definir valores de prueba
            string nombreUsuario = "Jugador1";
            string numeroSala = "Sala1";

            // Obtener el diccionario privado estático "jugadoresConectados" usando reflexión y agregar un jugador
            var jugadoresConectadosField = typeof(ServicioJuego)
                .GetField("jugadoresConectados", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectados = (Dictionary<string, string>)jugadoresConectadosField.GetValue(null);

            // Agregar el jugador al diccionario "jugadoresConectados"
            jugadoresConectados[nombreUsuario] = numeroSala;

            // Obtener el diccionario privado estático "jugadoresConectadosCallback" y agregar un valor de prueba
            var jugadoresConectadosCallbackField = typeof(ServicioJuego)
                .GetField("jugadoresConectadosCallback", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectadosCallback = (Dictionary<string, IJuegoAdministradorCallback>)jugadoresConectadosCallbackField.GetValue(null);

            // Agregar un valor de prueba al diccionario de callbacks
            jugadoresConectadosCallback[nombreUsuario] = null; // Usamos null en lugar de un mock

            // Llamar al método que se desea probar
            servicioJuego.EliminarJugadorDeJuego(nombreUsuario);

            // Verificar que el jugador fue eliminado de jugadoresConectados
            Assert.IsFalse(jugadoresConectados.ContainsKey(nombreUsuario), "El jugador no fue eliminado de jugadoresConectados");

            // Verificar que el callback fue eliminado de jugadoresConectadosCallback
            Assert.IsFalse(jugadoresConectadosCallback.ContainsKey(nombreUsuario), "El callback no fue eliminado de jugadoresConectadosCallback");
        }

        [TestMethod]
        public void EliminarJugadorDeJuegoTestFallido()
        {
            // Definir un nombre de usuario que no existe
            string nombreUsuario = "JugadorInexistente";

            // Obtener los diccionarios actuales antes de la llamada
            var jugadoresConectadosField = typeof(ServicioJuego)
                .GetField("jugadoresConectados", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectados = (Dictionary<string, string>)jugadoresConectadosField.GetValue(null);

            var jugadoresConectadosCallbackField = typeof(ServicioJuego)
                .GetField("jugadoresConectadosCallback", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var jugadoresConectadosCallback = (Dictionary<string, IJuegoAdministradorCallback>)jugadoresConectadosCallbackField.GetValue(null);

            // Guardar el conteo inicial de elementos en los diccionarios
            int conteoInicialConectados = jugadoresConectados.Count;
            int conteoInicialCallback = jugadoresConectadosCallback.Count;

            // Llamar al método que se desea probar con un usuario inexistente
            servicioJuego.EliminarJugadorDeJuego(nombreUsuario);

            // Verificar que el conteo de elementos no ha cambiado
            Assert.AreEqual(conteoInicialConectados, jugadoresConectados.Count, "El conteo de jugadoresConectados cambió al intentar eliminar un jugador inexistente.");
            Assert.AreEqual(conteoInicialCallback, jugadoresConectadosCallback.Count, "El conteo de jugadoresConectadosCallback cambió al intentar eliminar un jugador inexistente.");
        }
    }
}
