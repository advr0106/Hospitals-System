using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital
{
    public partial class Splash : Base
    {
        FMenuPrincipal menuPrincipal = new FMenuPrincipal();
        private int ticks;
        public Splash()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            timer1.Start();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(235, Color.White);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ticks++;
            if (ticks == 70)
            {
                timer1.Stop();
                this.Hide();
                menuPrincipal.ShowDialog();
                this.Close();
            }
        }
    }
}
