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
    public partial class Pacientes : Base
    {
        int id, cedula;
        bool click;
        Clases.Pacientes pacientes = new Clases.Pacientes();

        public Pacientes()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            Clases.Pacientes paciente = new Clases.Pacientes(id, txtCedula.Text, txtNombre.Text, radioButton1.Checked ? true : false);
            if (paciente.valid)
            {
                paciente.Insertar();
                click = false;
                habilitadores();
            }
        }
        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e)
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


        private void dgvPaciente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow dataGridViewRow = dgvPaciente.Rows[e.RowIndex];
                txtId.Text = dataGridViewRow.Cells[0].Value.ToString();
                txtCedula.Text = dataGridViewRow.Cells[1].Value.ToString();
                txtNombre.Text = dataGridViewRow.Cells[2].Value.ToString();
                bool seguro = dataGridViewRow.Cells[3].Value.ToString() == "Asegurado" ? true : false;
                if (seguro)
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
                click = true;
                habilitadores();
            }
        }

        private void Pacientes_Load(object sender, EventArgs e)
        {
            dgvPaciente.DataSource = pacientes.Fill("general");
            txtId.Text = pacientes.NextID();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            pacientes.Clean(groupBox1);
            txtId.Text = pacientes.NextID();
            
            txtFiltro.Text = "";
            comboBox1.SelectedIndex = -1;
            radioSi.Checked = false;
            radioNo.Checked = false;
            groupSeguro.Enabled = false;
            txtFiltro.Enabled = false;

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnRegistro.Enabled = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            Clases.Pacientes paciente = new Clases.Pacientes(id);
            if (paciente.valid)
            {
                paciente.Eliminar();
                click = false;
                habilitadores();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            radioSi.Checked = false;
            radioNo.Checked = false;
            dgvPaciente.DataSource=pacientes.Fill("general");
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1)
            {
                txtFiltro.Enabled = true;
                groupSeguro.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                txtFiltro.Enabled = false;
                groupSeguro.Enabled = true;
            }
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
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
        }
        private void txtFiltro_KeyUp(object sender, KeyEventArgs e)
        {

            if (comboBox1.SelectedIndex == 0)
            {
                pacientes.fill = txtFiltro.Text;
                dgvPaciente.DataSource = pacientes.Fill("filtroNombre");
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                pacientes.fill = txtFiltro.Text;
                dgvPaciente.DataSource = pacientes.Fill("filtroCedula");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ReportePaciente reporte = new ReportePaciente();
            ReportDocument oRep = new ReportDocument();
            ParameterFieldDefinitions pfds;
            ParameterFieldDefinition pfd;
            ParameterValues pvs = new ParameterValues();
            ParameterDiscreteValue pdv = new ParameterDiscreteValue();
            oRep.Load(@"..\..\Reportes\ReportePaciente.rpt");
            oRep.VerifyDatabase();
            oRep.Refresh();
            if (comboBox1.SelectedIndex == -1)
            {
                pacientes.fill = "";   
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@nombre"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@cedula"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@seguro"];
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
                pfd = pfds["@cedula"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@seguro"];
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
                pfd = pfds["@cedula"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@seguro"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@nombre"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@cedula"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = radioSi.Checked ? 1 : 0;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@seguro"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);
            }
            reporte.crystalReportViewer1.ReportSource = oRep;
            reporte.crystalReportViewer1.Refresh();
            reporte.ShowDialog();
        }

        private void radioSi_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSi.Checked)
            {
                pacientes.fill = "1";
                dgvPaciente.DataSource = pacientes.Fill("filtroSeguro");
            }
        }

        private void radioNo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioNo.Checked)
            {
                pacientes.fill ="0";
                dgvPaciente.DataSource = pacientes.Fill("filtroSeguro");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            Clases.Pacientes paciente = new Clases.Pacientes(id, txtCedula.Text, txtNombre.Text, radioButton1.Checked ? true : false);
            if (paciente.valid)
            {
                paciente.Editar();
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
                    dgvPaciente.DataSource = pacientes.Fill("general");
                    pacientes.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    radioSi.Checked = false;
                    radioNo.Checked = false;
                    groupSeguro.Enabled = false;
                    txtFiltro.Enabled = false;
                    txtId.Text = pacientes.NextID();
                }

            }
            else if (btnEditar.Enabled)
            {
                if (!click)
                {
                    dgvPaciente.DataSource = pacientes.Fill("general");
                    pacientes.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    radioSi.Checked = false;
                    radioNo.Checked = false;
                    groupSeguro.Enabled = false;
                    txtFiltro.Enabled = false;
                    txtId.Text = pacientes.NextID();
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnRegistro.Enabled = true;
                }
            }
        }

    }
}
