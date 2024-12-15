using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    [TestClass]
    public class ObtenerHistorialPartidasTest
    {

        private BibliotecaClases.Sala nuevaSala;
        private BibliotecaClases.Sala nuevaParticipante;

        [TestInitialize]
        public void TestInitialize()
        {
            nuevaSala = new BibliotecaClases.Sala
            {
                nombreSala = "Bellakos",
                tipoSala = "Normal",
                tipoPartida = "Publica",
                noJugadores = 4,
                codigo = "123450000",
                idAdministrador = "Pinku",
                fecha = "22/10/24",
                ganador = "Ninguno",
                idSala = "123450000",

            };
            AccesoSala.AgregarPartidaABaseDeDatos(nuevaSala);

            nuevaParticipante = new BibliotecaClases.Sala
            {
                idSala = "123450000",
                jugador = "Jugador1"
            };
            AccesoSala.AgregarParticipante(nuevaParticipante);
        }

        [TestMethod]
        public void TestObtenerHistorialDelJugadorExitoso()
        {
            var amigos = AccesoSala.ObtenerHistorialPartidas("Jugador1");

            Assert.AreEqual(1, amigos.Count);
        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var participante = contexto.Participantes.FirstOrDefault(s => s.IdPartida == "123450000");
                if (participante != null)
                {
                    contexto.Participantes.Remove(participante);
                    contexto.SaveChanges();
                }

                var sala = contexto.Sala.FirstOrDefault(s => s.IdSala == "123450000");
                if (sala != null)
                {
                    contexto.Sala.Remove(sala);
                    contexto.SaveChanges();
                }

            }
        }
    }
}
