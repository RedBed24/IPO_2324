﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_gui_wpf
{

    public class Usuario
    {
        public string NombreUsuario { set; get; }
        public string Nombre { set; get; }
        public string Apellido { set; get; }
        public string Contrasena { set; get; }
        public DateTime FechaContratacion { set; get; }

        public DateTime UltimoAcceso { set; get; }
        public Uri Imagen { set; get; }
        public Usuario(string nombreusuario, string nombre, string apellido, string contraseña, DateTime fecha, DateTime acceso, Uri imagen)
        {
            NombreUsuario = nombreusuario;
            Nombre = nombre;
            Apellido = apellido;
            Contrasena = contraseña;
            FechaContratacion = fecha;
            UltimoAcceso = acceso;
            Imagen = imagen;
        }
        public bool ValidarContrasena(string contrasenaActual)
        {
            return Contrasena == contrasenaActual;
        }
    }
}