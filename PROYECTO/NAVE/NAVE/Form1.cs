using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDiseno;

namespace NAVE
{
    public partial class Form1 : Form
    {
        ToolTip ayuda_tp = new ToolTip();
		public Form1()
		{
			InitializeComponent();
			dateTimePicker1.Format = DateTimePickerFormat.Custom;
			dateTimePicker1.CustomFormat = "yyyy-MM-dd";
			dateTimePicker2.Format = DateTimePickerFormat.Custom;
			dateTimePicker2.CustomFormat = "yyyy-MM-dd";
			dateTimePicker3.Format = DateTimePickerFormat.Custom;
			dateTimePicker3.CustomFormat = "yyyy-MM-dd";
			string[] alias = { "Cod", "Cliente", "Empleado", "Fecha", "Estado" };
			navegador1.asignarAlias(alias);
			navegador1.asignarSalida(this);
			navegador1.asignarAyuda("1");
			navegador1.asignarTabla("tbl_reservaciones");
			navegador1.asignarComboConTabla(comboBox1,"tbl_clientes", "nombres_cliente", 1);
			navegador1.asignarComboConTabla(comboBox2, "tbl_empleado", "nombres", 1);
			ayuda_tp.IsBalloon = true;
			List<Control> campos = new List<Control>();
			campos.Add(textBox1);
			campos.Add(comboBox1);
			campos.Add(comboBox2);
			campos.Add(dateTimePicker1);
			campos.Add(textBox8);
			navegador1.ObtenerCamposMantenimiento(campos);
			navegador1.ObtenerDataGidView(dataGridView1);
			/////////////////////DETALLE/////////////////////////
			string[] alias2 = { "Linea", "Reservacion", "Habitacion", "LLegada", "Salida", "Estado" };
			navegador1.asignarAlias2(alias2);
			navegador1.asignarSalida(this);
			navegador1.asignarAyuda("1");
			navegador1.asignarTabla2("tbl_detalle_reservacion");
			navegador1.asignarComboConTabla(comboBox3, "tbl_habitaciones", "KidNumeroHabitacion", 0);
			ayuda_tp.IsBalloon = true;
			List<Control> campos2 = new List<Control>();
			campos2.Add(textBox14);
			campos2.Add(textBox13);
			campos2.Add(comboBox3);
			campos2.Add(dateTimePicker2);
			campos2.Add(dateTimePicker3);
			campos2.Add(textBox5);
			navegador1.ObtenerCamposMantenimiento2(campos2);
			navegador1.ObtenerDataGidView2(dataGridView2);
			foreach (string  item in alias2)
			{
				dataGridView3.Columns.Add(item,item);
			}
		
			
		
        }





		private void Form1_Load(object sender, EventArgs e)
        {
            frm_login login = new frm_login();
            login.ShowDialog();
            string aplicacionActiva = "1";
            navegador1.ObtenerIdUsuario(login.obtenerNombreUsuario());
            navegador1.botonesYPermisosInicial(login.obtenerNombreUsuario(), aplicacionActiva);
            navegador1.ObtenerIdAplicacion(aplicacionActiva);
			
			
			

		
			
				
			


		}



		private void Navegador1_Load(object sender, EventArgs e)
        {

        }

        private void Navegador1_Load_1(object sender, EventArgs e)
        {
     
        }

        private void Button1_Click(object sender, EventArgs e)
        {
			navegador1.inicio();
        }

		private void Button2_Click(object sender, EventArgs e)
		{
			navegador1.anterior();
		}

		private void Button3_Click(object sender, EventArgs e)
		{
			navegador1.siguiente();
		}

		private void Button4_Click(object sender, EventArgs e)
		{
			navegador1.fin();
		}

		private void Label1_Click(object sender, EventArgs e)
		{

		}

		private void Button6_Click(object sender, EventArgs e)
		{
			dataGridView3.Rows.Add(textBox14.Text, textBox13.Text, comboBox3.Text, dateTimePicker2.Text, dateTimePicker3.Text, textBox5.Text);
		}

		private void Button5_Click(object sender, EventArgs e)
		{
			if (dataGridView3.Rows.Count > 1 && dataGridView3.CurrentCell.RowIndex!= dataGridView3.Rows.Count-1)
			{
				dataGridView3.Rows.RemoveAt(dataGridView3.CurrentCell.RowIndex);
			}
			
		}

		private void Timer1_Tick(object sender, EventArgs e)
		{
			textBox13.Text = textBox1.Text;

		}

		private void DataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			navegador1.cambioIndice();
		}

		private void Label2_Click(object sender, EventArgs e)
		{

		}
	}
}
