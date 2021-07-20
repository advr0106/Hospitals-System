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
    class Ingresos : ICRUD
    {
        public string fill { get; set; }
        public bool valid { get; set; }
        ConexionesBD conex = new ConexionesBD();
        private DateTime fecha;
        Pacientes pacientes = new Pacientes();
        Habitaciones habitaciones = new HabitacionesDobles();
        public int IdPaciente { get => pacientes.Id; set => pacientes.Id = value; }
        public int IdHabitacion { get => habitaciones.Id; set => habitaciones.Id = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public int Id { get; set; }
        public Ingresos()
        {

        }
        public Ingresos(int id, DateTime fecha, int idHabitacion, int idPaciente)
        {
            if (id == 0 || fecha == null  || idHabitacion == 0 || idPaciente == 0)
            {
                MessageBox.Show("Debe llenar todos los datos de la cita");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
                Fecha = fecha;
                IdHabitacion = idHabitacion;
                IdPaciente = idPaciente;
            }
        }
        public Ingresos(int id)
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
                    ((ComboBox)control).SelectedIndex = 1;
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
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EditarIngreso";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);
                conex.command.Parameters.AddWithValue("@fechaI", Fecha);
                conex.command.Parameters.AddWithValue("@idHabitacion", IdHabitacion);
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
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EliminarIngreso";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha eliminado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar un ingreso que está activo");
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
                        conex.command.CommandText = "ConsultIngreso";
                        conex.command.Parameters.AddWithValue("@fecha", "");
                        conex.command.Parameters.AddWithValue("@habitacion", "");
                        break;
                    case "paciente":
                        conex.command.CommandText = "FillPacienteCombo";
                        break;
                    case "habitacion":
                        conex.command.CommandText = "FillHabitacionCombo";
                        break;
                    case "filtroFecha":
                        conex.command.CommandText = "ConsultIngreso";
                        conex.command.Parameters.AddWithValue("@fecha", fill);
                        conex.command.Parameters.AddWithValue("@habitacion", "");
                        break;
                    case "filtroHabitacion":
                        conex.command.CommandText = "ConsultIngreso";
                        conex.command.Parameters.AddWithValue("@fecha", "");
                        conex.command.Parameters.AddWithValue("@habitacion", fill);
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
                conex.command.CommandText = "RegistrarIngreso";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@fechaI", Fecha);
                conex.command.Parameters.AddWithValue("@idHabitacion", IdHabitacion);
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
            conex.command.CommandText = "NextIdIngreso";
            conex.command.CommandType = CommandType.StoredProcedure;

            result = Convert.ToString(conex.command.ExecuteScalar());

            conex.controlConexion();

            return result;
        }
    }
}
