using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_gui_wpf
{
    public enum TipoPersonal
    {
        Sanitario,
        Limpieza
    }
    public class Personal : Persona
    {
        public TipoPersonal Tipo { get; set; }

        public Personal(int id, string nombre, string apellido, int edad, int telefono, string direccion, Genero genero, Uri imagen, string correo, TipoPersonal tipo)
            : base(id, nombre, apellido, edad, telefono, direccion, genero, imagen, correo)
        {
            Tipo = tipo;
        }
    }
}
