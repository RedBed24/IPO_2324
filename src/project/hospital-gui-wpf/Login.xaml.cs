using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
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
        private BitmapImage imagQuestion = new BitmapImage(new Uri("/imagenes/baseline_help_white_24dp.png", UriKind.Relative));
        private Dictionary<string, string> usuarios = new Dictionary<string, string>
        {
            { "noelia", "1234" },
            { "samuel", "E5pej0" },
            { "antonio", "contrasena"}
    
        };

        public Login()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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


        private bool ComprobarEntradaNombre(string valorIntroducido,
        Control componenteEntrada, Image imagenFeedBack)
        {
            bool valido = false;
            if (usuarios.ContainsKey(valorIntroducido))
            {
                componenteEntrada.BorderBrush = Brushes.Green;
                componenteEntrada.Background = Brushes.LightGreen;
                imagenFeedBack.Source = imagCheck;
                valido = true;
            
            }
            else
            {
                // El nombre de usuario no existe
                componenteEntrada.BorderBrush = Brushes.Red;
                componenteEntrada.Background = Brushes.LightCoral;
                imagenFeedBack.Source = imagCross;
            }

            return valido;
        }

        private bool ComprobarEntradaContraseña(string valorIntroducido, string valorValido,
        Control componenteEntrada, Image imagenFeedBack)
        {
            bool valido = false;
            if (usuarios[valorIntroducido].Equals(valorValido))
            {
                componenteEntrada.BorderBrush = Brushes.Green;
                componenteEntrada.Background = Brushes.LightGreen;
                imagenFeedBack.Source = imagCheck;
                valido = true;
            }
            else
            {
                componenteEntrada.BorderBrush = Brushes.Red;
                componenteEntrada.Background = Brushes.LightCoral;
                imagenFeedBack.Source = imagCross;
            }
   
            return valido;
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (ComprobarEntradaNombre(txtUser.Text, txtUser, imgUser))
                {
                    txtPass.IsEnabled = true;
                    txtPass.Focus();
                    txtUser.IsEnabled = false;
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ComprobarEntradaContraseña(txtUser.Text, txtPass.Password, txtPass, imgPass))
            {
                MainWindow ventana_principal = new MainWindow();
                ventana_principal.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
            }
        }

        private void imgUser_MouseEnter(object sender, MouseEventArgs e)
        {

            switch (imgUser.Source)
            {
                case var _ when imgUser.Source == imagCheck:
                    imgUser.ToolTip = "Usuario correcto";


                    break;
                case var _ when imgUser.Source == imagCross:
                    imgUser.ToolTip = "Usuario incorrecto";

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
                    imgPass.ToolTip = "Contraseña correcta";
                    blockPass.Foreground = Brushes.Green;
                    break;
                case var _ when imgPass.Source == imagCross:
                    imgPass.ToolTip = "Contraseña incorrecta";
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

