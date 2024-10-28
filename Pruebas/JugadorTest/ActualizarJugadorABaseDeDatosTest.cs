using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.JugadorTest
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
            AccesoJugador.AgregarJugadorABaseDeDatos(jugador);
        }

        [TestMethod()]
        public void TestActualizarJugadorABaseDeDatosMismaInformacionExitoso()
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
                int filasAfectadas = AccesoJugador.ActualizarJugadorABaseDeDatos(jugador);

            using (var contexto = new EntidadesGloom())
            {
                Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide");
             
            }
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestActualizarJugadorABaseDeDatosDiferenteInformacionExitoso()
        {
           
                jugador = new AccesoDatos.Jugador
                {
                    NombreUsuario = "TacoDoradoDePato",
                    Nombre = "Gloria",
                    Apellidos = "Trevi",
                    Correo = "gloriaADIos@hotmail.com",
                    Contraseña = "123456",
                    Tipo = "Registrado",
                    Icono = "Icono3",
                };
                int filasAfectadas = AccesoJugador.ActualizarJugadorABaseDeDatos(jugador);

            Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide");
        
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
