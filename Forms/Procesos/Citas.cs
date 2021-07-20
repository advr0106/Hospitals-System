using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Hospital.Reportes;
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
    public partial class Citas : Base
    {
        int id, idPaciente, idMedico;
        bool click;

        Clases.Citas citas = new Clases.Citas();

        public Citas()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void Citas_Load(object sender, EventArgs e)
        {
            dgvCita.DataSource = citas.Fill("general");
            
            txtId.Text = citas.NextID();
            
            cbMedico.DataSource = citas.Fill("medico");
            cbMedico.DisplayMember = "Nombre";
            cbMedico.ValueMember = "ID";

            cbPaciente.DataSource = citas.Fill("paciente");
            cbPaciente.DisplayMember = "Nombre";
            cbPaciente.ValueMember = "ID";
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            citas.Clean(groupBox1);
            txtId.Text = citas.NextID();

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
           
            idMedico = Convert.ToInt32(cbMedico.SelectedValue);
            idPaciente = Convert.ToInt32(cbPaciente.SelectedValue);

            Clases.Citas cita = new Clases.Citas(id,date.Value.Date,time.Value.TimeOfDay,idMedico,idPaciente);
            if (cita.valid)
            {
                cita.Editar();
                click = false;
                habilitadores();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            Clases.Citas cita = new Clases.Citas(id);
            if (cita.valid)
            {
                cita.Eliminar();
                click = false;
                habilitadores();
            }
        }

        private void dgvCitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow dataGridViewRow = dgvCita.Rows[e.RowIndex];
                    txtId.Text = dataGridViewRow.Cells[0].Value.ToString();
                    date.Value = Convert.ToDateTime(dataGridViewRow.Cells[1].Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    time.Text = dataGridViewRow.Cells[2].Value.ToString();
                    cbMedico.Text = dataGridViewRow.Cells[3].Value.ToString();
                    cbPaciente.Text = dataGridViewRow.Cells[4].Value.ToString();
                    click = true;
                    habilitadores();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void txtFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                citas.fill = txtFiltro.Text;
                dgvCita.DataSource = citas.Fill("filtroPaciente");
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                citas.fill = txtFiltro.Text;
                dgvCita.DataSource = citas.Fill("filtroMedico");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            dgvCita.DataSource = citas.Fill("general");
            if (comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 2)
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                citas.fill = dateTimePicker1.Text;
                dgvCita.DataSource = citas.Fill("filtroFecha");
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
            oRep.Load(@"..\..\Reportes\ReporteCita.rpt");
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
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@doctor"];
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
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@doctor"];
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
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@doctor"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fecha"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = txtFiltro.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@doctor"];
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
            idMedico = Convert.ToInt32(cbMedico.SelectedValue);
            idPaciente = Convert.ToInt32(cbPaciente.SelectedValue);

            Clases.Citas cita = new Clases.Citas(id, date.Value.Date, time.Value.TimeOfDay, idMedico, idPaciente);
            if (cita.valid)
            {
                cita.Insertar();
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
                    dgvCita.DataSource = citas.Fill("general");
                    citas.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    groupSeguro.Enabled = false;
                    txtFiltro.Enabled = false;
                    txtId.Text = citas.NextID();
                }

            }
            else if (btnEditar.Enabled)
            {
                if (!click)
                {
                    dgvCita.DataSource = citas.Fill("general");
                    citas.Clean(groupBox1);
                    txtFiltro.Text = "";
                    comboBox1.SelectedIndex = -1;
                    groupSeguro.Enabled = false;
                    txtFiltro.Enabled = false;
                    txtId.Text = citas.NextID();
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnRegistro.Enabled = true;
                }
            }
        }
    }
}
