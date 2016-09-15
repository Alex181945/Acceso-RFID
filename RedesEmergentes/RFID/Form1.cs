using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID
{
    public partial class Form1 : Form
    {

        //Variables
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
        
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

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

            //Validacion de imagen
            if (bImagen == null)
            {
                MessageBox.Show("Captura la imagen del estudiante");
                return;
            }

              


        }
    }
}
