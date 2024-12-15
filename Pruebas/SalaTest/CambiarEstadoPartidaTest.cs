using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Linq;
using System.ServiceModel;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class CambiarEstadoParaPartidaTest
    {
        private BibliotecaClases.Sala sala;

        [TestInitialize]
        public void TestInitialize()
        {
            using (var contexto = new EntidadesGloom())
            {
                var salaExistente = contexto.Sala.FirstOrDefault(s => s.IdSala == "12345" || s.NombreSala == "Redbull");
                if (salaExistente != null)
                {
                    contexto.Sala.Remove(salaExistente);
                    contexto.SaveChanges();
                }
            }

            sala = new BibliotecaClases.Sala
            {
                nombreSala = "Redbull",
                tipoSala = "Normal",
                tipoPartida = "Publica",
                noJugadores = 4,
                codigo = "12345",
                idAdministrador = "Pinku",
                fecha = "22/10/24",
                ganador = "Sin jugador",
                idSala = "12345"
            };

            AccesoSala.AgregarPartidaABaseDeDatos(sala);
        }

        [TestMethod]
        public void TestCambiarEstadoParaPartidaExitoso()
        {
           
            string nuevoGanador = "JugadorGanador";
            ServicioJuego servicio = new ServicioJuego(); 

            servicio.CambiarEstadoParaPartida(sala.idSala, nuevoGanador);

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
