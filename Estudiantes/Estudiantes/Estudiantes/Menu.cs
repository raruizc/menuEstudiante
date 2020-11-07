using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estudiantes
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void estudiantesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 formulario1 = new Form1();
            formulario1.Show();
            this.Hide();
        }

        private void papeleríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Papeleria papeleria = new Papeleria();
            papeleria.Show();
            this.Hide();
        }
    }
}
