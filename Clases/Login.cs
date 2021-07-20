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
    class Login
    {
        ConexionesBD conex = new ConexionesBD();
        private string user;
        private string pass;

        public Login(string user, string pass)
        {
            this.user = user;
            this.pass = pass;
        }
        public bool autenticar()
        {
            try
            {
                conex.command = new SqlCommand($"SELECT users, pass FROM Login WHERE users='{user}' AND pass='{pass}'", conex.controlConexion());
                conex.dataReader = conex.command.ExecuteReader();
                conex.result = conex.dataReader.Read() ? true : false;
            }
            catch (Exception err)
            {
                MessageBox.Show("Error " + err.Message);
            }
            finally
            {
                conex.controlConexion();
            }
            return conex.result;
        }
        public void Insertar()
        {
            try
            {
                conex.command.Connection = conex.controlConexion();
                conex.command.CommandText = "Usuario";
                conex.command.CommandType = CommandType.StoredProcedure;
                conex.command.Parameters.AddWithValue("@user", user);
                conex.command.Parameters.AddWithValue("@pass", pass);

                conex.command.ExecuteNonQuery();

                MessageBox.Show("Se ha insertado correctamente");
            }
            catch (SqlException err)
            {
                if (err.Number == 2627)
                {
                    MessageBox.Show("El usuario que deseas crear ya existe");
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
    }
}
