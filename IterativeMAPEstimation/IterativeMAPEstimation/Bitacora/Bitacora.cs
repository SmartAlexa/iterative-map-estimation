using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IterativeMAPEstimation.Bitacora
{
    public class Bitacora
    {

        private static string path;
        private string pathArchivo;
        private string fecha = DateTime.Now.ToShortDateString();
        private StreamWriter sw;

    public Bitacora()
    {
    
        path = Application.StartupPath;
        pathArchivo = path + "//bitacora//bitacora.txt";


    
    }

        public void Escribir(string texto)
        {
            try
            {
                sw = new StreamWriter(pathArchivo,true);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n" + "Error loading the StreamWriter ");
            }
            string hora = DateTime.Now.ToShortTimeString();
            sw.WriteLine(fecha + "  " + hora + "  " + texto + "\n");
            sw.Close();
        
        }

    }
}
