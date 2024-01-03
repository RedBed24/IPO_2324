using System;

namespace hospital_gui_wpf.src.dominio
{
    public class Historial
    {
        public string Dolencias { get; set; }
        public string Tratamientos { get; set; }
        public DateTime FechaAtencion { get; set; }
        public Uri Foto { get; set; }

        public Historial(string dolencias, string tratamientos, DateTime fechaAtencion, Uri foto)
        {
			Dolencias = dolencias;
			Tratamientos = tratamientos;
			FechaAtencion = fechaAtencion;
			Foto = foto;
		}

    }
}
