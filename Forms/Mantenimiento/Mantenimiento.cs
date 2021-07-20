using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Forms
{
    public partial class Mantenimiento : Base
    {
       
        public Mantenimiento()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Medicos medicos = new Medicos();
            this.Hide();
            medicos.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pacientes pacientes = new Pacientes();
            this.Hide();
            pacientes.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Habitaciones habitaciones = new Habitaciones();
            this.Hide();
            habitaciones.ShowDialog();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Mantenimiento_Load(object sender, EventArgs e)
        {

        }
    }
}
