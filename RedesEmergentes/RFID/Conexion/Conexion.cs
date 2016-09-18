using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID.Conexion
{
    public class Conexion
    {
        public static MySqlConnection ConexionDB()
        {
            //Variables para la conexion
            MySqlConnection conexion = new MySqlConnection();
            String url = "Server=localhost; Database= ; Uid=root; Pwd=1234";

            //Conexion
            try
            {
                conexion.ConnectionString = url; 
                conexion.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Error Contacte con Sistemas");
            }

            return conexion;
        }
    }
}
