using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // Manejo de Archivos. Escritura Lectura
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization; // Manejo de Archivos XML.

namespace Estudiantes
{
    public partial class Form1 : Form
    {
        //Declarar objetos
        //ArrayList listaAlumnos = new ArrayList();        

        
        public List<Alumno> listaAlumnos = new List<Alumno>();
        public List<Alumno> alumnosBorrados = new List<Alumno>();
        ValidarCajas validacion = new ValidarCajas();
        public Form1()
        {
            InitializeComponent();
        }

                
        private void tsAdd_Click(object sender, EventArgs e)
        {
            //------ Código de validación ----//
            if (!validacion.Vacio(txtCode, errorM, "El código no puede ser vacio."))
                if (validacion.TipoNumero(txtCode, errorM, "El código debe ser un número."))
                    if (!validacion.Vacio(txtName, errorM, "El nombre no puede ser vacio."))
                        if (validacion.TipoTexto(txtName, errorM, "El nombre debe ser un texto"))
                            if (!validacion.Vacio(txtEmail, errorM, "El correo no puede estar vacio"))
                                if (validacion.TipoCorreo(txtEmail, errorM, "Debe ser un correo"))
                                    if (!validacion.Vacio(txtGrade1, errorM, "La nota 1 no puede estar vacio"))
                                        if (validacion.TipoNumero(txtGrade1, errorM, "La nota 1 debe ser un número"))
                                            if (!validacion.Vacio(txtGrade2, errorM, "La nota 2 no puede estar vacio"))
                                                if (validacion.TipoNumero(txtGrade2, errorM, "La nota 2 debe ser un número"))
                                                    if (!validacion.Vacio(txtGrade3, errorM, "La nota 3 no puede estar vacio"))
                                                        if (validacion.TipoNumero(txtGrade3, errorM, "La nota 3 debe ser un número"))
                                                            if (!validacion.Vacio(txtGrade4, errorM, "La nota 4 no puede estar vacio"))
                                                                if (validacion.TipoNumero(txtGrade4, errorM, "La nota 4 debe ser un número"))
                                                                {
                                                                    if (!ExisteCodigo(Convert.ToInt32(txtCode.Text)))
                                                                    {
                                                                        insertarDatos();
                                                                        limpiar();
                                                                        errorM.Clear();
                                                                    }
                                                                    else
                                                                    {
                                                                        errorM.SetError(txtCode, "El código ya existe");
                                                                        txtCode.Focus();
                                                                        return;
                                                                    }

                                                                }
            //---------------------------------//

        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            // Crear Nuestro Archivo xml...
            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>)); // Crear un archivo XML
            TextWriter escribirXml = new StreamWriter("C:/net/listaAlumno.xml");
            codificador.Serialize(escribirXml, listaAlumnos);
            escribirXml.Close();

        }

        private void txXML_Click(object sender, EventArgs e)
        {
            // Cargar los datos del XML
            //generar una lista 
            // Mostrar esa lista en el DG

            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
            FileStream leerXml = File.OpenRead("C:/net/listaAlumno.xml");
            listaAlumnos = (List<Alumno>)codificador.Deserialize(leerXml);
            leerXml.Close();



            dgList.DataSource = null;
            dgList.DataSource = listaAlumnos;

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            listaAlumnos.Clear();
            alumnosBorrados.Clear();

            if (File.Exists("C:/net/listaAlumno.xml"))
            {
                XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
                FileStream leerXml = File.OpenRead("C:/net/listaAlumno.xml");
                listaAlumnos = (List<Alumno>)codificador.Deserialize(leerXml);
                leerXml.Close();
            }
            if (File.Exists("C:/net/alumnosBorrados.xml")){
                XmlSerializer codificadorAlumnosBorrados = new XmlSerializer(typeof(List<Alumno>));
                FileStream leerAlumnosBorrados = File.OpenRead("C:/net/alumnosBorrados.xml");
                alumnosBorrados = (List<Alumno>)codificadorAlumnosBorrados.Deserialize(leerAlumnosBorrados);
                leerAlumnosBorrados.Close();                
            }

            dgList.DataSource = null;
            dgList.DataSource = listaAlumnos;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Guardar el Archivo xml al salir del formulario //
            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>)); // Crear un archivo XML
            TextWriter escribirXml = new StreamWriter("C:/net/listaAlumno.xml");
            TextWriter escribirBorrados = new StreamWriter("C:/net/alumnosBorrados.xml");
            codificador.Serialize(escribirXml, listaAlumnos);
            codificador.Serialize(escribirBorrados, alumnosBorrados);
            escribirXml.Close();
            escribirBorrados.Close();
        }

        private void tsExit_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("¿Desea salir de la aplicación?", "Estudiantes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (r == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        
        private void txSearch_Click(object sender, EventArgs e)
        {
            //---- Buscar alumno según el código
            //// vamos a buscar a un aluno por el codigo 

            // valido que no este vacia
            if (!validacion.Vacio(txtCode, errorM, "Para buscar debe haber un codigo"))
            {
                if (validacion.TipoNumero(txtCode, errorM, "El Codigo en numerico"))
                {
                    // valiar que existe
                    if (ExisteCodigo(Convert.ToInt32(txtCode.Text)))
                    {
                        Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtCode.Text));
                        txtName.Text = myAlumno.Nombre;
                        txtEmail.Text = myAlumno.Correo;
                        txtGrade1.Text = myAlumno.Nota1.ToString();
                        txtGrade2.Text = myAlumno.Nota2.ToString();
                        txtGrade3.Text = myAlumno.Nota3.ToString();
                        txtGrade4.Text = myAlumno.Nota4.ToString();
                        tsEdit.Enabled = true; // Activa los botones
                        tsDelete.Enabled = true;
                        txtCode.Enabled = false;
                    }
                    else
                    {
                        errorM.SetError(txtCode, "El Codigo no exite en la lineas");
                        txtCode.Focus();
                        LimpiarCajar2();
                        return;
                    }
                }
            }
        }

        private void tsEdit_Click(object sender, EventArgs e)
        {
            //----------Editar los datos----//
                    if (!validacion.Vacio(txtName, errorM, "El nombre no puede ser vacio."))
                        if (validacion.TipoTexto(txtName, errorM, "El nombre debe ser un texto"))
                            if (!validacion.Vacio(txtEmail, errorM, "El correo no puede estar vacio"))
                                if (validacion.TipoCorreo(txtEmail, errorM, "Debe ser un correo"))
                                    if (!validacion.Vacio(txtGrade1, errorM, "La nota 1 no puede estar vacio"))
                                        if (validacion.TipoNumero(txtGrade1, errorM, "La nota 1 debe ser un número"))
                                            if (!validacion.Vacio(txtGrade2, errorM, "La nota 2 no puede estar vacio"))
                                                if (validacion.TipoNumero(txtGrade2, errorM, "La nota 2 debe ser un número"))
                                                    if (!validacion.Vacio(txtGrade3, errorM, "La nota 3 no puede estar vacio"))
                                                        if (validacion.TipoNumero(txtGrade3, errorM, "La nota 3 debe ser un número"))
                                                            if (!validacion.Vacio(txtGrade4, errorM, "La nota 4 no puede estar vacio"))
                                                                if (validacion.TipoNumero(txtGrade4, errorM, "La nota 4 debe ser un número"))
                                                                {
                                                                    guardarCambios();

                                                                }

        }

        //--- Métodos para validación de Datos y manipulación de Datos--
        private void insertarDatos()
        {
            //---------------- Agregar un Alumno ------//

            Alumno myAlumno = new Alumno(); // instanciar la clase Alumno y creo el objeto "myAlumno"
            // los elementos del formulario creo el alumno 
            
            myAlumno.Codigo = Convert.ToInt32(txtCode.Text);
            myAlumno.Nombre = txtName.Text;
            myAlumno.Correo = txtEmail.Text;
            myAlumno.Nota1 = Convert.ToDouble(txtGrade1.Text);
            myAlumno.Nota2 = Convert.ToDouble(txtGrade2.Text);
            myAlumno.Nota3 = Convert.ToDouble(txtGrade3.Text);
            myAlumno.Nota4 = Convert.ToDouble(txtGrade4.Text);
            myAlumno.NotaFinal = (myAlumno.Nota1 + myAlumno.Nota2 + myAlumno.Nota3 + myAlumno.Nota4) / 4;
            if (myAlumno.NotaFinal >= 3.5)
            {
                myAlumno.NotaConcepto = "Aprobado";
            }
            else
            {
                myAlumno.NotaConcepto = "Reprobado";
            }
            //--------- Creación del Objeto -----//
            //---Adicionar a la lista ---//
            listaAlumnos.Add(myAlumno);            
            //--Agregar Datos al Data Grid View--//
            dgList.DataSource = null;
            dgList.DataSource = listaAlumnos;
            verArreglo();
        }

        private Boolean ExisteCodigo(int codigo)
        {

            foreach (Alumno myAlumno in listaAlumnos)
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
            foreach (Alumno myAlumno in listaAlumnos)
            {
                if (myAlumno.Codigo == codigo)
                {
                    return myAlumno;
                }
            }
            return null;
        }
        
        //--- Metodo funcion para ver el array en consola ---/
        private void verArreglo()
        {
            foreach (Alumno itemAlumno in listaAlumnos)
            {
                Console.WriteLine("//----------------------------------------//");
                Console.WriteLine(itemAlumno.Codigo);
                Console.WriteLine(itemAlumno.Nombre);
                Console.WriteLine(itemAlumno.Correo);
                Console.WriteLine(itemAlumno.Nota1);
                Console.WriteLine(itemAlumno.Nota2);
                Console.WriteLine(itemAlumno.Nota3);
                Console.WriteLine(itemAlumno.Nota4);
                Console.WriteLine(itemAlumno.NotaFinal);
                Console.WriteLine(itemAlumno.NotaConcepto);
                Console.WriteLine("//----------------------------------------//");
            }
        }
        private void limpiar()
        {
            txtCode.Clear();
            txtEmail.Clear();
            txtName.Clear();
            txtGrade1.Clear();
            txtGrade2.Clear();
            txtGrade3.Clear();
            txtGrade4.Clear();
        }

        private void LimpiarCajar2()
        {

            txtEmail.Clear();
            txtName.Clear();
            txtGrade1.Clear();
            txtGrade2.Clear();
            txtGrade3.Clear();
            txtGrade4.Clear();

            txtCode.Focus();
        }
        private void guardarCambios()
        {
            Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtCode.Text));
            myAlumno.Nombre = txtName.Text;
            myAlumno.Correo = txtEmail.Text;
            myAlumno.Nota1 = Convert.ToDouble(txtGrade1.Text);
            myAlumno.Nota2 = Convert.ToDouble(txtGrade2.Text);
            myAlumno.Nota3 = Convert.ToDouble(txtGrade3.Text);
            myAlumno.Nota4 = Convert.ToDouble(txtGrade4.Text);
            myAlumno.NotaFinal = (myAlumno.Nota1 + myAlumno.Nota2 + myAlumno.Nota3 + myAlumno.Nota4) / 4;
            if (myAlumno.NotaFinal >= 3.5)
            {
                myAlumno.NotaConcepto = "Aprobado";
            }
            else
            {
                myAlumno.NotaConcepto = "Reprobado";
            }
            dgList.DataSource = null;
            dgList.DataSource = listaAlumnos;

            tsEdit.Enabled = false; // Desactiva los botones
            tsDelete.Enabled = false;
            txtCode.Enabled = true;
            limpiar();

        }

        private void tsDelete_Click(object sender, EventArgs e)
        {
            //----Borrar datos del estudiante----//
            DialogResult confirmarBorrar = MessageBox.Show("¿Quiere borrar los datos del alumno?", "Estudiantes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirmarBorrar == DialogResult.Yes)
            {

                Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtCode.Text));
                alumnosBorrados.Add(myAlumno);
                listaAlumnos.Remove(myAlumno);

                dgList.DataSource = null;
                dgList.DataSource = listaAlumnos;

                tsEdit.Enabled = false; //Desactiva los botones
                tsDelete.Enabled = false;
                txtCode.Enabled = true;
                limpiar();

            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
        //------------------------------------------ Fin de Metodos
    }
}
