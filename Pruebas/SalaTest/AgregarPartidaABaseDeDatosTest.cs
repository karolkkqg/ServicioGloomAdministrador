using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.ServiceModel;

namespace Pruebas
{
    [TestClass]
    public class AgregarPartidaABaseDeDatosTest
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
        }

        [TestMethod()]
        public void TestRegistrarPartidaABaseDeDatosExitoso()
        {
            int filasAfectadas = AccesoSala.AgregarPartidaABaseDeDatos(sala);

            using (var contexto = new EntidadesGloom())
            {
                Assert.AreEqual(filasAfectadas, 1, "El número de filas afectadas no coincide");
            }

            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestRegistrarPartidaABaseDeDatosFallidoNombreSalaRepetido()
        {
            AccesoSala.AgregarPartidaABaseDeDatos(sala);

            var nombreSalaRepetido = new AccesoDatos.Sala
            {
                NombreSala = "Bellakos",
                TipoSala = "Normal",
                TipoPartida = "Publica",
                NoJugadores = 3,
                Codigo = "12346",
                IdAdministrador = "PinkuKozakura",
                Fecha = "23/10/24",
                Ganador = "Ninguno",
                IdSala = "12346",
            };

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoSala.AgregarPartidaABaseDeDatos(sala);
            });
            Assert.AreEqual("8", excepcion.Detail.mensaje);

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