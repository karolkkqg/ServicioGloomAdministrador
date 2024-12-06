using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ServicioGloomm.Tests
{
    //[TestClass]
    /*public class ServicioJuegoTests
    {
        private ServicioJuego servicio;

        [TestInitialize]
        public void Initialize()
        {
            servicio = new ServicioJuego();

            // Inicializar datos necesarios
            servicio.familias = new Dictionary<string, List<(string nombrePersonaje, int vida)>>
            {
                { "Ores", new List<(string, int)> { ("Didorian", 0), ("Zael", 0), ("Pablian", 0), ("Lorenzeo", 0) } },
                { "Corbat", new List<(string, int)> { ("Gaia", 0), ("Arialyn", 0), ("Aris", 0), ("Abelith", 0) } }
            };

            // Limpiar los campos estáticos antes de cada prueba
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
            servicio.personajesFamiliaDeUsuario.Clear();
            ServicioJuego.salaJugadoresCallback.Clear();
        }

        [TestMethod]
        public void SeleccionarFamilia_FamiliaValida_AsignaFamiliaCorrectamente()
        {
            // Arrange
            string nombreUsuario = "Usuario1";
            string nombreFamilia = "Ores";
            string idSala = "1234";

            // Act
            servicio.SeleccionarFamilia(nombreUsuario, nombreFamilia,idSala);

            // Assert
            Assert.IsTrue(ServicioJuego.familiasSeleccionadasPorSala[idSala].Contains(nombreFamilia)); // Acceso estático
            Assert.IsTrue(servicio.personajesFamiliaDeUsuario.ContainsKey(nombreUsuario)); // Acceso de instancia
            Assert.AreEqual(servicio.personajesFamiliaDeUsuario[nombreUsuario], servicio.familias[nombreFamilia]);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void SeleccionarFamilia_FamiliaNoExiste_LanzaExcepcion()
        {
            // Arrange
            string nombreUsuario = "Usuario1";
            string nombreFamilia = "Inexistente";
            string idSala = "1234";

            // Act
            servicio.SeleccionarFamilia(nombreUsuario, nombreFamilia, idSala);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ManejadorExcepciones>))]
        public void SeleccionarFamilia_FamiliaYaSeleccionada_LanzaExcepcion()
        {
            // Arrange
            string nombreUsuario1 = "Usuario1";
            string nombreUsuario2 = "Usuario2";
            string nombreFamilia = "Ores";
            string idSala = "1234";

            servicio.SeleccionarFamilia(nombreUsuario1, nombreFamilia, idSala);

            // Act
            servicio.SeleccionarFamilia(nombreUsuario2, nombreFamilia, idSala);
        }

        [TestMethod]
        public void SeleccionarFamilia_CambioDeFamilia_LiberaFamiliaAnterior()
        {
            // Arrange
            string nombreUsuario = "Usuario1";
            string primeraFamilia = "Ores";
            string segundaFamilia = "Corbat";
            string idSala = "1234";

            servicio.SeleccionarFamilia(nombreUsuario, primeraFamilia, idSala);

            // Act
            servicio.SeleccionarFamilia(nombreUsuario, segundaFamilia, idSala);

            // Assert
            Assert.IsFalse(ServicioJuego.familiasSeleccionadasPorSala[idSala].Contains(primeraFamilia)); // Acceso estático
            Assert.IsTrue(ServicioJuego.familiasSeleccionadasPorSala[idSala].Contains(segundaFamilia)); // Acceso estático
            Assert.AreEqual(servicio.personajesFamiliaDeUsuario[nombreUsuario], servicio.familias[segundaFamilia]); // Acceso de instancia
        }
    }*/
}
