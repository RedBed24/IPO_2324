﻿using System;
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
        public SolidColorBrush originalBorderColor;
        public Gestor GestorDatos;
        public Usuario UsuarioActual;
        public List<Personal> Personal;
        private bool cerrarDesdeCodigo = false;

        //Copias para que las fechas no se cambien aun teniendo mal el formato
        private DateTime fechaNacimientoPacienteCopia;
        private DateTime fechaNacimientoPersonalCopia;
        private DateTime fechaAtencionHistorialCopia;
        private DateTime fechaAtencionCitaCopia;
        private string duracionCitaCopia;

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
            lstListaCitas.ItemsSource = GestorDatos.Pacientes;
            txtMedicoCitas.ItemsSource = GestorDatos.Sanitarios;            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!cerrarDesdeCodigo)
            {
                MessageBox.Show("Gracias por usar nuestra aplicación...\n\n¡Hasta luego!",
                    "Despedida", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Establece el estado de la ventana a maximizado
            WindowState = WindowState.Maximized;
            originalBorderColor = (SolidColorBrush)txtTelefonoPacientes.BorderBrush;
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
                        if ((seleccionado.Nombre.ToUpper() == txtnombrePacientes.Text.ToUpper() && seleccionado.Apellido.ToUpper() == txtApellidoPacientes.Text.ToUpper() && seleccionado.Telefono == Convert.ToInt32(txtTelefonoPacientes.Text)))
                        {
                            MessageBox.Show("No puedes añadir a un cliente que ya lo estaba.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Por favor, complete todos los campos requeridos del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            MessageBox.Show("No puedes añadir a un personal que ya estaba.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Por favor, complete todos los campos requeridos del personal.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (tabContent == tabHistorial)
            {
                if (CamposRequeridosLlenos())
                {
                    // Crear un nuevo paciente con la información proporcionada
                    Historial nuevoHistorial = new Historial

                    (
                        txtDolenciaHistorial.Text,
                        txtTratamientoHistorial.Text,
                        dpFechaAtencionHistorial.SelectedDate ?? DateTime.Now,
                        new Uri("/datos/imagenes/usuario_cualquiera.jpg", UriKind.Relative)
                    );
                    Paciente pacienteSeleccionado = lstListaHistoriales.SelectedItem as Paciente;
                    if (pacienteSeleccionado != null)
                    {
                        pacienteSeleccionado.Historiales.Add(nuevoHistorial);
                        actualizarHistoriales(pacienteSeleccionado);
                        
                    }
                    else
                    {
                        MessageBox.Show("Por favor, selecciona un paciente antes de confirmar la adición de un historial.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    // Limpiar los campos después de agregar el paciente
                    LimpiarCampos();
                } 
                else
                {
                    MessageBox.Show("Por favor, complete todos los campos requeridos del historial.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else if (tabContent == tabCita)
            {
                if (CamposRequeridosLlenos())
                {
                    Paciente pacienteSeleccionado = lstListaCitas.SelectedItem as Paciente;
                    
                        Cita nuevaCita = new Cita
                        (
                            dpFechaAtencionCita.SelectedDate ?? DateTime.Now,
                            TimeSpan.Parse(txtDuracionCitas.Text),
                            pacienteSeleccionado,
                            txtMedicoCitas.SelectedItem as Personal,
                            txtInfoCitas.Text
                        );
                    if (pacienteSeleccionado != null)
                    {
                        pacienteSeleccionado.Citas.Add(nuevaCita);
                        actualizarCitas(pacienteSeleccionado);
                    }
                    else
                    {
                        MessageBox.Show("Por favor, selecciona un paciente antes de confirmar la adición de una cita.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, complete todos los campos requeridos de la cita.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }
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
            else if (tabContent == tabHistorial)
            {
                Historial historialSeleccionado = lstListaHistorialesPacientes.SelectedItem as Historial;
                if (historialSeleccionado != null)
                {
                    ConfirmarAltaYPosibleEliminacion(historialSeleccionado);
                }  
            }
            else if (tabContent == tabCita)
            {
                Cita citaSeleccionada = lstListaCitasPacientes.SelectedItem as Cita;
                if (citaSeleccionada != null)
                {
                    ConfirmarAltaYPosibleEliminacion(citaSeleccionada);
                }
            }
        }
        private void ConfirmarAltaYPosibleEliminacion(object elementoSeleccionado)
        {
            // Primera confirmación
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                    else if (elementoSeleccionado is Historial)
                    {
                        Historial historialSeleccionado = elementoSeleccionado as Historial;
                        Paciente pacienteSeleccionado = lstListaHistoriales.SelectedItem as Paciente;
                        pacienteSeleccionado.Historiales.Remove(historialSeleccionado);
                        actualizarHistoriales(pacienteSeleccionado);
                    }   
                    else if (elementoSeleccionado is Cita)
                    {
                        Cita citaSeleccionada = elementoSeleccionado as Cita;
                        Paciente pacienteSeleccionado = lstListaCitas.SelectedItem as Paciente;
                        pacienteSeleccionado.Citas.Remove(citaSeleccionada);
                        actualizarCitas(pacienteSeleccionado);
                    }
                    {
                        
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
            GestorDatos.Sanitarios.Clear();
            GestorDatos.Limpieza.Clear();
            txtMedicoCitas.ItemsSource = null;
            List<Personal> NuevoPersonal = new List<Personal>();
            foreach (Personal p in Personal)
            {
                NuevoPersonal.Add(p);
                if(p.Tipo == TipoPersonal.Sanitario)
                    GestorDatos.Sanitarios.Add(p);
                else
                    GestorDatos.Limpieza.Add(p);
            }
            lstListaPersonal.ItemsSource = NuevoPersonal;
            txtMedicoCitas.ItemsSource = GestorDatos.Sanitarios;
        }
        private void actualizarHistoriales(Paciente pacienteSeleccionado)
        {
            lstListaHistorialesPacientes.SelectedItem = null;
            List<Historial> NuevoHistorial = new List<Historial>();
            foreach (Historial h in pacienteSeleccionado.Historiales)
            {
                NuevoHistorial.Add(h);
            }
            lstListaHistorialesPacientes.ItemsSource = NuevoHistorial;
        }   
        private void actualizarCitas(Paciente pacienteSeleccionado)
        {
            lstListaCitasPacientes.SelectedItem = null;
            List<Cita> NuevaCita = new List<Cita>();
            foreach (Cita c in pacienteSeleccionado.Citas)
            {
                NuevaCita.Add(c);
            }
            lstListaCitasPacientes.ItemsSource = NuevaCita;
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
            else if (tabContent == tabHistorial)
            {
                return !string.IsNullOrEmpty(txtDolenciaHistorial.Text) &&
                    !string.IsNullOrEmpty(txtTratamientoHistorial.Text) &&
                   dpFechaAtencionHistorial.SelectedDate.HasValue;
            }
            else if (tabContent == tabCita)
            {
                return txtMedicoCitas.SelectedItem != null &&
               !string.IsNullOrEmpty(txtDuracionCitas.Text) &&
               dpFechaAtencionCita.SelectedDate.HasValue;
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
            {

            }
            else if(tabContent == tabCita)
            {

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
            Historial historialSeleccionado = lstListaHistorialesPacientes.SelectedItem as Historial;

            if (historialSeleccionado != null)
            {
                Paciente pacienteSeleccionado = lstListaHistoriales.SelectedItem as Paciente;
                // Primera confirmación
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la modificación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {   // Segunda confirmación
                    result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Actualizar las propiedades del historial seleccionado
                        historialSeleccionado.Tratamientos = txtDolenciaHistorial.Text;
                        historialSeleccionado.Dolencias = txtTratamientoHistorial.Text;
                        historialSeleccionado.FechaAtencion = dpFechaAtencionHistorial.SelectedDate ?? DateTime.Now;
                        actualizarHistoriales(pacienteSeleccionado);

                        // Limpiar los campos después de la modificación
                        LimpiarCampos();
                    }
                }
            } 
            else
            {
                MessageBox.Show("Por favor, selecciona un historial de un paciente antes de confirmar la modificación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
        private void btnConfirmarModificacionCitas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Cita citaSeleccionada = lstListaCitasPacientes.SelectedItem as Cita;

            if (citaSeleccionada != null)
            {
                Paciente pacienteSeleccionado = lstListaCitas.SelectedItem as Paciente;
                // Primera confirmación
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de confirmar la modificación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {   // Segunda confirmación
                    result = MessageBox.Show("¿Estás realmente seguro?", "Confirmar de nuevo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        TimeSpan duracion;
                        if (TimeSpan.TryParse(txtDuracionCitas.Text, out duracion))
                        {
                            citaSeleccionada.Duracion = duracion;
                        }
                        else
                        {
                            MessageBox.Show("La duración de la cita debe ser un número entero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            txtDuracionCitas.Text = string.Empty;
                            return;
                        }
                        // Actualizar las propiedades del historial seleccionado
                        citaSeleccionada.Paciente = pacienteSeleccionado;
                        citaSeleccionada.Fecha = dpFechaAtencionCita.SelectedDate ?? DateTime.Now;
                        citaSeleccionada.Personal = txtMedicoCitas.SelectedItem as Personal;
                        citaSeleccionada.InfoAdicional = txtInfoCitas.Text;
                        actualizarCitas(pacienteSeleccionado);

                        // Limpiar los campos después de la modificación
                        LimpiarCampos();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una cita de un paciente antes de confirmar la modificación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

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
            if (string.IsNullOrEmpty(txtTelefonoPacientes.Text))
            {


                txtTelefonoPacientes.BorderBrush = originalBorderColor;

            }
            // Realiza la validación
            else if (numeroTelefono.Length == 9 && int.TryParse(numeroTelefono, out _))
            {
                // El número de teléfono tiene 9 dígitos y es un número válido
                txtTelefonoPacientes.BorderBrush = originalBorderColor;
            }
            else
            {
                BindingExpression bindingExpression = txtTelefonoPacientes.GetBindingExpression(TextBox.TextProperty);
                bindingExpression.UpdateSource();  // Esto debería eliminar el error de validación visual
                MessageBox.Show("El número de teléfono debe tener exactamente 9 dígitos enteros.");
                txtTelefonoPacientes.Text = string.Empty;
                txtTelefonoPacientes.BorderBrush = Brushes.Red;
            }
        }

       
        private void txtTelefonoPersonal_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el número de teléfono del TextBox
            string numeroTelefono = txtTelefonoPersonal.Text;

            // Realiza la validación
            if (string.IsNullOrEmpty(txtTelefonoPersonal.Text))
            {


                txtTelefonoPersonal.BorderBrush = originalBorderColor;

            }
            else if (numeroTelefono.Length == 9 && int.TryParse(numeroTelefono, out _))
            {
                // El número de teléfono tiene 9 dígitos y es un número válido
                txtTelefonoPersonal.BorderBrush = originalBorderColor;
            }
            else
            {
                MessageBox.Show("El número de teléfono debe tener exactamente 9 dígitos enteros.");
                txtTelefonoPersonal.Text = string.Empty;
                txtTelefonoPersonal.BorderBrush = Brushes.Red;
            }
        }

        private void txtnombrePacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtnombrePacientes.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                txtnombrePacientes.BorderBrush = originalBorderColor;
                // El nombre no contiene dígitos

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtnombrePacientes.Text = string.Empty;
                txtnombrePacientes.BorderBrush = Brushes.Red;
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
                txtnombreHistorial.BorderBrush = originalBorderColor;

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtnombreHistorial.Text = string.Empty;
                txtnombreHistorial.BorderBrush = Brushes.Red;
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
                txtnombrePersonal.BorderBrush = originalBorderColor;
            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtnombrePersonal.Text = string.Empty;
                txtnombrePersonal.BorderBrush = Brushes.Red;
            }
        }


        private void txtApellidoPacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtén el nombre del TextBox
            string nombre = txtApellidoPacientes.Text;

            // Utiliza una expresión regular para verificar que no contiene dígitos
            if (!Regex.IsMatch(nombre, @"\d"))
            {
                txtApellidoPacientes.BorderBrush = originalBorderColor;
                // El nombre no contiene dígitos

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtApellidoPacientes.Text = string.Empty;
                txtApellidoPacientes.BorderBrush = Brushes.Red;
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
                txtApellidoHistorial.BorderBrush = originalBorderColor;

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtApellidoHistorial.Text = string.Empty;
                txtApellidoHistorial.BorderBrush = Brushes.Red;
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
                txtApellidoPersonal.BorderBrush = originalBorderColor;

            }
            else
            {
                MessageBox.Show("El nombre no puede contener dígitos.");
                txtApellidoPersonal.Text = string.Empty;
                txtApellidoPersonal.BorderBrush = Brushes.Red;
            }

        }

        private void txtCorreoPacientes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCorreoPacientes.Text))
            {

                txtCorreoPacientes.BorderBrush = originalBorderColor;


            }
            else if (!txtCorreoPacientes.Text.Contains("@"))
            {
                MessageBox.Show("La dirección de correo electrónico debe contener '@'.");
                txtCorreoPacientes.Text = string.Empty;
                txtCorreoPacientes.BorderBrush = Brushes.Red;

            }
            else
            {
                txtCorreoPacientes.BorderBrush = originalBorderColor;
            }

        }

        private void txtCorreoPersonal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCorreoPersonal.Text))
            {

                txtCorreoPersonal.BorderBrush = originalBorderColor;

            }
            else if (!txtCorreoPersonal.Text.Contains("@"))
            {
                MessageBox.Show("La dirección de correo electrónico debe contener '@'.");
                txtCorreoPersonal.Text = string.Empty;
                txtCorreoPersonal.BorderBrush = Brushes.Red;
            }
            else
            {
                txtCorreoPersonal.BorderBrush = originalBorderColor;
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
            if (string.IsNullOrEmpty(dpFechaNacimientoPacientes.Text))
            {
                dpFechaNacimientoPacientes.BorderBrush = originalBorderColor;
            }
            // Validar que la fecha esté dentro del rango permitido
            else if (selectedDate < fechaMinima || selectedDate > fechaMaxima)
            {
                MessageBox.Show("La fecha de nacimiento debe estar entre " + fechaMinima.ToShortDateString() + " y " + fechaMaxima.ToShortDateString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Limpiar la fecha en caso de no cumplir la validación
                if (lstListaPacientes.SelectedItem == null)
                {
                    dpFechaNacimientoPacientes.SelectedDate = null;
                    dpFechaNacimientoPacientes.BorderBrush = Brushes.Red;
                }
                else
                {
                    dpFechaNacimientoPacientes.BorderBrush = originalBorderColor;
                    dpFechaNacimientoPacientes.SelectedDate = fechaNacimientoPacienteCopia;
                }

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

            if (string.IsNullOrEmpty(dpFechaNacimientoPersonal.Text))
            {
                dpFechaNacimientoPersonal.BorderBrush = originalBorderColor;
            }
            // Validar que la fecha esté dentro del rango permitido
            else if (selectedDate < fechaMinima || selectedDate > fechaMaxima)
            {
                MessageBox.Show("La fecha de nacimiento debe estar entre " + fechaMinima.ToShortDateString() + " y " + fechaMaxima.ToShortDateString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Limpiar la fecha en caso de no cumplir la validación
                if (lstListaPersonal.SelectedItem == null)
                {
                    dpFechaNacimientoPersonal.SelectedDate = null;
                    dpFechaNacimientoPersonal.BorderBrush = Brushes.Red;
                }
                else
                {
                    dpFechaNacimientoPersonal.BorderBrush = originalBorderColor;
                    dpFechaNacimientoPersonal.SelectedDate = fechaNacimientoPersonalCopia;
                }
            }
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
        private void dpFechaAtencionHistorial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true; // Bloquea la entrada de texto directa
        }
        private void dpFechaAtencionCita_PreviewKeyDown(object sender, KeyEventArgs e)
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
            Regex regex = new Regex(@"^00:(1[0-9]|2[0-9]|30)$");
            if (string.IsNullOrEmpty(txtDuracionCitas.Text))
            {
                txtDuracionCitas.BorderBrush = originalBorderColor;
            }
            // Validar el formato utilizando la expresión regular
            else if (!regex.IsMatch(textoDuracion))
            {
                // El formato no es válido, mostrar un mensaje de error
                MessageBox.Show("Formato de duración incorrecto. Utiliza el formato 'hh:mm'. Además debe ser una cita entre 10 y 30 minutos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Puedes limpiar el TextBox o tomar otra acción según tus necesidades
                if (lstListaCitasPacientes.SelectedItem == null)
                {
                    txtDuracionCitas.Text = string.Empty;
                    txtDuracionCitas.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtDuracionCitas.BorderBrush = originalBorderColor;
                    txtDuracionCitas.Text = duracionCitaCopia;
                }
               // txtDuracionCitas.Text = string.Empty;
                //txtDuracionCitas.BorderBrush = Brushes.Red;
                //lstListaCitasPacientes.SelectedItem = null;
            }
            else
            {
                txtDuracionCitas.BorderBrush = originalBorderColor;
                if (lstListaCitasPacientes.SelectedItem == null)
                {
                    txtDuracionCitas.Text = duracionCitaCopia;
                }
               
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
                fechaNacimientoPacienteCopia = pacienteSeleccionado.FechaNacimiento;
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
                // Obtener el personal seleccionado
                Personal personalSeleccionado = lstListaPersonal.SelectedItem as Personal;
                fechaNacimientoPersonalCopia = personalSeleccionado.FechaNacimiento;

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

        private void lstListaHistoriales_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Obtén el elemento bajo el puntero del ratón
            var hitTestResult = VisualTreeHelper.HitTest(lstListaHistoriales, e.GetPosition(lstListaHistoriales));
            // Si el elemento bajo el puntero del ratón no es un elemento de la ListBox, deselecciona el elemento actual
            if (hitTestResult.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lstListaHistoriales.SelectedItem = null;
                lstListaHistorialesPacientes.ItemsSource = null;
            }
        }

        private void lstListaHistoriales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstListaHistoriales.SelectedItem != null)
            {
                // Obtener el paciente seleccionado
                Paciente pacienteSeleccionado = lstListaHistoriales.SelectedItem as Paciente;
                lstListaHistorialesPacientes.ItemsSource = pacienteSeleccionado.Historiales;
                imagenHistorial.Source = null;

            }
        }

        private void lstListaHistorialesPacientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Historial historialSeleccionado = lstListaHistorialesPacientes.SelectedItem as Historial;

            if (historialSeleccionado != null)
            {
                fechaAtencionHistorialCopia = historialSeleccionado.FechaAtencion;
                // Actualiza la imagen usando el historial seleccionado
                imagenHistorial.Source = new BitmapImage(new Uri(historialSeleccionado.Foto.ToString(), UriKind.Relative));
            }
            else
            {
                // Puedes establecer una imagen predeterminada o limpiar la imagen si no hay historial seleccionado
                imagenHistorial.Source = null;
            }
        }
        private void lstListaCitas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstListaCitas.SelectedItem != null)
            {
                // Obtener el paciente seleccionado
                Paciente pacienteSeleccionado = lstListaCitas.SelectedItem as Paciente;
                lstListaCitasPacientes.ItemsSource = pacienteSeleccionado.Citas;
            }
        }
        private void lstListaHistorialesPacientes_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Obtén el elemento bajo el puntero del ratón
            var hitTestResult = VisualTreeHelper.HitTest(lstListaHistorialesPacientes, e.GetPosition(lstListaHistorialesPacientes));
            // Si el elemento bajo el puntero del ratón no es un elemento de la ListBox, deselecciona el elemento actual
            if (hitTestResult.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lstListaHistorialesPacientes.SelectedItem = null;
            }
        }
        private void lstListaCitasPacientes_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Obtén el elemento bajo el puntero del ratón
            var hitTestResult = VisualTreeHelper.HitTest(lstListaCitasPacientes, e.GetPosition(lstListaCitasPacientes));
            // Si el elemento bajo el puntero del ratón no es un elemento de la ListBox, deselecciona el elemento actual
            if (hitTestResult.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lstListaCitasPacientes.SelectedItem = null;
            }
        }
        private void lstListaCitas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var hitTestResult = VisualTreeHelper.HitTest(lstListaCitas, e.GetPosition(lstListaCitas));
            // Si el elemento bajo el puntero del ratón no es un elemento de la ListBox, deselecciona el elemento actual
            if (hitTestResult.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lstListaCitas.SelectedItem = null;
                lstListaCitasPacientes.ItemsSource = null;
            }
        }

        private void dpFechaAtencionHistorial_LostFocus(object sender, RoutedEventArgs e)
        {
            // Obtener la fecha seleccionada
            DateTime selectedDate = dpFechaAtencionHistorial.SelectedDate ?? DateTime.Now;

            // Calcular la fecha mínima permitida (hoy menos 24 años)
            DateTime fechaMinima = DateTime.Now.AddYears(-24);

            // Calcular la fecha máxima permitida (hoy)
            DateTime fechaMaxima = DateTime.Now;
            if (string.IsNullOrEmpty(dpFechaAtencionHistorial.Text))
            {
                dpFechaAtencionHistorial.BorderBrush = originalBorderColor;
            }
            // Validar que la fecha esté dentro del rango permitido
            else if (selectedDate < fechaMinima || selectedDate > fechaMaxima)
            {
                MessageBox.Show("La fecha de nacimiento debe estar entre " + fechaMinima.ToShortDateString() + " y " + fechaMaxima.ToShortDateString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (lstListaHistorialesPacientes.SelectedItem == null)
                {
                    dpFechaAtencionHistorial.SelectedDate = null;
                    dpFechaAtencionHistorial.BorderBrush = Brushes.Red;
                }
                else
                {
                    dpFechaAtencionHistorial.BorderBrush = originalBorderColor;
                    dpFechaAtencionHistorial.SelectedDate = fechaAtencionHistorialCopia;
                }
            }
        }
        private void dpFechaAtencionCita_LostFocus(object sender, RoutedEventArgs e)
        {

            // Obtener la fecha seleccionada
            DateTime? selectedDate = dpFechaAtencionCita.SelectedDate;

            // Calcular la fecha mínima permitida (a partir de mañana)
            DateTime fechaMinima = DateTime.Now;

            // Calcular la fecha máxima permitida (hoy)
            DateTime fechaMaxima = DateTime.Now.AddYears(5);
            if (string.IsNullOrEmpty(dpFechaAtencionCita.Text))
            {
                dpFechaAtencionCita.BorderBrush = originalBorderColor;
            }
            // Validar que la fecha esté dentro del rango permitido
            else if (selectedDate < fechaMinima || selectedDate > fechaMaxima)
            {
                MessageBox.Show("La fecha de nacimiento debe estar entre " + fechaMinima.ToShortDateString() + " y " + fechaMaxima.ToShortDateString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Limpiar la fecha en caso de no cumplir la validación
                if (lstListaCitasPacientes.SelectedItem == null)
                {
                    dpFechaAtencionCita.SelectedDate = null;
                    dpFechaAtencionCita.BorderBrush = Brushes.Red;
                }
                else
                {
                    dpFechaAtencionCita.BorderBrush = originalBorderColor;
                    dpFechaAtencionCita.SelectedDate = fechaAtencionCitaCopia;
                }
            }
        }

        private void lstListaCitasPacientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cita citaSeleccionada = lstListaCitasPacientes.SelectedItem as Cita;
            if (citaSeleccionada != null)
            {
                // Obtener el personal seleccionado
                fechaAtencionCitaCopia = citaSeleccionada.Fecha;
                duracionCitaCopia = citaSeleccionada.Duracion.ToString(@"hh\:mm");
                txtMedicoCitas.IsEnabled = false;
                
            }
            else
            {
                txtMedicoCitas.IsEnabled = true;
            }

        }
    }
}

