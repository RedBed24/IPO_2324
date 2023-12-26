using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_gui_wpf
{

    public enum Genero
    {
        Mujer,
        Hombre,
        Otro
    }
    public class Persona
    {
        public string Nombre { set; get; }
        public string Apellido { set; get; }
        public int Edad { set; get; }
        public int Telefono { set; get; }
        public string Direccion { set; get; }
        public Genero Genero { get; set; }
        public Uri Imagen { set; get; }
        public string Correo { set; get; }

        public Persona(string nombre, string apellido, int edad, int telefono, string direccion, Genero genero, Uri imagen, string correo)
        {
            Nombre = nombre;
            Apellido = apellido;
            Edad = edad;
            Telefono = telefono;
            Direccion = direccion;
            Genero = genero;
            Imagen = imagen;
            Correo = correo;
            
        }
    }
}
