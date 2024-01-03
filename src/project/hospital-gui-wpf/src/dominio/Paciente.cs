using System;
using System.Collections.Generic;

namespace hospital_gui_wpf.src.dominio
{
	internal class Paciente : Persona
	{
		public List<Cita> Citas { set; get; }
		public List<Historial> Historiales { set; get; }

		public Paciente(int id, string nombre, string apellido, DateTime fechaNacimiento, int telefono, string direccion, Genero genero, Uri imagen, string correo, List<Cita> citas,List<Historial> historiales)
			: base(id, nombre, apellido, fechaNacimiento, telefono, direccion, genero, imagen, correo)
		{
			Citas = citas;
			Historiales = historiales;
		}
	}
}
