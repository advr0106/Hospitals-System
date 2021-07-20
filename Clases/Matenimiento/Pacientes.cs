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
    class Pacientes : Persona
    {
        ConexionesBD conex = new ConexionesBD();
        string cedula;
        public string Cedula { get => cedula; set => cedula = value; }
        bool asegurado;
        public bool Asegurado { get => asegurado; set => asegurado = value; }

        public Pacientes() { }
        public Pacientes(int id, string cedula, string nombre, bool asegurado)
        {
            if (id == 0 || nombre == "" || cedula =="")
            {
                MessageBox.Show("Debe llenar todos los datos del paciente");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
                Nombre = nombre;
                Cedula = cedula;
                Asegurado = asegurado;
            }
        }
        public Pacientes(int id)
        {
            if (id == 0)
            {
                MessageBox.Show("Debe llenar todos los datos del paciente");
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
                conex.command.CommandText = "EditarPaciente";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);
                conex.command.Parameters.AddWithValue("@nombre", Nombre);
                conex.command.Parameters.AddWithValue("@cedula", cedula);
                conex.command.Parameters.AddWithValue("@asegurado", Asegurado);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha editado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar un paciente que está activo");
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

        public override void Eliminar()
        {

            try
            {
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EliminarPaciente";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha eliminado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar un paciente que actualmente está activo");
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
                if (ente == "general")
                {
                    conex.command.CommandText = "ConsultPaciente";
                    conex.command.Parameters.AddWithValue("@nombre", "");
                    conex.command.Parameters.AddWithValue("@cedula", "");
                    conex.command.Parameters.AddWithValue("@seguro", "");
                }
                else if (ente=="filtroNombre")
                {
                    conex.command.CommandText = "ConsultPaciente";
                    conex.command.Parameters.AddWithValue("@nombre", fill);
                    conex.command.Parameters.AddWithValue("@cedula", "");
                    conex.command.Parameters.AddWithValue("@seguro", "");
                }
                else if (ente == "filtroCedula")
                {
                    conex.command.CommandText = "ConsultPaciente";
                    conex.command.Parameters.AddWithValue("@cedula", fill);
                    conex.command.Parameters.AddWithValue("@nombre", "");
                    conex.command.Parameters.AddWithValue("@seguro", "");
                }
                else if (ente == "filtroSeguro")
                {
                    conex.command.CommandText = "ConsultPaciente";
                    conex.command.Parameters.AddWithValue("@cedula", "");
                    conex.command.Parameters.AddWithValue("@nombre", "");
                    conex.command.Parameters.AddWithValue("@seguro", fill);
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
                conex.command = new SqlCommand();
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "RegistrarPaciente";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@nombre", Nombre);
                conex.command.Parameters.AddWithValue("@cedula", cedula);
                conex.command.Parameters.AddWithValue("@asegurado", Asegurado);

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

        public override string NextID()
        {
            string result;
            conex.command = new SqlCommand();
            conex.command.Connection = conex.controlConexion();
            conex.command.CommandText = "NextIdPaciente";
            conex.command.CommandType = CommandType.StoredProcedure;

            result = Convert.ToString(conex.command.ExecuteScalar());

            conex.controlConexion();

            return result;
        }
    }
}
