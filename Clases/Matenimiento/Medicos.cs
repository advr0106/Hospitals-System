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
    class Medicos : Persona
    {
        ConexionesBD conex = new ConexionesBD();
        private int execuatur;
        public int Execuatur { get => execuatur; set => execuatur = value; }
        private string especialidad;
        public string Especialidad { get => especialidad; set => especialidad = value; }

        public Medicos() { }
        public Medicos(int id, string nombre, int execuatur, string especialidad)
        {
            if (id == 0 || nombre == "" || execuatur == 0 || especialidad == "")
            {
                MessageBox.Show("Debe llenar todos los datos del médico");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
                Nombre = nombre;
                Execuatur = execuatur;
                Especialidad = especialidad;
            }
        }
        public Medicos(int id)
        {
            if (id == 0)
            {
                MessageBox.Show("Debe llenar todos los datos del médico");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
            }
        }
        public override void Editar()
        {
            try
            {
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EditarMedico";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);
                conex.command.Parameters.AddWithValue("@nombre", Nombre);
                conex.command.Parameters.AddWithValue("@execuatur", Execuatur);
                conex.command.Parameters.AddWithValue("@especialidad", Especialidad);

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

        public override void Eliminar()
        {
            try
            {
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EliminarMedico";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha eliminado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar un médico está activo");
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

        public override DataTable Fill(string ente)
        {
            try
            {
                conex.dataTable = new DataTable();
                conex.command = new SqlCommand();
                conex.command.Connection = conex.controlConexion();
                switch (ente)
                {
                    case "general":
                        conex.command.CommandText = "ConsultMedico";
                        conex.command.Parameters.AddWithValue("@nombre", "");
                        conex.command.Parameters.AddWithValue("@especialidad", "");
                        break;
                    case "filtroNombre":
                        conex.command.CommandText = "ConsultMedico";
                        conex.command.Parameters.AddWithValue("@nombre", fill);
                        conex.command.Parameters.AddWithValue("@especialidad", "");
                        break;
                    case "filtroEspecialidad":
                        conex.command.CommandText = "ConsultMedico";
                        conex.command.Parameters.AddWithValue("@nombre", "");
                        conex.command.Parameters.AddWithValue("@especialidad", fill);
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

        public override void Insertar()
        {
            try
            {
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "RegistrarMedico";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@nombre", Nombre);
                conex.command.Parameters.AddWithValue("@execuatur", Execuatur);
                conex.command.Parameters.AddWithValue("@especialidad", Especialidad);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha insertado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar un médico que actualmente están en uso");
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

        public override string NextID()
        {
            string result;
            conex.command = new SqlCommand();
            conex.command.Connection = conex.controlConexion();
            conex.command.CommandText = "NextIdMedico";
            conex.command.CommandType = CommandType.StoredProcedure;

            result = Convert.ToString(conex.command.ExecuteScalar());

            conex.controlConexion();

            return result;
        }
    }
}
