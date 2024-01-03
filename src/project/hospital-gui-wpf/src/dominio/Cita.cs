using System;

namespace hospital_gui_wpf.src.dominio
{
	internal class Cita
	{
		public DateTime Fecha { get; set; }
		public TimeSpan Duracion { get; set; }
		public Paciente Paciente { get; set; }
		public Personal Personal { get; set; }
		public string InfoAdicional { get; set; }

		public Cita(DateTime fecha, TimeSpan duracion, Paciente paciente, Personal personal, string infoAdicional)
		{
			Fecha = fecha;
			Duracion = duracion;
			Paciente = paciente;
			Personal = personal;
			InfoAdicional = infoAdicional;
		}
	}
}
