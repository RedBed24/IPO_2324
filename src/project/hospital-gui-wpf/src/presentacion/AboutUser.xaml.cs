using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace hospital_gui_wpf.src.presentacion
{
    public partial class AboutUser : Window
    {
        public AboutUser()
        {
            InitializeComponent();
            Usuario usuarioActual = new Usuario("Antoniocg73", "Antonio", "Campallo", "contrasena", new DateTime(2003, 9, 1), new DateTime(2023, 12, 31), new Uri("/datos/imagenes/Antonio.jpg", UriKind.Relative));
            DataContext = usuarioActual;
        }

        private void btnModificarContraseña_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CambiarContrasena cambiarContrasenaWindow = new CambiarContrasena();
            bool? result = cambiarContrasenaWindow.ShowDialog();

            if (result == true)
            {
                string nuevaContrasena = cambiarContrasenaWindow.NuevaContrasena;
                MessageBox.Show($"La nueva contraseña es: {nuevaContrasena}");
            }
            else
            {
                MessageBox.Show("Cambio de contraseña cancelado.");
            }
        }
    }
}
