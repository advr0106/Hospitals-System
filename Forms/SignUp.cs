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
    public partial class SignUp : Base
    {
        public SignUp()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (txtUser.Text == "" || txtPass.Text == "" || textBox1.Text == "")
            {
                MessageBox.Show("Debes de llenar todos lo campos!");
            }
            else if (txtPass.Text != textBox1.Text)
            {
                MessageBox.Show("Las contraseñas no están iguales");
            }
            else
            {
                Clases.Login login = new Clases.Login(txtUser.Text,txtPass.Text);
                login.Insertar();
                txtPass.Clear();
                txtUser.Clear();
                textBox1.Clear();
            }
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text == "Usuario")
            {
                txtUser.Text = "";
                txtUser.ForeColor = Color.Black;
            }
        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                txtUser.Text = "Usuario";
                txtUser.ForeColor = Color.DimGray;
            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "Contraseña")
            {
                txtPass.Text = "";
                txtPass.ForeColor = Color.Black;
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (txtPass.Text == "")
            {
                txtPass.Text = "Contraseña";
                txtPass.ForeColor = Color.DimGray;
                txtPass.UseSystemPasswordChar = false;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Contraseña repetida")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
                textBox1.UseSystemPasswordChar = true;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Contraseña";
                textBox1.ForeColor = Color.DimGray;
                textBox1.UseSystemPasswordChar = false;
            }
        }

    }
}
