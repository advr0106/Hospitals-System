﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Clases
{
    class HabitacionesSuites : Habitaciones
    {
        public HabitacionesSuites() { }
        public HabitacionesSuites(int id)
        {
            if (id == 0)
            {
                MessageBox.Show("Debe llenar todos los datos de la habitación");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
            }
        }
        public HabitacionesSuites(int id, int numero, int idTipo, int precio)
        {
            if (id == 0 || numero == 0 || idTipo == 0 || precio == 0)
            {
                MessageBox.Show("Debe llenar todos los datos de la habitación");
                valid = false;
            }
            else
            {
                valid = true;
                Id = id;
                Numero = numero;
                IdTipo = idTipo;
                Precio = precio;
            }
        }
        public override void Mensaje()
        {
            if (accion=="inserto")
            {
                MessageBox.Show("Se ha insertado correctamente una habitación suite");
            }
            else if(accion == "edito")
            {
                MessageBox.Show("Se ha editado correctamente una habitación suite");
            }
            else if (accion == "elimino")
            {
                MessageBox.Show("Se ha eliminado correctamente una habitación suite");
            }
        }
    }
}
