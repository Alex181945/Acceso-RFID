using AForge.Video;
using AForge.Video.DirectShow;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID
{
    public partial class Form1 : Form
    {

        //Variables para formulario
        private int    cNumeroC  = 0;
        private string cNombre   = "";
        private string cAP1      = "";
        private string cAP2      = "";
        private string cSangre   = "";
        private string cCarrera  = "";
        private int    cSemestre = 0;
        private string cTutor    = "";
        private string cDirecc   = "";
        private int    cTel      = 0;


        //Variable para imagen
        private byte[] bImagen;
        private bool ExisteDispositivo = false;
        private FilterInfoCollection DispositivoDeVideo;
        private VideoCaptureDevice FuenteDeVideo = null;


        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            BuscarDispositivos();
            //Arduino
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = "COM8";
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.ReceivedBytesThreshold = 1;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(LeerID);
            serialPort.Open();
            serialPort.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Registro();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Capurar Imagen
            if (btnImagen.Text == "Capturar")
            {
                if (ExisteDispositivo)
                {
                    FuenteDeVideo = new VideoCaptureDevice(DispositivoDeVideo[0].MonikerString);
                    FuenteDeVideo.NewFrame += new NewFrameEventHandler(Video_NuevoFrame);
                    FuenteDeVideo.Start();
                    btnImagen.Text = "Guardar";

                }
                else
                    MessageBox.Show("Error: No se encuentra dispositivo.");
            }

            else
            {
                if (FuenteDeVideo.IsRunning)
                {
                    TerminarFuenteDeVideo();
                    MemoryStream ms = new MemoryStream();
                    boxImagen.Image.Save(ms, ImageFormat.Jpeg);
                    bImagen = ms.ToArray();
                }
            }
        }

        public void BuscarDispositivos()
        {
            DispositivoDeVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (DispositivoDeVideo.Count == 0)
            {
                ExisteDispositivo = false;
            }

            else
            {
                ExisteDispositivo = true;
            }
        }

        public void TerminarFuenteDeVideo()
        {
            if (!(FuenteDeVideo == null))
                if (FuenteDeVideo.IsRunning)
                {
                    FuenteDeVideo.SignalToStop();
                    FuenteDeVideo = null;
                }

        }

        public void Video_NuevoFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //Imagen que si vizualiza en el picture box
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            boxImagen.Image = Imagen;
        }

        public String Validaciones()
        {
            if (cNumeroC == 0)
            {
                return "No hay Numero de Control Asociaso al Alumno";
            }
            if (cNombre == "" || cNombre == null){
                return "Ingrese el Nombre del Alumno";
            }

            if (cAP1 == "" || cAP1 == null){
                return "Ingrese el Apellido Paterno del Alumno";
            }

            if (cCarrera == "" || cCarrera == null){
                return "Ingrese la Carrera del Alumno";
            }

            if (cSemestre == 0){
                return "Ingrese el Semestre del Alumno";
            }

            if (cTel == 0){
                return "Ingrese el Numero Telefonico del Alumno";
            }

            if (cDirecc == "" || cDirecc == null){
                return "Ingrese la Direccion del Alumno";
            }

            if (cTutor == "" || cTutor == null){
                return "Ingrese el Nombre del Tutor del Alumno";
            }

            if (bImagen == null)
            {
                return "Capture la Foto del Alumno";
            }

            return "";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            const string message = "Estas a punto de salir, ¿deseas continuar?";
            const string caption = "Cerrando Aplicacion";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
            }
            else {
                Application.Exit();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //Modificacion de los campos

            if (ConsultaLectura() == true){

                //Quita solo lectura en consulta
                txtNombre.ReadOnly    = false;
                txtAP1.ReadOnly       = false;
                txtAP2.ReadOnly       = false;
                txtCarrera.ReadOnly   = false;
                txtSemestre.ReadOnly  = false;
                txtSangre.ReadOnly    = false;
                txtTelefono.ReadOnly  = false;
                txtDireccion.ReadOnly = false;
                txtTutor.ReadOnly     = false;

            }
            else
            {
                //Conexion a la Base de Datos
                MySqlConnection conn = Conexion.Conexion.ConexionDB();
                MySqlCommand cmd = new MySqlCommand();

                try
                {
                    //Recapturando Datos
                    cNumeroC = int.Parse(txtNumeroC.Text);
                    cNombre = txtNombre.Text.ToUpper();
                    cAP1 = txtAP1.Text.ToUpper();
                    cAP2 = txtAP2.Text.ToUpper();
                    cSangre = txtSangre.Text.ToUpper();
                    cCarrera = txtCarrera.Text.ToUpper();
                    cSemestre = int.Parse(txtSemestre.Text);
                    cTutor = txtTutor.Text.ToUpper();
                    cDirecc = txtDireccion.Text.ToUpper();
                    cTel = int.Parse(txtTelefono.Text);

                    //Almacenamiento de registro en base de datos
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE alumnos SET nombre = @nombre, Apaterno = @Apaterno, Amaterno = @Amaterno, carrera = @carrera," +
                        "semestre = @semestre, tipo_sandre = @tipo_sandre, telecont = @telecont, dircont = @dircont, nom_tutor = @nom_tutor," +
                        "imagen = @imagen WHERE num_control = @num_control";
                    cmd.Parameters.AddWithValue("@num_control", cNumeroC);
                    cmd.Parameters.AddWithValue("@nombre", cNombre);
                    cmd.Parameters.AddWithValue("@Apaterno", cAP1);
                    cmd.Parameters.AddWithValue("@Amaterno", cAP2);
                    cmd.Parameters.AddWithValue("@carrera", cCarrera);
                    cmd.Parameters.AddWithValue("@semestre", cSemestre);
                    cmd.Parameters.AddWithValue("@tipo_sandre", cSangre);
                    cmd.Parameters.AddWithValue("@telecont", cTel);
                    cmd.Parameters.AddWithValue("@dircont", cDirecc);
                    cmd.Parameters.AddWithValue("@nom_tutor", cTutor);
                    cmd.Parameters.AddWithValue("@imagen", bImagen);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cambio Guardados");
                    Limpiar();
                }
                catch (Exception)
                {
                    MessageBox.Show("Error Intente Mas Tarde");
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void Consulta(int numControl)
        {

            //Conexion a la Base de Datos
            MySqlConnection conn = Conexion.Conexion.ConexionDB();
            MySqlCommand cmd = new MySqlCommand();


            //Campos para solo lectura en consulta
            txtNombre.ReadOnly    = true;
            txtAP1.ReadOnly       = true;
            txtAP2.ReadOnly       = true;
            txtCarrera.ReadOnly   = true;
            txtSemestre.ReadOnly  = true;
            txtSangre.ReadOnly    = true;
            txtTelefono.ReadOnly  = true;
            txtDireccion.ReadOnly = true;
            txtTutor.ReadOnly     = true;

            //Consulta exclusiva para datos, no foto
            try
            {

                //Consulta de registro en base de datos
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM alumnos WHERE num_control = @num_control";
                
                cmd.Parameters.AddWithValue("@num_control", numControl);

                MySqlDataReader table = cmd.ExecuteReader();

                while (table.Read())
                    {
                        //txtNumeroC.Text = table.GetInt32("num_control").ToString();
                        txtNombre.Text = table.GetString("nombre");
                        txtAP1.Text = table.GetString("Apaterno");
                        txtAP2.Text = table.GetString("Amaterno");
                        txtCarrera.Text = table.GetString("carrera");
                        txtSemestre.Text = table.GetInt32("semestre").ToString();
                        txtSangre.Text = table.GetString("tipo_sandre");
                        txtTelefono.Text = table.GetInt32("telecont").ToString();
                        txtDireccion.Text = table.GetString("dircont");
                        txtTutor.Text = table.GetString("nom_tutor");
                    }

                conn.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Error Intente Mas Tarde");
                throw;
            }

            //Consulta exclusiva para foto, no datos
            try
            {
                conn = Conexion.Conexion.ConexionDB();

                //Consulta de registro en base de datos
                cmd.Connection = conn;
                cmd.CommandText = "SELECT imagen FROM alumnos WHERE num_control = @num_control";

                bImagen = (byte[])cmd.ExecuteScalar();
                var stream = new MemoryStream(bImagen);

                Image img = Image.FromStream(stream);
                boxImagen.Image = img;

                conn.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Error Intente Mas Tarde");
                throw;
            }

        }

        public Boolean ConsultaLectura()
        {
            if (txtNumeroC.ReadOnly == true &&
                txtNombre.ReadOnly == true &&
                txtAP1.ReadOnly == true &&
                txtAP2.ReadOnly == true &&
                txtCarrera.ReadOnly == true &&
                txtSemestre.ReadOnly == true &&
                txtSangre.ReadOnly == true &&
                txtTelefono.ReadOnly == true &&
                txtDireccion.ReadOnly == true &&
                txtTutor.ReadOnly == true)
            {
                return true;
            }

            return false;
            
        }

        public void Limpiar()
        {
            txtNumeroC.Text = "";
            txtNombre.Text = "";
            txtAP1.Text = "";
            txtAP2.Text = "";
            txtCarrera.Text = "";
            txtSemestre.Text = "";
            txtSangre.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            txtTutor.Text = "";
        }

        public void Registro()
        {
            //Capturar Registro y Mandar a la Base

            //Obtencion de valores de cajas de texto
            cNumeroC = int.Parse(txtNumeroC.Text);
            cNombre = txtNombre.Text.ToUpper();
            cAP1 = txtAP1.Text.ToUpper();
            cAP2 = txtAP2.Text.ToUpper();
            cSangre = txtSangre.Text.ToUpper();
            cCarrera = txtCarrera.Text.ToUpper();
            cSemestre = int.Parse(txtSemestre.Text);
            cTutor = txtTutor.Text.ToUpper();
            cDirecc = txtDireccion.Text.ToUpper();
            cTel = int.Parse(txtTelefono.Text);

            //Validacion de campos
            if (Validaciones() != "")
                MessageBox.Show(Validaciones());
            else
            {

                //Conexion a la Base de Datos
                MySqlConnection conn = Conexion.Conexion.ConexionDB();
                MySqlCommand cmd = new MySqlCommand();

                try
                {

                    //Almacenamiento de registro en base de datos
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO alumnos VALUES (@num_control,@nombre," +
                        "@Apaterno,@Amaterno,@carrera,@semestre,@tipo_sandre,@telecont,@dircont,@nom_tutor,@imagen)";
                    cmd.Parameters.AddWithValue("@num_control", cNumeroC);
                    cmd.Parameters.AddWithValue("@nombre", cNombre);
                    cmd.Parameters.AddWithValue("@Apaterno", cAP1);
                    cmd.Parameters.AddWithValue("@Amaterno", cAP2);
                    cmd.Parameters.AddWithValue("@carrera", cCarrera);
                    cmd.Parameters.AddWithValue("@semestre", cSemestre);
                    cmd.Parameters.AddWithValue("@tipo_sandre", cSangre);
                    cmd.Parameters.AddWithValue("@telecont", cTel);
                    cmd.Parameters.AddWithValue("@dircont", cDirecc);
                    cmd.Parameters.AddWithValue("@nom_tutor", cTutor);
                    cmd.Parameters.AddWithValue("@imagen", bImagen);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registro Guardado");

                    Limpiar();
                }
                catch (Exception)
                {
                    MessageBox.Show("Error Intente Mas Tarde");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void LeerID(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            String aux = sp.ReadExisting();

            Decision(int.Parse(aux));
            
        }

        public void Decision(int numControl)
        {
            //Conexion a la Base de Datos
            MySqlConnection conn = Conexion.Conexion.ConexionDB();
            MySqlCommand cmd = new MySqlCommand();

            //Consulta de registro en base de datos
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM alumnos WHERE num_control = @num_control";
            cmd.Parameters.AddWithValue("@num_control", numControl);

            MySqlDataReader table = cmd.ExecuteReader();

            if (table.Read())
            {
                conn.Close();
                Consulta(numControl);
            }
            else
            {
                conn.Close();
                Registro();
            }

        }
    }
}
