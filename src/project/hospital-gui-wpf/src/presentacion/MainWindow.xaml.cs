using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
        public Gestor GestorDatos;
        public Usuario UsuarioActual;
        public List<Personal> Personal;
        private bool cerrarDesdeCodigo = false;


        public MainWindow(Gestor gestorDatos, Usuario usuarioActual)
        {
            InitializeComponent();
            UsuarioActual = usuarioActual;
            GestorDatos = gestorDatos;

            btnPerfilUsuario.Source = new BitmapImage(UsuarioActual.Imagen);

            Personal = new List<Personal>();
            Personal.AddRange(GestorDatos.Sanitarios);
            Personal.AddRange(GestorDatos.Limpieza);

            lstListaPacientes.ItemsSource = GestorDatos.Pacientes;
            lstListaPersonal.ItemsSource = Personal;
            lstListaHistoriales.ItemsSource = GestorDatos.Pacientes;
            //lstHistorialPaciente.ItemsSource = GestorDatos.Pacientes;
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!cerrarDesdeCodigo)
            {
                MessageBox.Show("Gracias por usar nuestra aplicación...", "Despedida");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Establece el estado de la ventana a maximizado
            WindowState = WindowState.Maximized;
        }
        // Solo se da altas o bajas de pacientes, al añadir a un paciente mediante baja se crean los historiales y citas vacios, estos se podrán modificar en sus tabs con el boton de modificar
        private void btnAlta_Click(object sender, RoutedEventArgs e)
        {
            object tabContent = tabControl.SelectedItem;
            if (tabContent == tabPaciente)
            {
                Paciente pacienteSeleccionado = lstListaPacientes.SelectedItem as Paciente;
                if (pacienteSeleccionado != null)
                {
                    ConfirmarAltaYPosibleEliminacion(pacienteSeleccionado);
                }
                
            }
            else if (tabContent == tabPersonal)
            {
                Personal personalSeleccionado = lstListaPersonal.SelectedItem as Personal;
                if (personalSeleccionado != null)
                {
                    ConfirmarAltaYPosibleEliminacion(personalSeleccionado);
                }
                   
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un paciente o personal antes de confirmar la alta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        GestorDatos.Pacientes.Remove(elementoSeleccionado as Paciente);
                        actualizarPacientes();


                    }
                    else if (elementoSeleccionado is Personal)
                    {
                        Personal personalSeleccionado = elementoSeleccionado as Personal;
                        if (personalSeleccionado.Tipo == TipoPersonal.Sanitario)
                            GestorDatos.Sanitarios.Remove(personalSeleccionado);
                        else
                            GestorDatos.Limpieza.Remove(personalSeleccionado);
                        Personal.Remove(personalSeleccionado); 
                        
                        actualizarPersonal();
                    }
                    LimpiarCampos();
                }
            }
        }
        private void actualizarPacientes()
        {
            // Para evitar errores se debe dejar las listas sin seleccionar:
            lstListaPacientes.SelectedItem = null;
            List<Paciente> NuevoPaciente = new List<Paciente>();
            foreach (Paciente p in GestorDatos.Pacientes)
            {
                NuevoPaciente.Add(p);
            }
            lstListaPacientes.ItemsSource = NuevoPaciente;
    }
    private void actualizarPersonal()
        {
            lstListaPersonal.SelectedItem = null;
            List<Personal> NuevoPersonal = new List<Personal>();
            foreach (Personal p in Personal)
            {
                NuevoPersonal.Add(p);
            }
            lstListaPersonal.ItemsSource = NuevoPersonal;
        }
    private bool CamposRequeridosLlenos()
        {
            object tabContent = tabControl.SelectedItem;
            if (tabContent == tabPaciente)
            {
                return !string.IsNullOrEmpty(txtnombrePacientes.Text) &&
               !string.IsNullOrEmpty(txtApellidoPacientes.Text) &&
               dpFechaNacimientoPacientes.SelectedDate.HasValue &&
               !string.IsNullOrEmpty(txtTelefonoPacientes.Text) &&
               !string.IsNullOrEmpty(txtDireccionPacientes.Text) &&
               !string.IsNullOrEmpty(txtCorreoPacientes.Text);
            }
            else if (tabContent == tabPersonal)
            {
                return !string.IsNullOrEmpty(txtnombrePersonal.Text) &&
                   !string.IsNullOrEmpty(txtApellidoPersonal.Text) &&
                   dpFechaNacimientoPersonal.SelectedDate.HasValue &&
                   !string.IsNullOrEmpty(txtTelefonoPersonal.Text) &&
                   !string.IsNullOrEmpty(txtDireccionPersonal.Text) &&
                   !string.IsNullOrEmpty(txtCorreoPersonal.Text);
            }
            else return false;

            
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
            object tabContent= tabControl.SelectedItem;
            if (tabContent == tabPaciente)
            {
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

            }
            else if (tabContent == tabPersonal)
            {
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
            else if(tabContent == tabHistorial)
                txtHistorial.Text = string.Empty;   
            else if(tabContent == tabCita)
            {
                dpFechaCita.SelectedDate = null;
                txtDuracionCitas.Text = string.Empty;
            }
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
                        actualizarPacientes();
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
                        actualizarPersonal();
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
                BindingExpression bindingExpression = txtTelefonoPacientes.GetBindingExpression(TextBox.TextProperty);
                bindingExpression.UpdateSource();  // Esto debería eliminar el error de validación visual
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
            Login ventanaLogin = Login.InstanciaActual;
            if (ventanaLogin != null)
            {
                ventanaLogin.cerrarDesdeCodigo = true;
                ventanaLogin.Close();
            }
        }

        private void btnEnlarge_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private bool ExistePacienteEnLista(string nombre, string apellido, int telefono)
        {
            // Verificar si el nuevo paciente coincide con alguno de la lista
            return GestorDatos.Pacientes.Any(p => p.Nombre.ToUpper() == nombre.ToUpper() && p.Apellido.ToUpper() == apellido.ToUpper() && p.Telefono == telefono);
        }
        private bool ExistePersonalEnLista(string nombre, string apellido, int telefono)
        {
            // Verificar si el nuevo paciente coincide con alguno de la lista
            return lstListaPersonal.Items.Cast<Personal>().Any(p => p.Nombre.ToUpper() == nombre.ToUpper() && p.Apellido.ToUpper() == apellido.ToUpper() && p.Telefono == telefono);

        }
        private void btnBaja_Click(object sender, RoutedEventArgs e)
        {
            object tabContent = tabControl.SelectedItem;
            if (tabContent == tabPaciente)
            {
                if (CamposRequeridosLlenos())
                {   
                    Paciente seleccionado = lstListaPacientes.SelectedItem as Paciente;
                    if (seleccionado != null)
                    {
                        if (( seleccionado.Nombre.ToUpper() == txtnombrePacientes.Text.ToUpper() && seleccionado.Apellido.ToUpper() == txtApellidoPacientes.Text.ToUpper() && seleccionado.Telefono == Convert.ToInt32(txtTelefonoPacientes.Text)))
                        {
                            MessageBox.Show("No puedes dar de baja a un cliente que ya lo estaba.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; // Salir del método si el nombre ya existe
                        }

                    }
                    else
                    {
                        if (ExistePacienteEnLista(txtnombrePacientes.Text, txtApellidoPacientes.Text, Convert.ToInt32(txtTelefonoPacientes.Text)))
                        {
                            MessageBox.Show("El nuevo paciente coincide con uno que ya está en la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            LimpiarCampos();
                            return; // Salir del método si el nombre ya existe
                        }
                    }
                    Random random = new Random();
                    // Crear un nuevo paciente con la información proporcionada
                    Paciente nuevoPaciente = new Paciente

                    (
                        random.Next(100, 1000001), // aleatorio
                        txtnombrePacientes.Text,
                        txtApellidoPacientes.Text,
                        dpFechaNacimientoPacientes.SelectedDate ?? DateTime.Now,
                        Convert.ToInt32(txtTelefonoPacientes.Text),
                        txtDireccionPacientes.Text,
                        ObtenerGeneroSeleccionado(),
                        new Uri("/datos/imagenes/usuario_cualquiera.jpg", UriKind.Relative),
                        txtCorreoPacientes.Text,
                        new List<Cita>(),
                        new List<Historial>()
                    );

                    // Agregar el nuevo paciente a la lista
                    GestorDatos.Pacientes.Add(nuevoPaciente);
                    actualizarPacientes();

                    // Limpiar los campos después de agregar el paciente
                    LimpiarCampos();

                    
                }
                else
                {
                    MessageBox.Show("Por favor, complete todos los campos requeridos del cliente.");
                }
            }
            else if (tabContent == tabPersonal)
            {
                if (CamposRequeridosLlenos())
                {
                    Personal seleccionado = lstListaPersonal.SelectedItem as Personal;
                    if (seleccionado != null)
                    {
                        if ((seleccionado.Nombre.ToUpper() == txtnombrePersonal.Text.ToUpper() && seleccionado.Apellido.ToUpper() == txtApellidoPersonal.Text.ToUpper() && seleccionado.Telefono == Convert.ToInt32(txtTelefonoPersonal.Text)))
                        {
                            MessageBox.Show("No puedes dar de baja a un personal que ya estaba.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; // Salir del método si el nombre ya existe
                        }

                    }
                    else
                    {
                        if (ExistePersonalEnLista(txtnombrePersonal.Text, txtApellidoPersonal.Text, Convert.ToInt32(txtTelefonoPersonal.Text)))
                        {
                            MessageBox.Show("El nuevo personal coincide con uno que ya está en la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            LimpiarCampos();
                            return; // Salir del método si el nombre ya existe
                        }
                    }
                    Random random = new Random();
                    // Crear un nuevo paciente con la información proporcionada
                    Personal nuevoPersonal = new Personal

                    (
                        random.Next(100, 1000001), // aleatorio
                        txtnombrePersonal.Text,
                        txtApellidoPersonal.Text,
                        dpFechaNacimientoPersonal.SelectedDate ?? DateTime.Now,
                        Convert.ToInt32(txtTelefonoPersonal.Text),
                        txtDireccionPersonal.Text,
                        ObtenerGeneroSeleccionado(),
                        new Uri("/datos/imagenes/usuario_cualquiera.jpg", UriKind.Relative),
                        txtCorreoPersonal.Text,
                        ObtenerTipoSeleccionado(),
                        new List<Cita>()
                    );

                    // Agregar el nuevo paciente a la lista
                    if (nuevoPersonal.Tipo == TipoPersonal.Sanitario)
                        GestorDatos.Sanitarios.Add(nuevoPersonal);
                    else
                        GestorDatos.Limpieza.Add(nuevoPersonal);
                    Personal.Add(nuevoPersonal);

                    actualizarPersonal();

                    // Limpiar los campos después de agregar el paciente
                    LimpiarCampos();

                    // Puedes realizar otras acciones después de agregar el paciente

                }
                else
                {
                    MessageBox.Show("Por favor, complete todos los campos requeridos del personal.");
                }
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
        private void btnPerfilUsuario_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AboutUser aboutuser = new AboutUser(UsuarioActual, GestorDatos);
            aboutuser.Closed += AboutUserClosed;
            aboutuser.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void AboutUserClosed(object sender, EventArgs e)
        {
            // Este método se ejecutará cuando la ventana AboutUser.xaml se cierre
            this.Visibility = Visibility.Visible; 
            
        }

        private void txtDuracionCitas_LostFocus(object sender, RoutedEventArgs e)
        {
            string textoDuracion = txtDuracionCitas.Text;
            // Definir la expresión regular para el formato "xx:yy"
            Regex regex = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");
            // Validar el formato utilizando la expresión regular
            if (regex.IsMatch(textoDuracion))
            {
                // El formato es correcto, puedes convertirlo a TimeSpan
                TimeSpan duracion = TimeSpan.Parse(textoDuracion);
                if (duracion.TotalHours > 2 || duracion.TotalHours < 0 || duracion.Minutes < 10 || duracion.Minutes > 59)
                {
                    MessageBox.Show("La duración debe estar entre 0h:10m y 2h:59m.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtDuracionCitas.Clear();
                }
            }
            else
            {
                // El formato no es válido, mostrar un mensaje de error
                MessageBox.Show("Formato de duración incorrecto. Utiliza el formato 'xx:yy'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Puedes limpiar el TextBox o tomar otra acción según tus necesidades
                txtDuracionCitas.Text= string.Empty;
                // También puedes establecer el foco en el TextBox nuevamente
            }
        }

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            cerrarDesdeCodigo = true;
            if (Login.InstanciaActual != null)
            {
                Login.InstanciaActual.Visibility = Visibility.Visible;
                this.Close();
            }
        }

        private void lstListaPacientes_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Obtén el elemento bajo el puntero del ratón
            var hitTestResult = VisualTreeHelper.HitTest(lstListaPacientes, e.GetPosition(lstListaPacientes));
            // Si el elemento bajo el puntero del ratón no es un elemento de la ListBox, deselecciona el elemento actual
            if (hitTestResult.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lstListaPacientes.SelectedItem = null;
            }
        }

        private void lstListaPersonal_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Obtén el elemento bajo el puntero del ratón
            var hitTestResult = VisualTreeHelper.HitTest(lstListaPersonal, e.GetPosition(lstListaPersonal));
            // Si el elemento bajo el puntero del ratón no es un elemento de la ListBox, deselecciona el elemento actual
            if (hitTestResult.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lstListaPersonal.SelectedItem = null;
            }
        }

        private void lstListaPacientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificar si hay algún elemento seleccionado en la ListBox
            if (lstListaPacientes.SelectedItem != null)
            {
                // Obtener el paciente seleccionado
                Paciente pacienteSeleccionado = lstListaPacientes.SelectedItem as Paciente;

                // Mostrar la imagen del paciente seleccionado en la interfaz
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = pacienteSeleccionado.Imagen;
                bitmapImage.EndInit();

                // Mostrar la imagen del paciente seleccionado en la interfaz
                imagenPaciente.Source = bitmapImage;
            }
            else
            {
                // No hay elemento seleccionado, establecer la imagen a null
                imagenPaciente.Source = null;
            }
        }

        private void lstListaPersonal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificar si hay algún elemento seleccionado en la ListBox
            if (lstListaPersonal.SelectedItem != null)
            {
                // Obtener el paciente seleccionado
                Personal personalSeleccionado = lstListaPersonal.SelectedItem as Personal;

                // Mostrar la imagen del paciente seleccionado en la interfaz
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = personalSeleccionado.Imagen;
                bitmapImage.EndInit();

                // Mostrar la imagen del paciente seleccionado en la interfaz
                imagenPersonal.Source = bitmapImage;
            }
            else
            {
                // No hay elemento seleccionado, establecer la imagen a null
                imagenPersonal.Source = null;
            }
        }
    }
}

