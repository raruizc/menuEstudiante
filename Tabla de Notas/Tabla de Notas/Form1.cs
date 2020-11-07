using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization; // crear
using System.IO; // Escribir
using System.Drawing.Text;
using System.Collections.ObjectModel;

namespace Tabla_de_Notas
{
    public partial class Form1 : Form
    {
        //Declarar objetos
        //ArrayList listaAlumnos = new ArrayList();

        public List<Alumno> listaAlumnos = new List<Alumno>(); // Lo hacemos público para acceder en el formulario de la papelera
        ValidarCajas validacion = new ValidarCajas();
        public List<Alumno> alumnosBorrados = new List<Alumno>();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("C:/net/listaAlumno.xml")&& File.Exists("C:/net/alumnosBorrados.xml"))
            {
                
                listaAlumnos.Clear();
                alumnosBorrados.Clear();
                XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
                FileStream leerXml = File.OpenRead("C:/net/listaAlumno.xml");
                FileStream leerXmlAlumnosBorrados = File.OpenRead("C:/net/alumnosBorrados.xml");// Abrimos un Xml de los alumnos que se van a eliminar
                listaAlumnos = (List<Alumno>)codificador.Deserialize(leerXml);
                alumnosBorrados = (List<Alumno>)codificador.Deserialize(leerXmlAlumnosBorrados);// utilizamos el codificador para leer el archivo de alumnos borrados y guardarlo en la lista de los borrados
                leerXml.Close();
                leerXmlAlumnosBorrados.Close();
            }

