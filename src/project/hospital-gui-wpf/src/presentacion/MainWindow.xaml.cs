using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using hospital_gui_wpf.src.dominio;
using Microsoft.Win32;


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
        // Solo se da altas o bajas de pacientes, al añadir a un paciente mediante baja se crean los historiales y citas vacios, estos se podrán modificar en sus tabs con el boton de modificar
        private void btnAltaPaciente(object sender, RoutedEventArgs e)
        {
            Paciente pacienteSeleccionado = lstListaPacientes.SelectedItem as Paciente;
            if (pacienteSeleccionado != null)
            {
                ConfirmarAltaYPosibleEliminacion(pacienteSeleccionado);
            }
            else
            {
                Personal personalSeleccionado = lstListaPersonal.SelectedItem as Personal;
                if (personalSeleccionado != null)
                {
                    ConfirmarAltaYPosibleEliminacion(personalSeleccionado);
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un paciente o personal antes de confirmar el alta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ConfirmarAltaYPosibleEliminacion(object elementoSeleccionado)
        {
            // Primera confirmación
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la alta?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Segunda confirmación
                result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Eliminar el elemento seleccionado de la lista correspondiente
                    if (elementoSeleccionado is Paciente)
                    {
                        lstListaPacientes.Items.Remove(elementoSeleccionado);
                    }
                    else if (elementoSeleccionado is Personal)
                    {
                        lstListaPersonal.Items.Remove(elementoSeleccionado);
                    }
                    LimpiarCampos();
                }
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
            if (radioFemeninoPacientes.IsChecked == true || radioFemeninoPersonal.IsChecked == true)
                return Genero.Mujer;
            else if (radioMasculinoPacientes.IsChecked == true || radioMasculinoPersonal.IsChecked == true)
                return Genero.Hombre;
            else return Genero.Otro;
        }
        private TipoPersonal ObtenerTipoSeleccionado()
        {
            // Obtener el género seleccionado
            if (radioSanitarioPersonal.IsChecked == true)
                return TipoPersonal.Sanitario;
            else return TipoPersonal.Limpieza;
        }
        private void LimpiarCampos()
        {
            // Limpiar los campos después de agregar el paciente o al personal
            txtnombrePacientes.Text = string.Empty;
            txtApellidoPacientes.Text = string.Empty;
            dpFechaNacimientoPacientes.SelectedDate = null;
            txtTelefonoPacientes.Text = string.Empty;
            txtDireccionPacientes.Text = string.Empty;
            txtCorreoPacientes.Text = string.Empty;
            radioFemeninoPacientes.IsChecked = false;
            radioMasculinoPacientes.IsChecked = false;
            radioOtroPacientes.IsChecked = false;
            imagenPaciente.Source = null;
            txtnombrePersonal.Text = string.Empty;
            txtApellidoPersonal.Text = string.Empty;
            dpFechaNacimientoPersonal.SelectedDate = null;
            txtTelefonoPersonal.Text = string.Empty;
            txtDireccionPersonal.Text = string.Empty;
            txtCorreoPersonal.Text = string.Empty;
            radioMasculinoPersonal.IsChecked = false;
            radioFemeninoPersonal.IsChecked = false;
            radioOtroPersonal.IsChecked = false;
            radioSanitarioPersonal.IsChecked = false;
            radioLimpiezaPersonal.IsChecked = false;
            imagenPersonal.Source = null;
        }
        private void btnConfirmarModificacionPacientes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Obtener el paciente seleccionado
            Paciente pacienteSeleccionado = lstListaPacientes.SelectedItem as Paciente;

            if (pacienteSeleccionado != null)
            {
                // Primera confirmación
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la modificación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Segunda confirmación
                    result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Actualizar las propiedades del paciente seleccionado
                        pacienteSeleccionado.Nombre = txtnombrePacientes.Text;
                        pacienteSeleccionado.Apellido = txtApellidoPacientes.Text;
                        pacienteSeleccionado.FechaNacimiento = dpFechaNacimientoPacientes.SelectedDate ?? DateTime.Now;
                        pacienteSeleccionado.Telefono = Convert.ToInt32(txtTelefonoPacientes.Text);
                        pacienteSeleccionado.Direccion = txtDireccionPacientes.Text;
                        pacienteSeleccionado.Genero = ObtenerGeneroSeleccionado();
                        pacienteSeleccionado.Correo = txtCorreoPacientes.Text;  
                        // Limpiar los campos después de la modificación
                        LimpiarCampos();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un paciente antes de confirmar la modificación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            // Obtener el paciente seleccionado
            Personal personalSeleccionado = lstListaPersonal.SelectedItem as Personal;

            if (personalSeleccionado != null)
            {
                // Primera confirmación
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la modificación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Segunda confirmación
                    result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Actualizar las propiedades del paciente seleccionado
                        personalSeleccionado.Nombre = txtnombrePersonal.Text;
                        personalSeleccionado.Apellido = txtApellidoPersonal.Text;
                        personalSeleccionado.FechaNacimiento = dpFechaNacimientoPersonal.SelectedDate ?? DateTime.Now;
                        personalSeleccionado.Telefono = Convert.ToInt32(txtTelefonoPersonal.Text);
                        personalSeleccionado.Direccion = txtDireccionPersonal.Text;
                        personalSeleccionado.Genero = ObtenerGeneroSeleccionado();
                        personalSeleccionado.Correo = txtCorreoPersonal.Text;
                        personalSeleccionado.Tipo = ObtenerTipoSeleccionado();
                        // Limpiar los campos después de la modificación
                        LimpiarCampos();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un personal antes de confirmar la modificación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

       

        private void btnBaja_Click(object sender, RoutedEventArgs e)
        {
            if (CamposRequeridosLlenos())
            {   
                Random random = new Random();
                // Crear un nuevo paciente con la información proporcionada
                Paciente nuevoPaciente = new Paciente
                
                (
                    random.Next(100,1000001), // aleatorio
                    txtnombrePacientes.Text,
                    txtApellidoPacientes.Text,
                    dpFechaNacimientoPacientes.SelectedDate ?? DateTime.Now,                   
                    Convert.ToInt32(txtTelefonoPacientes.Text),
                    txtDireccionPacientes.Text,
                    ObtenerGeneroSeleccionado(),
                    new Uri("/datos/imagenes/cross.png", UriKind.Relative),
                    txtCorreoPacientes.Text,
                    new List<Cita>(),
                    new List<Historial>()
                );

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
        private void dpFechaNacimientoHistorial_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtener la fecha seleccionada
            DateTime selectedDate = dpFechaNacimientoHistorial.SelectedDate ?? DateTime.Now;

            // Calcular la fecha mínima permitida (hoy menos 100 años)
            DateTime fechaMinima = DateTime.Now.AddYears(-100);

            // Calcular la fecha máxima permitida (hoy)
            DateTime fechaMaxima = DateTime.Now;

            // Validar que la fecha esté dentro del rango permitido
            if (selectedDate < fechaMinima || selectedDate > fechaMaxima)
            {
                MessageBox.Show("La fecha de nacimiento debe estar entre " + fechaMinima.ToShortDateString() + " y " + fechaMaxima.ToShortDateString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Limpiar la fecha en caso de no cumplir la validación
                dpFechaNacimientoHistorial.SelectedDate = null;
            }
        }
        private void dpFechaNacimientoPacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtener la fecha seleccionada
            DateTime selectedDate = dpFechaNacimientoPacientes.SelectedDate ?? DateTime.Now;

            // Calcular la fecha mínima permitida (hoy menos 100 años)
            DateTime fechaMinima = DateTime.Now.AddYears(-100);

            // Calcular la fecha máxima permitida (hoy)
            DateTime fechaMaxima = DateTime.Now;

            // Validar que la fecha esté dentro del rango permitido
            if (selectedDate < fechaMinima || selectedDate > fechaMaxima)
            {
                MessageBox.Show("La fecha de nacimiento debe estar entre " + fechaMinima.ToShortDateString() + " y " + fechaMaxima.ToShortDateString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Limpiar la fecha en caso de no cumplir la validación
                dpFechaNacimientoPacientes.SelectedDate = null;
            }
        }

        private void dpFechaNacimientoPersonal_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtener la fecha seleccionada
            DateTime selectedDate = dpFechaNacimientoPersonal.SelectedDate ?? DateTime.Now;

            // Calcular la fecha mínima permitida (hoy menos 100 años)
            DateTime fechaMinima = DateTime.Now.AddYears(-100);

            // Calcular la fecha máxima permitida (hoy)
            DateTime fechaMaxima = DateTime.Now;

            // Validar que la fecha esté dentro del rango permitido
            if (selectedDate < fechaMinima || selectedDate > fechaMaxima)
            {
                MessageBox.Show("La fecha de nacimiento debe estar entre " + fechaMinima.ToShortDateString() + " y " + fechaMaxima.ToShortDateString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Limpiar la fecha en caso de no cumplir la validación
                dpFechaNacimientoPersonal.SelectedDate = null;
            }
        }

        private void dpFechaCita_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtener la fecha seleccionada
            DateTime? selectedDate = dpFechaCita.SelectedDate;

            if (selectedDate == null)
                return; // No hacer nada si no hay fecha seleccionada (por ejemplo, si se borra la fecha

            // Calcular la fecha mínima permitida
            DateTime fechaMinima = DateTime.Now;

            // Validar que la fecha sea como mínimo 1 día después de hoy
            if (selectedDate < fechaMinima)
            {
                MessageBox.Show("La fecha debe ser como mínimo 1 día después de hoy.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Limpiar la fecha en caso de no cumplir la validación
                dpFechaCita.SelectedDate = null;
            }
        }

        private void dpFechaCita_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true; // Bloquea la entrada de texto directa
        }

        private void dpFechaNacimientoPacientes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true; // Bloquea la entrada de texto directa
        }

        private void dpFechaNacimientoHistorial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true; // Bloquea la entrada de texto directa
        }

        private void dpFechaNacimientoPersonal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true; // Bloquea la entrada de texto directa
        }

        private void imagenPaciente_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Uri baseUri = new Uri("/datos/imagenes", UriKind.Relative);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Todos los archivos|*.*";
            openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,baseUri.OriginalString);

            if (openFileDialog.ShowDialog() == true)
            {
                // Obtén la ruta de la imagen seleccionada
                string rutaImagen = openFileDialog.FileName;

                // Construye la ruta relativa desde la carpeta "DATOS"
                Uri fullPath = new Uri(rutaImagen);
                Uri relativeUri = baseUri.MakeRelativeUri(fullPath);
                string rutaRelativa = Uri.UnescapeDataString(relativeUri.ToString());

                // Muestra la imagen en algún lugar de la interfaz de usuario (opcional)
                imagenPaciente.Source = new BitmapImage(new Uri(rutaRelativa, UriKind.Relative));
            }
        }
    }
}

