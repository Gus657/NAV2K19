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
			string[] alias = { "Cod", "Sucursal", "Venta", "Fecha", "Nombre", "nit", "Dirección", "Estado" };
			navegador1.asignarAlias(alias);
			navegador1.asignarSalida(this);
			navegador1.asignarAyuda("1");
			navegador1.asignarTabla("encabezado");
			ayuda_tp.IsBalloon = true;
			List<Control> campos = new List<Control>();
			campos.Add(textBox1);
			campos.Add(textBox2);
			campos.Add(textBox3);
			campos.Add(dateTimePicker1);
			campos.Add(textBox6);
			campos.Add(textBox4);
			campos.Add(textBox9);
			campos.Add(textBox8);
			navegador1.ObtenerCamposMantenimiento(campos);
			navegador1.ObtenerDataGidView(dataGridView1);
			//////////////////////////////////////////////
			string[] alias2 = { "Linea", "Encabezado", "Producto", "Precio", "Cantidad", "Estado" };
			navegador1.asignarAlias2(alias2);
			navegador1.asignarSalida(this);
			navegador1.asignarAyuda("1");
			navegador1.asignarTabla2("detalle");
			ayuda_tp.IsBalloon = true;
			List<Control> campos2 = new List<Control>();
			campos2.Add(textBox14);
			campos2.Add(textBox13);
			campos2.Add(textBox12);
			campos2.Add(textBox11);
			campos2.Add(textBox10);
			campos2.Add(textBox5);
			navegador1.ObtenerCamposMantenimiento2(campos2);
			navegador1.ObtenerDataGidView2(dataGridView2);
		
        }





		private void Form1_Load(object sender, EventArgs e)
        {
            frm_login login = new frm_login();
            login.ShowDialog();
            string aplicacionActiva = "1";
            navegador1.ObtenerIdUsuario(login.obtenerNombreUsuario());
            navegador1.botonesYPermisosInicial(login.obtenerNombreUsuario(), aplicacionActiva);
            //navegador1.registros();
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
			dataGridView1.Rows.Add("five", "six", "seven", "eight");
		}

		private void Button5_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
		}

		private void Timer1_Tick(object sender, EventArgs e)
		{
			textBox13.Text = textBox1.Text;

		}
	}
}
