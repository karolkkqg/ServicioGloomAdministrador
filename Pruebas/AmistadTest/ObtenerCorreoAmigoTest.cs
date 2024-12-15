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
    public class ObtenerCorreoAmigoTest
    {
        private AccesoDatos.Jugador jugador;
        private AccesoDatos.Jugador jugadorAmigo;

        [TestInitialize]
        public void TestInitialize()
        {

            jugador = new AccesoDatos.Jugador
            {
                NombreUsuario = "Pinku",
                Nombre = "Sabrina",
                Apellidos = "Ionescu",
                Correo = "exoypurkings@gmail.com",
                Contraseña = "123456",
                Tipo = "Registrado",
                Icono = "Icono1",
            };
            AccesoJugador.AgregarJugadorABaseDeDatos(jugador);

            jugadorAmigo = new AccesoDatos.Jugador
            {
                NombreUsuario = "Doriana",
                Nombre = "Breanna",
                Apellidos = "Stewart",
                Correo = "doristodoriana@gmail.com",
                Contraseña = "abcdef",
                Tipo = "Registrado",
                Icono = "Icono2",
            };
            AccesoJugador.AgregarJugadorABaseDeDatos(jugadorAmigo);


            Amistad solicitud = new Amistad
            {
                nombreUsuario = new BibliotecaClases.Jugador { nombreUsuario = "Pinku" },
                jugadorAmigo = new BibliotecaClases.Jugador { nombreUsuario = "Doriana" },
                estado = "Aceptado"
            };
            AccesoAmigos.AgregarSolcitudAmistad(solicitud);

        }


        [TestMethod()]
        public void TestObteneCorreoAmigoExitoso()
        {
            string correo = AccesoAmigos.BuscarCorreoAmigo("Doriana");

            Assert.IsNotNull(correo, "El correo no fue encontrado para este jugador amigo");

            Assert.AreEqual("doristodoriana@gmail.com", correo, "El correo no coincide");

            //LimpiarDatosDePrueba();
        }

        [ClassCleanup]
        public static void LimpiarDatosDePrueba()
        {
            using (var contexto = new EntidadesGloom())
            {
                var amistad = contexto.Amigos.FirstOrDefault(a => a.NombreUsuario == "Pinku");
                if (amistad != null)
                {
                    contexto.Amigos.Remove(amistad);
                    contexto.SaveChanges();
                }

                var jugador1 = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "Pinku");
                if (jugador1 != null)
                {
                    contexto.Jugador.Remove(jugador1);
                    contexto.SaveChanges();
                }

                var jugador2 = contexto.Jugador.FirstOrDefault(j => j.NombreUsuario == "Doriana");
                if (jugador2 != null)
                {
                    contexto.Jugador.Remove(jugador2);
                    contexto.SaveChanges();
                }
            }
        }
    }
}