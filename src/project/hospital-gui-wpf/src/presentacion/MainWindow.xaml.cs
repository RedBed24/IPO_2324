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

        public MainWindow()
        {
            InitializeComponent();
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

