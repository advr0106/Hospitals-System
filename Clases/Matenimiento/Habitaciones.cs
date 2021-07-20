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
    abstract class Habitaciones : ICRUD
    {
        public string fill { get; set; }
        public bool valid { get; set; }
        public string accion;
        ConexionesBD conex = new ConexionesBD();
        private int numero, idTipo, precio;
        public int Id { get ; set; }
        public int Numero { get => numero; set => numero = value; }
        public int IdTipo { get => idTipo; set => idTipo = value; }
        public int Precio { get => precio; set => precio = value; }
        public void Clean(GroupBox group)
        {
            foreach (var txt in group.Controls)
            {
                if (txt is TextBox)
                {
                    ((TextBox)txt).Clear();
                }
            }
        }

        public void Editar()
        {
            try
            {
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "EditarHabitacion";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);
                conex.command.Parameters.AddWithValue("@numero", Numero);
                conex.command.Parameters.AddWithValue("@costo", Precio);
                conex.command.Parameters.AddWithValue("@idTipo", IdTipo);

                conex.command.ExecuteNonQuery();
                accion = "edito";
                Mensaje();
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
                conex.command.CommandText = "EliminarHabitacion";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@id", Id);

                conex.command.ExecuteNonQuery();

                accion = "elimino";
                Mensaje();
            }
            catch (SqlException err)
            {
                if (err.Number == 547)
                {
                    MessageBox.Show("No puedes eliminar una habitación que actualmente están en uso");
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
                        conex.command.CommandText = "ConsultHabitacion";
                        conex.command.Parameters.AddWithValue("@tipo", "");
                        break;
                    case "tipo":
                        conex.command.CommandText = "FillTipoCombo";
                        break;
                    case "filtroTipo":
                        conex.command.CommandText = "ConsultHabitacion";
                        conex.command.Parameters.AddWithValue("@tipo", fill);
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
                conex.command.CommandText = "RegistrarHabitacion";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@numero", Numero);
                conex.command.Parameters.AddWithValue("@costo", Precio);
                conex.command.Parameters.AddWithValue("@idTipo", IdTipo);

                conex.command.ExecuteNonQuery();

                accion = "inserto";
                Mensaje();
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
            conex.command.CommandText = "NextIdHabitacion";
            conex.command.CommandType = CommandType.StoredProcedure;

            result = Convert.ToString(conex.command.ExecuteScalar());

            conex.controlConexion();

            return result;
        }
        public virtual void Mensaje() { }
    }
}
