using KEYPAD.Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KEYPAD.Dao
{
    class CRUDAlumno
    {
        public void InsertarAlumno(Alumno alumno)
        {
            //Este metodo va a guardar el registro de un alumno

            //Conexion a la Base de Datos
            MySqlConnection conn = ConexionBD.ConexionDB();
            MySqlCommand cmd = new MySqlCommand();

            //Insertando registro
            try
            {
                //Almacenamiento de registro en base de datos
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO alumnos VALUES (@num_control,@nombre," +
                    "@Apaterno,@Amaterno,@carrera,@semestre,@tipo_sandre,@telecont,@dircont,@nom_tutor,@imagen)";
                cmd.Parameters.AddWithValue("@num_control", alumno.NumeroC);
                cmd.Parameters.AddWithValue("@nombre", alumno.Nombre);
                cmd.Parameters.AddWithValue("@Apaterno", alumno.AP1);
                cmd.Parameters.AddWithValue("@Amaterno", alumno.AP2);
                cmd.Parameters.AddWithValue("@carrera", alumno.Carrera);
                cmd.Parameters.AddWithValue("@semestre", alumno.Semestre);
                cmd.Parameters.AddWithValue("@tipo_sandre", alumno.Sangre);
                cmd.Parameters.AddWithValue("@telecont", alumno.Telefono);
                cmd.Parameters.AddWithValue("@dircont", alumno.Direccion);
                cmd.Parameters.AddWithValue("@nom_tutor", alumno.Tutor);
                cmd.Parameters.AddWithValue("@imagen", alumno.Imagen);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Registro Almacenado");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: {0}" + e.Message.ToString());
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void ActualizarAlumno(Alumno alumno)
        {
            
        }

        public Alumno ConsultaAlumno(string numeroControl)
        {
            return null;
        }
        
        public void BorrarAlumno(string numeroControl)
        {

        }
    }
}
