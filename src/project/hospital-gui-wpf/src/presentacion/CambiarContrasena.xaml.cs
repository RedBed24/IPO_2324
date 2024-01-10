using hospital_gui_wpf.src.dominio;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace hospital_gui_wpf.src.presentacion
{
    public partial class CambiarContrasena : Window
    {
        public Usuario UsuarioActual;
        public Gestor GestorDatos;

        public string NuevaContrasena { get; private set; }

        public CambiarContrasena(Usuario usuarioActual, Gestor gestorDatos)
        {
            InitializeComponent();
            DataContext = usuarioActual;
            UsuarioActual = usuarioActual;
            GestorDatos = gestorDatos;
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (!UsuarioActual.ValidarContrasena(txtContrasenaActual.Password))
            {
                MessageBox.Show("La contraseña actual no es correcta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtContrasenaActual.Clear();
                txtContrasenaActual.Focus();
                return;
            }
            else
            {
                string nuevaContrasena = txtNuevaContrasena.Password;
                if (nuevaContrasena.Length >= 4)
                {
                    NuevaContrasena = nuevaContrasena;
                    UsuarioActual.Contrasena = nuevaContrasena;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("La nueva contraseña debe tener al menos 4 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNuevaContrasena.Clear();
                    txtNuevaContrasena.Focus();
                }
            }
        }
        private void actualizarUsuarios()
        {
            Usuario UsuarioEnLista = GestorDatos.Usuarios.Find(u => u.NombreUsuario == UsuarioActual.NombreUsuario);
            UsuarioEnLista.Contrasena = UsuarioActual.Contrasena;

        }
    }
}
