using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccesoDatos;
using System.Linq;
using BibliotecaClases;
using System.ServiceModel;

namespace AccesoDatos.Tests
{
    [TestClass()]
    public class AgregarSolicitudAmistadTest
    {
        private AccesoDatos.Jugador jugador;
        private AccesoDatos.Jugador jugadorAmigo;

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

            jugadorAmigo = new AccesoDatos.Jugador
            {
                NombreUsuario = "AmigoDelTaco",
                Nombre = "Carlos",
                Apellidos = "Perez Ramirez",
                Correo = "amigodeltaco@gmail.com",
                Contraseña = "abcdef",
                Tipo = "Registrado",
                Icono = "Icono2",
            };
            AccesoJugador.AgregarJugadorABaseDeDatos(jugadorAmigo);
        }

        [TestMethod()]
        public void TestAgregarSolicitudAmistadExitoso()
        {
            Amistad solicitud = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "TacoDoradoDePato"
                },
                jugadorAmigo = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "AmigoDelTaco",
                },
                estado = "Pendiente"
            };

            int filasAfectadas = AccesoAmigos.AgregarSolcitudAmistad(solicitud);
            Assert.AreEqual(1, filasAfectadas, "El número de filas afectadas no coincide.");
        }

        [TestMethod()]
        public void TestAgregarSolicitudAmistadSolcitudPendienteFallido()
        {
            Amistad solicitud = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "TacoDoradoDePato"
                },
                jugadorAmigo = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "AmigoDelTaco",
                },
                estado = "Pendiente"
            };
            AccesoAmigos.AgregarSolcitudAmistad(solicitud);
            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoAmigos.AgregarSolcitudAmistad(solicitud);
            });
            Assert.AreEqual("5", exception.Detail.codigo);
            
        }

        [TestMethod()]
        public void TestAgregarSolicitudAmistadAmigosFallido()
        {
            Amistad solicitud = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "TacoDoradoDePato"
                },
                jugadorAmigo = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "AmigoDelTaco",
                },
                estado = "Aceptado"
            };
            AccesoAmigos.AgregarSolcitudAmistad(solicitud);
            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoAmigos.AgregarSolcitudAmistad(solicitud);
            });
            Assert.AreEqual("4", exception.Detail.codigo);

        }

        [TestMethod()]
        public void TestAgregarSolicitudMismoJugadorFallido()
        {
            Amistad solicitud = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "TacoDoradoDePato"
                },
                jugadorAmigo = new BibliotecaClases.Jugador
                {
                    nombreUsuario = "TacoDoradoDePato",
                },
                estado = "Aceptado"
            };
            var exception = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                AccesoAmigos.AgregarSolcitudAmistad(solicitud);
            });
            Assert.AreEqual("6", exception.Detail.codigo);

        }

        [TestCleanup]
        public void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var jugadorAmistad = contexto.Amigos.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");
                if (jugadorAmistad != null)
                {
                    contexto.Amigos.Remove(jugadorAmistad);
                    contexto.SaveChanges();
                }

                var jugador = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "TacoDoradoDePato");
                if (jugador != null)
                {
                    contexto.Jugador.Remove(jugador);
                    contexto.SaveChanges();
                }

                var jugadorAmigo = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "AmigoDelTaco");
                if (jugadorAmigo != null)
                {
                    contexto.Jugador.Remove(jugadorAmigo);
                    contexto.SaveChanges();
                }
            }
        }

        public static AccesoDatos.Amigos ConvertirASolicitud(BibliotecaClases.Amistad solicitud)
        {
            return new AccesoDatos.Amigos
            {
                NombreUsuario = solicitud.nombreUsuario.nombreUsuario,
                JugadorAmigo = solicitud.jugadorAmigo.nombreUsuario,
                Estado = solicitud.estado
            };
        }
    }
}