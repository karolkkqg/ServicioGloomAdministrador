using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class CrearPartidaTest
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
                idAdministrador = "Pinku",
                fecha = "22/10/24",
                ganador = "Ninguno"
            };
        }

        [TestMethod]
        public void TestCrearPartidaExitoso()
        {
            var servicioJuego = new ServicioJuego();

            int resultado = servicioJuego.CrearPartida(sala);

            using (var contexto = new EntidadesGloom())
            {
                var partidaRegistrada = contexto.Sala
                    .FirstOrDefault(p => p.NombreSala == sala.nombreSala);

                Assert.IsNotNull(partidaRegistrada, "La partida no fue registrada en la base de datos.");
                Assert.AreEqual(resultado, 1, "El número de filas afectadas no coincide.");
                Assert.AreEqual(partidaRegistrada.NombreSala, sala.nombreSala, "El nombre de la sala registrada no coincide.");
            }

            LimpiarDatosDePrueba();
        }

        [TestMethod]
        public void TestCrearPartidaFallidaNombreSalaRepetido()
        {
            var servicioJuego = new ServicioJuego();
            servicioJuego.CrearPartida(sala);

            var salaRepetida = new BibliotecaClases.Sala
            {
                nombreSala = "Bellakos",
                tipoSala = "Normal",
                tipoPartida = "Publica",
                noJugadores = 3,
                idAdministrador = "OtroAdmin",
                fecha = "23/10/24",
                ganador = "Ninguno"
            };

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.CrearPartida(salaRepetida);
            });

            Assert.AreEqual("8", excepcion.Detail.mensaje, "El código de la excepción no es el esperado.");

            LimpiarDatosDePrueba();
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var partidas = contexto.Sala
                    .Where(p => p.NombreSala == "Bellakos")
                    .ToList();

                if (partidas.Any())
                {
                    contexto.Sala.RemoveRange(partidas);
                    contexto.SaveChanges();
                }
            }
        }
    }
}
