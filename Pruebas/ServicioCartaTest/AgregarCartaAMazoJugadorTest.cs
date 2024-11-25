using BibliotecaClases;
using BlbibliotecaClases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioGloomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.ServicioCartaTest
{
    [TestClass]
    public class AgregarCartaAMazoJugadorTest
    {
        private ServicioJuego servicioJuego;

        [TestInitialize]
        public void SetUp()
        {
            servicioJuego = new ServicioJuego();

            ServicioJuego.barajaJugadores.Clear();
            ServicioJuego.cartasSobrantesGlobal.Clear();

            ServicioJuego.barajaJugadores.Add("Jugador1", new List<Carta>());
            ServicioJuego.barajaJugadores["Jugador1"].Add(new Carta { identificador = "Carta1" });

            ServicioJuego.cartasSobrantesGlobal.Add(new Carta { identificador = "CartaSobrante1" });
            ServicioJuego.cartasSobrantesGlobal.Add(new Carta { identificador = "CartaSobrante2" });
        }

        [TestMethod]
        public void AgregarCartaAMazoJugadorExitoso()
        {
            servicioJuego.AgregarCartaAMazoJugador("Jugador1");

            Assert.AreEqual(2, ServicioJuego.barajaJugadores["Jugador1"].Count, "La carta no fue agregada correctamente al mazo del jugador.");
            Assert.AreEqual("CartaSobrante1", ServicioJuego.barajaJugadores["Jugador1"][1].identificador, "La carta agregada no es la esperada.");
            Assert.AreEqual(1, ServicioJuego.cartasSobrantesGlobal.Count, "La carta no fue removida correctamente de las cartas sobrantes.");
        }

        [TestMethod]
        public void AgregarCartaAMazoJugadorFallaPorDemasiadasCartas()
        {
            ServicioJuego.barajaJugadores["Jugador1"].AddRange(new List<Carta>
            {
                new Carta { identificador = "CartaExtra1" },
                new Carta { identificador = "CartaExtra2" },
                new Carta { identificador = "CartaExtra3" },
                new Carta { identificador = "CartaExtra4" },
                new Carta { identificador = "CartaExtra5" },
                new Carta { identificador = "CartaExtra6" },
                new Carta { identificador = "CartaExtra7" }
            });

            var excepcion = Assert.ThrowsException<FaultException<ManejadorExcepciones>>(() =>
            {
                servicioJuego.AgregarCartaAMazoJugador("Jugador1");
            });

            Assert.AreEqual("20", excepcion.Detail.mensaje, "El mensaje de error no coincide con el esperado.");
        }
    }
}
