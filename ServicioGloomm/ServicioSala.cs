using AccesoDatos;
using BibliotecaClases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioGloomm
{

    public partial class ServicioJuego : ISala
    {
        public static readonly Dictionary<string, ISalaCallback> salaJugadoresCallback = new Dictionary<string, ISalaCallback>();
        public static readonly Dictionary<string, (string nombrePersonaje, int vida)> personajesPorUsuario = new Dictionary<string, (string, int)>();
        public static readonly List<string> personajesUsados = new List<string>();
        private static readonly Dictionary<string, HashSet<string>> jugadoresEnSala = new Dictionary<string, HashSet<string>>();
        private static readonly Dictionary<string, ISalaCallback> usuariosSalaCallback = new Dictionary<string, ISalaCallback>();
        private static readonly Dictionary<string, BibliotecaClases.Sala> salasActivasEnMemoria = new Dictionary<string, BibliotecaClases.Sala>();

        //public static readonly List<string> familiaSeleccionada = new List<string>();
        public static readonly Dictionary<string, HashSet<string>> familiasSeleccionadasPorSala = new Dictionary<string, HashSet<string>>();
        public static readonly Dictionary<string, (string nombrePersonaje, int vidaPersonaje1, int vidaPersonaje2, int vidaPersonaje3, int vidaPersonaje4)> familiaPorJugador = new Dictionary<string, (string, int, int, int, int)>();
        public Dictionary<string, List<(string nombrePersonaje, int vida)>> personajesFamiliaDeUsuario = new Dictionary<string, List<(string, int)>>();



        public int AgregarParticipantesAPartida(BibliotecaClases.Sala sala)
        {
            int resultado = AccesoSala.AgregarParticipante(sala);
            return resultado;
        }

        /*public int CrearPartida(BibliotecaClases.Sala sala)
        {

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
                salasActivasEnMemoria[sala.idSala] = sala;
                jugadoresEnSala[sala.idSala] = new HashSet<string>();
                jugadoresEnSala[sala.idSala].Add(sala.idAdministrador);
                //usuariosSalaCallback[sala.idAdministrador] = OperationContext.Current.GetCallbackChannel<ISalaCallback>();

                ActualizarSalasParaTodos();
                return resultado;
            }


            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }*/

        public int CrearPartida(BibliotecaClases.Sala sala)
        {
            try
            {
                // Generar código y configurar sala
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

                if (!familiasSeleccionadasPorSala.ContainsKey(sala.idSala))
                {
                    familiasSeleccionadasPorSala[sala.idSala] = new HashSet<string>();
                }

                // Operaciones en memoria después
                salasActivasEnMemoria[sala.idSala] = sala;

                // Registrar al administrador como jugador inicial
                jugadoresEnSala[sala.idSala] = new HashSet<string>();
                jugadoresEnSala[sala.idSala].Add(sala.idAdministrador);

                // Notificar a todos los clientes sobre las salas activas
                ActualizarSalasParaTodos();

                return resultado;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
            catch (CommunicationException ex)
            {
                // Manejar errores de comunicación al registrar el canal
                throw new FaultException<ManejadorExcepciones>(
                    new ManejadorExcepciones("Error de comunicación: " + ex.Message)); //EL nombre se repitio
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("9"));

            }

            return valido;
        }



        public BibliotecaClases.Sala BuscarSalaExistente(String idSala, String codigo)
        {
            try
            {
                AccesoDatos.Sala SalaDb;
                BibliotecaClases.Sala SalaBiblioteca = new BibliotecaClases.Sala();

                SalaDb = AccesoSala.BuscarPartida(idSala, codigo);
                SalaBiblioteca.fecha = SalaDb.Fecha;
                SalaBiblioteca.idSala = idSala;
                SalaBiblioteca.tipoSala = SalaDb.TipoSala;
                SalaBiblioteca.tipoPartida = SalaDb.TipoPartida;
                SalaBiblioteca.ganador = SalaDb.Ganador;
                SalaBiblioteca.codigo = codigo;
                SalaBiblioteca.nombreSala = SalaDb.NombreSala;
                SalaBiblioteca.noJugadores = SalaDb.NoJugadores;
                SalaBiblioteca.idAdministrador = SalaDb.IdAdministrador;

                return SalaBiblioteca;
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }

        }

        public void ConectarConSala(string nombreUsuario)
        {
            //AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            if (!salaJugadoresCallback.ContainsKey(nombreUsuario))
            {
                salaJugadoresCallback.Add(nombreUsuario, OperationContext.Current.GetCallbackChannel<ISalaCallback>());
                foreach (var jugador in salaJugadoresCallback)
                {
                    if (salaJugadoresCallback.ContainsKey(jugador.Key))
                    {
                        try
                        {
                            salaJugadoresCallback[jugador.Key].ActualizarNumeroJugadores();
                        }
                        catch (CommunicationException ex)
                        {
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                        }
                        catch (TimeoutException ex)
                        {
                            throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                        }
                    }
                }
            }
        }

        /*public List<string> ObtenerJugadoresConectados(String nombreUsuario)
        {
            return salaJugadoresCallback.Keys.ToList();
        }*/

        public void SeleccionarPersonaje(string nombreUsuario, string nombrePersonaje, int vida)
        {
            string personajeAnterior = "sin personaje";

            EstaSiendoUtilizado(nombrePersonaje);

            if (personajesPorUsuario.ContainsKey(nombreUsuario))
            {
                personajeAnterior = personajesPorUsuario[nombreUsuario].Item1;
                personajesPorUsuario[nombreUsuario] = (nombrePersonaje, vida);
                personajesUsados.Remove(personajeAnterior);
            }
            else
            {
                personajesPorUsuario.Add(nombreUsuario, (nombrePersonaje, vida));
            }
            //AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            foreach (var jugador in salaJugadoresCallback)
            {
                if (salaJugadoresCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        salaJugadoresCallback[jugador.Key].ActualizarImagenPersonaje(nombrePersonaje, personajeAnterior);
                    }
                    catch (CommunicationException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }
        private void EstaSiendoUtilizado(string nombrePersonaje)
        {
            if (personajesUsados.Contains(nombrePersonaje))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("14"));
            }
            else
            {
                personajesUsados.Add(nombrePersonaje);
            }
        }

        public void ValidarPersonajesSeleccionados(int cantidadJugadores)
        {
            if (cantidadJugadores != personajesUsados.Count())
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("15"));
            }
        }

        public Dictionary<string, (string nombrePersonaje, int vida)> ObtenerUsuariosYPersonajesSala()
        {
            return new Dictionary<string, (string nombrePersonaje, int vida)>(personajesPorUsuario);
        }

        public void LimpiarListaJugadores()
        {
            personajesPorUsuario.Clear();
        }

        public void LimpiarListaPersonajes()
        {
            personajesUsados.Clear();
        }

        public void EmpezarPartida(string idSala)
        {
            //AdministradorDeComportamiento.CambiarModoComportamientoReentrante();
            foreach (var jugador in salaJugadoresCallback)
            {
                if (salaJugadoresCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        salaJugadoresCallback[jugador.Key].EmpezarJuego();
                    }
                    catch (CommunicationException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        public void SacarDeSala(string nombreUsuario)
        {
            EliminarJugadorDeSala(nombreUsuario);
            string personaje = EliminarJugadorYPersonaje(nombreUsuario);
            EliminarPersonajeUsado(personaje);
        }

        public void EliminarJugadorDeSala(string nombreUsuario)
        {
            //AdministradorDeComportamiento.CambiarModoComportamientoReentrante();

            salaJugadoresCallback.Remove(nombreUsuario);
            foreach (var jugador in salaJugadoresCallback)
            {
                if (salaJugadoresCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        salaJugadoresCallback[jugador.Key].ActualizarNumeroJugadores();
                    }
                    catch (CommunicationException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException ex)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        private void EliminarPersonajeUsado(string nombrePersonaje)
        {
            personajesUsados.Remove(nombrePersonaje);
        }

        private string EliminarJugadorYPersonaje(string nombreUsuario)
        {
            personajesPorUsuario.TryGetValue(nombreUsuario, out var InformacionPersonaje);
            personajesPorUsuario.Remove(nombreUsuario);
            personajesUsados.Remove(InformacionPersonaje.nombrePersonaje);
            return InformacionPersonaje.nombrePersonaje;
        }

        public List<string> ObtenerPersonajesUsados()
        {
            return new List<string>(personajesUsados);
        }

        public List<BibliotecaClases.Sala> ObtenerSalasActivas()
        {


            foreach (var sala in salasActivasEnMemoria.Values)
            {
                sala.noJugadoresActuales = jugadoresEnSala.ContainsKey(sala.idSala) ? jugadoresEnSala[sala.idSala].Count : 0;
            }

            /*var salasDisponibles = salasActivasEnMemoria.Values
               .Where(sala => sala.noJugadoresActuales < sala.noJugadores)
               .ToList();*/
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("23"));
            }
        }


        public void UnirseASalaPrivadaNormal(string idUsuario, string idSala, string codigoAcceso)
        {
            if (!salasActivasEnMemoria.TryGetValue(idSala, out var sala) || sala.tipoSala != "Normal" || sala.codigo != codigoAcceso)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("24"));
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
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("24"));
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

        string ISala.ObtenerCodigoSala(string idAdminsitrador, string nombreSala)
        {
            try
            {
                string codigoSala;

                codigoSala = AccesoSala.BuscarCodigoSala(idAdminsitrador, nombreSala);

                return codigoSala.Trim();
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones(ex.Detail.mensaje));
            }
        }

        public List<string> ObtenerFamiliaSeleccionada(string idSala)
        {
            if (familiasSeleccionadasPorSala.ContainsKey(idSala))
            {
                return familiasSeleccionadasPorSala[idSala].ToList();
            }

            throw new KeyNotFoundException($"No hay familias seleccionadas para la sala con ID '{idSala}'.");
        }


        public Dictionary<string, List<(string nombrePersonaje, int vida)>> familias = new Dictionary<string, List<(string, int)>>
        {
            {"Ores", new List<(string, int)> { ("Didorian", 0), ("Zael", 0), ("Pablian", 0), ("Lorenzeo", 0) }},
            {"Corbat", new List<(string, int)> { ("Gaia", 0), ("Arialyn", 0), ("Aris", 0), ("Abelith", 0) }},
            {"Garlo", new List<(string, int)> { ("Tucani", 0), ("Lusiel", 0), ("Angelus", 0), ("Luan", 0) }},
            {"Ramfez", new List<(string, int)> { ("Seti", 0), ("Merit", 0), ("Neferu", 0), ("Sobek", 0) }},
        };



        public void SeleccionarFamilia(string nombreUsuario, string nombreFamilia, string idSala)
        {
            ValidarFamiliaExiste(nombreFamilia);
            LiberarFamiliaAnterior(nombreUsuario, idSala);
            ValidarFamiliaNoUsada(nombreFamilia, idSala);
            AsignarFamilia(nombreUsuario, nombreFamilia, idSala);
            NotificarSeleccionFamilia(nombreUsuario, nombreFamilia);
        }


        private void ValidarFamiliaExiste(string nombreFamilia)
        {
            if (!familias.ContainsKey(nombreFamilia))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("Familia no encontrada"));
            }
        }

        private void ValidarFamiliaNoUsada(string nombreFamilia, string salaId)
        {
            if (familiasSeleccionadasPorSala.ContainsKey(salaId) && familiasSeleccionadasPorSala[salaId].Contains(nombreFamilia))
            {
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("Familia ya seleccionada"));
            }
        }

        //Pedillos pra volver a seleccionar una familia ya seleccionada
        /*private void LiberarFamiliaAnterior(string nombreUsuario)
        {
            if (personajesFamiliaDeUsuario.ContainsKey(nombreUsuario))
            {
                var familiaAnterior = personajesFamiliaDeUsuario[nombreUsuario];
                var nombreFamiliaAnterior = familias.FirstOrDefault(kv => kv.Value == familiaAnterior).Key;

                Console.WriteLine($"Liberando familia anterior para usuario: {nombreUsuario}, Familia: {nombreFamiliaAnterior}");

                if (!string.IsNullOrEmpty(nombreFamiliaAnterior))
                {
                    familiaSeleccionada.Remove(nombreFamiliaAnterior);
                    Console.WriteLine($"Familia '{nombreFamiliaAnterior}' eliminada de la lista seleccionada.");
                }

                personajesFamiliaDeUsuario.Remove(nombreUsuario);
                Console.WriteLine($"Usuario '{nombreUsuario}' eliminado del diccionario de personajes por usuario.");
            }
        }*/

        private void LiberarFamiliaAnterior(string nombreUsuario, string salaId)
        {
            if (personajesFamiliaDeUsuario.ContainsKey(nombreUsuario))
            {
                var familiaAnterior = personajesFamiliaDeUsuario[nombreUsuario];
                var nombreFamiliaAnterior = familias.FirstOrDefault(kv => kv.Value == familiaAnterior).Key;

                if (!string.IsNullOrEmpty(nombreFamiliaAnterior) && familiasSeleccionadasPorSala[salaId].Contains(nombreFamiliaAnterior))
                {
                    // Quitar la familia de la lista seleccionada

                    familiasSeleccionadasPorSala[salaId].Remove(nombreFamiliaAnterior);

                }

                // Eliminar la asociación del usuario con la familia anterior
                personajesFamiliaDeUsuario.Remove(nombreUsuario);
            }
        }



        private void AsignarFamilia(string nombreUsuario, string nombreFamilia, string salaId)
        {
            if (!familiasSeleccionadasPorSala.ContainsKey(salaId))
            {
                familiasSeleccionadasPorSala[salaId] = new HashSet<string>();
            }

            familiasSeleccionadasPorSala[salaId].Add(nombreFamilia);
            personajesFamiliaDeUsuario[nombreUsuario] = familias[nombreFamilia];
        }


        private void NotificarSeleccionFamilia(string nombreUsuario, string nombreFamilia)
        {
            foreach (var jugador in salaJugadoresCallback)
            {
                if (salaJugadoresCallback.ContainsKey(jugador.Key))
                {
                    try
                    {
                        salaJugadoresCallback[jugador.Key].ActualizarSeleccionFamilia(nombreUsuario, nombreFamilia);
                    }
                    catch (CommunicationException)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("16"));
                    }
                    catch (TimeoutException)
                    {
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("18"));
                    }
                }
            }
        }

        /*void ISala.ValidarFamiliaSeleccionada(int cantidadJugadores, string idSala)
        {
            /*if (!familiasSeleccionadasPorSala.ContainsKey(idSala) || cantidadJugadores != familiasSeleccionadasPorSala.Count())
            {
                Console.WriteLine($"Validación fallida: cantidadJugadores={cantidadJugadores}, familiasSeleccionadas={(familiasSeleccionadasPorSala.ContainsKey(idSala) ? familiasSeleccionadasPorSala[idSala].Count : 0)}");
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("15"));
            }

            if (familiasSeleccionadasPorSala.ContainsKey(idSala))
            {
                int cantidadFamiliasSeleccionadas = familiasSeleccionadasPorSala[idSala].Count;

                if (cantidadFamiliasSeleccionadas != cantidadJugadores)
                {
                    Console.WriteLine($"Validación fallida: cantidadJugadores={cantidadJugadores}, familiasSeleccionadas={cantidadFamiliasSeleccionadas}");
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("15"));
                }
            }
            else
            {
                Console.WriteLine($"No hay familias seleccionadas para la sala {idSala}.");
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("15"));
            }
        }*/

        void ISala.ValidarFamiliaSeleccionada(int cantidadJugadores, string idSala)
        {
            try
            {
                if (familiasSeleccionadasPorSala.ContainsKey(idSala))
                {
                    int cantidadFamiliasSeleccionadas = familiasSeleccionadasPorSala[idSala].Count;

                    if (cantidadFamiliasSeleccionadas != cantidadJugadores)
                    {
                        Console.WriteLine($"Validación fallida: cantidadJugadores={cantidadJugadores}, familiasSeleccionadas={cantidadFamiliasSeleccionadas}");
                        throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("No se han seleccionado todas las familias. Faltan jugadores por seleccionar una familia."));
                    }
                }
                else
                {
                    Console.WriteLine($"No hay familias seleccionadas para la sala {idSala}.");
                    throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("No hay familias seleccionadas para esta sala."));
                }
            }
            catch (FaultException<ManejadorExcepciones> ex)
            {
                Console.WriteLine($"Error en la validación de familias: {ex.Detail.mensaje}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado en la validación de familias: {ex.Message}");
                throw new FaultException<ManejadorExcepciones>(new ManejadorExcepciones("Error inesperado en la validación de familias."));
            }
        }

        public Dictionary<string, HashSet<string>> ObtenerFamiliasSeleccionadasPorSala()
        {
            return familiasSeleccionadasPorSala;
        }


        Dictionary<string, string> ISala.ObtenerFamiliaPorJugador()
        {
            throw new NotImplementedException();
        }

        Dictionary<string, List<(string nombrePersonaje, int vida)>> ISala.ObtenerFamiliasYPersonajes()
        {
            throw new NotImplementedException();
        }
    }
}