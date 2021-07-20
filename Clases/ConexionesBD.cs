using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Clases
{
    class ConexionesBD
    {
        private SqlConnection conn = new SqlConnection("server=DESKTOP-BTMKGAN;database=hospital; integrated security= true;");
        public SqlCommand command = new SqlCommand();
        public SqlDataReader dataReader;
        public DataTable dataTable;
        public bool result;
            
        public SqlConnection controlConexion()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error en la conexión " + err.Message);                
            }
            return conn;
        }
    }
}
