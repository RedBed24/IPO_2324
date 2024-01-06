using hospital_gui_wpf.src.persistencia;
using System.Collections.Generic;

namespace hospital_gui_wpf.src.dominio
{
	public class Gestor
	{
		public List<Paciente> Pacientes;
		public List<Personal> Limpieza;
		public List<Personal> Sanitarios;
		public List<Usuario> Usuarios;

		public Gestor()
		{
			// el uso de literales abstrae de si ha una base de datos o no xdd
			// aunque es un churro, pero bueno, no se nos pide complicarnos
			Usuarios = DataReader.CargarUsuariosXML("datos/usuarios.xml");
			Pacientes = DataReader.CargarPacientesXML("datos/pacientes.xml");
			List<Personal> Personal = DataReader.CargarPersonalXML("datos/personal.xml");

			Limpieza = Personal.FindAll(p => p.Tipo == TipoPersonal.Limpieza);
			Sanitarios =  Personal.FindAll(p => p.Tipo == TipoPersonal.Sanitario);

			DataReader.CargarCitasXML("datos/citas.xml", Pacientes, Sanitarios);
			DataReader.CargarHistorialXML("datos/historial.xml", Pacientes);
		}
	}
}
