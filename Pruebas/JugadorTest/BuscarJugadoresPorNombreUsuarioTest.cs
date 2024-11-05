using AccesoDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.JugadorTest
{
    internal class BuscarJugadoresPorNombreUsuarioTest
    {
        [TestClass()]
        public class BuscarJugadoresPorNombreUsuarioParcialTest
        {
            private AccesoDatos.Jugador jugador1;
            private AccesoDatos.Jugador jugador2;

            [TestInitialize]
            public void TestInitialize()
            {
                jugador1 = new AccesoDatos.Jugador
                {
                    NombreUsuario = "TacoDoradoDePato",
                    Nombre = "Hector",
                    Apellidos = "Juarez Castillo",
                    Correo = "hectJuarPato@gmail.com",
                    Contraseña = "123456",
                    Tipo = "Registrado",
                    Icono = "Icono1",
                };
                AccesoJugador.AgregarJugadorABaseDeDatos(jugador1);

                jugador2 = new AccesoDatos.Jugador
                {
                    NombreUsuario = "AmigoDelTaco",
                    Nombre = "Carlos",
                    Apellidos = "Perez Ramirez",
                    Correo = "amigodeltaco@gmail.com",
                    Contraseña = "abcdef",
                    Tipo = "Registrado",
                    Icono = "Icono2",
                };
                AccesoJugador.AgregarJugadorABaseDeDatos(jugador2);
            }

            [TestMethod()]
            public void TestBuscarJugadoresPorNombreUsuarioParcialExitoso()
            {
                var jugadores = AccesoJugador.BuscarJugadoresPorNombreUsuario("ta");

                Assert.IsNotNull(jugadores, "No se encontraron jugadores con esa coincidencia.");
                Assert.AreEqual(2, jugadores.Count, "El número de jugadores encontrados no es correcto.");

                var jugadorEncontrado1 = jugadores.FirstOrDefault(j => j.nombreUsuario == "TacoDoradoDePato");
                var jugadorEncontrado2 = jugadores.FirstOrDefault(j => j.nombreUsuario == "AmigoDelTaco");

                Assert.IsNotNull(jugadorEncontrado1, "El jugador 'TacoDoradoDePato' no fue encontrado.");
                Assert.IsNotNull(jugadorEncontrado2, "El jugador 'AmigoDelTaco' no fue encontrado.");

                Assert.AreEqual("Hector", jugadorEncontrado1.nombre, "El nombre del jugador 1 no coincide.");
                Assert.AreEqual("Juarez Castillo", jugadorEncontrado1.apellidos, "Los apellidos del jugador 1 no coinciden.");
                Assert.AreEqual("hectJuarPato@gmail.com", jugadorEncontrado1.correo, "El correo del jugador 1 no coincide.");

                Assert.AreEqual("Carlos", jugadorEncontrado2.nombre, "El nombre del jugador 2 no coincide.");
                Assert.AreEqual("Perez Ramirez", jugadorEncontrado2.apellidos, "Los apellidos del jugador 2 no coinciden.");
                Assert.AreEqual("amigodeltaco@gmail.com", jugadorEncontrado2.correo, "El correo del jugador 2 no coincide.");

                LimpiarDatosDePrueba();
            }

            [ClassCleanup]
            public static void LimpiarDatosDePrueba()
            {
                using (var contexto = new EntidadesGloom())
                {
                    var jugador1 = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");
                    if (jugador1 != null)
                    {
                        contexto.Jugador.Remove(jugador1);
                        contexto.SaveChanges();
                    }

                    var jugador2 = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "AmigoDelTaco");
                    if (jugador2 != null)
                    {
                        contexto.Jugador.Remove(jugador2);
                        contexto.SaveChanges();
                    }
                }
            }
        }
    }
}
