using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.SalaTest
{
    public class ObtenerParticipantesDeSalaTest
    {
        private BibliotecaClases.Sala nuevaSala;
        private BibliotecaClases.Sala nuevaParticipante;

        [TestInitialize]
        public void TestInitialize()
        {
            nuevaSala = new BibliotecaClases.Sala
            {
                idSala = "Sala0000",
                nombreSala = "Sala de Juegos",
                tipoSala = "Competitiva",
                tipoPartida = "Multijugador",
                noJugadores = 4,
                codigo = "1234",
                idAdministrador = "TacoDePato",
                fecha = DateTime.Now.ToString(),
                ganador = "Jugador1"
            };
            AccesoSala.AgregarPartidaABaseDeDatos(nuevaSala);

            nuevaParticipante = new BibliotecaClases.Sala
            {
                idSala = "Sala0000",
                jugador = "Jugador1"
            };
            AccesoSala.AgregarParticipante(nuevaParticipante);

            nuevaParticipante = new BibliotecaClases.Sala
            {
                idSala = "Sala0000",
                jugador = "Jugador2"
            };
            AccesoSala.AgregarParticipante(nuevaParticipante);
        }

        [TestMethod]
        public void TestObtenerAmigosDelJugadorExitoso()
        {
            var amigos = AccesoSala.ObtenerParticipantesDeSala("Sala0000");

            Assert.AreEqual(2, amigos.Count);
        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var participante = contexto.Participantes.FirstOrDefault(s => s.IdPartida == "Sala0000");
                if (participante != null)
                {
                    contexto.Participantes.Remove(participante);
                    contexto.SaveChanges();
                }

                var sala = contexto.Sala.FirstOrDefault(s => s.IdSala == "Sala0000");
                if (sala != null)
                {
                    contexto.Sala.Remove(sala);
                    contexto.SaveChanges();
                }

            }
        }
    }
}
