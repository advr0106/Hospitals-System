using Hospital.Forms;
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
    class Altas : ICRUD
    {

        public string fill { get; set; }
        public bool valid { get; set; }
        ConexionesBD conex = new ConexionesBD();
        int costo;
        private DateTime fechaF;
        Ingresos ingresos = new Ingresos();
        public int Id { get; set; }
        public int IdIngreso { get => ingresos.Id; set => ingresos.Id = value; }
        public int Costo { get => costo; set => costo = value; }
        public DateTime FechaF { get => fechaF; set => fechaF = value; }
        public Altas()
        {

        }
        public Altas(int id, DateTime fecha, int costo, int idIngreso)
        {
            if (id == 0 || fecha == null || costo == 0 || idIngreso == 0)
            {
                MessageBox.Show("Debe llenar todos los datos del alta médica");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
                FechaF = fecha;
                Costo = costo;
                IdIngreso = idIngreso;
            }
        }
        public Altas(int id)
        {
            if (id == 0)
            {
                MessageBox.Show("Debe llenar todos los datos del alta médica");
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
                conex.command = new SqlCommand();
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EditarAlta";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);
                conex.command.Parameters.AddWithValue("@fechaF", FechaF);
                conex.command.Parameters.AddWithValue("@costo", Costo);
                conex.command.Parameters.AddWithValue("@idIngreso", IdIngreso);
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
                conex.command.CommandText = "EliminarAlta";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha eliminado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar un alta médica que actualmente está activa");
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
                    case "alta":
                        conex.command.CommandText = "ConsultAlta";
                        conex.command.Parameters.AddWithValue("@fechaA", "");
                        conex.command.Parameters.AddWithValue("@paciente", "");
                        conex.command.Parameters.AddWithValue("@fechaI", "");
                        break;
                    case "ingreso":
                        conex.command.CommandText = "ConsultIngreso";
                        conex.command.Parameters.AddWithValue("@fecha", "");
                        conex.command.Parameters.AddWithValue("@habitacion", "");
                        break;
                    case "filtroFechaA":
                        conex.command.CommandText = "ConsultAlta";
                        conex.command.Parameters.AddWithValue("@fechaA", fill);
                        conex.command.Parameters.AddWithValue("@paciente", "");
                        conex.command.Parameters.AddWithValue("@fechaI", "");
                        break;
                    case "filtroPaciente":
                        conex.command.CommandText = "ConsultAlta";
                        conex.command.Parameters.AddWithValue("@fechaA", "");
                        conex.command.Parameters.AddWithValue("@paciente", fill);
                        conex.command.Parameters.AddWithValue("@fechaI", "");
                        break;
                    case "filtroFechaI":
                        conex.command.CommandText = "ConsultAlta";
                        conex.command.Parameters.AddWithValue("@fechaA", "");
                        conex.command.Parameters.AddWithValue("@paciente", "");
                        conex.command.Parameters.AddWithValue("@fechaI", fill);
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
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "RegistrarAlta";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@fechaF", FechaF);
                conex.command.Parameters.AddWithValue("@costo", Costo);
                conex.command.Parameters.AddWithValue("@idIngreso", IdIngreso);

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
            conex.command.CommandText = "NextIdAlta";
            conex.command.CommandType = CommandType.StoredProcedure;

            result = Convert.ToString(conex.command.ExecuteScalar());

            conex.controlConexion();

            return result;
        }
        public string Cobrar(DateTime FechaInicio, DateTime FechaFinal, string habitacion)
        {
            string result="";
            try
            {
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "cobrar";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@fechaF", FechaFinal);
                conex.command.Parameters.AddWithValue("@fechaI", FechaInicio);
                conex.command.Parameters.AddWithValue("@habitacion", habitacion);

                result = Convert.ToString(conex.command.ExecuteScalar());
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
            finally
            {
                conex.controlConexion();
            }
            return result;
        }
    }
}
