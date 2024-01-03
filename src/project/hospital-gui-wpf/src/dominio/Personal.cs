using System;
using System.Collections.Generic;

namespace hospital_gui_wpf.src.dominio
{
    public enum TipoPersonal
    {
        Sanitario,
        Limpieza
    }

    internal class Personal : Persona
    {
        public TipoPersonal Tipo { get; set; }

        public List<Cita> Citas { set; get; }


        public Personal(int id, string nombre, string apellido, DateTime fechaNacimiento, int telefono, string direccion, Genero genero, Uri imagen, string correo, TipoPersonal tipo, List<Cita> citas)
            : base(id, nombre, apellido, fechaNacimiento, telefono, direccion, genero, imagen, correo)
        {
            Tipo = tipo;
            Citas = citas;
        }
    }
}
