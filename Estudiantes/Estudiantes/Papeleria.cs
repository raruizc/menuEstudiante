using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace Estudiantes
{
    public partial class Papeleria : Form
    {
        public Papeleria()
        {
            InitializeComponent();
        }

        Form1 formulario1 = new Form1();
        ValidarCajas validacion = new ValidarCajas();
        private void btnVolver_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void Papeleria_Load(object sender, EventArgs e)
        {
            
            formulario1.listaAlumnos.Clear();
            formulario1.alumnosBorrados.Clear();
            

            if (File.Exists("C:/net/listaAlumno.xml"))
            {
                XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
                FileStream leerXml = File.OpenRead("C:/net/listaAlumno.xml");
                formulario1.listaAlumnos = (List<Alumno>)codificador.Deserialize(leerXml);
                leerXml.Close();
            }
            if (File.Exists("C:/net/alumnosBorrados.xml"))
            {
                XmlSerializer codificadorAlumnosBorrados = new XmlSerializer(typeof(List<Alumno>));
                FileStream leerAlumnosBorrados = File.OpenRead("C:/net/alumnosBorrados.xml");
                formulario1.alumnosBorrados = (List<Alumno>)codificadorAlumnosBorrados.Deserialize(leerAlumnosBorrados);
                leerAlumnosBorrados.Close();
            }

            dgvPapelera.DataSource = null;
            dgvPapelera.DataSource = formulario1.alumnosBorrados;
            
        }

        private void Papeleria_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>)); // Crear un archivo XML
            TextWriter escribirXml = new StreamWriter("C:/net/listaAlumno.xml");
            TextWriter escribirBorrados = new StreamWriter("C:/net/alumnosBorrados.xml");
            codificador.Serialize(escribirXml, formulario1.listaAlumnos);
            codificador.Serialize(escribirBorrados, formulario1.alumnosBorrados);
            escribirXml.Close();
            escribirBorrados.Close();
        }

        private Boolean ExisteCodigo(int codigo)
        {

            foreach (Alumno myAlumno in formulario1.alumnosBorrados)
            {
                if (myAlumno.Codigo == codigo)
                {
                    return true;
                }
            }
            return false;
        }
        private Alumno obtenerDatos(int codigo)
        {
            foreach (Alumno myAlumno in formulario1.alumnosBorrados)
            {
                if (myAlumno.Codigo == codigo)
                {
                    return myAlumno;
                }
            }
            return null;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!validacion.Vacio(txtConsulta, errorProvider, "Para buscar debe haber un codigo"))
            {
                if (validacion.TipoNumero(txtConsulta, errorProvider, "El Codigo en numerico"))
                {
                    // valiar que existe
                    if (ExisteCodigo(Convert.ToInt32(txtConsulta.Text)))
                    {
                        Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtConsulta.Text));
                        MessageBox.Show("Se encontró el alumno", "Estudiantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnRecuperar.Enabled = true;
                        btnEliminar.Enabled = true;
                        txtConsulta.ReadOnly = true;
                        
                    }
                    else
                    {
                        errorProvider.SetError(txtConsulta, "El Codigo no exite en la lineas");
                        txtConsulta.Focus();                        
                        return;
                    }
                }
            }
        }

        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            if (!validacion.Vacio(txtConsulta, errorProvider, "Para buscar debe haber un codigo"))
            {
                if (validacion.TipoNumero(txtConsulta, errorProvider, "El Codigo en numerico"))
                {
                    // valiar que existe
                    if (ExisteCodigo(Convert.ToInt32(txtConsulta.Text)))
                    {
                        Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtConsulta.Text));
                        formulario1.listaAlumnos.Add(myAlumno);
                        formulario1.alumnosBorrados.Remove(myAlumno);

                        dgvPapelera.DataSource = null;
                        dgvPapelera.DataSource = formulario1.alumnosBorrados;
                        MessageBox.Show("Se restauró correctamente el alumno", "Estudiantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtConsulta.Clear();
                        txtConsulta.Focus();
                        btnRecuperar.Enabled = false;
                        btnEliminar.Enabled = false;
                        txtConsulta.ReadOnly = false;

                    }
                    else
                    {
                        errorProvider.SetError(txtConsulta, "El Codigo no exite en la lineas");
                        txtConsulta.Focus();
                        return;
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //----Borrar datos del estudiante----//
            DialogResult confirmarBorrar = MessageBox.Show("¿Quiere borrar los datos del alumno?", "Estudiantes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirmarBorrar == DialogResult.Yes)
            {
                Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtConsulta.Text));                
                formulario1.alumnosBorrados.Remove(myAlumno);
                dgvPapelera.DataSource = null;
                dgvPapelera.DataSource = formulario1.alumnosBorrados;
                txtConsulta.Clear();
                txtConsulta.Focus();
                btnRecuperar.Enabled = false;
                btnEliminar.Enabled = false;
                txtConsulta.ReadOnly = false;
            }
        }
    }
}
