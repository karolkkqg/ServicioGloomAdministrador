using AccesoDatos;
using BlbibliotecaClases;
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

namespace ServicioGloom.Tests
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

             int filasAfectadas = AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
            

            using (var contexto = new EntidadesGloom())
            {
                Assert.AreEqual(filasAfectadas, 1, "El número de filas afectadas no coincide");
                /*
                var jugadorInsertado = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");

                Assert.IsNotNull(jugadorInsertado, "El jugador no fue encontrado en la base de datos");

                Assert.AreEqual("Hector", jugadorInsertado.Nombre, "El nombre del jugador no coincide");
                Assert.AreEqual("Juarez Castillo", jugadorInsertado.Apellidos, "Los apellidos del jugador no coinciden");
                Assert.AreEqual("hectJuarPato@gmail.com", jugadorInsertado.Correo, "El correo del jugador no coincide");
                Assert.AreEqual("Registrado", jugadorInsertado.Tipo, "El tipo de jugador no coincide");
                Assert.AreEqual("Icono1", jugadorInsertado.Icono, "El iconoc del jugador no coincide");
                */
            }
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosFallidoCorreoRepetido()
        {
            AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
            });
            Assert.AreEqual("2", exception.Detail.mensaje);
            LimpiarDatosDePrueba();
        }

        [TestMethod()]
        public void TestInsertarJugadorABaseDeDatosFallidoNombreRepetido()
        {
            AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugador);
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
                AccesoBaseDeDatos.AgregarJugadorABaseDeDatos(jugadorConNombreRepetido);
            });

            Assert.AreEqual("1", exception.Detail.mensaje);


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

