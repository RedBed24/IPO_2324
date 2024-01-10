using System;
using System.Collections.Generic;
using System.Xml;
using hospital_gui_wpf.src.dominio;

namespace hospital_gui_wpf.src.persistencia
{
	internal class DataReader
	{
		public static List<Paciente> CargarPacientesXML(string filename)
		{
			List<Paciente> listado = new List<Paciente>();
			int id;
			string nombre;
			string apellido;
			DateTime fechaNacimiento;
			int telefono;
			string direccion;
			Genero genero;
			Uri imagen;
			string correo;

			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				id = Convert.ToInt32(node["Id"].InnerText);
				nombre = node["Nombre"].InnerText;
				apellido = node["Apellido"].InnerText;
				fechaNacimiento = DateTime.ParseExact(node["FechaNacimiento"].InnerText, "yyyy/MM/dd", null);
				telefono = Convert.ToInt32(node["Telefono"].InnerText);
				direccion = node["Direccion"].InnerText;
				genero = (Genero)Enum.Parse(typeof(Genero), node["Genero"].InnerText, true);
				imagen = new Uri(node["Imagen"].InnerText, UriKind.Relative);
				correo = node["Correo"].InnerText;

				listado.Add(new Paciente(id, nombre, apellido, fechaNacimiento, telefono, direccion, genero, imagen, correo, new List<Cita>(), new List<Historial>()));
			}
			return listado;
		}

		public static List<Personal> CargarPersonalXML(string filename)
		{
			List<Personal> listado = new List<Personal>();
			int id;
			string nombre;
			string apellido;
			DateTime fechaNacimiento;
			int telefono;
			string direccion;
			Genero genero;
			Uri imagen;
			string correo;
			TipoPersonal tipoPersonal;

			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				id = Convert.ToInt32(node["Id"].InnerText);
				nombre = node["Nombre"].InnerText;
				apellido = node["Apellido"].InnerText;
				fechaNacimiento = DateTime.ParseExact(node["FechaNacimiento"].InnerText, "yyyy/MM/dd", null);
				telefono = Convert.ToInt32(node["Telefono"].InnerText);
				direccion = node["Direccion"].InnerText;
				genero = (Genero)Enum.Parse(typeof(Genero), node["Genero"].InnerText, true);
				imagen = new Uri(node["Imagen"].InnerText, UriKind.Relative);
				correo = node["Correo"].InnerText;
				tipoPersonal = (TipoPersonal)Enum.Parse(typeof(TipoPersonal), node["TipoPersonal"].InnerText, true);

				listado.Add(new Personal(id, nombre, apellido, fechaNacimiento, telefono, direccion, genero, imagen, correo, tipoPersonal, new List<Cita>()));
			}
			return listado;
		}

		public static void CargarHistorialXML(string filename, List<Paciente> pacientes)
		{
			Paciente paciente;
			string dolencias;
			string tratamientos;
			DateTime fechaAtencion;
			Uri foto;

			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				paciente = pacientes.Find(p => p.Id == Convert.ToInt32(node["Paciente"].InnerText));

				dolencias = node["Dolencias"].InnerText;
				tratamientos = node["Tratamientos"].InnerText;
				fechaAtencion = DateTime.ParseExact(node["FechaAtencion"].InnerText, "yyyy/MM/dd", null);
				foto = new Uri(node["Foto"].InnerText, UriKind.Relative);

				paciente.Historiales.Add(new Historial(dolencias, tratamientos, fechaAtencion, foto));
			}
		}

		public static void CargarCitasXML(string filename, List<Paciente> pacientes, List<Personal> personal)
		{
			DateTime fechaCita;
			TimeSpan duracion;
			Paciente paciente;
			Personal medico;
			string info;

			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				fechaCita = DateTime.ParseExact(node["FechaCita"].InnerText, "yyyy/MM/dd HH:mm", null);
				duracion = TimeSpan.ParseExact(node["Duracion"].InnerText, @"hh\:mm", null);

				paciente = pacientes.Find(p => p.Id == Convert.ToInt32(node["Paciente"].InnerText));
				medico = personal.Find(p => p.Id == Convert.ToInt32(node["Medico"].InnerText));

				info = node["InfoAdicional"].InnerText;

				paciente.Citas.Add(new Cita(fechaCita, duracion, paciente, medico, info));
			}
		}

		public static List<Usuario> CargarUsuariosXML(string filename)
		{
			List<Usuario> listado = new List<Usuario>();
			string nombreUsuario;
			string nombre;
			string apellido;
			string contrasena;
			DateTime fechaContratacion;
			DateTime ultimoAcceso;
			Uri imagen;

			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				nombreUsuario = node["NombreUsuario"].InnerText;
				nombre = node["Nombre"].InnerText;
				apellido = node["Apellido"].InnerText;
				contrasena = node["Contrasena"].InnerText;
				fechaContratacion = DateTime.ParseExact(node["FechaContratacion"].InnerText, "yyyy/MM/dd", null);
				ultimoAcceso = DateTime.ParseExact(node["UltimoAcceso"].InnerText, "yyyy/MM/dd HH:mm", null);
				imagen = new Uri(node["Imagen"].InnerText, UriKind.Relative);

				listado.Add(new Usuario(nombreUsuario, nombre, apellido, contrasena, fechaContratacion, ultimoAcceso, imagen));
			}
			return listado;
		}
	}
}
