using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using hospital_gui_wpf.src.dominio;

namespace hospital_gui_wpf.src.presentacion
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
            //listadoPacientes = CargarPacientesXML();
            lstListaPacientes.ItemsSource = listadoPacientes;

            listadoHistorial = new List<Historial>();
            //listadoHistorial = CargarHistorialXML();
            lstListaHistoriales.ItemsSource = listadoHistorial;

            listadoPersonal = new List<Personal>();
            //listadoPersonal = CargarPersonalXML();
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

            /*
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
            */

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


            /*
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
            */
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

                    /*
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
                    */
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

        private void btnPerfilUsuario_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AboutUser aboutuser = new AboutUser();
            aboutuser.Closed += AboutUserClosed;
            aboutuser.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void AboutUserClosed(object sender, EventArgs e)
        {
            // Este método se ejecutará cuando la ventana AboutUser.xaml se cierre
            this.Visibility = Visibility.Visible; 
            
        }
    }
}

