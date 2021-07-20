using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Hospital.Clases;
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
    public partial class Habitaciones : Base
    {
        int id, numero, idTipo, precio;
        bool click;
        Clases.Habitaciones habitacion = new HabitacionesDobles();
        public Habitaciones()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void Habitaciones_Load(object sender, EventArgs e)
        {
            dgvHabitacion.DataSource = habitacion.Fill("general");
            txtId.Text = habitacion.NextID();
            cbTipo.DataSource = habitacion.Fill("tipo");
            cbTipo.DisplayMember = "Nombre";
            cbTipo.ValueMember = "ID";
        }
        private void onlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            dgvHabitacion.DataSource = habitacion.Fill("general");

            habitacion.Clean(groupBox1);
            txtId.Text = habitacion.NextID();

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnRegistro.Enabled = true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            int.TryParse(txtNumero.Text, out numero);
            int.TryParse(txtPrecio.Text, out precio);
            idTipo = Convert.ToInt32(cbTipo.SelectedValue);
            switch (idTipo)
            {
                case 1:
                    habitacion = new Clases.HabitacionesDobles(id, numero, idTipo, precio);
                    break;
                case 2:
                    habitacion = new Clases.HabitacionesPrivadas(id, numero, idTipo, precio);
                    break;
                case 3:
                    habitacion = new Clases.HabitacionesSuites(id, numero, idTipo, precio);
                    break;
            }
            if (habitacion.valid)
            {
                habitacion.Editar();
                click = false;
                habilitadores();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            switch (Convert.ToInt32(cbTipo.SelectedValue))
            {
                case 1:
                    habitacion = new Clases.HabitacionesDobles(id);
                    break;
                case 2:
                    habitacion = new Clases.HabitacionesPrivadas(id);
                    break;
                case 3:
                    habitacion = new Clases.HabitacionesSuites(id);
                    break;
            }

            if (habitacion.valid)
            {
                habitacion.Eliminar();
                click = false;
                habilitadores();
            }
        }

        private void txtFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            habitacion.fill = txtFiltro.Text;
            dgvHabitacion.DataSource = habitacion.Fill("filtroTipo");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReporteHabitacion reporte = new ReporteHabitacion();
            ReportDocument oRep = new ReportDocument();
            ParameterFieldDefinitions pfds;
            ParameterFieldDefinition pfd;
            ParameterValues pvs = new ParameterValues();
            ParameterDiscreteValue pdv = new ParameterDiscreteValue();
            oRep.Load(@"..\..\Reportes\ReporteHabitacion.rpt");
            oRep.VerifyDatabase();
            oRep.Refresh();
            pdv.Value = txtFiltro.Text;
            pfds = oRep.DataDefinition.ParameterFields;
            pfd = pfds["@tipo"];
            pvs.Add(pdv);
            pfd.ApplyCurrentValues(pvs);
            reporte.crystalReportViewer1.ReportSource = oRep;

            reporte.crystalReportViewer1.Refresh();

            reporte.ShowDialog();

        }

        private void dgvHabitacion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dataGridViewRow = dgvHabitacion.Rows[e.RowIndex];
                txtId.Text = dataGridViewRow.Cells[0].Value.ToString();
                txtNumero.Text = dataGridViewRow.Cells[1].Value.ToString();
                txtPrecio.Text = dataGridViewRow.Cells[2].Value.ToString();
                cbTipo.Text = dataGridViewRow.Cells[3].Value.ToString();
                click = true;
                habilitadores();
            }
        }
        private void btnRegistro_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            int.TryParse(txtNumero.Text, out numero);
            int.TryParse(txtPrecio.Text, out precio);
            idTipo = Convert.ToInt32(cbTipo.SelectedValue);
            switch (idTipo)
            {
                case 1:
                    habitacion = new Clases.HabitacionesDobles(id, numero, idTipo, precio);
                    break;
                case 2:
                    habitacion = new Clases.HabitacionesPrivadas(id, numero, idTipo, precio);
                    break;
                case 3:
                    habitacion = new Clases.HabitacionesSuites(id, numero, idTipo, precio);
                    break;
            }
            if (habitacion.valid)
            {
                habitacion.Insertar();
                click = false;
                habilitadores();
            }
        }
        public void habilitadores()
        {
            if (btnRegistro.Enabled)
            {
                if (click)
                {
                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                    btnRegistro.Enabled = false;
                    click = false;
                }
                else
                {
                    dgvHabitacion.DataSource = habitacion.Fill("general");
                    habitacion.Clean(groupBox1);
                    txtFiltro.Text = "";
                    txtId.Text = habitacion.NextID();
                }

            }
            else if (btnEditar.Enabled)
            {
                if (!click)
                {
                    dgvHabitacion.DataSource = habitacion.Fill("general");
                    habitacion.Clean(groupBox1);
                    txtFiltro.Text = "";
                    txtId.Text = habitacion.NextID();
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnRegistro.Enabled = true;
                }
            }
        }
    }
}
