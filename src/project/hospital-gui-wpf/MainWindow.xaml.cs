using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace hospital_gui_wpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Persona> listadoPacientes;
        private List<Historial> listadoHistorial;
        private List<Personal> listadoPersonal;

        public MainWindow()
        {
            InitializeComponent();
            listadoPacientes = new List<Persona>();
            listadoPacientes = CargarPacientesXML();
            lstListaPacientes.ItemsSource = listadoPacientes;

            listadoHistorial = new List<Historial>();
            listadoHistorial = CargarHistorialXML();
            lstListaHistoriales.ItemsSource = listadoHistorial;

            listadoPersonal = new List<Personal>();
            listadoPersonal = CargarPersonalXML();
            lstListaPersonal.ItemsSource = listadoPersonal;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("Gracias por usar nuestra aplicación...", "Despedida");

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Establece el estado de la ventana a maximizado
            WindowState = WindowState.Maximized;

            // Deshabilita el botón de minimizar
            ResizeMode = ResizeMode.NoResize;
        }

        private List<Persona> CargarPacientesXML()
        {
            List<Persona> listado = new List<Persona>();
            
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/persona.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var persona = new Persona("", "", 0, 0, "", Genero.Otro, null, "");
                persona.Nombre = node.Attributes["Nombre"].Value;
                persona.Apellido = node.Attributes["Apellido"].Value;
                persona.Edad = Convert.ToInt32(node.Attributes["Edad"].Value);
                persona.Telefono = Convert.ToInt32(node.Attributes["Telefono"].Value);
                persona.Direccion = node.Attributes["Direccion"].Value;
                persona.Genero = (Genero)Enum.Parse(typeof(Genero), node.Attributes["Genero"].Value, true);
                persona.Imagen = new Uri(node.Attributes["Imagen"].Value, UriKind.Relative); 
                persona.Correo = node.Attributes["Correo"].Value;
                listado.Add(persona);
            }
            return listado;
        }

        private List<Historial> CargarHistorialXML()
        {
            List<Historial> listadoHistorial = new List<Historial>();

            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/persona.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var historial = new Historial("", "", 0, 0, "", Genero.Otro, null, "", "");
                historial.Nombre = node.Attributes["Nombre"].Value;
                historial.Apellido = node.Attributes["Apellido"].Value;
                historial.Edad = Convert.ToInt32(node.Attributes["Edad"].Value);
                historial.Telefono = Convert.ToInt32(node.Attributes["Telefono"].Value);
                historial.Direccion = node.Attributes["Direccion"].Value;
                historial.Genero = (Genero)Enum.Parse(typeof(Genero), node.Attributes["Genero"].Value, true);
                historial.Imagen = new Uri(node.Attributes["Imagen"].Value, UriKind.Relative);
                historial.Correo = node.Attributes["Correo"].Value;
                historial.Descripcion = node.Attributes["Descripcion"].Value;
                listadoHistorial.Add(historial);
            }
            return listadoHistorial;
        }

        private List<Personal> CargarPersonalXML()
        {
            List<Personal> listadoPersonal = new List<Personal>();

            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/persona.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var personal = new Personal("", "", 0, 0, "", Genero.Otro, null, "", TipoPersonal.Sanitario);
                personal.Nombre = node.Attributes["Nombre"].Value;
                personal.Apellido = node.Attributes["Apellido"].Value;
                personal.Edad = Convert.ToInt32(node.Attributes["Edad"].Value);
                personal.Telefono = Convert.ToInt32(node.Attributes["Telefono"].Value);
                personal.Direccion = node.Attributes["Direccion"].Value;
                personal.Genero = (Genero)Enum.Parse(typeof(Genero), node.Attributes["Genero"].Value, true);
                personal.Imagen = new Uri(node.Attributes["Imagen"].Value, UriKind.Relative);
                personal.Correo = node.Attributes["Correo"].Value;
                personal.Tipo = (TipoPersonal)Enum.Parse(typeof(TipoPersonal), node.Attributes["Tipo"].Value, true);
                listadoPersonal.Add(personal);
            }
            return listadoPersonal;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAltaPaciente(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnBajaPaciente(object sender, RoutedEventArgs e)
        {
            //string nombrePacienteSeleccionado = txtnombrePacientes.Text;
            string nombrePacienteSeleccionado = "Natalia";

            //var fichero = Application.GetResourceStream(new Uri("Datos/persona.xml", UriKind.Relative));
            //XDocument doc = XDocument.Load(fichero.Stream);
            string path = AppDomain.CurrentDomain.BaseDirectory + "Datos/persona.xml";
            XDocument doc = XDocument.Load(path);

            //var pacienteSeleccionado = doc.Descendants("Persona")
            //    .Where(p => p.Element("Nombre") == nombrePacienteSeleccionado).First().Value();

            var pacienteSeleccionado = doc.Descendants("Persona")
                .Where(p => (string)p.Attribute("Nombre") == nombrePacienteSeleccionado)
                .FirstOrDefault();
            
            //Dictionary<int, Dictionary<string, string>> dict = doc.Descendants("TestDriveRequest")
           //.GroupBy(x => (int)x.Attribute("Record"), y => y.Elements("element")
            //.GroupBy(a => (string)a.Attribute("name"), b => (string)b)
            //.ToDictionary(a => a.Key, b => b.FirstOrDefault()))
        //.ToDictionary(x => x.Key, y => y.FirstOrDefault());

            if (pacienteSeleccionado != null)
            {
                pacienteSeleccionado.Remove();
             
                doc.Save(path);
            }
            else
            {
                MessageBox.Show("Paciente no encontrado");
            }
        }

        private void PrintDialog(string v)
        {
            throw new NotImplementedException();
        }
    }
}


