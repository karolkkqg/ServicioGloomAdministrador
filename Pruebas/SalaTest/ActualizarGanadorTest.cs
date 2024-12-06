using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ActualizarGanadorTest
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

        [TestMethod]
        public void TestActualizarGanadorExitoso()
        {
            string nuevoGanador = "JugadorGanador";
            int filasAfectadas = AccesoSala.ActualizarGanador(sala.idSala, nuevoGanador);

            Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide.");

            using (var contexto = new EntidadesGloom())
            {
                var salaActualizada = contexto.Sala.FirstOrDefault(s => s.IdSala == sala.idSala);
                Assert.IsNotNull(salaActualizada, "La sala no se encontró en la base de datos.");
                Assert.AreEqual(nuevoGanador, salaActualizada.Ganador, "El ganador no se actualizó correctamente.");
            }
        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var salaPrueba = contexto.Sala.FirstOrDefault(s => s.IdSala == sala.idSala);
                if (salaPrueba != null)
                {
                    contexto.Sala.Remove(salaPrueba);
                    contexto.SaveChanges();
                }
            }
        }
    }
}
