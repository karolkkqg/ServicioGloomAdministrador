using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.JugadorTest
{
    [TestClass()]
    public class AgregarJugadorABaseDeDatosTest
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
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosExitoso()
        {

             int filasAfectadas = AccesoJugador.AgregarJugadorABaseDeDatos(jugador);
            

            using (var contexto = new EntidadesGloom())
            {
                Assert.AreEqual(filasAfectadas, 1, "El número de filas afectadas no coincide");
               
            }
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosFallidoCorreoRepetido()
        {
            AccesoJugador.AgregarJugadorABaseDeDatos(jugador);
            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoJugador.AgregarJugadorABaseDeDatos(jugador);
            });
            Assert.AreEqual("2", exception.Detail.codigo);
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosFallidoNombreRepetido()
        {
            AccesoJugador.AgregarJugadorABaseDeDatos(jugador);
            var jugadorConNombreRepetido = new AccesoDatos.Jugador
            {
                NombreUsuario = "TacoDoradoDePato",
                Nombre = "Nombre2",
                Apellidos = "Apellido2",
                Correo = "correo3332@example.com",
                Contraseña = "Contraseña2",
                Tipo = "Tipo2",
                Icono = "Icono2"
            };

            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoJugador.AgregarJugadorABaseDeDatos(jugadorConNombreRepetido);
            });

            Assert.AreEqual("1", exception.Detail.codigo);


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

