using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Clases
{
    abstract class Persona : ICRUD
    {
        public string fill { get; set; }
        public bool valid { get; set; }
        public int Id { get; set; }
        private string nombre;
        public string Nombre { get => nombre; set => nombre = value; }
        public abstract void Editar();
        public abstract void Eliminar();
        public abstract DataTable Fill(string ente);
        public abstract void Insertar();
        public abstract string NextID();
        public virtual void Clean(GroupBox group)
        {
            foreach (var txt in group.Controls)
            {
                if(txt is TextBox)
                {
                    ((TextBox)txt).Clear();
                }
            }
        }
    }
}
