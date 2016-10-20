using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEYPAD.Modelo
{
    class Alumno
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
        private byte[] bImagen;

        //Propiedades
        public string NumeroC
        {
            get { return cNumeroC; }
            set { cNumeroC = value; }
        }

        public string Nombre
        {
            get { return cNombre; }
            set { cNombre = value;  }
        }

        public string AP1
        {
            get { return cAP1; }
            set { cAP1 = value; }
        }

        public string AP2
        {
            get { return cAP2;  }
            set { cAP2 = value; }
        }

        public string Sangre
        {
            get { return cSangre; }
            set { cSangre = value; }
        }

        public string Carrera
        {
            get { return cCarrera; }
            set { cCarrera = value; }
        }

        public string Semestre
        {
            get { return cSemestre; }
            set { cSemestre = value; }
        } 

        public string Tutor
        {
            get { return cTutor; }
            set { cTutor = value; }
        }

        public string Direccion
        {
            get { return cDirecc; }
            set { cDirecc = value; }
        }

        public string Telefono
        {
            get { return cTel; }
            set { cTel = value; }
        }

        public byte[] Imagen
        {
            get { return bImagen; }
            set { bImagen = value; }
        }

    }
}
