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
        public string NuevaContrasena { get; private set; }

        public CambiarContrasena()
        {
            InitializeComponent();
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            NuevaContrasena = txtNuevaContrasena.Password;
            DialogResult = true;
            Close();
        }
    }

}