            dgtablaDatos.DataSource = null;
            dgtablaDatos.DataSource = listaAlumnos;
        }

        

        private void tsExit_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("¿Desea salir de la aplicación?", "Estudiantes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            
            if (r == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private Boolean validar()
        {
            if (!validacion.Vacio(txtcodigoIn, errorM, "El código no puede ser vacío"))
                if (validacion.TipoNumero(txtcodigoIn, errorM, "Debe digitar numeros"))
                    if (!validacion.Vacio(txtnombreIn, errorM, "El nombre no puede ser vacío"))
                        if (validacion.TipoTexto(txtnombreIn, errorM, "Debe digitar letras"))
                            if (!validacion.Vacio(txtcorreoIn, errorM, "El correo no puede ser vacío"))
                                if (validacion.TipoCorreo(txtcorreoIn, errorM, "El correo no cumple con el formato"))
                                    if (!validacion.Vacio(txtNota1, errorM, "La Nota 1 no puede estar vacía"))
                                        if (validacion.TipoNumero(txtNota1, errorM, "Debe digitar numeros"))
                                            if (!validacion.Vacio(txtNota2, errorM, "La Nota 2 no puede estar vacía"))
                                                if (validacion.TipoNumero(txtNota2, errorM, "Debe digitar numeros"))
                                                    if (!validacion.Vacio(txtNota3, errorM, "La Nota 3 no puede estar vacía"))
                                                        if (validacion.TipoNumero(txtNota3, errorM, "Debe digitar numeros"))
                                                            if (!validacion.Vacio(txtNota4, errorM, "La Nota 4 no puede estar vacía"))
                                                                if (validacion.TipoNumero(txtNota4, errorM, "Debe digitar numeros"))
                                                                    return true;
            return false;
        }

        private void tsAddUser_Click(object sender, EventArgs e)
        {
            //---------------- Agregar Alumno

            if(validar() == true)
            {
                if (!ExisteCodigo(Convert.ToInt32(txtcodigoIn.Text)))
                {
                    insertardatos();
                    LimpiarCajas();
                    errorM.Clear();

                }
                else
                {
                    errorM.SetError(txtcodigoIn, "El código ya existe");
                    txtcodigoIn.Focus();
                    return;
                }
            }

            /*if (!validacion.Vacio(txtcodigoIn, errorM, "El código no puede ser vacío"))
                if (validacion.TipoNumero(txtcodigoIn, errorM, "Debe digitar numeros"))
                    if (!validacion.Vacio(txtnombreIn, errorM, "El nombre no puede ser vacío"))
                        if (validacion.TipoTexto(txtnombreIn, errorM, "Debe digitar letras"))
                            if (!validacion.Vacio(txtcorreoIn, errorM, "El correo no puede ser vacío"))
                                if (validacion.TipoCorreo(txtcorreoIn, errorM, "El correo no cumple con el formato"))
                                    if (!validacion.Vacio(txtNota1, errorM, "La Nota 1 no puede estar vacía"))
                                        if (validacion.TipoNumero(txtNota1, errorM, "Debe digitar numeros"))
                                            if (!validacion.Vacio(txtNota2, errorM, "La Nota 2 no puede estar vacía"))
                                                if (validacion.TipoNumero(txtNota2, errorM, "Debe digitar numeros"))
                                                    if (!validacion.Vacio(txtNota3, errorM, "La Nota 3 no puede estar vacía"))
                                                        if (validacion.TipoNumero(txtNota3, errorM, "Debe digitar numeros"))
                                                            if (!validacion.Vacio(txtNota4, errorM, "La Nota 4 no puede estar vacía"))
                                                                if (validacion.TipoNumero(txtNota4, errorM, "Debe digitar numeros"))
                                                                {
                                                                    if(!ExisteCodigo(Convert.ToInt32(txtcodigoIn.Text)))
                                                                    {
                                                                        insertardatos();
                                                                        LimpiarCajas();
                                                                        errorM.Clear();

                                                                    }
                                                                    else
                                                                    {
                                                                        errorM.SetError(txtcodigoIn, "El código ya existe");
                                                                        txtcodigoIn.Focus();
                                                                        return;
                                                                    }
                                                                }*/


                                                                    
        }
        //---------- metodo para ver el array en consola
        private void verArreglo()
        {
            foreach (Alumno itemalumno in listaAlumnos)
            {
                Console.WriteLine("---------------");
                Console.WriteLine(itemalumno.Codigo);
                Console.WriteLine(itemalumno.Nombre);
                Console.WriteLine(itemalumno.Correo);
                Console.WriteLine(itemalumno.Nota1);
                Console.WriteLine(itemalumno.Nota2);
                Console.WriteLine(itemalumno.Nota3);
                Console.WriteLine(itemalumno.Nota4);
                Console.WriteLine(itemalumno.NotaFinal);
                Console.WriteLine(itemalumno.NotaConcepto);
                Console.WriteLine("---------------");
            }

        }

        private void tsSaveFile_Click(object sender, EventArgs e)
        {
            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>)); // Codificador que recibe un tipo Alumno
            TextWriter escribirXml = new StreamWriter("C:/net/listaAlumno.xml");// Crear un Archivo XML 
            TextWriter escribirXmlAlumnosBorrados = new StreamWriter("C:/net/alumnosBorrados.xml"); // creamos el Xml de los alumnos borrados
            codificador.Serialize(escribirXml, listaAlumnos);// Guardar el archivo
            codificador.Serialize(escribirXmlAlumnosBorrados, alumnosBorrados);
            escribirXml.Close(); //Cerrar el archivo
            escribirXmlAlumnosBorrados.Close();
        }

        private void tsOpenFile_Click(object sender, EventArgs e)
        {
            //cargar los datos del xml
            //generar una lista con esos datos
            // mostrar esa lista en el DG

            listaAlumnos.Clear();
            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
            FileStream leerXml = File.OpenRead("C:/net/listaAlumno.xml");
            listaAlumnos = (List<Alumno>)codificador.Deserialize(leerXml);
            leerXml.Close();

            dgtablaDatos.DataSource = null;
            dgtablaDatos.DataSource = listaAlumnos;

        }
        private void insertardatos()
        {
            Alumno nuevoAlumno = new Alumno();

            // Desde los elementos del formulario creo el alumno nuevo
            nuevoAlumno.Codigo = Convert.ToInt32(txtcodigoIn.Text);
            nuevoAlumno.Nombre = txtnombreIn.Text;
            nuevoAlumno.Correo = txtcorreoIn.Text;
            nuevoAlumno.Nota1 = Convert.ToDouble(txtNota1.Text);
            nuevoAlumno.Nota2 = Convert.ToDouble(txtNota2.Text);
            nuevoAlumno.Nota3 = Convert.ToDouble(txtNota3.Text);
            nuevoAlumno.Nota4 = Convert.ToDouble(txtNota4.Text);
            nuevoAlumno.NotaFinal = (nuevoAlumno.Nota1 + nuevoAlumno.Nota2 + nuevoAlumno.Nota3 + nuevoAlumno.Nota4) / 4;
            if (nuevoAlumno.NotaFinal >= 3.5)
                nuevoAlumno.NotaConcepto = "Aprobado";
            else
                nuevoAlumno.NotaConcepto = "Reprobado";
            //--------------------------------------
            
            listaAlumnos.Add(nuevoAlumno);
            dgtablaDatos.DataSource = null;
            dgtablaDatos.DataSource = listaAlumnos;
        }

        private void LimpiarCajas()
        {
            txtcodigoIn.Clear();
            txtnombreIn.Clear();
            txtcorreoIn.Clear();
            txtNota1.Clear();
            txtNota2.Clear();
            txtNota3.Clear();
            txtNota4.Clear();
            txtcodigoIn.Focus();
        }

        private void LimpiarCajas2()
        {
            
            txtnombreIn.Clear();
            txtcorreoIn.Clear();
            txtNota1.Clear();
            txtNota2.Clear();
            txtNota3.Clear();
            txtNota4.Clear();
            txtcodigoIn.Focus();
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlSerializer codificador = new XmlSerializer(typeof(List<Alumno>));
            TextWriter escribirXml = new StreamWriter("C:/net/listaAlumno.xml");
            codificador.Serialize(escribirXml, listaAlumnos);
            escribirXml.Close();
        }

        //vamos a usar metodos para la validación y manipulacion de datos
        // validar que el nuevo codigo no exista en la lista

        private Boolean ExisteCodigo(int codigo)
        {
            foreach (Alumno myAlumno in listaAlumnos)
            {
                if (myAlumno.Codigo == codigo)
                    return true;
            }
            return false;
        }
        //*******************************************************
        private void tsSearchUser_Click(object sender, EventArgs e)
        {

            // vamos a buscar a un alumno
            // validar que no esté vacio

            if (!validacion.Vacio(txtcodigoIn, errorM, "Para buscar, debe ingresar un código"))
                if (validacion.TipoNumero(txtcodigoIn, errorM, "Debe digitar numeros"))
                {
                    if (ExisteCodigo(Convert.ToInt32(txtcodigoIn.Text)))
                    {
                        Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtcodigoIn.Text));
                        txtnombreIn.Text = myAlumno.Nombre;
                        txtcorreoIn.Text = myAlumno.Correo;
                        txtNota1.Text = myAlumno.Nota1.ToString();
                        txtNota2.Text = myAlumno.Nota2.ToString();
                        txtNota3.Text = myAlumno.Nota3.ToString();
                        txtNota4.Text = myAlumno.Nota4.ToString();
                        tsEditUser.Enabled = true;
                        tsDeleteUser.Enabled = true;
                        txtcodigoIn.Enabled = false;

                    }
                    else
                    {
                        errorM.SetError(txtcodigoIn, "El código no existe");
                        txtcodigoIn.Focus();
                        LimpiarCajas2();
                        return;                        
                    }
                
            }

        }
        private Alumno obtenerDatos (int codigo)
        {
            foreach (Alumno myAlumno in listaAlumnos)
            {
                if (myAlumno.Codigo == codigo)
                    return myAlumno;
            }
            return null;
        }       

        
        private void guardarCambios()
        {
            // Metodo crea un objeto alumno con el " con el codigo que esta en  caja texto "
            // como el objeto es de la lista actualiza los valores 
            Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtcodigoIn.Text));
            myAlumno.Nombre = txtnombreIn.Text;
            myAlumno.Correo = txtcorreoIn.Text;
            myAlumno.Nota1 = Convert.ToDouble(txtNota1.Text);
            myAlumno.Nota2 = Convert.ToDouble(txtNota2.Text);
            myAlumno.Nota3 = Convert.ToDouble(txtNota3.Text);
            myAlumno.Nota4 = Convert.ToDouble(txtNota4.Text);
            //----------------------------------- calculamos el resto 
            myAlumno.NotaFinal = (myAlumno.Nota1 + myAlumno.Nota2 + myAlumno.Nota3 + myAlumno.Nota4) / 4;
            if (myAlumno.NotaFinal >= 3.5)
            {
                myAlumno.NotaConcepto = "Aprobado";
            }
            else
            {
                myAlumno.NotaConcepto = "Reprobado";
            }

            dgtablaDatos.DataSource = null;
            dgtablaDatos.DataSource = listaAlumnos;

            // activar los botones

            tsEditUser.Enabled = false;
            tsDeleteUser.Enabled = false;
            txtcodigoIn.Enabled = true;
            LimpiarCajas();
        }

        private void tsEditUser_Click_1(object sender, EventArgs e)
        {
            // Editar Datos 
            // hace las validaciones 

            if(validar() == true)
            {
                guardarCambios();
            }

            /*if (!validacion.Vacio(txtnombreIn, errorM, "El nombre no puede ser vacio"))
                if (validacion.TipoTexto(txtnombreIn, errorM, "Debe digitar texto"))
                    if (!validacion.Vacio(txtcorreoIn, errorM, "El correo  no puede ser vacio"))
                        if (validacion.TipoCorreo(txtcorreoIn, errorM, "El correo no  cumple con el formato"))
                            if (!validacion.Vacio(txtNota1, errorM, "La nota no puede ser vacio"))
                                if (validacion.TipoNumero(txtNota1, errorM, "Debe ser Numerico"))
                                    if (!validacion.Vacio(txtNota2, errorM, "La nota no puede ser vacio"))
                                        if (validacion.TipoNumero(txtNota2, errorM, "Debe ser Numerico"))
                                            if (!validacion.Vacio(txtNota3, errorM, "La nota no puede ser vacio"))
                                                if (validacion.TipoNumero(txtNota3, errorM, "Debe ser Numerico"))
                                                    if (!validacion.Vacio(txtNota4, errorM, "La nota no puede ser vacio"))
                                                        if (validacion.TipoNumero(txtNota4, errorM, "Debe ser Numerico"))
                                                            guardarCambios();*/
        }

        private void tsDeleteUser_Click(object sender, EventArgs e)
        {
            // Borrar Datos del  Estudiante 

            DialogResult confirmarBorrar = MessageBox.Show("Esta seguro de borrar", "Comfirmar Borrada", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (confirmarBorrar == DialogResult.OK)
            {
                Alumno myAlumno = obtenerDatos(Convert.ToInt32(txtcodigoIn.Text));
                listaAlumnos.Remove(myAlumno);
                alumnosBorrados.Add(myAlumno);

                dgtablaDatos.DataSource = null;
                dgtablaDatos.DataSource = listaAlumnos;

                // activar los botones

                tsEditUser.Enabled = false;
                tsDeleteUser.Enabled = false;
                txtcodigoIn.Enabled = true;
                LimpiarCajas();
            }
        }

        private void papeleraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Papelera papelera = new Papelera();
            papelera.Show();
            this.Hide();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }
        //---------------------------------------------------------


    }


}

