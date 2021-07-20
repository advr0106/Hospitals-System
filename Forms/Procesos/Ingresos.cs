using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class Ingresos : Base
    {
        int id, idPaciente, idHabitacion;
        bool click;
        Clases.Ingresos ingresos = new Clases.Ingresos();
        public Ingresos()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void Ingresos_Load(object sender, EventArgs e)
        {
            dgvIngresos.DataSource = ingresos.Fill("general");

            txtId.Text = ingresos.NextID();

            cbHabitacion.DataSource = ingresos.Fill("habitacion");
            cbHabitacion.DisplayMember = "Habitación";
            cbHabitacion.ValueMember = "ID";

            cbPaciente.DataSource = ingresos.Fill("paciente");
            cbPaciente.DisplayMember = "Nombre";
            cbPaciente.ValueMember = "ID";
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            ingresos.Clean(groupBox1);
            txtId.Text = ingresos.NextID();

            txtFiltro.Text = "";
            comboBox1.SelectedIndex = -1;
            groupSeguro.Enabled = false;
            txtFiltro.Enabled = false;

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnRegistro.Enabled = true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);

            idHabitacion = Convert.ToInt32(cbHabitacion.SelectedValue);
            idPaciente = Convert.ToInt32(cbPaciente.SelectedValue);

            Clases.Ingresos ingreso = new Clases.Ingresos(id, date.Value.Date, idHabitacion, idPaciente);
            if (ingreso.valid)
            {
                ingreso.Editar();
                click = false;
                habilitadores();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            Clases.Ingresos ingreso = new Clases.Ingresos(id);
            if (ingreso.valid)
            {
                ingreso.Eliminar();
                click = false;
                habilitadores();
            }
        }

        private void dgvIngresos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow dataGridViewRow = dgvIngresos.Rows[e.RowIndex];
                    txtId.Text = dataGridViewRow.Cells[0].Value.ToString();
                    date.Value = Convert.ToDateTime(dataGridViewRow.Cells[1].Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    cbHabitacion.Text = dataGridViewRow.Cells[2].Value.ToString();
                    cbPaciente.Text = dataGridViewRow.Cells[3].Value.ToString();
                    click = true;
                    habilitadores();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            dgvIngresos.DataSource = ingresos.Fill("general");
            if (comboBox1.SelectedIndex == 1 )
            {
                txtFiltro.Enabled = true;
                groupSeguro.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                txtFiltro.Enabled = false;
                groupSeguro.Enabled = true;
            }
        }

        private void txtFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                ingresos.fill = txtFiltro.Text;
                dgvIngresos.DataSource = ingresos.Fill("filtroHabitacion");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                ingresos.fill = dateTimePicker1.Text;
                dgvIngresos.DataSource = ingresos.Fill("filtroFecha");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReporteCita reporte = new ReporteCita();
            ReportDocument oRep = new ReportDocument();
            ParameterFieldDefinitions pfds;
            ParameterFieldDefinition pfd;
            ParameterValues pvs = new ParameterValues();
            ParameterDiscreteValue pdv = new ParameterDiscreteValue();
            oRep.Load(@"..\..\Reportes\ReporteIngreso.rpt");
            oRep.VerifyDatabase();
            oRep.Refresh();
            if (comboBox1.SelectedIndex == -1)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fecha"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@habitacion"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

            }
            else if (comboBox1.SelectedIndex == 0)
            {
                pdv.Value = dateTimePicker1.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fecha"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@habitacion"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

            }
            else if (comboBox1.SelectedIndex == 1)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fecha"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = txtFiltro.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@habitacion"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);
            }
            reporte.crystalReportViewer1.ReportSource = oRep;
            reporte.crystalReportViewer1.Refresh();
            reporte.ShowDialog();
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            idHabitacion = Convert.ToInt32(cbHabitacion.SelectedValue);
            idPaciente = Convert.ToInt32(cbPaciente.SelectedValue);

            Clases.Ingresos ingreso = new Clases.Ingresos(id, date.Value.Date, idHabitacion, idPaciente);
            if (ingreso.valid)
            {
                ingreso.Insertar();
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
                    dgvIngresos.DataSource = ingresos.Fill("general");
                    ingresos.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    groupSeguro.Enabled = false;
                    txtFiltro.Enabled = false;
                    txtId.Text = ingresos.NextID();
                }

            }
            else if (btnEditar.Enabled)
            {
                if (!click)
                {
                    dgvIngresos.DataSource = ingresos.Fill("general");
                    ingresos.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    groupSeguro.Enabled = false;
                    txtFiltro.Enabled = false;
                    txtId.Text = ingresos.NextID();
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnRegistro.Enabled = true;
                }
            }
        }
    }
}
