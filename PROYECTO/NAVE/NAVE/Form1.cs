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
			string[] alias = { "Cod Producto", "Producto", "Proveedor", "Presentación", "Fecha Compra", "Unidades Minimas", "Bodega", "Estado" };
			navegador1.asignarAlias(alias);
			navegador1.asignarSalida(this);
			navegador1.asignarAyuda("1");
			navegador1.asignarTabla("tbl_producto");
			ayuda_tp.IsBalloon = true;
			List<Control> campos = new List<Control>();
			campos.Add(textBox1);
			campos.Add(textBox2);
			campos.Add(textBox3);
			campos.Add(textBox6);
			campos.Add(dateTimePicker1);
			campos.Add(textBox4);
			campos.Add(textBox9);
			campos.Add(textBox8);
			navegador1.ObtenerCamposMantenimiento(campos);
			navegador1.ObtenerDataGidView(dataGridView1);

		
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
		
        }
    }
}
