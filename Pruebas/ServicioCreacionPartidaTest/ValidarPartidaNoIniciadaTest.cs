using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioCreacionPartidaTest
{
    [TestClass]
    public class ValidarPartidaNoIniciadaTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();
            ServicioJuego.partidaYaIniciada.Clear();
            ServicioJuego.partidaYaIniciada.Add("Sala1", false);
            ServicioJuego.partidaYaIniciada.Add("Sala2", true);

        }

        [TestMethod]
        public void ValidarPartidaNoIniciada_NoLanzaExcepcionSiPartidaNoEstaIniciada()
        {
            servicioJuego.ValidarPartidaNoIniciada("Sala1");
        }

        [TestMethod]
        public void ValidarPartidaNoIniciada_LanzaExcepcionSiPartidaEstaIniciada()
        {
            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.ValidarPartidaNoIniciada("Sala2");
            });
            Assert.AreEqual("39", excepcion.Detail.Mensaje, "El mensaje de error no coincide con el esperado.");
        }
    }
}
