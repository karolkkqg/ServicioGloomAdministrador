using AccesoDatos;
using BibliotecaClases;
using ServicioGlomm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{

    public partial class ServicioJuego : ISala
    {
        public static readonly Dictionary<string, Dictionary<string, ISalaCallback>> salaJugadoresPorSala = new Dictionary<string, Dictionary<string, ISalaCallback>>();
        public static readonly Dictionary<string, List<string>> salaJugadores = new Dictionary<string, List<string>>();
        public static readonly Dictionary<string, Dictionary<string, (string nombrePersonaje, int vida)>> personajesPorSala = new Dictionary<string, Dictionary<string, (string, int)>>();
        public static readonly Dictionary<string, List<string>> personajesUsadosPorSala = new Dictionary<string, List<string>>();
        private static readonly Dictionary<string, HashSet<string>> jugadoresEnSala = new Dictionary<string, HashSet<string>>();
        private static readonly Dictionary<string, ISalaCallback> usuariosSalaCallback = new Dictionary<string, ISalaCallback>();
        private static readonly Dictionary<string, BibliotecaClases.Sala> salasActivasEnMemoria = new Dictionary<string, BibliotecaClases.Sala>();


        public int AgregarParticipantesAPartida(BibliotecaClases.Sala sala)
        {
            int resultado = AccesoSala.AgregarParticipante(sala);
            return resultado;
        }

        public int CrearPartida(BibliotecaClases.Sala sala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                String codigoGenerado = GenerarCodigo();
                sala.codigo = codigoGenerado;
                sala.idSala = codigoGenerado;

                var nuevaPartida = new BibliotecaClases.Sala
                {
                    nombreSala = sala.nombreSala,
                    tipoSala = sala.tipoSala,
                    tipoPartida = sala.tipoPartida,
                    noJugadores = sala.noJugadores,
                    codigo = sala.codigo,
                    idAdministrador = sala.idAdministrador,
                    fecha = sala.fecha,
                    ganador = sala.ganador,
                    idSala = sala.idSala,

                };


                int resultado = AccesoSala.AgregarPartidaABaseDeDatos(nuevaPartida);
                String mensaje = "Partida creada " + sala.nombreSala;
                return resultado;
            }


            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        private string GenerarCodigo()
        {
            string codigoGenerado;

            do
            {
                string caracteresPermitidos = "0123456789";
                Random random = new Random();

                codigoGenerado = new string(Enumerable.Repeat(caracteresPermitidos, 5)
                    .Select(selection => selection[random.Next(selection.Length)]).ToArray());


            } while (!CodigoValido(codigoGenerado));
            return codigoGenerado;
        }

        private bool CodigoValido(String codigo)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            bool valido = false;
            try
            {
                using (var contexto = new EntidadesGloom())
                {
                    var codigoExistente = contexto.Sala.FirstOrDefault(salaCodigo => salaCodigo.Codigo == codigo);
                    if (codigoExistente == null)
                    {
                        valido = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                valido = false;
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("9"));

            }

            return valido;
        }

        public void ConectarConSala(string numeroSala, string nombreUsuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (!salaJugadoresPorSala.ContainsKey(numeroSala))
            {
                salaJugadoresPorSala[numeroSala] = new Dictionary<string, ISalaCallback>();
            }
            if (!salaJugadoresPorSala[numeroSala].ContainsKey(nombreUsuario))
            {
                ISalaCallback callback = OperationContext.Current.GetCallbackChannel<ISalaCallback>();
                salaJugadoresPorSala[numeroSala].Add(nombreUsuario, callback);

                foreach (var jugador in salaJugadoresPorSala[numeroSala])
                {
                    try
                    {
                        jugador.Value.ActualizarNumeroJugadores();
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        salaJugadoresPorSala[numeroSala].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        public void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            EstaSiendoUtilizado(numeroSala, nombrePersonaje);

            string personajeAnterior = AgregarJugadorYPersonaje(nombreUsuario, nombrePersonaje, numeroSala);

            ValidarJugadorNoListo(nombreUsuario);

            if (salaJugadoresPorSala.ContainsKey(numeroSala))
            {
                foreach (var jugador in salaJugadoresPorSala[numeroSala])
                {
                    try
                    {
                        jugador.Value.ActualizarImagenPersonaje(nombrePersonaje, personajeAnterior);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[numeroSala].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        private string AgregarJugadorYPersonaje(string nombreUsuario, string nombrePersonaje, string numeroSala)
        {
            InicializarSalaSiNoExiste(numeroSala);

            string personajeAnterior = ActualizarPersonajeAnterior(nombreUsuario, nombrePersonaje, numeroSala);
            AgregarPersonajeUsado(nombrePersonaje, personajesUsadosPorSala[numeroSala]);

            return personajeAnterior;
        }

        private void AgregarPersonajeUsado(string nombrePersonaje, List<string> personajesUsados)
        {
            if (!personajesUsados.Contains(nombrePersonaje))
            {
                personajesUsados.Add(nombrePersonaje);
            }
        }

        private void InicializarSalaSiNoExiste(string numeroSala)
        {
            if (!personajesPorSala.ContainsKey(numeroSala))
            {
                personajesPorSala[numeroSala] = new Dictionary<string, (string nombrePersonaje, int vida)>();
            }

            if (!personajesUsadosPorSala.ContainsKey(numeroSala))
            {
                personajesUsadosPorSala[numeroSala] = new List<string>();
            }
        }

        private string ActualizarPersonajeAnterior(string nombreUsuario, string nombrePersonaje, string numeroSala)
        {
            var personajesEnSala = personajesPorSala[numeroSala];
            var personajesUsados = personajesUsadosPorSala[numeroSala];

            string personajeAnterior = "sin personaje";

            if (personajesEnSala.ContainsKey(nombreUsuario))
            {
                personajeAnterior = personajesEnSala[nombreUsuario].nombrePersonaje;
                personajesEnSala[nombreUsuario] = (nombrePersonaje, 0);

                if (personajesUsados.Contains(personajeAnterior))
                {
                    personajesUsados.Remove(personajeAnterior);
                }
            }
            else
            {
                personajesEnSala[nombreUsuario] = (nombrePersonaje, 0);
            }

            return personajeAnterior;
        }

        private void ValidarJugadorNoListo(string nombreUsuario)
        {
            if (jugadoresConectadosListos.ContainsKey(nombreUsuario))
            {
                jugadoresConectadosListos.Remove(nombreUsuario);
            }

        }

        private void EstaSiendoUtilizado(string numeroSala, string nombrePersonaje)
        {
            if (personajesUsadosPorSala[numeroSala].Contains(nombrePersonaje))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("14"));
            }
            else
            {
                personajesUsadosPorSala[numeroSala].Add(nombrePersonaje);
            }
        }

        public void ValidarPersonajesSeleccionados(string numeroSala, int cantidadJugadores)
        {
            if (cantidadJugadores != personajesUsadosPorSala[numeroSala].Count())
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("15"));
            }
        }

        public Dictionary<string, (string nombrePersonaje, int vida)> ObtenerUsuariosYPersonajes(string numeroSala)
        {
            return new Dictionary<string, (string nombrePersonaje, int vida)>(personajesPorSala[numeroSala]);
        }

        public void LimpiarListaJugadores()
        {
            foreach (var sala in personajesPorSala)
            {
                sala.Value.Clear();
            }
        }

        public void LimpiarListaPersonajes()
        {
            personajesUsadosPorSala.Clear();
        }

        public void EmpezarPartida(string idSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (salaJugadoresPorSala.ContainsKey(idSala))
            {
                foreach (var jugador in salaJugadoresPorSala[idSala])
                {
                    try
                    {
                        jugador.Value.EmpezarJuego();
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[idSala].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[idSala].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        public void SacarDeSala(string numeroSala, string nombreUsuario)
        {
            if (salaJugadores.ContainsKey(numeroSala) && salaJugadores[numeroSala].Contains(nombreUsuario))
            {
                salaJugadores[numeroSala].Remove(nombreUsuario);

                if (salaJugadores[numeroSala].Count == 0)
                {
                    salaJugadores.Remove(numeroSala);
                }
            }
            EliminarJugadorDeSala(numeroSala, nombreUsuario);

            EliminarJugadorYActualizarPersonaje(numeroSala, nombreUsuario);
        }

        private void EliminarJugadorYActualizarPersonaje(string numeroSala, string nombreUsuario)
        {
            if (personajesPorSala.ContainsKey(numeroSala) && personajesPorSala[numeroSala].ContainsKey(nombreUsuario))
            {
                var personajeSeleccionado = personajesPorSala[numeroSala][nombreUsuario].nombrePersonaje;
                if (!string.IsNullOrEmpty(personajeSeleccionado))
                {
                    string personaje = EliminarJugadorYPersonaje(numeroSala, nombreUsuario);
                    EliminarPersonajeUsado(numeroSala, personaje);
                }
            }
        }

        public void EliminarJugadorDeSala(string idSala, string nombreUsuario)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (salaJugadoresPorSala.ContainsKey(idSala))
            {
                if (salaJugadoresPorSala[idSala].Remove(nombreUsuario))
                {
                    foreach (var jugador in salaJugadoresPorSala[idSala])
                    {
                        try
                        {
                            jugador.Value.ActualizarNumeroJugadores();
                        }
                        catch (CommunicationException ex)
                        {
                            administradorLogger.RegistroError(ex);

                            salaJugadoresPorSala[idSala].Remove(jugador.Key);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                        }
                        catch (TimeoutException ex)
                        {
                            administradorLogger.RegistroError(ex);

                            salaJugadoresPorSala[idSala].Remove(jugador.Key);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                        }
                    }

                    if (salaJugadoresPorSala[idSala].Count == 0)
                    {
                        salaJugadoresPorSala.Remove(idSala);
                    }
                }

            }
        }

        private void EliminarPersonajeUsado(string numeroSala, string nombrePersonaje)
        {
            if (personajesUsadosPorSala.ContainsKey(numeroSala))
            {
                if (personajesUsadosPorSala[numeroSala].Contains(nombrePersonaje))
                {
                    personajesUsadosPorSala[numeroSala].Remove(nombrePersonaje);
                }

                if (personajesUsadosPorSala[numeroSala].Count == 0)
                {
                    personajesUsadosPorSala.Remove(numeroSala);
                }
            }
        }

        private string EliminarJugadorYPersonaje(string numeroSala, string nombreUsuario)
        {
            (string nombrePersonaje, int vida) personaje = personajesPorSala[numeroSala][nombreUsuario];


            personajesPorSala[numeroSala].Remove(nombreUsuario);

            if (personajesPorSala[numeroSala].Count == 0)
            {
                personajesPorSala.Remove(numeroSala);
            }

            return personaje.nombrePersonaje;
        }


        public List<string> ObtenerPersonajesUsados(string numeroSala)
        {
            return new List<string>(personajesUsadosPorSala[numeroSala]);
        }

        public void SumarVidaPersonaje(string numeroSala, string nombreUsuario, int cantidadVida)
        {
            if (personajesPorSala.TryGetValue(numeroSala, out var personajesEnSala) && personajesEnSala.ContainsKey(nombreUsuario))
            {
                var (nombrePersonaje, vidaActual) = personajesEnSala[nombreUsuario];
                int nuevaVida = vidaActual + cantidadVida;
                personajesEnSala[nombreUsuario] = (nombrePersonaje, nuevaVida);
            }
        }

        private string ObtenerGanador(string numeroSala)
        {
            var personajesEnSala = personajesPorSala[numeroSala];
            var personajeConMenorVida = personajesEnSala.OrderBy(p => p.Value.vida).First();

            return personajeConMenorVida.Key;
        }

        public void SacarATodosLosJugadoresDeSala(string numeroSala)
        {
            var jugadores = salaJugadoresPorSala[numeroSala].Keys.ToList();

            foreach (var jugador in jugadores)
            {
                SacarJugadorDeSala(numeroSala, jugador);
            }
        }

        private void SacarJugadoresFinalPartidaMini(string numeroSala)
        {
            var jugadores = salaJugadoresPorSala[numeroSala].Keys.ToList();

            foreach (var jugador in jugadores)
            {
                EliminarEstructurasSala(numeroSala, jugador);
            }
        }

        private void EliminarEstructurasSala(string numeroSala, string jugador)
        {
            if (salaJugadoresPorSala.ContainsKey(numeroSala))
            {
                if (salaJugadoresPorSala[numeroSala].ContainsKey(jugador))
                {
                    EliminarJugadorYActualizarPersonaje(numeroSala, jugador);

                    salaJugadoresPorSala[numeroSala].Remove(jugador);

                    if (salaJugadoresPorSala[numeroSala].Count == 0)
                    {
                        salaJugadoresPorSala.Remove(numeroSala);
                    }
                }
            }
        }

        private void SacarJugadorDeSala(string numeroSala, string jugador)
        {
            if (salaJugadoresPorSala.ContainsKey(numeroSala))
            {
                if (salaJugadoresPorSala[numeroSala].ContainsKey(jugador))
                {
                    EliminarJugadorYActualizarPersonaje(numeroSala, jugador);

                    RedirigirJugador(numeroSala, jugador);

                    salaJugadoresPorSala[numeroSala].Remove(jugador);

                    if (salaJugadoresPorSala[numeroSala].Count == 0)
                    {
                        salaJugadoresPorSala.Remove(numeroSala);
                    }
                }
            }
        }
        private void RedirigirJugador(string sala, string jugador)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            if (salaJugadoresPorSala.ContainsKey(sala))
            {
                if (salaJugadoresPorSala[sala].ContainsKey(jugador))
                {
                    try
                    {
                        salaJugadoresPorSala[sala][jugador].SacarDeSalaATodosJugadores();
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[sala].Remove(jugador);

                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[sala].Remove(jugador);

                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        public List<BibliotecaClases.Sala> ObtenerSalasActivas()
        {
            AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            return salasActivasEnMemoria.Values.ToList();
        }

        public List<BibliotecaClases.Sala> ObtenerSalasActivasConEstado()
        {
            AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            var listaSalasActivas = new List<BibliotecaClases.Sala>();

            foreach (var sala in salasActivasEnMemoria.Values)
            {
                if (jugadoresEnSala.ContainsKey(sala.idSala))
                {
                    sala.noJugadores = jugadoresEnSala[sala.idSala].Count;
                }
                listaSalasActivas.Add(sala);
            }

            return listaSalasActivas;
        }

        public void UnirseASalaPublica(string idSala, string idUsuario)
        {
            AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            if (!salasActivasEnMemoria.TryGetValue(idSala, out var sala) || sala.tipoPartida != "Publica")
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("23"));
            }

            if (!jugadoresEnSala.ContainsKey(idSala))
            {
                jugadoresEnSala[idSala] = new HashSet<string>();
            }
            jugadoresEnSala[idSala].Add(idUsuario);
            usuariosSalaCallback[idUsuario] = OperationContext.Current.GetCallbackChannel<ISalaCallback>();
            ActualizarSalasParaTodos();

            NotificarResultadoUnirseASala(idUsuario, idSala, true);
        }

        public void UnirseASalaPrivada(string idUsuario, string idSala, string codigoAcceso)
        {
            AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            if (!salasActivasEnMemoria.TryGetValue(idSala, out var sala) || sala.codigo != codigoAcceso)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("24"));
            }

            if (sala.codigo == codigoAcceso)
            {
                if (!jugadoresEnSala.ContainsKey(idSala))
                {
                    jugadoresEnSala[idSala] = new HashSet<string>();
                }
                jugadoresEnSala[idSala].Add(idUsuario);
                usuariosSalaCallback[idUsuario] = OperationContext.Current.GetCallbackChannel<ISalaCallback>();
                ActualizarSalasParaTodos();
                NotificarResultadoUnirseASala(idUsuario, idSala, true);
            }
            else
            {
                NotificarResultadoUnirseASala(idUsuario, idSala, false);
            }
        }

        public void SalirDeSala(string idSala, string idUsuario)
        {
            AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            if (jugadoresEnSala.ContainsKey(idSala) && jugadoresEnSala[idSala].Remove(idUsuario))
            {
                if (jugadoresEnSala[idSala].Count == 0)
                {
                    jugadoresEnSala.Remove(idSala);
                    salasActivasEnMemoria.Remove(idSala);
                }
                ActualizarSalasParaTodos();
            }
        }

        private void ActualizarSalasParaTodos()
        {
            AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            var listaActualizada = ObtenerSalasActivasConEstado();
            foreach (var callback in usuariosSalaCallback.Values)
            {
                callback.ActualizarSalasActivas(listaActualizada);
            }
        }

        private void NotificarResultadoUnirseASala(string idUsuario, string idSala, bool esExitoso)
        {
            if (usuariosSalaCallback.TryGetValue(idUsuario, out var callback))
            {
                callback.ResultadoUnirseASala(idSala, salasActivasEnMemoria[idSala].codigo, esExitoso);
            }
        }
    }
}
