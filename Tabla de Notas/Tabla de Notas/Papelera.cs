using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Tabla_de_Notas
{
    public partial class Papelera : Form
    {
        public Papelera()
        {
            InitializeComponent();
        }
        Form1 formulario1 = new Form1();
        ValidarCajas validacion = new ValidarCajas();
        
        private void btnVolver_Click(object sender, EventArgs e)
        {
            Form1 formulario1 = new Form1();
            formulario1.Show();
            this.Close();
        }


        private void Papelera_Load(object sender, EventArgs e)
        {
            if (File.Exists("C:/net/listaAlumno.xml") && File.Exists("C:/net/alumnosBorrados.xml"))
            {
                formulario1.listaAlumnos.Clear();
                formulario1.alumnosBorrados.Clear();
                XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
                FileStream leerXml = File.OpenRead("C:/net/listaAlumno.xml");
                FileStream leerXmlAlumnosBorrados = File.OpenRead("C:/net/alumnosBorrados.xml");// Abrimos un Xml de los alumnos que se van a eliminar
                formulario1.listaAlumnos = (List<Alumno>)codificador.Deserialize(leerXml);
                formulario1.alumnosBorrados = (List<Alumno>)codificador.Deserialize(leerXmlAlumnosBorrados);// utilizamos el codificador para leer el archivo de alumnos borrados y guardarlo en la lista de los borrados
                leerXml.Close();
                leerXmlAlumnosBorrados.Close();
            }
            dtgPapelera.DataSource = null;
            dtgPapelera.DataSource = formulario1.alumnosBorrados;
            
        }

        private void Papelera_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
            TextWriter escribirXml = new StreamWriter("C:/net/listaAlumno.xml");
            TextWriter escribirXmlAlumnosBorrados = new StreamWriter("C:/net/alumnosBorrados.xml");
            codificador.Serialize(escribirXml, formulario1.listaAlumnos);
            codificador.Serialize(escribirXmlAlumnosBorrados, formulario1.alumnosBorrados);
            escribirXml.Close();
            escribirXmlAlumnosBorrados.Close();
        }

        public Boolean ExisteCodigo(int codigo)
        {
            foreach (Alumno myAlumno in formulario1.alumnosBorrados)
            {
                if (myAlumno.Codigo == codigo)
                    return true;
            }
            return false;
        }

        private Alumno obtenerDatos(int codigo)
        {
            foreach (Alumno myAlumno in formulario1.alumnosBorrados)
            {
                if (myAlumno.Codigo == codigo)
                    return myAlumno;
            }
            return null;
        }

        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            if (!validacion.Vacio(txtCodigo, errorProvider1, "Para buscar, debe ingresar un código"))
                if (validacion.TipoNumero(txtCodigo, errorProvider1, "Debe digitar numeros"))
                {
                    if (ExisteCodigo(Convert.ToInt32(txtCodigo.Text)))
                    {
                        Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtCodigo.Text));
                        MessageBox.Show("Alumno Restaurado", "Tabla de Notas");
                        formulario1.listaAlumnos.Add(myAlumno);
                        formulario1.alumnosBorrados.Remove(myAlumno);

                        dtgPapelera.DataSource = null;
                        dtgPapelera.DataSource = formulario1.alumnosBorrados;
                        

                    }
                    else
                    {
                        errorProvider1.SetError(txtCodigo, "El código no existe");
                        txtCodigo.Focus();
                        txtCodigo.Clear();
                        return;
                    }
                }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!validacion.Vacio(txtCodigo, errorProvider1, "Para buscar, debe ingresar un código"))
                if (validacion.TipoNumero(txtCodigo, errorProvider1, "Debe digitar numeros"))
                {
                    if (ExisteCodigo(Convert.ToInt32(txtCodigo.Text)))
                    {
                        Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtCodigo.Text));                        
                        formulario1.alumnosBorrados.Remove(myAlumno);
                        MessageBox.Show("Alumno Eliminado Definitivamente", "Tabla de Notas");
                        dtgPapelera.DataSource = null;
                        dtgPapelera.DataSource = formulario1.alumnosBorrados;


                    }
                    else
                    {
                        errorProvider1.SetError(txtCodigo, "El código no existe");
                        txtCodigo.Focus();
                        txtCodigo.Clear();
                        return;
                    }
                }
        }
    }
}
