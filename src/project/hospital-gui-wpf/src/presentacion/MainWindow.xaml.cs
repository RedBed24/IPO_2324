using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        }

        private void btnAltaPaciente(object sender, RoutedEventArgs e)
        {
            //string idPacienteSeleccionado = txtIdPacientes.Text;
            string nombrePacienteSeleccionado = txtnombrePacientes.Text;
            string apellidoPacienteSeleccionado = txtApellidoPacientes.Text;
            //string edadPacienteSeleccionado = dpFechaNacimientoPacientes;
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
            if (CamposRequeridosLlenos())
            {
                // Crear un nuevo paciente con la información proporcionada
                Paciente nuevoPaciente = new Paciente
                {
                    Id = 0, // HAY QUE VER QUE ID COGER
                    Nombre = txtnombrePacientes.Text,
                    Apellido = txtApellidoPacientes.Text,
                    FechaNacimiento = dpFechaNacimientoPacientes.SelectedDate ?? DateTime.Now,                   
                    Telefono = Convert.ToInt32(txtTelefonoPacientes.Text),
                    Direccion = txtDireccionPacientes.Text,
                    Genero = ObtenerGeneroSeleccionado(),
                    Imagen = new Uri("/datos/imagenes/cross.png", UriKind.Relative),
                    Correo = txtCorreoPacientes.Text,
                    Citas = new List<Cita>(),
                    Historiales = new List<Historial>()
                };

                // Agregar el nuevo paciente a la lista
                lstListaPacientes.Items.Add(nuevoPaciente);

                // Limpiar los campos después de agregar el paciente
                LimpiarCampos();

                // Puedes realizar otras acciones después de agregar el paciente
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.");
            }
        }
        private bool CamposRequeridosLlenos()
        {
            // Verificar que todos los campos requeridos estén llenos
            return !string.IsNullOrEmpty(txtnombrePacientes.Text) &&
                   !string.IsNullOrEmpty(txtApellidoPacientes.Text) &&
                   dpFechaNacimientoPacientes.SelectedDate.HasValue &&
                   !string.IsNullOrEmpty(txtTelefonoPacientes.Text) &&
                   !string.IsNullOrEmpty(txtDireccionPacientes.Text) &&
                   !string.IsNullOrEmpty(txtCorreoPacientes.Text);
        }

        private Genero ObtenerGeneroSeleccionado()
        {
            // Obtener el género seleccionado
            if (radioFemeninoPacientes.IsChecked == true)
                return Genero.Mujer;
            else if (radioMasculinoPacientes.IsChecked == true)
                return Genero.Hombre;
            else if (radioOtroPacientes.IsChecked == true)
                return Genero.Otro;
            else return Genero.Otro;
        }

        private void LimpiarCampos()
        {
            // Limpiar los campos después de agregar el paciente
            txtnombrePacientes.Text = string.Empty;
            txtApellidoPacientes.Text = string.Empty;
            dpFechaNacimientoPacientes.SelectedDate = null;
            txtTelefonoPacientes.Text = string.Empty;
            txtDireccionPacientes.Text = string.Empty;
            txtCorreoPacientes.Text = string.Empty;
            radioFemeninoPacientes.IsChecked = false;
            radioMasculinoPacientes.IsChecked = false;
            radioOtroPacientes.IsChecked = false;
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

        private void txtTelefonoPacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el número de teléfono del TextBox
            string numeroTelefono = txtTelefonoPacientes.Text;

            // Realiza la validación
            if (numeroTelefono.Length == 9 && int.TryParse(numeroTelefono, out _))
            {
                // El número de teléfono tiene 9 dígitos y es un número válido
            }
            else
            {
                MessageBox.Show("El número de teléfono debe tener exactamente 9 dígitos enteros.");
                txtTelefonoPacientes.Text = string.Empty;
            }
        }

        private void txtTelefonoHistorial_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el número de teléfono del TextBox
            string numeroTelefono = txtTelefonoHistorial.Text;

            // Realiza la validación
            if (numeroTelefono.Length == 9 && int.TryParse(numeroTelefono, out _))
            {
                // El número de teléfono tiene 9 dígitos y es un número válido
            }
            else
            {
                MessageBox.Show("El número de teléfono debe tener exactamente 9 dígitos enteros.");
                txtTelefonoHistorial.Text = string.Empty;
            }
        }

        private void txtTelefonoPersonal_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el número de teléfono del TextBox
            string numeroTelefono = txtTelefonoPersonal.Text;

            // Realiza la validación
            if (numeroTelefono.Length == 9 && int.TryParse(numeroTelefono, out _))
            {
                // El número de teléfono tiene 9 dígitos y es un número válido
            }
            else
            {
                MessageBox.Show("El número de teléfono debe tener exactamente 9 dígitos enteros.");
                txtTelefonoPersonal.Text = string.Empty;
            }
        }

        private void txtnombrePacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtnombrePacientes.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                // El nombre no contiene dígitos
                
            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtnombrePacientes.Text = string.Empty;
            }
        }

        private void txtnombreHistorial_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtnombreHistorial.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                // El nombre no contiene dígitos

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtnombreHistorial.Text = string.Empty;
            }
        }

        private void txtnombrePersonal_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtnombrePersonal.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                // El nombre no contiene dígitos

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtnombrePersonal.Text = string.Empty;
            }
        }

        private void txtApellidoPacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtApellidoPacientes.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                // El nombre no contiene dígitos

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtApellidoPacientes.Text = string.Empty;
            }
        }

        private void txtApellidoHistorial_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtApellidoHistorial.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                // El nombre no contiene dígitos

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtApellidoHistorial.Text = string.Empty;
            }
        }

        private void txtApellidoPersonal_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtApellidoPersonal.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                // El nombre no contiene dígitos

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtApellidoPersonal.Text = string.Empty;
            }
        }

        private void txtCorreoPacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtCorreoPacientes.Text.Contains("@"))
            {

                MessageBox.Show("La dirección de correo electrónico debe contener '@'.");
                txtCorreoPacientes.Text = string.Empty;
            }

        }

        private void txtCorreoPersonal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtCorreoPersonal.Text.Contains("@"))
            {

                MessageBox.Show("La dirección de correo electrónico debe contener '@'.");
                txtCorreoPersonal.Text = string.Empty;
            }

        }

        private void txtCorreoHistorial_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtCorreoHistorial.Text.Contains("@"))
            {

                MessageBox.Show("La dirección de correo electrónico debe contener '@'.");
                txtCorreoHistorial.Text = string.Empty;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEnlarge_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}

