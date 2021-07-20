using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Clases
{
    interface ICRUD
    {
        int Id { get; set; }
        bool valid { get; set; }
        string fill { get; set; }
        void Insertar();
        void Editar();
        DataTable Fill(string ente);
        void Eliminar();
        string NextID();
        void Clean(GroupBox group);
    }
}
