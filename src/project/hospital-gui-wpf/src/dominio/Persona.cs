using System;

namespace hospital_gui_wpf.src.dominio
{
	public enum Genero
    {
        Mujer,
        Hombre,
        Otro
    }

    public class Persona
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string Apellido { set; get; }
        public DateTime FechaNacimiento { set; get; }
        public int Telefono { set; get; }
        public string Direccion { set; get; }
        public Genero Genero { get; set; }
        public Uri Imagen { set; get; }
        public string Correo { set; get; }

        public Persona(int id, string nombre, string apellido, DateTime fechaNaciemiento, int telefono, string direccion, Genero genero, Uri imagen, string correo)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            FechaNacimiento = fechaNaciemiento;
            Telefono = telefono;
            Direccion = direccion;
            Genero = genero;
            Imagen = imagen;
            Correo = correo;
        }
    }
}
