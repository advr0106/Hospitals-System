using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Clases
{
    class Citas : ICRUD
    {
        public string fill { get; set; }
        public bool valid { get; set; }
        ConexionesBD conex = new ConexionesBD();
        private DateTime fecha;
        private TimeSpan hora;
        Medicos medicos = new Medicos();
        Pacientes pacientes = new Pacientes();
        public int IdMedico { get => medicos.Id; set => medicos.Id = value; }
        public int IdPaciente { get => pacientes.Id; set => pacientes.Id = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public TimeSpan Hora { get => hora; set => hora = value; }
        public int Id { get; set; }
        public Citas() { }
        public Citas(int id, DateTime fecha, TimeSpan hora, int idMedico, int idPaciente)
        {
            if (id == 0 || fecha == null || hora == null || idMedico == 0 || idPaciente == 0)
            {
                MessageBox.Show("Debe llenar todos los datos de la cita");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
                Fecha = fecha;
                Hora = hora;
                IdMedico = idMedico;
                IdPaciente = idPaciente;
            }
        }
        public Citas(int id)
        {
            if (id == 0)
            {
                MessageBox.Show("Debe llenar todos los datos de la cita");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
            }
        }

        public void Clean(GroupBox group)
        {
            foreach (var control in group.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).ResetText();
                }
            }
        }

        public void Editar()
        {
            try
            {
                conex.command = new SqlCommand();
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EditarCita";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);
                conex.command.Parameters.AddWithValue("@fecha", Fecha);
                conex.command.Parameters.AddWithValue("@hora", Hora);
                conex.command.Parameters.AddWithValue("@idMedico", IdMedico);
                conex.command.Parameters.AddWithValue("@idPaciente", IdPaciente);
                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha editado correctamente");
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
            finally
            {
                conex.command.Parameters.Clear();
                conex.controlConexion();
            }
        }

        public void Eliminar()
        {
            try
            {
                conex.command = new SqlCommand();
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EliminarCita";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha eliminado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar una cita que actualmente está activa");
                }
                else
                {
                    MessageBox.Show("Error: " + err.Message);
                }
            }
            finally
            {
                conex.command.Parameters.Clear();
                conex.controlConexion();
            }
        }

        public DataTable Fill(string ente)
        {
            try
            {
                conex.dataTable = new DataTable();
                conex.command = new SqlCommand();
                conex.command.Connection = conex.controlConexion();
                switch (ente)
                {
                    case "general":
                        conex.command.CommandText = "FillCitasDGV";
                        conex.command.CommandText = "ConsultCita";
                        conex.command.Parameters.AddWithValue("@fecha", "");
                        conex.command.Parameters.AddWithValue("@paciente", "");
                        conex.command.Parameters.AddWithValue("@doctor", "");
                        break;
                    case "paciente":
                        conex.command.CommandText = "FillPacienteCombo";
                        break;
                    case "medico":
                        conex.command.CommandText = "FillMedicoCombo";
                        break;
                    case "filtroFecha":
                        conex.command.CommandText = "ConsultCita";
                        conex.command.Parameters.AddWithValue("@fecha", fill);
                        conex.command.Parameters.AddWithValue("@paciente", "");
                        conex.command.Parameters.AddWithValue("@doctor", "");
                        break;
                    case "filtroPaciente":
                        conex.command.CommandText = "ConsultCita";
                        conex.command.Parameters.AddWithValue("@fecha", "");
                        conex.command.Parameters.AddWithValue("@paciente", fill);
                        conex.command.Parameters.AddWithValue("@doctor", "");
                        break;
                    case "filtroMedico":
                        conex.command.CommandText = "ConsultCita";
                        conex.command.Parameters.AddWithValue("@fecha", "");
                        conex.command.Parameters.AddWithValue("@paciente", "");
                        conex.command.Parameters.AddWithValue("@doctor", fill);
                        break;
                }
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.dataReader = conex.command.ExecuteReader();
                conex.dataTable.Load(conex.dataReader);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
            finally
            {
                conex.controlConexion();
            }
            return conex.dataTable;
        }

        public void Insertar()
        {
            try
            {
                conex.command = new SqlCommand();
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "RegistrarCita";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@fecha", Fecha);
                conex.command.Parameters.AddWithValue("@hora", Hora);
                conex.command.Parameters.AddWithValue("@idMedico", IdMedico);
                conex.command.Parameters.AddWithValue("@idPaciente", IdPaciente);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha insertado correctamente");
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
            finally
            {
                conex.command.Parameters.Clear();
                conex.controlConexion();
            }
        }

        public string NextID()
        {
            string result;
            conex.command = new SqlCommand();
            conex.command.Connection = conex.controlConexion();
            conex.command.CommandText = "NextIdCita";
            conex.command.CommandType = CommandType.StoredProcedure;

            result = Convert.ToString(conex.command.ExecuteScalar());

            conex.controlConexion();

            return result;
        }
    }
}
