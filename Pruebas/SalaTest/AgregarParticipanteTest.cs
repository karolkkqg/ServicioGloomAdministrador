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
    public class AgregarParticipanteTest
    {
        private BibliotecaClases.Sala nuevaSala;
        private BibliotecaClases.Sala nuevaParticipante;

        [TestInitialize]
        public void TestInitialize()
        {
            LimpiarDatosDePrueba();
            nuevaSala = new BibliotecaClases.Sala
            {
                idSala = "Omagaaaaa",
                nombreSala = "Sala de Juegos",
                tipoSala = "Normal",
                tipoPartida = "Pública",
                noJugadores = 4,
                codigo = "12345",
                idAdministrador = "TacoDePato",
                fecha = DateTime.Now.ToString(),
                ganador = "Jugador1"
            };
            AccesoSala.AgregarPartidaABaseDeDatos(nuevaSala);

            nuevaParticipante = new BibliotecaClases.Sala
            {
                idSala = "Omagaaaaa",
                jugador = "Jugador010"
            };
        }

        [TestMethod]
        public void TestAgregarSalaExitoso()
        {
            int filasAfectadas = AccesoSala.AgregarParticipante(nuevaParticipante);
            Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide.");

        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var participante = contexto.Participantes.FirstOrDefault(s => s.NombreUsuario == "Jugador010");
                if (participante != null)
                {
                    contexto.Participantes.Remove(participante);
                    contexto.SaveChanges();
                }

                var sala = contexto.Sala.FirstOrDefault(s => s.IdSala == "Omagaaaaa");
                if (sala != null)
                {
                    contexto.Sala.Remove(sala);
                    contexto.SaveChanges();
                }

            }
        }

    }
}
