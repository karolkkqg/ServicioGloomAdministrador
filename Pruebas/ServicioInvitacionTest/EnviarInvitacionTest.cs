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
    public class EmailServicioTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void Setup()
        {
            servicioJuego = new ServicioJuego();
        }

        [TestMethod]
        public void EnviarInvitacionCasoExitoso()
        {
            string correo = "prueba@gmail.com";
            string codigo = "CODIGO123";
            string administrador = "AdministradorTest";

            bool resultado = servicioJuego.EnviarInvitacion(correo, codigo, administrador);

            Assert.IsTrue(resultado, "La invitación no se envió correctamente cuando se esperaba un éxito.");
        }

        [TestMethod]
        public void EnviarInvitacionCorreoInvalido()
        {
          
            string correo = "correo_invalido";
            string codigo = "CODIGO123";
            string administrador = "AdministradorTest";

            Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.EnviarInvitacion(correo, codigo, administrador);
            }, "No se lanzó la excepción esperada para un correo inválido.");
        }

    }
}
