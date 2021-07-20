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
    public partial class Procesos : Base
    {
        public Procesos()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Citas citas = new Citas();
            this.Hide();
            citas.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ingresos ingresos = new Ingresos();
            this.Hide();
            ingresos.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Altas altas = new Altas();
            this.Hide();
            altas.ShowDialog();
            this.Show();
        }

        private void Procesos_Load(object sender, EventArgs e)
        {

        }
    }
}
