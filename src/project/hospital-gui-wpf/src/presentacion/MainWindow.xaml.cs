using System;
using System.Collections.Generic;
using System.IO;
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
        XmlDocument doc;

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
                var persona = new Persona(0, "", "", 0, 0, "", Genero.Otro, null, "");
                persona.Id = Convert.ToInt32(node["Id"].InnerText);
                persona.Nombre = node["Nombre"].InnerText;
                persona.Apellido = node["Apellido"].InnerText;
                persona.Edad = Convert.ToInt32(node["Edad"].InnerText);
                persona.Telefono = Convert.ToInt32(node["Telefono"].InnerText);
                persona.Direccion = node["Direccion"].InnerText;
                persona.Genero = (Genero)Enum.Parse(typeof(Genero), node["Genero"].InnerText, true);
                persona.Imagen = new Uri(node["Imagen"].InnerText, UriKind.Relative);
                persona.Correo = node["Correo"].InnerText;
                listado.Add(persona);
            }
            return listado;
        }

        private List<Historial> CargarHistorialXML()
        {
            List<Historial> listadoHistorial = new List<Historial>();

            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/historial.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var historial = new Historial(0, "", "", 0, 0, "", Genero.Otro, null, "", "");
                historial.Id = Convert.ToInt32(node["Id"].InnerText);
                historial.Nombre = node["Nombre"].InnerText;
                historial.Apellido = node["Apellido"].InnerText;
                historial.Edad = Convert.ToInt32(node["Edad"].InnerText);
                historial.Telefono = Convert.ToInt32(node["Telefono"].InnerText);
                historial.Direccion = node["Direccion"].InnerText;
                historial.Genero = (Genero)Enum.Parse(typeof(Genero), node["Genero"].InnerText, true);
                historial.Imagen = new Uri(node["Imagen"].InnerText, UriKind.Relative);
                historial.Correo = node["Correo"].InnerText;
                historial.Descripcion = node["Descripcion"].InnerText;
                listadoHistorial.Add(historial);
            }
            return listadoHistorial;
        }

        private List<Personal> CargarPersonalXML()
        {
            List<Personal> listadoPersonal = new List<Personal>();

            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/personal.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var personal = new Personal(0, "", "", 0, 0, "", Genero.Otro, null, "", TipoPersonal.Sanitario);
                personal.Id = Convert.ToInt32(node["Id"].InnerText);
                personal.Nombre = node["Nombre"].InnerText;
                personal.Apellido = node["Apellido"].InnerText;
                personal.Edad = Convert.ToInt32(node["Edad"].InnerText);
                personal.Telefono = Convert.ToInt32(node["Telefono"].InnerText);
                personal.Direccion = node["Direccion"].InnerText;
                personal.Genero = (Genero)Enum.Parse(typeof(Genero), node["Genero"].InnerText, true);
                personal.Imagen = new Uri(node["Imagen"].InnerText, UriKind.Relative);
                personal.Correo = node["Correo"].InnerText;
                personal.Tipo = (TipoPersonal)Enum.Parse(typeof(TipoPersonal), node["Tipo"].InnerText, true);
                listadoPersonal.Add(personal);
            }
            return listadoPersonal;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAltaPaciente(object sender, RoutedEventArgs e)
        {
            //string idPacienteSeleccionado = txtIdPacientes.Text;
            string nombrePacienteSeleccionado = txtnombrePacientes.Text;
            string apellidoPacienteSeleccionado = txtApellidoPacientes.Text;
            string edadPacienteSeleccionado = txtEdadPacientes.Text;
            string telefonoPacienteSeleccionado = txtTelefonoPacientes.Text;
            string correoPacienteSeleccionado = txtCorreoPacientes.Text;
            string direccionPacienteSeleccionado = txtDireccionPacientes.Text;
            //string imagenPacienteSeleccionado = imagenPacientes.Source.ToString().Replace("pack://application:,,,", "");

            string generoPacienteSeleccionado = "";
            if (radioFemeninoPacientes.IsChecked == true)
            {
                generoPacienteSeleccionado = "Mujer";
            }
            else if (radioMasculinoPacientes.IsChecked == true)
            {
                generoPacienteSeleccionado = "Hombre";
            }
            else if (radioOtroPacientes.IsChecked == true)
            {
                generoPacienteSeleccionado = "Otro";
            }

            string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName + "\\Datos\\persona.xml";

            XDocument doc = XDocument.Load(path);

            XElement nuevoPaciente = new XElement("Persona",
                //new XElement("Id", idPacienteSeleccionado),
                new XElement("Nombre", nombrePacienteSeleccionado),
                new XElement("Apellido", apellidoPacienteSeleccionado),
                new XElement("Edad", edadPacienteSeleccionado),
                new XElement("Genero", generoPacienteSeleccionado),
                new XElement("Telefono", telefonoPacienteSeleccionado),
                new XElement("Correo", correoPacienteSeleccionado),
                new XElement("Direccion", direccionPacienteSeleccionado)
            //new XElement("Imagen", imagenPacienteSeleccionado)
            );


            doc.Root.Add(nuevoPaciente);

            doc.Save(path);

        }
        private void btnBajaPaciente(object sender, RoutedEventArgs e)
        {
            //string idPacienteSeleccionado = txtIdPacientes.Text;
            string nombrePacienteSeleccionado = txtnombrePacientes.Text;
            string apellidoPacienteSeleccionado = txtApellidoPacientes.Text;
            string edadPacienteSeleccionado = txtEdadPacientes.Text;
            string telefonoPacienteSeleccionado = txtTelefonoPacientes.Text;
            string correoPacienteSeleccionado = txtCorreoPacientes.Text;
            string direccionPacienteSeleccionado = txtDireccionPacientes.Text;
            //string imagenPacienteSeleccionado = imagenPacientes.Source.ToString().Replace("pack://application:,,,", "");

            string generoPacienteSeleccionado = "";
            if (radioFemeninoPacientes.IsChecked == true)
            {
                generoPacienteSeleccionado = "Mujer";
            }
            else if (radioMasculinoPacientes.IsChecked == true)
            {
                generoPacienteSeleccionado = "Hombre";
            }
            else if (radioOtroPacientes.IsChecked == true)
            {
                generoPacienteSeleccionado = "Otro";
            }


            string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName + "\\Datos\\persona.xml";

            XDocument doc = XDocument.Load(path);

            var pacienteSeleccionado = doc.Descendants("Persona")
                            .Where(p =>
                            {
                                var IdElem = p.Element("Id");
                                var nombreElem = p.Element("Nombre");
                                var apellidoElem = p.Element("Apellido");
                                var edadElem = p.Element("Edad");
                                var telefonoElem = p.Element("Telefono");
                                var correoElem = p.Element("Correo");
                                var direccionElem = p.Element("Direccion");
                                var generoElem = p.Element("Genero");
                                var imagenElem = p.Element("Imagen");

                                return nombreElem.Value.Equals(nombrePacienteSeleccionado) &&
                                       apellidoElem.Value.Equals(apellidoPacienteSeleccionado) &&
                                       edadElem.Value.Equals(edadPacienteSeleccionado) &&
                                       telefonoElem.Value.Equals(telefonoPacienteSeleccionado) &&
                                       correoElem.Value.Equals(correoPacienteSeleccionado) &&
                                       direccionElem.Value.Equals(direccionPacienteSeleccionado) &&
                                       generoElem.Value.Equals(generoPacienteSeleccionado);
                                //imagenElem.Value.Equals(imagenPacienteSeleccionado);
                            }).ToList();

            foreach (var paciente in pacienteSeleccionado)
            {
                paciente.Remove();
            }

            doc.Save(path);
        }
        private void btnConfirmarModificacionPacientes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Primera confirmación
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la modificación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {   // Segunda confirmación
                result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    string nombrePacienteSeleccionado = "XD";
                    string nuevoNombre = "Miriam";

                    string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName + "\\Datos\\persona.xml";

                    XDocument doc = XDocument.Load(path);

                    var pacienteSeleccionado = doc.Descendants("Persona")
                    .Where(p => {
                        var nombreElem = p.Element("Nombre");
                        return nombreElem.Value.Equals(nombrePacienteSeleccionado);
                    }).ToList();

                    foreach (var paciente in pacienteSeleccionado)
                    {
                        paciente.Element("Nombre").Value = nuevoNombre;
                    }

                    doc.Save(path);
                }

            }


        }

        private void btnConfirmarModificacionHistorial_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Primera confirmación
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la modificación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {   // Segunda confirmación
                result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {

                }

            }
        }

        private void btnConfirmarModificacionPersonal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Primera confirmación
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la modificación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {   // Segunda confirmación
                result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {

                }

            }
        }
    }
}

