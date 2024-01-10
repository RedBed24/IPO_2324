using hospital_gui_wpf.src.dominio;
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
        public Usuario UsuarioActual;
        public Gestor GestorDatos;
        public AboutUser(Usuario usuarioActual, Gestor gestorDatos)
        {
            InitializeComponent();
            DataContext = usuarioActual;
            UsuarioActual = usuarioActual;
            GestorDatos = gestorDatos;
        }

        private void btnModificarContraseña_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CambiarContrasena cambiarContrasenaWindow = new CambiarContrasena(UsuarioActual, GestorDatos);
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
