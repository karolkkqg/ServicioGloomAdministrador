using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class BuscarSalaExistenteTest
    {
        private BibliotecaClases.Sala sala;

        [TestInitialize]

        public void TestInitialize()
        {
            sala = new BibliotecaClases.Sala
            {
                nombreSala = "Bellakos",
                tipoSala = "Normal",
                tipoPartida = "Publica",
                noJugadores = 4,
                codigo = "12345",
                idAdministrador = "Pinku",
                fecha = "22/10/24",
                ganador = "Ninguno",
                idSala = "12345",


            };
            AccesoSala.AgregarPartidaABaseDeDatos(sala);
        }

        [TestMethod()]
        public void TestBuscarpartidaExitoso()
        {
            var resultado= AccesoSala.BuscarPartida("Bellakos", "12345");

           Assert.IsNotNull(resultado);

            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestBuscarPartidasCodigoIncorrectoFallido()
        {
            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoSala.BuscarPartida("Bellakos", "654321");
            });
            Assert.AreEqual("10", excepcion.Detail.codigo);

            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestBuscarPartidasIdIncorrectoFallido()
        {
            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoSala.BuscarPartida("654321", "12345");
            });
            Assert.AreEqual("10", excepcion.Detail.codigo);

            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestBuscarPartidasIdyCodioIncorrectoFallido()
        {
            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoSala.BuscarPartida("654321", "654321");
            });
            Assert.AreEqual("10", excepcion.Detail.codigo);

            LimpiarDatosDePrueba();
        }


        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var sala = contexto.Sala
                    .FirstOrDefault(p => p.NombreSala == "Bellakos");

                if (sala != null)
                {
                    contexto.Sala.Remove(sala);
                    contexto.SaveChanges();
                }

            }
        }
    }
}
