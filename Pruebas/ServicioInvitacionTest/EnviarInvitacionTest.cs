using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServicioGloomm;
using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.ServiceModel;

namespace Pruebas.ServicioInvitacionTest
{
    [TestClass]
    public class EmailServiceTests
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void Setup()
        {
            servicioJuego = new ServicioJuego();
        }

        [TestMethod]
        public void EnviarInvitacion_CasoExitoso()
        {
            // Configuración del caso exitoso
            string correo = "prueba@gmail.com";
            string codigo = "CODIGO123";
            string administrador = "AdministradorTest";

            bool resultado = servicioJuego.EnviarInvitacion(correo, codigo, administrador);

            Assert.IsTrue(resultado, "La invitación no se envió correctamente cuando se esperaba un éxito.");
        }

        [TestMethod]
        public void EnviarInvitacion_CorreoInvalido()
        {
            // Configuración de un correo inválido para simular fallo
            string correo = "correo_invalido";
            string codigo = "CODIGO123";
            string administrador = "AdministradorTest";

            Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.EnviarInvitacion(correo, codigo, administrador);
            }, "No se lanzó la excepción esperada para un correo inválido.");
        }

        [TestMethod]
        public void EnviarInvitacion_PlantillaNoEncontrada()
        {
            // Cambia temporalmente la ruta para asegurar que el archivo no se encuentra
            string correo = "prueba@gmail.com";
            string codigo = "CODIGO123";
            string administrador = "AdministradorTest";

            // Al modificar la ruta temporalmente en el método ObtenerDireccionPlantilla, puedes simular la falta del archivo.
            // Asegúrate de restaurar la ruta original después de la prueba.
            var originalPath = AppDomain.CurrentDomain.BaseDirectory;
            var incorrectPath = Path.Combine(originalPath, "Host/no/existe");

            // Simular que la plantilla no existe cambiando la ruta
            typeof(ServicioJuego).GetMethod("ObtenerDireccionPlantilla", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(servicioJuego, new object[] { "PlantillaNoExistente.html" });

            Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.EnviarInvitacion(correo, codigo, administrador);
            }, "No se lanzó la excepción esperada cuando no se encuentra la plantilla de correo.");
        }

    }
}
