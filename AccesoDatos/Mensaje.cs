//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccesoDatos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mensaje
    {
        public string JugadorEmisor { get; set; }
        public string JugadorReceptor { get; set; }
        public string Contenido { get; set; }
        public string Estado { get; set; }
    
        public virtual Jugador Jugador { get; set; }
    }
}
