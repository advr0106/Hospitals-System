using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Forms
{
    public partial class Medicos : Base
    {
        int id, execuatur;
        bool click;
        Clases.Medicos medicos = new Clases.Medicos();
        public Medicos()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            int.TryParse(txtExecuatur.Text, out execuatur);
            Clases.Medicos medico = new Clases.Medicos(id, txtNombre.Text, execuatur, txtEspecialidad.Text);
            if (medico.valid)
            {
                medico.Insertar();
                click = false;
                habilitadores();
            }
        }

        private void txtExecuatur_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dgvMedico_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dataGridViewRow = dgvMedico.Rows[e.RowIndex];
                txtId.Text = dataGridViewRow.Cells[0].Value.ToString();
                txtNombre.Text = dataGridViewRow.Cells[1].Value.ToString();
                txtExecuatur.Text = dataGridViewRow.Cells[2].Value.ToString();
                txtEspecialidad.Text = dataGridViewRow.Cells[3].Value.ToString();
                click = true;
                habilitadores();
            }
        }

        private void Medicos_Load(object sender, EventArgs e)
        {

            dgvMedico.DataSource = medicos.Fill("general");
            txtId.Text = medicos.NextID();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            comboBox1.SelectedIndex = -1;
            txtFiltro.Enabled = false;
            dgvMedico.DataSource = medicos.Fill("general");

            medicos.Clean(groupBox1);
            txtId.Text = medicos.NextID();

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnRegistro.Enabled = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            Clases.Medicos medico = new Clases.Medicos(id);
            if (medico.valid)
            {
                medico.Eliminar();
                click = false;
                habilitadores();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            int.TryParse(txtExecuatur.Text, out execuatur);
            Clases.Medicos medico = new Clases.Medicos(id, txtNombre.Text, execuatur, txtEspecialidad.Text);
            if (medico.valid)
            {
                medico.Editar();
                click = false;
                habilitadores();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            dgvMedico.DataSource = medicos.Fill("general");
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1)
            {
                txtFiltro.Enabled = true;
            }
        }

        private void txtFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                medicos.fill = txtFiltro.Text;
                dgvMedico.DataSource = medicos.Fill("filtroNombre");
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                medicos.fill = txtFiltro.Text;
                dgvMedico.DataSource = medicos.Fill("filtroEspecialidad");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReporteMedico reporte = new ReporteMedico();
            ReportDocument oRep = new ReportDocument();
            ParameterFieldDefinitions pfds;
            ParameterFieldDefinition pfd;
            ParameterValues pvs = new ParameterValues();
            ParameterDiscreteValue pdv = new ParameterDiscreteValue();
            oRep.Load(@"..\..\Reportes\ReporteMedico.rpt");
            oRep.VerifyDatabase();
            oRep.Refresh();
            if (comboBox1.SelectedIndex == -1)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@nombre"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@especialidad"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

            }
            else if (comboBox1.SelectedIndex == 0)
            {
                pdv.Value = txtFiltro.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@nombre"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@especialidad"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@nombre"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = txtFiltro.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@especialidad"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);
            }
            
            reporte.crystalReportViewer1.ReportSource = oRep;
            reporte.crystalReportViewer1.Refresh();
            reporte.ShowDialog();
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
                    dgvMedico.DataSource = medicos.Fill("general");
                    medicos.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    txtFiltro.Enabled = false;
                    txtId.Text = medicos.NextID();
                }
            }
            else if (btnEditar.Enabled)
            {
                if (!click)
                {
                    dgvMedico.DataSource = medicos.Fill("general");
                    medicos.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    txtFiltro.Enabled = false;
                    txtId.Text = medicos.NextID();
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnRegistro.Enabled = true;
                }
            }
        }
    }
}
