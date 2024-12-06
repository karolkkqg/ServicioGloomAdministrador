using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;
using System.Linq;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ObtenerCodigoSalaTest
    {
        private BibliotecaClases.Sala sala;

        [TestInitialize]
        public void TestInitialize()
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var salaExistente = contexto.Sala.FirstOrDefault(p => p.NombreSala == "SalaTest");
                    if (salaExistente != null)
                    {
                        contexto.Sala.Remove(salaExistente);
                        contexto.SaveChanges();
                    }
                }

                sala = new BibliotecaClases.Sala
                {
                    nombreSala = "SalaTest",
                    tipoSala = "Normal",
                    tipoPartida = "Publica",
                    noJugadores = 4,
                    codigo = "99999",
                    idAdministrador = "AdminTest",
                    fecha = "01/12/2024",
                    ganador = "Ninguno",
                    idSala = "99999",
                };

                AccesoSala.AgregarPartidaABaseDeDatos(sala);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Excepción en TestInitialize: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestObtenerCodigoSalaExitoso()
        {
            var codigoObtenido = AccesoSala.BuscarCodigoSala(sala.idAdministrador, sala.nombreSala);

            Assert.IsNotNull(codigoObtenido, "El código de la sala no debería ser nulo.");
            Assert.AreEqual(sala.codigo.Trim(), codigoObtenido.Trim(), "El código obtenido no coincide con el esperado.");
        }

        [TestMethod]
        public void TestObtenerCodigoSalaFallidoAdministradorInvalido()
        {
            var administradorInvalido = "AdminInexistente";

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoSala.BuscarCodigoSala(administradorInvalido, sala.nombreSala);
            });

            Assert.AreEqual("Código no encontrado", excepcion.Detail.mensaje, "El mensaje de error no coincide con el esperado.");
        }

        [TestMethod]
        public void TestObtenerCodigoSalaFallidoNombreSalaInvalido()
        {
            var nombreSalaInvalido = "SalaInexistente";

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoSala.BuscarCodigoSala(sala.idAdministrador, nombreSalaInvalido);
            });

            Assert.AreEqual("Código no encontrado", excepcion.Detail.mensaje, "El mensaje de error no coincide con el esperado.");
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var sala = contexto.Sala.FirstOrDefault(p => p.NombreSala == "SalaTest");

                    if (sala != null)
                    {
                        contexto.Sala.Remove(sala);
                        contexto.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ClassCleanup: {ex.Message}");
            }
        }
    }
}
