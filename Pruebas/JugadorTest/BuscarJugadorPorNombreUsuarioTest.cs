using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.JugadorTest
{
    internal class BuscarJugadorPorNombreUsuarioTest
    {
        [TestClass()]
        public class ActualizarJugadorABaseDeDatosTest
        {
            private AccesoDatos.Jugador jugador;

            [TestInitialize]
            public void TestInitialize()
            {
                jugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = "TacoDoradoDePato",
                    Nombre = "Hector",
                    Apellidos = "Juarez Castillo",
                    Correo = "hectJuarPato@gmail.com",
                    Contraseña = "123456",
                    Tipo = "Registrado",
                    Icono = "Icono1",
                };
                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
            }

            [TestMethod()]
            public void TestBuscarJugadorPorNombreUsuarioExitoso()
            {
                Jugador jugador = AccesoBaseDeDatos.BuscarJugadorPorNombreUsuario("TacoDoradoDePato");


                Assert.IsNotNull(jugador, "El jugador no fue encontrado en la base de datos");

                Assert.AreEqual("Hector", jugador.Nombre, "El nombre del jugador no coincide");
                Assert.AreEqual("Juarez Castillo", jugador.Apellidos, "Los apellidos del jugador no coinciden");
                Assert.AreEqual("hectJuarPato@gmail.com", jugador.Correo, "El correo del jugador no coincide");
                Assert.AreEqual("Registrado", jugador.Tipo, "El tipo de jugador no coincide");
                Assert.AreEqual("Icono1", jugador.Icono, "El iconoc del jugador no coincide");
                LimpiarDatosDePrueba();

            }

            [ClassCleanup]
            public static void LimpiarDatosDePrueba()
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugador = contexto.Jugador
                        .FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");

                    if (jugador != null)
                    {
                        contexto.Jugador.Remove(jugador);
                        contexto.SaveChanges();
                    }

                }
            }
        }
    }
}
