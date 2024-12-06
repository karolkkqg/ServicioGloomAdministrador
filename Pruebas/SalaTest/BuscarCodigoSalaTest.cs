using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pruebas.SalaTest
{
    /// <summary>
    /// Descripción resumida de BuscarCodigoSalaTest
    /// </summary>
    /*[TestClass]
    public class BuscarCodigoSalaTest
    {
        private AccesoDatos.Sala sala;

        [TestInitialize]

        public void TestInitialize()
        {
            sala = new AccesoDatos.Sala
            {
                NombreSala = "Bellakos",
                TipoSala = "Normal",
                TipoPartida = "Publica",
                NoJugadores = 4,
                Codigo = "12345",
                IdAdministrador = "Pinku",
                Fecha = "22/10/24",
                Ganador = "Ninguno",
                IdSala = "12345",

            };

            AccesoSala.AgregarPartidaABaseDeDatos(sala);
        }

        [TestMethod()]
        public void TestBuscarCodigoSalaExitoso()
        {
            string sala = AccesoSala.BuscarCodigoSala("Pinku", "Bellakos");

            Assert.IsNotNull(sala, "El codigo no fue encontrado para este admisnitrador y nombre de sala");

            Assert.AreEqual("12345", sala.Trim(), "El codigo no coincide");
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
    }*/
}
