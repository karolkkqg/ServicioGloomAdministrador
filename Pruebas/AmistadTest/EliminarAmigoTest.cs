using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System.Linq;

namespace Pruebas.AmistadTest
{
    [TestClass]
    public class ArchivarAmistadTest
    {
        private AccesoDatos.Jugador jugador;
        private AccesoDatos.Jugador jugadorAmigo;

        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void TestInitialize()
        {

            servicioJuego = new ServicioJuego();
           
            jugador = new AccesoDatos.Jugador
            {
                NombreUsuario = "UsuarioTest1",
                Nombre = "Juan",
                Apellidos = "Lopez",
                Correo = "juanlopez@gmail.com",
                Contraseña = "123456",
                Tipo = "Registrado",
                Icono = "Icono1"
            };
            AccesoJugador.AgregarJugadorABaseDeDatos(jugador);

            jugadorAmigo = new AccesoDatos.Jugador
            {
                NombreUsuario = "UsuarioTest2",
                Nombre = "Pedro",
                Apellidos = "Martinez",
                Correo = "pedromartinez@gmail.com",
                Contraseña = "abcdef",
                Tipo = "Registrado",
                Icono = "Icono2"
            };
            AccesoJugador.AgregarJugadorABaseDeDatos(jugadorAmigo);

            var amistad = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador { nombreUsuario = jugador.NombreUsuario },
                jugadorAmigo = new BibliotecaClases.Jugador { nombreUsuario = jugadorAmigo.NombreUsuario },
                estado = "Aceptado"
            };
            AccesoAmigos.AgregarSolcitudAmistad(amistad);
        }

        [TestMethod]
        public void ArchivarAmistad_EliminaAmistadCorrectamente()
        {
            var solicitud = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador { nombreUsuario = jugador.NombreUsuario },
                jugadorAmigo = new BibliotecaClases.Jugador { nombreUsuario = jugadorAmigo.NombreUsuario }
            };

            int filasAfectadas = servicioJuego.ArchivarAmistad(solicitud);

            Assert.AreEqual(1, filasAfectadas, "La amistad no fue eliminada correctamente.");

            using (var contexto = new EntidadesGloom())
            {
                var amistadEliminada = contexto.Amigos
                    .FirstOrDefault(a => a.NombreUsuario == jugador.NombreUsuario && a.JugadorAmigo == jugadorAmigo.NombreUsuario);
                Assert.IsNull(amistadEliminada, "La amistad no fue eliminada de la base de datos.");
            }
        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var amistad = contexto.Amigos.FirstOrDefault(a => a.NombreUsuario == "UsuarioTest1");
                if (amistad != null)
                {
                    contexto.Amigos.Remove(amistad);
                    contexto.SaveChanges();
                }

                var jugador = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "UsuarioTest1");
                if (jugador != null)
                {
                    contexto.Jugador.Remove(jugador);
                    contexto.SaveChanges();
                }

                var jugadorAmigo = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "UsuarioTest2");
                if (jugadorAmigo != null)
                {
                    contexto.Jugador.Remove(jugadorAmigo);
                    contexto.SaveChanges();
                }
            }
        }
    }
}
