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
    public partial class Altas : Base
    {
        int id, idIngreso, costo;
        bool click = false;
        Clases.Altas altas = new Clases.Altas();
        public Altas()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void dateAlta_ValueChanged(object sender, EventArgs e)
        {
            Clases.Altas altas = new Clases.Altas();
            if (dateAlta.Value > dateIngreso.Value)
            {
                txtPagar.Text = altas.Cobrar(dateIngreso.Value.Date, dateAlta.Value.Date, txtHabitacion.Text);
            }
            else if (dateAlta.Value <= dateIngreso.Value && !click)
            {

                txtPagar.Text = "";
                MessageBox.Show("La fecha del alta médica debe de ser después de la fecha de ingreso");

            }

        }
        private void Altas_Load(object sender, EventArgs e)
        {
            dgvAltas.DataSource = altas.Fill("alta");
            dgvIngresos.DataSource = altas.Fill("ingreso");

            txtId.Text = altas.NextID();
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            click = true;
            altas.Clean(groupBox1);
            altas.Clean(groupBox3);
            txtId.Text = altas.NextID();

            txtFiltro.Text = "";
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.ResetText();
            radioButton1.Checked = false;
            radioButton1.Checked = false;
            dateTimePicker1.Enabled = false;
            groupSeguro.Enabled = false;
            txtFiltro.Enabled = false;
            groupBox3.Enabled = false;

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnRegistro.Enabled = true;
            click = false;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            int.TryParse(txtIdIngreso.Text, out idIngreso);
            int.TryParse(txtPagar.Text, out costo);


            Clases.Altas alta = new Clases.Altas(id, dateAlta.Value.Date, costo, idIngreso);
            if (alta.valid)
            {
                alta.Editar();

                click = true;

                altas.Clean(groupBox1);
                altas.Clean(groupBox3);
                txtId.Text = altas.NextID();
                dgvAltas.DataSource = altas.Fill("alta");
                txtFiltro.Text = "";

                comboBox1.SelectedIndex = -1;
                dateTimePicker1.ResetText();
                radioButton1.Checked = false;
                radioButton1.Checked = false;
                dateTimePicker1.Enabled = false;
                groupSeguro.Enabled = false;
                txtFiltro.Enabled = false;

                groupBox3.Enabled = false;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnRegistro.Enabled = true;

                click = false;

            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            Clases.Altas alta = new Clases.Altas(id);
            if (alta.valid)
            {
                alta.Eliminar();

                click = true;
                altas.Clean(groupBox1);
                altas.Clean(groupBox3);
                txtId.Text = altas.NextID();
                dgvAltas.DataSource = altas.Fill("alta");
                dgvIngresos.Enabled = true;
                dgvAltas.Enabled = true;

                txtFiltro.Text = "";
                comboBox1.SelectedIndex = -1;
                dateTimePicker1.ResetText();
                radioButton1.Checked = false;
                radioButton1.Checked = false;
                dateTimePicker1.Enabled = false;
                groupSeguro.Enabled = false;
                txtFiltro.Enabled = false;

                groupBox3.Enabled = false;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnRegistro.Enabled = true;

                click = false;
            }
        }

        private void dgvAltas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow dataGridViewRow = dgvAltas.Rows[e.RowIndex];
                    txtId.Text = dataGridViewRow.Cells[0].Value.ToString();
                    txtIdIngreso.Text = dataGridViewRow.Cells[1].Value.ToString();
                    dateIngreso.Value = Convert.ToDateTime(dataGridViewRow.Cells[2].Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    txtHabitacion.Text = dataGridViewRow.Cells[3].Value.ToString();
                    txtPaciente.Text = dataGridViewRow.Cells[4].Value.ToString();
                    dateAlta.Value = Convert.ToDateTime(dataGridViewRow.Cells[5].Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    txtPagar.Text = dataGridViewRow.Cells[6].Value.ToString();


                    groupBox3.Enabled = true;

                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                    btnRegistro.Enabled = false;

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
            dgvAltas.DataSource = altas.Fill("alta");
            if (comboBox1.SelectedIndex == 1)
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
            if (comboBox1.SelectedIndex == 0&& radioButton1.Checked)
            {
                altas.fill = dateTimePicker1.Text;
                dgvAltas.DataSource = altas.Fill("filtroFechaA");
            }
            if (comboBox1.SelectedIndex == 0 && radioButton2.Checked)
            {
                altas.fill = dateTimePicker1.Text;
                dgvAltas.DataSource = altas.Fill("filtroFechaI");
            }
        }

        private void txtFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                altas.fill = txtFiltro.Text;
                dgvAltas.DataSource = altas.Fill("filtroPaciente");
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
            oRep.Load(@"..\..\Reportes\ReporteAlta.rpt");
            oRep.VerifyDatabase();
            oRep.Refresh();
            if (comboBox1.SelectedIndex == -1)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaA"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaI"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

            }
            else if (comboBox1.SelectedIndex == 0 && radioButton1.Checked)
            {
                pdv.Value = dateTimePicker1.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaA"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaI"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

            }
            else if (comboBox1.SelectedIndex == 0 && radioButton2.Checked)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaA"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = dateTimePicker1.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaI"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

            }
            else if (comboBox1.SelectedIndex == 1)
            {
                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaA"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = txtFiltro.Text;
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@paciente"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);

                pdv.Value = "";
                pfds = oRep.DataDefinition.ParameterFields;
                pfd = pfds["@fechaI"];
                pvs.Add(pdv);
                pfd.ApplyCurrentValues(pvs);
            }

            reporte.crystalReportViewer1.ReportSource = oRep;
            reporte.crystalReportViewer1.Refresh();
            reporte.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dgvAltas.DataSource = altas.Fill("alta");
            if (!dateTimePicker1.Enabled)
            {
                dateTimePicker1.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dgvAltas.DataSource = altas.Fill("alta");
            if (!dateTimePicker1.Enabled)
            {
                dateTimePicker1.Enabled = true;
            }
        }

        private void dgvIngresos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow dataGridViewRow = dgvIngresos.Rows[e.RowIndex];
                    txtIdIngreso.Text = dataGridViewRow.Cells[0].Value.ToString();
                    dateIngreso.Value = Convert.ToDateTime(dataGridViewRow.Cells[1].Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    txtHabitacion.Text = dataGridViewRow.Cells[2].Value.ToString();
                    txtPaciente.Text = dataGridViewRow.Cells[3].Value.ToString();


                    groupBox3.Enabled = true;

                    if (!groupBox3.Enabled && !btnEditar.Enabled)
                    {
                        btnRegistro.Enabled = true;
                    }
                    dateAlta.Value = dateIngreso.Value.AddDays(1);

                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
        private void btnRegistro_Click(object sender, EventArgs e)
        {
            int.TryParse(txtId.Text, out id);
            int.TryParse(txtIdIngreso.Text, out idIngreso);
            int.TryParse(txtPagar.Text, out costo);

            Clases.Altas alta = new Clases.Altas(id, dateAlta.Value.Date, costo, idIngreso);
            if (alta.valid)
            {
                alta.Insertar();

                click = true;

                altas.Clean(groupBox1);
                altas.Clean(groupBox3);
                txtId.Text = altas.NextID();
                dgvAltas.DataSource = altas.Fill("alta");
                dgvIngresos.Enabled = true;
                dgvAltas.Enabled = true;
                groupBox3.Enabled = false;

                click = false;
            }
        }
    }

}
