using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_gui_wpf
{
    public class Historial : Persona
    {
        public string Descripcion { get; set; }

        public Historial(string nombre, string apellido, int edad, int telefono, string direccion, Genero genero, Uri imagen, string correo, string descripcion)
            : base(nombre, apellido, edad, telefono, direccion, genero, imagen, correo)
        {
            Descripcion = descripcion;
        }
    }
}
