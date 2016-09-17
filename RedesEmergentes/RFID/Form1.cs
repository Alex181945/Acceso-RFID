using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID
{
    public partial class Form1 : Form
    {

        //Variables para formulario
        private string cNumeroC  = "";
        private string cNombre   = "";
        private string cAP1      = "";
        private string cAP2      = "";
        private string cSangre   = "";
        private string cCarrera  = "";
        private string cSemestre = "";
        private string cTutor    = "";
        private string cDirecc   = "";
        private string cTel      = "";


        //Variable para imagen
        private byte[] bImagen;
        private bool ExisteDispositivo = false;
        private FilterInfoCollection DispositivoDeVideo;
        private VideoCaptureDevice FuenteDeVideo = null;


        public Form1()
        {
            InitializeComponent();
            BuscarDispositivos();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Capturar Registro y Mandar a la Base
            //Validacion de imagen
            if (bImagen == null)
            {
                MessageBox.Show("Captura la imagen del estudiante");
                return;
            }

            //Obtencion de valores de cajas de texto
            cNumeroC   = txtNumeroC.Text;
            cNombre    = txtNombre.Text;
            cAP1       = txtAP1.Text;
            cAP2       = txtAP2.Text;
            cSangre    = txtSangre.Text;
            cCarrera   = txtCarrera.Text;
            cSemestre  = txtSemestre.Text;
            cTutor     = txtTutor.Text;
            cDirecc    = txtDireccion.Text;
            cTel       = txtTelefono.Text;



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
                    boxImagen.Image.Save(ms, ImageFormat.Gif);
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
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            boxImagen.Image = Imagen;
        }
    }
}
