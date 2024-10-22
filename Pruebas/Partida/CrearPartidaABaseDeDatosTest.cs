using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.Partida
{
    internal class CrearPartidaABaseDeDatosTest
    {
        public void TestInitialize()
        {
            partida = new AccesoDatos.Partida
            {
                IdPartida = "PAR05",
                Ganador = "cerezitaCool",
                Fecha = "26/10/24",
                IdAdministrador = "cerezitaCool",
                Tipo = "Normal",
                IdSala = "SAL04",
            }

    }

        [TestMethod()]
        public void TestInsertarPartidaABaseDeDatosExitoso()
        {
            int filasAfectadas = AccesoBaseDeDatos.AgregarPartidaABaseDeDatos(partida);

            using (var contexto = new EntidadesGloom())
            {
                Assert.AreEqual(filasAfectadas, 1, "El número de filas afectadas no coincide");

            }

            LimpiarDatosDePrueba();
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var partida = contexto.Partida
                        .FirstOrDefault(p => p.IdPartida == "PAR05");

                if (partida != null)
                {
                    contexto.Partida.Remove(partida);
                    contexto.SaveChanges();
                }

            }
        }

    }
}
