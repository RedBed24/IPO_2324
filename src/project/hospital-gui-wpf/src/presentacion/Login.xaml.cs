using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace hospital_gui_wpf.src.presentacion
{
	/// <summary>
	/// Lógica de interacción para Login.xaml
	/// </summary>
	public partial class Login : Window
	{
        private bool cerrarDesdeCodigo = false;
        private TextBlock txtPassWatermark;
        private readonly BitmapImage imagCheck = new BitmapImage(new Uri("/datos/imagenes/check.png", UriKind.Relative));
		private readonly BitmapImage imagCross = new BitmapImage(new Uri("/datos/imagenes/cross.png", UriKind.Relative));
		private readonly Dictionary<string, string> usuarios = new Dictionary<string, string>
		{
			{ "noelia", "1234" },
			{ "samuel", "E5pej0" },
			{ "antonio", "contrasena"}
		};

		public Login()
		{
			InitializeComponent();
			txtUser.Focus();
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
			this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
		}


		private bool ComprobarEntradaNombre(string valorIntroducido, Control componenteEntrada, Image imagenFeedBack)
		{
			bool valido = false;
			if (usuarios.ContainsKey(valorIntroducido))
			{
				componenteEntrada.BorderBrush = Brushes.Green;
				componenteEntrada.Background = Brushes.LightGreen;
				imagenFeedBack.Source = imagCheck;
				valido = true;
				imagenFeedBack.ToolTip = "Usuario correcto";
				txtPass.Focus();
				componenteEntrada.IsEnabled = false;
			}
			else
			{
				// El nombre de usuario no existe
				componenteEntrada.BorderBrush = Brushes.Red;
				componenteEntrada.Background = Brushes.LightCoral;
				imagenFeedBack.Source = imagCross;
				imagenFeedBack.ToolTip = "Usuario incorrecto";
				componenteEntrada.Focus();
			}

			return valido;
		}

		private bool ComprobarEntradaContraseña(string valorIntroducido, string valorValido, Control componenteEntrada, Image imagenFeedBack)
		{
			bool valido = false;
			if (usuarios[valorIntroducido].Equals(valorValido))
			{
				componenteEntrada.BorderBrush = Brushes.Green;
				componenteEntrada.Background = Brushes.LightGreen;
				imagenFeedBack.Source = imagCheck;
				imgPass.ToolTip = "Contraseña correcta";
				valido = true;
			}
			else
			{
				componenteEntrada.BorderBrush = Brushes.Red;
				componenteEntrada.Background = Brushes.LightCoral;
				imgPass.ToolTip = "Contraseña incorrecta";
				imagenFeedBack.Source = imagCross;
			}
   
			return valido;
		}

		private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return || e.Key == Key.Tab)
			{
				ComprobarEntradaNombre(txtUser.Text, txtUser, imgUser);
			}
		}

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			if (ComprobarEntradaNombre(txtUser.Text, txtUser, imgUser) && ComprobarEntradaContraseña(txtUser.Text, txtPass.Password, txtPass, imgPass))
			{
				MainWindow ventana_principal = new MainWindow();
				ventana_principal.Visibility = Visibility.Visible;
				cerrarDesdeCodigo = true;
				this.Close();
			}
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
				if (!cerrarDesdeCodigo)
				{
					MessageBox.Show("Gracias por usar nuestra aplicación...", "Despedida");
				}
		}

		private void txtPass_GotFocus(object sender, RoutedEventArgs e)
		{
			ComprobarEntradaNombre(txtUser.Text, txtUser, imgUser);
            txtPassWatermark = (TextBlock)txtPass.Template.FindName("txtPassWatermark", txtPass);
            txtPassWatermark.Visibility = Visibility.Collapsed;
        }


		private void Image_MouseEnter(object sender, MouseEventArgs e)
		{
			// TODO: Hacer algo con la imagen? tipo que parezca que el corazón late
		}

		private void Image_MouseLeave(object sender, MouseEventArgs e)
		{
			// TODO: Hacer algo con la imagen? tipo que parezca que el corazón late
		}

        private void txtUser_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUserWatermark.Visibility = Visibility.Collapsed;
        }

        private void txtUser_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text))
            {
                txtUserWatermark.Visibility = Visibility.Visible;
            }
        }

        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPassWatermark = (TextBlock)txtPass.Template.FindName("txtPassWatermark", txtPass);
            if (string.IsNullOrEmpty(txtPass.Password))
            {
                txtPassWatermark.Visibility = Visibility.Visible;
            }
        }
    }
}

