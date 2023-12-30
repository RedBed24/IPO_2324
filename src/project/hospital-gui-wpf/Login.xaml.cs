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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace hospital_gui_wpf
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private BitmapImage imagCheck = new BitmapImage(new Uri("/imagenes/check.png", UriKind.Relative)); 
        private BitmapImage imagCross = new BitmapImage(new Uri("/imagenes/cross.png", UriKind.Relative));
        private BitmapImage imagQuestion = new BitmapImage(new Uri("/imagenes/pregunta.png", UriKind.Relative));
        private string usuario = "noelia";
        private string password = "1234";

        public object Visible { get; private set; }

        public Login()
        {
            InitializeComponent();
            
            
            

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Establece el estado de la ventana a maximizado
            WindowState = WindowState.Maximized;

            // Deshabilita el botón de minimizar
            ResizeMode = ResizeMode.NoResize;
            imgUser.Source = imagQuestion;
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
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }


        private Boolean ComprobarEntrada(string valorIntroducido, string valorValido,
        Control componenteEntrada, Image imagenFeedBack) 
        {
            Boolean valido = false;
            if (valorIntroducido.Equals(valorValido))
            {
               
                componenteEntrada.BorderBrush = Brushes.Green;
                componenteEntrada.Background = Brushes.LightGreen;
                imagenFeedBack.Source = imagCheck;
                valido = true;
            }
            else
            {
                // marcamos borde en rojo
                componenteEntrada.BorderBrush = Brushes.Red;
                componenteEntrada.Background = Brushes.LightCoral;
                imagenFeedBack.Source = imagCross;

                valido = false;
            }
            return valido;
         }
        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (ComprobarEntrada(txtUser.Text, usuario,
                    txtUser, imgUser)) 
                {

                    txtPass.IsEnabled = true;
                    txtPass.Focus();
                    txtUser.IsEnabled = false;
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ComprobarEntrada(txtUser.Text, usuario,
                txtUser, imgUser)
                &&
                ComprobarEntrada(txtPass.Password, password,
                txtPass, imgPass))
            {
                //Application.Current.Shutdown(); 
                MainWindow ventana_principal = new MainWindow();
                ventana_principal.Visibility = Visibility.Visible;
                //this.Visibility = Visibility.Hidden;
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void imgUser_MouseEnter(object sender, MouseEventArgs e)
        {

            switch (imgUser.Source)
            {
                case var _ when imgUser.Source == imagCheck:
                    imgUser.ToolTip = "User correcto";


                    break;
                case var _ when imgUser.Source == imagCross:
                    imgUser.ToolTip = "User incorrecto";

                    break;
                default:
                    imgUser.ToolTip = "DNI+letra";
                    break;
            }

        }

        private void imgPass_MouseEnter(object sender, MouseEventArgs e)
        {

            switch (imgPass.Source)
            {
                case var _ when imgPass.Source == imagCheck:
                    imgPass.ToolTip = "Password correcta";
                    blockPass.Foreground = Brushes.Green;
                    break;
                case var _ when imgPass.Source == imagCross:
                    imgPass.ToolTip = "Password incorrecta";
                    blockPass.Foreground = Brushes.Red;
                    break;
                default:
                    
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            MessageBox.Show("Gracias por usar nuestra aplicación...", "Despedida");
        }
    }
}
