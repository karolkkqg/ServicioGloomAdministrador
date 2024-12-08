using AccesoDatos;
using BibliotecaClases;
using BlbibliotecaClases;
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
        public static readonly Dictionary<string, HashSet<string>> jugadoresEnSala = new Dictionary<string, HashSet<string>>();
        private static readonly Dictionary<string, ISalaCallback> usuariosSalaCallback = new Dictionary<string, ISalaCallback>();
        public static readonly Dictionary<string, HashSet<string>> familiasSeleccionadasPorSala = new Dictionary<string, HashSet<string>>();
        public static readonly Dictionary<string, List<(string nombrePersonaje, int vida)>> personajesFamiliaDeUsuario = new Dictionary<string, List<(string, int)>>();
        public static readonly Dictionary<string, BibliotecaClases.Sala> salasActivasEnMemoria = new Dictionary<string, BibliotecaClases.Sala>();
        public static readonly Dictionary<string, string> administradoresDeSala = new Dictionary<string, string>();
        public static Dictionary<string, List<(string nombrePersonaje, int vida)>> familias = new Dictionary<string, List<(string, int)>>
        {
            {"Ores", new List<(string, int)> { ("Didorian", 0), ("Zael", 0), ("Pablian", 0), ("Lorenzeo", 0) }},
            {"Corbat", new List<(string, int)> { ("Gaia", 0), ("Arialyn", 0), ("Aris", 0), ("Abelith", 0) }},
            {"Garlo", new List<(string, int)> { ("Tucani", 0), ("Lusiel", 0), ("Angelus", 0), ("Luan", 0) }},
            {"Ramfez", new List<(string, int)> { ("Seti", 0), ("Merit", 0), ("Neferu", 0), ("Sobek", 0) }},
        };


        public int AgregarParticipantesAPartida(BibliotecaClases.Sala sala)
        {
            int resultado = AccesoSala.AgregarParticipante(sala);
            return resultado;
        }

        public Dictionary<string, (string nombrePersonaje, int vida)> ObtenerUsuariosYPersonajes(string numeroSala)
        {
            return new Dictionary<string, (string nombrePersonaje, int vida)>(personajesPorSala[numeroSala]);
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
                Task<int> resultadoTask = Task.Run(() => AccesoSala.AgregarPartidaABaseDeDatos(sala));
                int resultado = resultadoTask.Result;

                AsegurarSalaExistente(sala.idSala);
                if (!familiasSeleccionadasPorSala.ContainsKey(sala.idSala))
                {
                    familiasSeleccionadasPorSala[sala.idSala] = new HashSet<string>();


                    salasActivasEnMemoria[sala.idSala] = sala;

                    administradoresDeSala[sala.idSala] = sala.idAdministrador;

                    jugadoresEnSala[sala.idSala] = new HashSet<string>();
                    jugadoresEnSala[sala.idSala].Add(sala.idAdministrador);


                    ActualizarSalasParaTodos();

                    return resultado;
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }
            catch (CommunicationException ex)
            {
                throw new FaultException<ManejadorExcepciones>(
                    new ManejadorExcepciones("16", ex.Message));
            }
            return -1;
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Number.ToString(), ex.Message ));

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
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
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
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("14", "Personaje utilizado"));
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("15", "No todos los jugadores han seleccionado un personaje"));
            }
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
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[idSala].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
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
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                        }
                        catch (TimeoutException ex)
                        {
                            administradorLogger.RegistroError(ex);

                            salaJugadoresPorSala[idSala].Remove(jugador.Key);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
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
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
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

                if (salaJugadoresPorSala.ContainsKey(numeroSala))
                {
                    foreach (var jugador in salaJugadoresPorSala[numeroSala])
                    {
                        try
                        {
                            jugador.Value.ActualizarImagenPersonaje("sin personaje", nombrePersonaje);
                        }
                        catch (CommunicationException ex)
                        {
                            administradorLogger.RegistroError(ex);

                            salaJugadoresPorSala[numeroSala].Remove(jugador.Key);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                        }
                        catch (TimeoutException ex)
                        {
                            administradorLogger.RegistroError(ex);
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                        }
                    }
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

                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);

                        salaJugadoresPorSala[sala].Remove(jugador);

                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }
        }

        public List<BibliotecaClases.Sala> ObtenerSalasActivas()
        {


            foreach (var sala in salasActivasEnMemoria.Values)
            {
                sala.noJugadoresActuales = jugadoresEnSala.ContainsKey(sala.idSala) ? jugadoresEnSala[sala.idSala].Count : 0;
            }

            return salasActivasEnMemoria.Values.ToList();
        }

        public List<BibliotecaClases.Sala> ObtenerSalasActivasConEstado()
        {
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
            if (salasActivasEnMemoria.TryGetValue(idSala, out var sala) && sala.tipoPartida == "Pública" && sala.tipoSala == "Normal")
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("23", "La sala no es pública o no existe."));
            }
        }

        public void SalirDeSala(string idSala, string idUsuario)
        {
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

        public Dictionary<string, string> ObtenerFamiliaPorJugador(string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            var jugadoresEnSalaConvertido = jugadoresEnSala.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
            var familiasSeleccionadasPorSalaConvertido = familiasSeleccionadasPorSala.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
            try
            {
                ValidarExistenciaSala(numeroSala, jugadoresEnSalaConvertido, familiasSeleccionadasPorSalaConvertido);

                var familiaPorJugador = new Dictionary<string, string>();

                foreach (var jugador in jugadoresEnSala[numeroSala])
                {
                    if (personajesFamiliaDeUsuario.TryGetValue(jugador, out var personajesFamilia) && personajesFamilia != null && personajesFamilia.Any())
                    {
                        string familia = familias.FirstOrDefault(kvp => kvp.Value.SequenceEqual(personajesFamilia)).Key;

                        if (!string.IsNullOrEmpty(familia))
                        {
                            familiaPorJugador[jugador] = familia;
                        }
                        else
                        {
                            familiaPorJugador[jugador] = "Familia no asignada";
                        }
                    }
                    else
                    {
                        familiaPorJugador[jugador] = "Familia no asignada";
                    }
                }

                return familiaPorJugador;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
            }
        }
        private void ValidarExistenciaSala(string numeroSala, Dictionary<string, object> jugadoresEnSala, Dictionary<string, object> familiasSeleccionadasPorSala)
        {
            if (!jugadoresEnSala.ContainsKey(numeroSala) || !familiasSeleccionadasPorSala.ContainsKey(numeroSala))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("50", "No hay información de la sala"));
            }
        }

        public List<string> ObtenerFamiliaSeleccionada(string idSala)
        {

            return new List<string>(familiasSeleccionadasPorSala[idSala]);

        }
        public Dictionary<string, (string familia, List<(string nombrePersonaje, int vida)> personajes)> ObtenerFamiliaYPersonajesPorUsuario(string numeroSala)
        {
            var resultado = new Dictionary<string, (string familia, List<(string nombrePersonaje, int vida)> personajes)>();

            try
            {
                if (!jugadoresEnSala.ContainsKey(numeroSala))
                {
                    return resultado;
                }

                if (!familiasSeleccionadasPorSala.ContainsKey(numeroSala))
                {
                    return resultado;
                }

                foreach (var usuario in jugadoresEnSala[numeroSala])
                {
                    if (personajesFamiliaDeUsuario.TryGetValue(usuario, out var personajesFamilia))
                    {
                        string familia = familias.FirstOrDefault(kv =>
                            kv.Value.OrderBy(p => p.nombrePersonaje).SequenceEqual(personajesFamilia.OrderBy(p => p.nombrePersonaje))
                        ).Key;

                        if (!string.IsNullOrEmpty(familia))
                        {
                            resultado[usuario] = (familia, personajesFamilia);
                        }
                        else
                        {
                            Console.WriteLine($"No se encontró una familia válida para el usuario {usuario}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"El usuario {usuario} no tiene personajes asociados.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerFamiliaYPersonajesPorUsuario: {ex.Message}");
            }

            return resultado;
        }
        public void ValidarFamiliaSeleccionada(int cantidadJugadores, string idSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            try
            {
                if (familiasSeleccionadasPorSala.ContainsKey(idSala))
                {
                    int cantidadFamiliasSeleccionadas = familiasSeleccionadasPorSala[idSala].Count;

                    if (cantidadFamiliasSeleccionadas != cantidadJugadores)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("48", "Faltan jugadores por seleccionar una familia."));
                    }
                }
                else
                {
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("49", "No hay familias seleccionadas para esta sala."));
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
            }

        }

        private bool TienePersonajesRestantes(string jugador)
        {
            if (personajesFamiliaDeUsuario.TryGetValue(jugador, out var personajesDelUsuario))
            {
                return personajesDelUsuario.Count > 0;
            }
            return false;
        }

        public void UnirseASalaPublicaNormal(string idSala, string idUsuario)
        {
            if (salasActivasEnMemoria.TryGetValue(idSala, out var sala) && sala.tipoPartida == "Pública" && sala.tipoSala == "Normal")
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("23", "La sala no es pública o no existe."));
            }
        }

        public void UnirseASalaPrivadaNormal(string idUsuario, string idSala, string codigoAcceso)
        {
            if (!salasActivasEnMemoria.TryGetValue(idSala, out var sala) || sala.tipoSala != "Normal" || sala.codigo != codigoAcceso)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("24", "Verifique el código de la sala."));
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



        public void UnirseASalaPrivadaMiniHistoria(string idUsuario, string idSala, string codigoAcceso)
        {
            if (!salasActivasEnMemoria.TryGetValue(idSala, out var sala) || sala.tipoSala != "Mini historia" || sala.codigo != codigoAcceso)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("24", "Verifique el código de la sala."));
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

        private void FamiliaEnSeleccion(string numeroSala, string nombreFamilia)
        {
            if (familiasSeleccionadasPorSala[numeroSala].Contains(nombreFamilia))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("14", "Verifique el código de la sala."));
            }
            else
            {
                familiasSeleccionadasPorSala[numeroSala].Add(nombreFamilia);
            }
        }

        private string AgregarJugadorYFamilia(string nombreUsuario, string nombreFamilia, string numeroSala)
        {
            string familiaAnterior = "sin familia";
            if (!familiasSeleccionadasPorSala.ContainsKey(numeroSala))
            {
                familiasSeleccionadasPorSala[numeroSala] = new HashSet<string>();
            }

            if (!personajesFamiliaDeUsuario.ContainsKey(numeroSala))
            {
                personajesFamiliaDeUsuario[numeroSala] = new List<(string nombrePersonaje, int vida)>();
            }

            var familiasEnSala = familiasSeleccionadasPorSala[numeroSala];

            if (!familiasEnSala.Contains(nombreFamilia))
            {
                familiasEnSala.Add(nombreFamilia);
            }

            return familiaAnterior;
        }

        public string ObtenerCodigoSala(string idAdminsitrador, string nombreSala)
        {
            try
            {
                string codigoSala;

                codigoSala = AccesoSala.BuscarCodigoSala(idAdminsitrador, nombreSala);

                return codigoSala.Trim();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.mensaje));
            }
        }

        public void SeleccionarFamilia(string nombreUsuario, string nombreFamilia, string salaId)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            string familiaAnterior = ObtenerFamiliaAnterior(nombreUsuario);
            if (familiaAnterior != "sin familia")
            {
                familiasSeleccionadasPorSala[salaId].Remove(familiaAnterior);
            }

            FamiliaEnSeleccion(salaId, nombreFamilia);

            if (!familiasSeleccionadasPorSala.ContainsKey(salaId))
            {
                familiasSeleccionadasPorSala[salaId] = new HashSet<string>();
            }
            familiasSeleccionadasPorSala[salaId].Add(nombreFamilia);

            if (!personajesFamiliaDeUsuario.ContainsKey(nombreUsuario))
            {
                personajesFamiliaDeUsuario[nombreUsuario] = familias[nombreFamilia];
            }
            else
            {
                personajesFamiliaDeUsuario[nombreUsuario] = familias[nombreFamilia];
            }

            ValidarJugadorNoListo(nombreUsuario);

            if (salaJugadoresPorSala.ContainsKey(salaId))
            {
                foreach (var jugador in salaJugadoresPorSala[salaId])
                {
                    try
                    {
                        jugador.Value.ActualizarSeleccionFamilia(nombreFamilia, familiaAnterior);
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        salaJugadoresPorSala[salaId].Remove(jugador.Key);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }
        }

        public string ObtenerFamiliaAnterior(string nombreUsuario)
        {
            string familiaAnterior = "sin familia";
            if (personajesFamiliaDeUsuario.ContainsKey(nombreUsuario))
            {
                var personajesFamilia = personajesFamiliaDeUsuario[nombreUsuario];
                familiaAnterior = familias.FirstOrDefault(kv => kv.Value.SequenceEqual(personajesFamilia)).Key;
                return familiaAnterior;
            }

            return familiaAnterior;
        }

        public Dictionary<string, HashSet<string>> ObtenerFamiliasSeleccionadasPorSala()
        {
            return familiasSeleccionadasPorSala;
        }

        public Dictionary<string, List<(string nombrePersonaje, int vida)>> ObtenerFamiliasYPersonajes(string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            var jugadoresEnSalaConvertido = jugadoresEnSala.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
            var familiasSeleccionadasPorSalaConvertido = familiasSeleccionadasPorSala.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
            try
            {
                ValidarExistenciaSala(numeroSala, jugadoresEnSalaConvertido, familiasSeleccionadasPorSalaConvertido);

                var familiasYPersonajes = new Dictionary<string, List<(string nombrePersonaje, int vida)>>();

                foreach (var familia in familiasSeleccionadasPorSala[numeroSala])
                {
                    if (familias.TryGetValue(familia, out var personajes))
                    {
                        familiasYPersonajes[familia] = personajes.Select(p => (p.nombrePersonaje, p.vida)).ToList();
                    }
                }

                return familiasYPersonajes;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
            }
        }

        public void AplicarModificadorPositivo(Carta carta, string usuarioObjetivo, string personajeObjetivo)
        {

            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());

            try
            {
                ValidarUsuarioObjetivo(usuarioObjetivo);

                ValidarPersonajeObjetivo(personajeObjetivo);

                var personajesDelObjetivo = personajesFamiliaDeUsuario[usuarioObjetivo];

                var personajeIndex = personajesDelObjetivo.FindIndex(p => p.nombrePersonaje == personajeObjetivo);

                ValidarPersonajeFamiliarDeJugador(personajeIndex, personajeObjetivo, usuarioObjetivo);

                var personaje = personajesDelObjetivo[personajeIndex];

                personaje.vida += carta.valor;

                personajesDelObjetivo[personajeIndex] = personaje;

                if (jugadoresConectadosTableroCallback.TryGetValue(usuarioObjetivo, out var callback))
                {
                    try
                    {
                        callback.ActualizarMazoJugador();
                    }
                    catch (CommunicationException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16", "No se pudo conectar el servidor con todos los jugadores"));
                    }
                    catch (TimeoutException ex)
                    {
                        administradorLogger.RegistroError(ex);
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18", "Se termino el tiempo de espera del servidor, intente realizar la operación más tarde"));
                    }
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
            }

        }

        private void ValidarUsuarioObjetivo(string usuarioObjetivo)
        {
            if (string.IsNullOrEmpty(usuarioObjetivo) || !personajesFamiliaDeUsuario.ContainsKey(usuarioObjetivo))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("51", "El jugador objetivo no tiene personajes asociados"));
            }
        }

        private void ValidarPersonajeObjetivo(string personajeObjetivo)
        {
            if (string.IsNullOrEmpty(personajeObjetivo))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("52", "El familiar objetivo no existe"));

            }
        }

        private void ValidarPersonajeFamiliarDeJugador(int personajeIndex, string personajeObjetivo, string usuarioObjetivo)
        {
            if (personajeIndex == -1)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("53", "El familiar objetivo no pertenece a la familia del jugador"));

            }
        }



        public void AplicarModificadorNegativo(Carta carta, string usuarioObjetivo, string personajeObjetivo)
        {

            ValidarPersonajeObjetivo(personajeObjetivo);

            ValidarUsuarioObjetivo(usuarioObjetivo);

            var personajesDelUsuario = personajesFamiliaDeUsuario[usuarioObjetivo];

            var personajeIndex = personajesDelUsuario.FindIndex(p => p.nombrePersonaje == personajeObjetivo);

            ValidarPersonajeFamiliarDeJugador(personajeIndex, personajeObjetivo, usuarioObjetivo);


            var personaje = personajesDelUsuario[personajeIndex];

            personaje.vida += carta.valor;

            personajesDelUsuario[personajeIndex] = personaje;

        }



        public void AplicarCartaMuerte(string numeroSala, string nombreUsuario, string personajeObjetivo)
        {


            ValidarPersonajeObjetivo(personajeObjetivo);

            ValidarUsuarioObjetivo(nombreUsuario);

            ValidarPersonajePropio(nombreUsuario, personajeObjetivo);


            var personajesDelUsuario = personajesFamiliaDeUsuario[nombreUsuario];
            var personaje = personajesDelUsuario.FirstOrDefault(p => p.nombrePersonaje == personajeObjetivo);

            personajesDelUsuario.Remove(personaje);
        }

        private void ValidarPersonajePropio(string nombreUsuario, string personajeObjetivo)
        {
            var personajesDelUsuario = personajesFamiliaDeUsuario[nombreUsuario];
            var personaje = personajesDelUsuario.FirstOrDefault(p => p.nombrePersonaje == personajeObjetivo);

            if (personaje.nombrePersonaje == null || string.IsNullOrEmpty(personaje.nombrePersonaje))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("53", "El familiar objetivo no pertenece a la familia del jugador"));
            }
        }

        public Dictionary<string, (string familia, int vidaTotal)> ObtenerResumenFamiliasPorSala(string numeroSala)
        {
            AdministradorLogger administradorLogger = new AdministradorLogger(this.GetType());
            var resultado = new Dictionary<string, (string familia, int vidaTotal)>();

            try
            {
                if (!jugadoresEnSala.ContainsKey(numeroSala) || !familiasSeleccionadasPorSala.ContainsKey(numeroSala))
                {
                    return resultado;
                }

                foreach (var jugador in jugadoresEnSala[numeroSala])
                {
                    if (personajesFamiliaDeUsuario.TryGetValue(jugador, out var personajesFamilia))
                    {
                        string familia = familias.FirstOrDefault(kvp =>
                            kvp.Value.SequenceEqual(personajesFamilia)).Key;

                        if (!string.IsNullOrEmpty(familia))
                        {
                            int vidaTotal = personajesFamilia.Sum(p => p.vida);
                            resultado[jugador] = (familia, vidaTotal);
                        }

                    }
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                administradorLogger.RegistroError(ex);
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.codigo, ex.Detail.Mensaje));
            }

            return resultado;
        }
    }
}
