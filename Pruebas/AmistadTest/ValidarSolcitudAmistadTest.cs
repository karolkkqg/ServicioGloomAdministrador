using AccesoDatos;
using BibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.AmistadTest
{
    [TestClass]
<<<<<<<< HEAD:Pruebas/AmistadTest/ValidarSolicitudAmistadTest.cs
    public class ValidarSolicitudAmistadTest
========
    public class ValidarSolcitudAmistadTest
>>>>>>>> fc57b69e650943319e3eb2a4a59d7695bacdd073:Pruebas/AmistadTest/ValidarSolcitudAmistadTest.cs
    {
        private AccesoDatos.Jugador jugador;
        private AccesoDatos.Jugador jugadorAmigo;

        [TestInitialize]
        public void TestInitialize()
        {
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
        }

        [TestMethod]
        public void TestCambiarEstadoSolicitudAceptadoExitoso()
        {
            Amistad solicitud = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador { nombreUsuario = "UsuarioTest1" },
                jugadorAmigo = new BibliotecaClases.Jugador { nombreUsuario = "UsuarioTest2" },
                estado = "Pendiente"
            };
            AccesoAmigos.AgregarSolcitudAmistad(solicitud);

            solicitud.estado = "Aceptado";
            int filasAfectadas = AccesoAmigos.CambiarEstadoSolicitud(solicitud);

            Assert.AreEqual(1, filasAfectadas, "El estado de la solicitud no fue actualizado correctamente.");
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
