using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using hospital_gui_wpf.src.dominio;

namespace hospital_gui_wpf.src.presentacion
{
	/// <summary>
	/// Lógica de interacción para Login.xaml
	/// </summary>
	public partial class Login : Window
	{
        public static Login InstanciaActual { get; set; }
		public Gestor gestorDatos;
        public bool cerrarDesdeCodigo = false;
        private readonly BitmapImage imagCheck, imagCross;

		public Login()
		{
			InitializeComponent();
			try
			{
				gestorDatos = new Gestor();
				InstanciaActual = this;
				imagCheck = new BitmapImage(new Uri("/datos/imagenes/check.png", UriKind.Relative));
				imagCross = new BitmapImage(new Uri("/datos/imagenes/cross.png", UriKind.Relative));
			}
			catch (Exception e)
			{
                MessageBox.Show("Error al cargar los datos de la aplicación: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                cerrarDesdeCodigo = true;
                this.Close();
            }
			
		}

		public Usuario getUser(string nombreUsuario)
		{
			return gestorDatos.Usuarios.Find(u => u.NombreUsuario == nombreUsuario);
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
			if (getUser(valorIntroducido) != null)
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
			if (getUser(valorIntroducido).Contrasena.Equals(valorValido))
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
				MainWindow ventana_principal = new MainWindow(gestorDatos, getUser(txtUser.Text));
				ventana_principal.Visibility = Visibility.Visible;
				cerrarDesdeCodigo = true;
				limpiarCampos();
				this.Visibility = Visibility.Hidden;
				
			}
		}
		private void limpiarCampos()
		{
			txtUser.Text = string.Empty;
			txtPass.Password = string.Empty;
			cerrarDesdeCodigo = false;
            txtUser.BorderBrush = Brushes.Gray;
            txtUser.Background = Brushes.Transparent;
			txtUser.ToolTip = "Usuario a loguearse";
            imgUser.Source = new BitmapImage(new Uri("/datos/imagenes/baseline_help_white_24dp.png", UriKind.Relative));
            txtPass.BorderBrush = Brushes.Gray;
            txtPass.Background = Brushes.Transparent;
			txtPass.ToolTip = "Contraseña del usuario, primero debes introducir un usuario válido";
            imgPass.Source = new BitmapImage(new Uri("/datos/imagenes/baseline_help_white_24dp.png", UriKind.Relative));
			txtUser.IsEnabled = true;

			
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
			if (ComprobarEntradaNombre(txtUser.Text, txtUser, imgUser))
			{
				txtPassWatermark.Visibility = Visibility.Collapsed;
			}
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
            if (string.IsNullOrEmpty(txtPass.Password))
            {
                txtPassWatermark.Visibility = Visibility.Visible;
            }
        }

		private void txtPass_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return || e.Key == Key.Tab)
			{
				btnLogin_Click(sender, e);
			}
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			txtUser.Focus();
        }
    }
}

