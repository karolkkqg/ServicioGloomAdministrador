using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class IngresarJugadorAJuegoTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ServicioJuego.jugadoresEnSala.Clear();
            ServicioJuego.salaJugadoresPorSala.Clear();
            ServicioJuego.salaJugadores.Clear();
            ServicioJuego.personajesPorSala.Clear();
            ServicioJuego.familiasSeleccionadasPorSala.Clear();
            ServicioJuego.personajesFamiliaDeUsuario.Clear();
            ServicioJuego.salasActivasEnMemoria.Clear();
            ServicioJuego.administradoresDeSala.Clear();
        }

        [TestMethod]
        public void TestIngresarJugadorAJuego_Exitoso()
        {
            string numeroSala = "Sala123";
            string nombreUsuario = "Jugador1";
            int numeroJugadores = 4;

            ServicioJuego.jugadoresEnSala[numeroSala] = new HashSet<string>();
            ServicioJuego.jugadoresConectadosListos[numeroSala] = new List<string>();

            var servicio = new ServicioJuego();

            servicio.IngresarJugadorAJuego(nombreUsuario, numeroSala, numeroJugadores);

            Assert.IsTrue(ServicioJuego.jugadoresConectadosListos[numeroSala].Contains(nombreUsuario),
                "El jugador no se agregó a 'jugadoresConectadosListos' correctamente.");

            using (var contexto = new EntidadesGloom())
            {
                var participante = contexto.Participantes.FirstOrDefault(p => p.IdPartida == numeroSala && p.NombreUsuario == nombreUsuario);
                Assert.IsNotNull(participante, "El jugador no fue agregado a la base de datos correctamente.");
            }
        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var participantes = contexto.Participantes.Where(p => p.IdPartida == "Sala123").ToList();
                if (participantes.Any())
                {
                    contexto.Participantes.RemoveRange(participantes);
                    contexto.SaveChanges();
                }
            }
        }
    }
}
