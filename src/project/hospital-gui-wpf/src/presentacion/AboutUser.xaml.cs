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
        Usuario UsuarioActual;
        public AboutUser(Usuario usuarioActual)
        {
            InitializeComponent();
            DataContext = usuarioActual;
            UsuarioActual = usuarioActual;
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
