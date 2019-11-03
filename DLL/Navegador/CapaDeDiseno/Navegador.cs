using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDeLogica;
using CapaDatos;

namespace CapaDeDiseno
{
	public partial class Navegador : UserControl
	{

		logicaNav logic = new logicaNav();
		List<Control> camposNav = new List<Control>();
		List<Control> camposNav2 = new List<Control>();
		Form cerrar;
		int correcto = 0;
		int combo = 0;
		string enca = "def";
		string deta = "def";
		int activar = 0;    //Variable para reconocer que funcion realizara el boton de guardar (1. Ingresar, 2. Modificar, 3. Eliminar)
		string[] aliasC = new string[40];
		int[] modoCampoCombo = new int[40];
		string[] aliasC2 = new string[40];
		string[] tipoCampo = new string[30];//
		string[] tipoCampo2 = new string[30];//
		string[] NomCampo = new string[40];
		string[] NomCampo2 = new string[40];
		string[] campoCombo = new string[30];
		string[] listaItems = new string[30];
		string[] tablaCombo = new string[30];
		int estado = 1;
		int noCombo = 0;
		bool presionado = false;
		sentencia sn = new sentencia(); //objeto del componente de seguridad para obtener el método de la bitácora
		string idUsuario = "";
		string idAplicacion = "";
		//las siguientes dos variables son para el método botonesYPermisos();
		string idyuda;
		string AsRuta;
		string AsIndice;
		string Asayuda;
		// string rutaa;
		ToolTip ayuda_tp = new ToolTip();
		public Navegador()
		{
			InitializeComponent();
			ayuda_tp.IsBalloon = true;
			ayuda_tp.SetToolTip(Btn_Ingresar, "Escribir nuevo registro");
			ayuda_tp.SetToolTip(Btn_Modificar, "Cambiar un registro");
			ayuda_tp.SetToolTip(Btn_Guardar, "Guardar cambios");
			ayuda_tp.SetToolTip(Btn_Cancelar, "Cancelar Acciones");
			ayuda_tp.SetToolTip(Btn_Eliminar, "Eliminar un registro");
			ayuda_tp.SetToolTip(Btn_Consultar, "Ir a Consultas inteligentes");
			ayuda_tp.SetToolTip(Btn_Imprimir, "Ir a Reporteador");
			ayuda_tp.SetToolTip(Btn_Refrescar, "Actualizar tabla");
			ayuda_tp.SetToolTip(Btn_FlechaInicio, "Primer registro");
			ayuda_tp.SetToolTip(Btn_Anterior, "Posición superior en tabla");
			ayuda_tp.SetToolTip(Btn_Siguiente, "Posición inferior en tabla");
			ayuda_tp.SetToolTip(Btn_FlechaFin, "Fin de la tabla");
			ayuda_tp.SetToolTip(Btn_MasAyuda, "Nueva Ayuda");
			ayuda_tp.SetToolTip(Btn_Ayuda, "Ayuda del formulario");
			ayuda_tp.SetToolTip(Btn_Salir, "Salir del formulario");
		}

		private void Navegador_Load(object sender, EventArgs e)
		{



			if (enca != "def" && deta != "def")
			{
				string TablaOK = logic.TestTabla(enca);
				string TablaOK2 = logic.TestTabla(deta);
				if (TablaOK == "" && correcto == 0 && TablaOK2 == "")
				{
					string EstadoOK = logic.TestEstado(enca);
					string EstadoOK2 = logic.TestEstado(deta);

					if (EstadoOK == "" && correcto == 0 && EstadoOK2 == "")
					{
						string[] camposEnc =logic.campos(enca);
						string[] tiposEnc = logic.tipos(enca);
						string idCampo = "";
					
						DataTable dt = logic.consultaLogica(enca);
						dataGridView1.DataSource = dt;
						if (dataGridView1.CurrentRow.Cells[0].Value.ToString() != "" && dataGridView1.CurrentRow.Cells[0].Value.ToString() != null)
						{
							if (tiposEnc[0] != "int")
							{
								idCampo += "'" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
							}
							else
							{
								idCampo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
							}
						}
						else
						{
							if (tiposEnc[0] != "int")
							{
								idCampo += "'0'";
							}
							else
							{
								idCampo = "0";
							}
						}
						DataTable dt2 = logic.consultaLogica2(deta, camposEnc[0],idCampo);
						dataGridView2.DataSource = dt2;
						Asayuda = logic.verificacion("");
						if (Asayuda == "0")
						{
							MessageBox.Show("No se encontró ningún registro en la enca Ayuda");
							Application.Exit();
						}
						else
						{

							if (numeroAlias() == logic.contarCampos(enca) && numeroAlias2() == logic.contarCampos(deta))
							{
								llenarCampos();
								llenarCampos2();
								int i = 0;
								int head = 0;
								while (head < logic.contarCampos(enca))
								{
									dataGridView1.Columns[head].HeaderText = aliasC[head];
									head++;
								}
								head = 0;
								while (head < logic.contarCampos(deta))
								{
									dataGridView2.Columns[head].HeaderText = aliasC2[head];
									head++;
								}
								deshabilitarcampos_y_botones();

								Btn_Modificar.Enabled = true;
								Btn_Eliminar.Enabled = true;

								if (logic.TestRegistros(enca) > 0 && logic.TestRegistros(deta) > 0)
								{
									foreach (Control componente in camposNav)
									{
										
											componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
											i++;
										
									}
									i = 0;
								}
								else
								{
									Btn_Anterior.Enabled = false;
									Btn_Siguiente.Enabled = false;
									Btn_FlechaInicio.Enabled = false;
									Btn_FlechaFin.Enabled = false;
									Btn_Modificar.Enabled = false;
									Btn_Eliminar.Enabled = false;
								}
							}
							else
							{
								if (numeroAlias() < logic.contarCampos(enca) || numeroAlias2() < logic.contarCampos(deta))
								{
									DialogResult validacion = MessageBox.Show(EstadoOK + "El numero de Alias asignados es menor que el requerido \n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
									if (validacion == DialogResult.OK)
									{
										Application.Exit();
									}
								}
								else
								{
									if (numeroAlias() > logic.contarCampos(enca) || numeroAlias2() < logic.contarCampos(deta))
									{
										DialogResult validacion = MessageBox.Show(EstadoOK + "El numero de Alias asignados es mayor que el requerido \n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
										if (validacion == DialogResult.OK)
										{
											Application.Exit();
										}
									}
								}
							}

						}
					}
					else
					{
						DialogResult validacion = MessageBox.Show(EstadoOK + "\n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
						if (validacion == DialogResult.OK)
						{
							Application.Exit();
						}
					}
				}
				else
				{
					DialogResult validacion = MessageBox.Show(TablaOK + "\n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
					if (validacion == DialogResult.OK)
					{
						Application.Exit();
					}
				}
			}

		}

		//-----------------------------------------------Funciones-----------------------------------------------//

		public void siguiente()
		{
			int i = 0;
			string[] Campos = logic.campos(deta);

			int fila = dataGridView2.SelectedRows[0].Index;
			if (fila < dataGridView2.Rows.Count - 1)
			{
				dataGridView2.Rows[fila + 1].Selected = true;
				dataGridView2.CurrentCell = dataGridView2.Rows[fila + 1].Cells[0];

				foreach (Control componente in camposNav2)
				{

					componente.Text = dataGridView2.CurrentRow.Cells[i].Value.ToString();
					i++;


				}

			}
		}

		public void anterior()
		{
			int i = 0;
			string[] Campos = logic.campos(deta);

			int fila = dataGridView2.SelectedRows[0].Index;
			if (fila > 0)
			{
				dataGridView2.Rows[fila - 1].Selected = true;
				dataGridView2.CurrentCell = dataGridView2.Rows[fila - 1].Cells[0];

				foreach (Control componente in camposNav2)
				{


					componente.Text = dataGridView2.CurrentRow.Cells[i].Value.ToString();
					i++;


				}
			}
		}

		public void fin()
		{
			dataGridView2.Rows[dataGridView2.Rows.Count - 2].Selected = true;
			dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView1.Rows.Count - 2].Cells[0];

			int i = 0;
			string[] Campos = logic.campos(deta);

			int fila = dataGridView2.SelectedRows[0].Index;
			if (fila < dataGridView2.Rows.Count - 1)
			{
				dataGridView2.Rows[dataGridView1.Rows.Count - 2].Selected = true;
				dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[0];

				foreach (Control componente in camposNav2)
				{

					componente.Text = dataGridView2.CurrentRow.Cells[i].Value.ToString();
					i++;
				}



			}
		}

		public void inicio()
		{
			dataGridView2.Rows[0].Selected = true;
			dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[0];

			int i = 0;
			string[] Campos = logic.campos(deta);

			int fila = dataGridView2.SelectedRows[0].Index;
			if (fila < dataGridView2.Rows.Count - 1)
			{
				dataGridView2.Rows[0].Selected = true;
				dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[0];


				foreach (Control componente in camposNav2)
				{
					if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
					{
						componente.Text = dataGridView2.CurrentRow.Cells[i].Value.ToString();
						i++;
					}
					if (componente is Button)
					{
						string var1 = dataGridView2.CurrentRow.Cells[i].Value.ToString();
						if (var1 == "0")
						{
							componente.Text = "Desactivado";
							componente.BackColor = Color.Red;
						}
						if (var1 == "1")
						{
							componente.Text = "Activado";
							componente.BackColor = Color.Green;
						}
					}

				}


			}
		}
		public void cambioIndice()
		{

			string[] camposEnc = logic.campos(enca);
			string[] tiposEnc = logic.tipos(enca);
			string idCampo = "";
			if (dataGridView1.CurrentRow.Cells[0].Value.ToString() != "" && dataGridView1.CurrentRow.Cells[0].Value.ToString() != null)
			{
				if (tiposEnc[0] != "int")
				{
					idCampo += "'" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
				}
				else
				{
					idCampo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
				}
			}
			else
			{
				if (tiposEnc[0] != "int")
				{
					idCampo += "'0'";
				}
				else
				{
					idCampo = "0";
				}
			}
			
			DataTable dt2 = logic.consultaLogica2(deta, camposEnc[0], idCampo);
			dataGridView2.DataSource = dt2;
		}


		public void ObtenerIdUsuario(string idUsuario)
		{
			this.idUsuario = idUsuario;
		}
		public void llenarCombo(string idUsuario)
		{
			this.idUsuario = idUsuario;
		}
		public void asignarComboConTabla(ComboBox combo, string tabla, string campo, int modo)
		{
			string[] items;
			string TablaOK = logic.TestTabla(tabla);
			if (TablaOK == "")
			{

				modoCampoCombo[noCombo]=modo;
				tablaCombo[noCombo] = tabla;
				campoCombo[noCombo] = campo;
					items = logic.items(tabla, campo);
				
				
					for (int i = 0; i < items.Length; i++)
					{
						if (items[i] != null)
						{
							if (items[i] != "")
							{
								combo.Items.Add(items[i]);
							}
						}

					}
				
				noCombo++;
			}
			else
			{
				DialogResult validacion = MessageBox.Show(TablaOK + ", o el campo seleccionado\n para el ComboBox es incorrecto", "Verificación de requisitos", MessageBoxButtons.OK);
				if (validacion == DialogResult.OK)
				{
					correcto = 1;
				}
			}


		}
		void limpiarLista(string cadena)
		{
			limpiarListaItems();
			int contadorCadena = 0;
			int contadorArray = 0;
			string palabra = "";
			while (contadorCadena < cadena.Length)
			{
				if (cadena[contadorCadena] != '|')
				{
					palabra += cadena[contadorCadena];
					contadorCadena++;
				}
				else
				{

					listaItems[contadorArray] = palabra;
					palabra = "";
					contadorArray++;
					contadorCadena++;
				}
			}
		}
		void limpiarListaItems()
		{
			for (int i = 0; i < listaItems.Length; i++)
			{
				listaItems[i] = "";
			}
		}


		public void asignarComboConLista(ComboBox combo, string lista)
		{
			
			limpiarLista(lista);
			for (int i = 0; i < listaItems.Length; i++)
			{
				if (listaItems[i] != null)
				{
					if (listaItems[i] != "")
					{
						combo.Items.Add(listaItems[i]);
					}
				}

			}
		}
		public void ObtenerDataGidView(DataGridView dtb)
		{
			dataGridView1 = dtb;
		}
		public void ObtenerDataGidView2(DataGridView dtb)
		{
			dataGridView2 = dtb;
		}
		public void ObtenerDataGidView3(DataGridView dtb)
		{
			dataGridView3 = dtb;
		}

		public void ObtenerCamposMantenimiento(List<Control> controles)
		{
			camposNav = controles;

		}
		public void ObtenerCamposMantenimiento2(List<Control> controles)
		{
			camposNav2 = controles;

		}
		private void insertTabla()
		{
			int i = 0;
			foreach (DataGridViewRow row in dataGridView3.Rows)
			{
				foreach (Control componente in camposNav2)
				{

					componente.Text = dataGridView3.CurrentRow.Cells[i].Value.ToString();
					i++;

				}
				i = 0;
				logic.nuevoQuery(crearInsert2());
				
					dataGridView3.Rows.RemoveAt(dataGridView3.CurrentCell.RowIndex);
				
			}
			i = 0;
			foreach (Control componente in camposNav2)
			{

				componente.Text = dataGridView3.CurrentRow.Cells[i].Value.ToString();
				i++;

			}
			
			logic.nuevoQuery(crearInsert2());

			dataGridView3.Rows.RemoveAt(dataGridView3.CurrentCell.RowIndex);

		}
		public void ObtenerIdAplicacion(string idAplicacion)
		{
			this.idAplicacion = idAplicacion;
		}

		private int numeroAlias()
		{
			int i = 0;
			foreach (string cad in aliasC)
			{
				if (cad != null && cad != "")
				{
					i++;
				}
			}
			return i;
		}
		private int numeroAlias2()
		{
			int i = 0;
			foreach (string cad in aliasC2)
			{
				if (cad != null && cad != "")
				{
					i++;
				}
			}
			return i;
		}

		public void asignarAyuda(string ayudar)
		{
			string AyudaOK = logic.TestTabla("ayuda");
			if (AyudaOK == "")
			{
				if (logic.contarRegAyuda(ayudar) > 0)
				{
					idyuda = ayudar;
					AsRuta = logic.MRuta(idyuda);
					AsIndice = logic.MIndice(idyuda);
					if (AsRuta == "" || AsIndice == "" || AsRuta == null || AsIndice == null)
					{
						DialogResult validacion = MessageBox.Show("La Ruta o índice de la ayuda está vacía", "Verificación de requisitos", MessageBoxButtons.OK);
						if (validacion == DialogResult.OK)
						{
							correcto = 1;
						}
					}
				}
				else
				{
					DialogResult validacion = MessageBox.Show("Por favor verifique el id de Ayuda asignado al form", "Verificación de requisitos", MessageBoxButtons.OK);
					if (validacion == DialogResult.OK)
					{
						correcto = 1;
					}
				}

			}
			else
			{
				DialogResult validacion = MessageBox.Show(AyudaOK + ", Por favor incluirla", "Verificación de requisitos", MessageBoxButtons.OK);
				if (validacion == DialogResult.OK)
				{
					correcto = 1;
				}
			}
		}

		public void asignarSalida(Form salida)
		{
			cerrar = salida;
		}

		public void asignarTabla(string table)
		{
			enca = table;
		}
		public void asignarTabla2(string table)
		{
			deta = table;
		}

		public void asignarAlias(string[] alias)
		{
			alias.CopyTo(aliasC, 0);
		}
		public void asignarAlias2(string[] alias)
		{
			alias.CopyTo(aliasC2, 0);
		}


		public void deshabilitarcampos_y_botones()
		{
			foreach (Control componente in camposNav)
			{

				componente.Enabled = false; //De esta menera bloqueamos todos los textbox por si solo quiere ver los registros


			}
			Btn_Modificar.Enabled = false;
			Btn_Eliminar.Enabled = false;
			Btn_Guardar.Enabled = false;
			Btn_Cancelar.Enabled = false;

		}

		public void habilitarcampos_y_botones()
		{
			foreach (Control componente in camposNav)
			{
				if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
				{
					componente.Enabled = true; //De esta menera bloqueamos todos los textbox por si solo quiere ver los registros

				}

			}
			Btn_Modificar.Enabled = true;
			Btn_Eliminar.Enabled = true;
			Btn_Guardar.Enabled = true;
			Btn_Cancelar.Enabled = true;

		}

		public void actualizardatagriew()
		{
			DataTable dt = logic.consultaLogica(enca);
			dataGridView1.DataSource = dt;
			int head = 0;
			while (head < logic.contarCampos(enca))
			{
				dataGridView1.Columns[head].HeaderText = aliasC[head];
				head++;
			}

			DataTable dt2 = logic.consultaLogica(deta);
			dataGridView2.DataSource = dt2;
			 head = 0;
			while (head < logic.contarCampos(deta))
			{
				dataGridView2.Columns[head].HeaderText = aliasC2[head];
				head++;
			}
		}

		string crearDelete()// crea el query de delete
		{
			//Cambiar el estadoPelicula por estado
			string query = "UPDATE " + enca + " SET estado=0";
			string whereQuery = " WHERE  ";
			int posCampo = 0;
			string campos = "";

			foreach (Control componente in camposNav)
			{

				if (posCampo == 0)
				{
					switch (tipoCampo[posCampo])
					{
						case "Text":
							
							whereQuery += NomCampo[posCampo] + " = '" + componente.Text;
							break;
						case "Num":
							whereQuery += NomCampo[posCampo] + " = " + componente.Text;
							break;
					}

				}
				posCampo++;


			}
			campos = campos.TrimEnd(' ');
			campos = campos.TrimEnd(',');
			//query += campos + whereQuery + ";";
			query += whereQuery + ";";
			Console.Write(query);
			sn.insertarBitacora(idUsuario, "Se eliminó un registro", enca);
			return query;
		}

	

		public void llenarCampos()
		{
			string[] Campos = logic.campos(enca);
			string[] Tipos = logic.tipos(enca);
			int i = 0;
			NomCampo = Campos;
			int fin = Campos.Length;
			while (i < fin)
			{



				switch (Tipos[i])
				{
					case "int":
						tipoCampo[i] = "Num";
						break;
					case "varchar":
						tipoCampo[i] = "Text";
						break;
					case "date":
						tipoCampo[i] = "Text";
						break;
					case "datetime":
						tipoCampo[i] = "Text";
						break;
					case "text":
						tipoCampo[i] = "Text";
						break;
					case "time":
						tipoCampo[i] = "Text";
						break;

					case "float":
						tipoCampo[i] = "Text";
						break;

					case "decimal":
						tipoCampo[i] = "Text";
						break;

					case "double":
						tipoCampo[i] = "Text";
						break;

					case "tinyint":
						tipoCampo[i] = "Num";
						break;

					default:

						if (Tipos[i] != null && Tipos[i] != "")
						{
							DialogResult validacion = MessageBox.Show("La enca " + enca + " posee un campo " + Tipos[i] + ", este tipo de dato no es reconocido por el navegador\n Solucione este problema...", "Verificación de requisitos", MessageBoxButtons.OK);
							if (validacion == DialogResult.OK)
							{
								Application.Exit();
							}
						}

						break;
				}

				i++;
			}
			}

		public void llenarCampos2()
		{
			string[] Campos = logic.campos(deta);
			string[] Tipos = logic.tipos(deta);
			int i = 0;
			NomCampo2 = Campos;
			int fin = Campos.Length;
			while (i < fin)
			{



				switch (Tipos[i])
				{
					case "int":
						tipoCampo2[i] = "Num";
						break;
					case "varchar":
						tipoCampo2[i] = "Text";
						break;
					case "date":
						tipoCampo2[i] = "Text";
						break;
					case "datetime":
						tipoCampo2[i] = "Text";
						break;
					case "text":
						tipoCampo2[i] = "Text";
						break;
					case "time":
						tipoCampo2[i] = "Text";
						break;

					case "float":
						tipoCampo2[i] = "Text";
						break;

					case "decimal":
						tipoCampo2[i] = "Text";
						break;

					case "double":
						tipoCampo2[i] = "Text";
						break;

					case "tinyint":
						tipoCampo2[i] = "Num";
						break;

					default:

						if (Tipos[i] != null && Tipos[i] != "")
						{
							DialogResult validacion = MessageBox.Show("La enca " + enca + " posee un campo " + Tipos[i] + ", este tipo de dato no es reconocido por el navegador\n Solucione este problema...", "Verificación de requisitos", MessageBoxButtons.OK);
							if (validacion == DialogResult.OK)
							{
								Application.Exit();
							}
						}

						break;
				}

				i++;
			}
		}
		string crearInsert()// crea el query de insert
        {
            string query = "INSERT INTO " + enca + " VALUES (";

            int posCampo = 0;
            string campos = "";
            foreach (Control componente in camposNav)
            {
                

                    switch (tipoCampo[posCampo])
                    {
                        case "Text":
						if (componente is ComboBox)
						{

							if (modoCampoCombo[combo] == 1)
							{
								campos += "'" + logic.llaveCampolo(tablaCombo[combo], campoCombo[combo], componente.Text) + "' , ";
							}
							else
							{
								campos += "'" + componente.Text + "' , ";
							}

							combo++;
						}
						else
						{
							campos += "'" + componente.Text + "' , ";
						}

						break;
                        case "Num":
						if (componente is ComboBox)
						{

							if (modoCampoCombo[combo] == 1)
							{
								campos += logic.llaveCampolo(tablaCombo[combo], campoCombo[	combo], componente.Text) + " , ";
							}
							else
							{
								campos += componente.Text + " , ";
							}

							combo++;
						}
						else
						{
							campos += componente.Text + " , ";
						}
						break;
                    }
                    posCampo++;

                
                if (componente is Button)
                {
                    switch (tipoCampo[posCampo])
                    {
                        case "Num":
                            campos += "'" + estado + "' , ";
                           // campos += "' 0 ' , ";
                            break;
                          
                    }
                    posCampo++;
                }

            }
            campos = campos.TrimEnd(' ');
            campos = campos.TrimEnd(',');
            query += campos + ");";
            sn.insertarBitacora(idUsuario, "Se creó un nuevo registro", enca);
            return query;
        }

		string crearInsert2()// crea el query de insert
		{
			string query = "INSERT INTO " + deta + " VALUES (";
			int comboDet = 0;
			int posCampo = 0;
		
			string campos = "";
			foreach (Control componente in camposNav2)
			{
				switch (tipoCampo2[posCampo])
				{
					case "Text":
						if (componente is ComboBox)
						{
							if (modoCampoCombo[combo] == 1)
							{
								campos += "'" + logic.llaveCampolo(tablaCombo[combo], campoCombo[combo], componente.Text) + "' , ";
							}
							else
							{
								campos += "'" + componente.Text + "' , ";
							}

							combo++;
							comboDet++;
						}
						else
						{
							campos += "'" + componente.Text + "' , ";
						}

						break;
					case "Num":
						if (componente is ComboBox)
						{

							if (modoCampoCombo[combo] == 1)
							{
								campos += logic.llaveCampolo(tablaCombo[combo], campoCombo[combo], componente.Text) + " , ";
							}
							else
							{
								campos += componente.Text + " , ";
							}

							combo++;
							comboDet++;
						}
						else
						{
							
							campos += componente.Text + " , ";
						}
						break;
				}
				posCampo++;


				if (componente is Button)
				{
					switch (tipoCampo[posCampo])
					{
						case "Num":
							campos += "'" + estado + "' , ";
							// campos += "' 0 ' , ";
							break;

					}
					posCampo++;
				}

			}
			campos = campos.TrimEnd(' ');
			campos = campos.TrimEnd(',');
			query += campos + ");";
			sn.insertarBitacora(idUsuario, "Se creó un nuevo registro", deta);
			combo = combo - comboDet;
			return query;
		}


		string crearUpdate()// crea el query de update
        {
            string query = "UPDATE " + enca + " SET ";
            string whereQuery = " WHERE  ";
            int posCampo = 0;
            string campos = "";
            foreach (Control componente in camposNav)
            {
                

                    if (posCampo > 0)
                    {
                        switch (tipoCampo[posCampo])
                        {

                            case "Text":
							
							campos +=  NomCampo[posCampo]+ " = '" + componente.Text + "' , ";
                                break;
                            case "Num":
							
							campos += NomCampo[posCampo] + " = " + componente.Text + " , ";
                                break;
                        }
                    }
                    else
                    {
                        switch (tipoCampo[posCampo])
                        {
                            case "Text":
                                whereQuery += NomCampo[posCampo] + " = '" + componente.Text;
                                break;
                            case "Num":
                                whereQuery += NomCampo[posCampo] + " = " + componente.Text;
                                break;
                        }

                    }
                    posCampo++;

                

            }
            campos = campos.TrimEnd(' ');
            campos = campos.TrimEnd(',');
            query += campos + whereQuery + ";";
            //contenido.Text = query;
            sn.insertarBitacora(idUsuario, "Se actualizó un registro", enca);
            return query;
        }

        public void guardadoforsozo()
        {
            logic.nuevoQuery(crearInsert());
            foreach (Control componente in camposNav)
            {
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    componente.Enabled = true;
                    componente.Text = "";

                }

            }
            actualizardatagriew();
        }

        public void habilitarallbotones()//habilita todos los botnes
        {
            Btn_Guardar.Enabled = true;
            Btn_Ingresar.Enabled = true;
            Btn_Modificar.Enabled = true;
            Btn_Cancelar.Enabled = false;
            Btn_Eliminar.Enabled = true;

        }




        //-----------------------------------------------Funcionalidad de Botones-----------------------------------------------//

        private void Btn_Ingresar_Click(object sender, EventArgs e)
        {
            string[] Tipos = logic.tipos(enca);

            //codigo para aplicar el autoincrementable             
            string[] Extras = logic.extras(enca);
            bool tipoInt = false;
            bool ExtraAI = false;
			string auxId = "" ;
			int auxLastId = 0;
			if (Tipos[0] == "int")
            {
                tipoInt = true;
				if (Extras[0] == "auto_increment")
				{
					ExtraAI = true;
					auxId = (logic.lastID(enca));
					auxLastId = Int32.Parse(auxId);
				}
			}

            activar = 2;
            habilitarcampos_y_botones();        

            foreach (Control componente in camposNav)
            {
                if (componente is TextBox && tipoInt && ExtraAI)
                {
                    //MessageBox.Show("El ID nuevo será: " + (auxLastId + 1));
                    auxLastId += 1;
                    componente.Text = auxLastId.ToString();
                    componente.Enabled = false;
                    tipoInt = false;
                    ExtraAI = false;
                }
                else if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    componente.Enabled = true;
                    componente.Text = "";
                }
                Btn_Ingresar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Eliminar.Enabled = false;
                Btn_Cancelar.Enabled = true;
            }

            //habilitar y deshabilitar según Usuario => Randy
            botonesYPermisos();
            Btn_Ingresar.Enabled = false;
            Btn_Guardar.Enabled = true;
            Btn_Modificar.Enabled = false;
            Btn_Eliminar.Enabled = false;           
            Btn_Cancelar.Enabled = true;
            Btn_Consultar.Enabled = false;
            Btn_Imprimir.Enabled = false;
            Btn_Refrescar.Enabled = false;            

        }

        private void Btn_Modificar_Click(object sender, EventArgs e)
        {
            habilitarcampos_y_botones();
            activar = 1;
            int i = 0;
            foreach (Control componente in camposNav)
            {
                 if (i==0)
                    {
                        componente.Enabled = false;
                    }
                    componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                    i++;
                             
            }

                        
            //habilitar y deshabilitar según Usuario
            botonesYPermisos();

            Btn_Ingresar.Enabled = false;
            Btn_Eliminar.Enabled = false;
            Btn_Modificar.Enabled = false;
            Btn_Consultar.Enabled = false;
            Btn_Imprimir.Enabled = false;
            Btn_Refrescar.Enabled = false;
        }

        private void Btn_Cancelar_Click(object sender, EventArgs e)
        {
            Btn_Modificar.Enabled = true;
            Btn_Guardar.Enabled = false;
            Btn_Cancelar.Enabled = false;
            Btn_Ingresar.Enabled = true;
            Btn_Eliminar.Enabled = true;
            Btn_Refrescar.Enabled = true;

            actualizardatagriew();            
            if (logic.TestRegistros(enca)>0)
            {
                int i = 0;
                foreach (Control componente in camposNav)
                {
                    
                        componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        componente.Enabled = false;
                        i++;
                   

                }
            }

            //habilitar y deshabilitar según Usuario
            botonesYPermisos();            
        }

        private void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            if (presionado == false)
            {
                Btn_Guardar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Eliminar.Enabled = true;
                Btn_Cancelar.Enabled = true;
                Btn_Ingresar.Enabled = false;
                presionado = true;
            }
            else
            {

                DialogResult Respuestamodieli;
                Respuestamodieli = MessageBox.Show("Desea eliminar el registro?", "Desea realizar la siguiente operación en el formulario ?", MessageBoxButtons.YesNo);
                if (Respuestamodieli == DialogResult.Yes)
                {
                    logic.nuevoQuery(crearDelete());
                    actualizardatagriew();
                    Btn_Modificar.Enabled = true;
                    Btn_Guardar.Enabled = false;
                    Btn_Cancelar.Enabled = true;
                    Btn_Eliminar.Enabled = true;
                    Btn_Ingresar.Enabled = true;
                    presionado = false;

                }
                else if (Respuestamodieli == DialogResult.No)
                {
                    Btn_Guardar.Enabled = false;
                    Btn_Modificar.Enabled = false;
                    Btn_Eliminar.Enabled = true;
                    Btn_Cancelar.Enabled = true;
                    Btn_Ingresar.Enabled = false;
                    presionado = true;

                }
                // presionado = false;
            }
            //habilitar y deshabilitar según Usuario
            botonesYPermisos();
            presionado = true;            
        }

        private void Btn_Consultar_Click(object sender, EventArgs e)
        {
            //DLL DE CONSULTAS

            //habilitar y deshabilitar según Usuario
            botonesYPermisos();
        }

        private void Btn_Imprimir_Click(object sender, EventArgs e)
        {
            //DLL DE IMPRESION, FORATO DE REPORTES.

            //habilitar y deshabilitar según Usuario
            botonesYPermisos();
        }

        private void Btn_Refrescar_Click(object sender, EventArgs e)
        {           
            actualizardatagriew();            

            //habilitar y deshabilitar según Usuario
            botonesYPermisos();            
        }

        private void Btn_Anterior_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] Campos = logic.campos(enca);

            int fila = dataGridView1.SelectedRows[0].Index;
            if (fila > 0)
            {
                dataGridView1.Rows[fila - 1].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[fila - 1].Cells[0];
                
                    foreach (Control componente in camposNav)
                    {
                        
                      
                           componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            i++;
                        
                   
                }                
            }            
        }

        private void Btn_Siguiente_Click(object sender, EventArgs e)
        {
            int i = 0;
                string[] Campos = logic.campos(enca);

                int fila = dataGridView1.SelectedRows[0].Index;
                if (fila < dataGridView1.Rows.Count - 1)
                {
                    dataGridView1.Rows[fila + 1].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[fila + 1].Cells[0];
                    
                        foreach (Control componente in camposNav)
                        {
                           
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                                i++;
                            
                   
					    }
                      
                 }
        }

        private void Btn_FlechaFin_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0];

            int i = 0;
            string[] Campos = logic.campos(enca);

            int fila = dataGridView1.SelectedRows[0].Index;
            if (fila < dataGridView1.Rows.Count - 1)
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0];
                
                    foreach (Control componente in camposNav)
                    {
                       
                            componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            i++;
                    }
                  
                
                
            }
        }

        private void Btn_FlechaInicio_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows[0].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];

            int i = 0;
            string[] Campos = logic.campos(enca);

            int fila = dataGridView1.SelectedRows[0].Index;
            if (fila < dataGridView1.Rows.Count - 1)
            {
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
               

                    foreach (Control componente in camposNav)
                    {
                        if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                        {
                            componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            i++;
                        }
                    if (componente is Button)
                    {
                        string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        if (var1 == "0")
                        {
                            componente.Text = "Desactivado";
                            componente.BackColor = Color.Red;
                        }
                        if (var1 == "1")
                        {
                            componente.Text = "Activado";
                            componente.BackColor = Color.Green;
                        }
                    }

                }

                
            }

        }

        private void Btn_Ayuda_Click(object sender, EventArgs e)
        {                    
            Help.ShowHelp(this, AsRuta, AsIndice);//Abre el menu de ayuda HTML            
        }

        private void Btn_Salir_Click(object sender, EventArgs e)
        {
            if (Btn_Guardar.Enabled == true && Btn_Cancelar.Enabled == true && Btn_Eliminar.Enabled == false && Btn_Modificar.Enabled == false && Btn_Ingresar.Enabled == false && Btn_Eliminar.Enabled == false)
                foreach (Control componente in Controls)
                {

                    if (componente.Text != "" && componente is TextBox)
                    {
                        //Opcion cuando esta guardando y queiere salir sin finalizar //
                        DialogResult Respuestagua;
                        Respuestagua = MessageBox.Show("Se ha detectado una operacion de guardado ¿Desea Guardar los datos? ", "Usted se enuentra abandonando el formulario ", MessageBoxButtons.YesNoCancel);
                        if (Respuestagua == DialogResult.Yes)
                        {
                            guardadoforsozo();
                            cerrar.Visible = false;
                        }
                        else if (Respuestagua == DialogResult.No)
                        {
                            cerrar.Visible = false;
                        }
                        else if (Respuestagua == DialogResult.Cancel)
                        {
                            return;
                        }

                        //------------------------------------------------------------------------------------------------------//
                    }
                }


            //Opcion cuando esta #modificando# o eliminando y queiere salir sin finalizar //
            if (Btn_Modificar.Enabled == true && Btn_Guardar.Enabled == true && Btn_Cancelar.Enabled == true && Btn_Ingresar.Enabled == false)
            {

                foreach (Control componente in Controls)
                {

                    if (componente.Text != "" && componente is TextBox)
                    {

                        DialogResult Respuestamodieli;
                        Respuestamodieli = MessageBox.Show("Se ha detectado una operacion de Modificado ¿Desea regresar? ", "Usted se enuentra abandonando el formulario ", MessageBoxButtons.YesNoCancel);
                        if (Respuestamodieli == DialogResult.Yes)
                        {
                            return;
                        }
                        else if (Respuestamodieli == DialogResult.No)
                        {
                            cerrar.Visible = false;
                        }
                        else if (Respuestamodieli == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }

            //------------------------------------------------------------------------------------------------------//
            //Opcion cuando esta modificando o #eliminando# y queiere salir sin finalizar //
            if (Btn_Eliminar.Enabled == true && Btn_Cancelar.Enabled == true && Btn_Modificar.Enabled == false && Btn_Guardar.Enabled == false && Btn_Ingresar.Enabled == false)
            {
                foreach (Control componente in Controls)
                {
                    if (componente.Text != "" && componente is TextBox)
                    {
                        DialogResult Respuestamodieli;
                        Respuestamodieli = MessageBox.Show("Se ha detectado una operacion de Eliminado ¿Desea regresar? ", "Usted se enuentra abandonando el formulario ", MessageBoxButtons.YesNoCancel);
                        if (Respuestamodieli == DialogResult.Yes)
                        {
                            return;
                        }
                        else if (Respuestamodieli == DialogResult.No)
                        {
                            cerrar.Visible = false;
                        }
                        else if (Respuestamodieli == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }




            cerrar.Visible = false;
            //---------------------------------------------------------------------------------//

        }

        private void Btn_Guardar_Click(object sender, EventArgs e)
        {
            bool lleno =true;
            foreach (Control componente in camposNav)
            {
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    if (componente.Text=="")
                    {
                        lleno = false;
                    }
                }
            }
            if (lleno==true)
            {
                switch (activar)
                {
                    case 1:
                        logic.nuevoQuery(crearUpdate());
                        break;
                    case 2:
						if (dataGridView3.Rows.Count > 1)
						{
							logic.nuevoQuery(crearInsert());
							insertTabla();
						}
						else
						{
							MessageBox.Show("Debe Ingresar almenos 1 Detalle");
						}
						
						Btn_Anterior.Enabled = true;
                        Btn_Siguiente.Enabled = true;
                        Btn_FlechaInicio.Enabled = true;
                        Btn_FlechaFin.Enabled = true;
                        Btn_Modificar.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Por favor llene todos los campos...");
            }
           
            actualizardatagriew();            
            if (logic.TestRegistros(enca)>0)
            {
                int i = 0;
                foreach (Control componente in camposNav)
                {
                   
                        componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        i++;
                    
                }
            }
           
            deshabilitarcampos_y_botones();
           
            Btn_Guardar.Enabled = false;
            Btn_Eliminar.Enabled = true;
            Btn_Cancelar.Enabled = false;
            Btn_Modificar.Enabled = true;
            Btn_Ingresar.Enabled = true;
            Btn_Refrescar.Enabled = true;            

            //habilitar y deshabilitar según Usuario
            botonesYPermisos();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = 0;
            foreach (Control componente in camposNav)
            {
                
                    componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                    i++;

            }
        }

       

        public void botonesYPermisos()
        {
            try
            {
                if (logic.TestRegistros(enca) <= 0)
                {                    
                    Btn_Ingresar.Enabled = false;
                    Btn_Modificar.Enabled = false;
                    Btn_Guardar.Enabled = false;
                    Btn_Cancelar.Enabled = false;
                    Btn_Eliminar.Enabled = false;
                    Btn_Consultar.Enabled = false;
                    Btn_Imprimir.Enabled = false;
                    Btn_Refrescar.Enabled = false;
                    Btn_FlechaInicio.Enabled = false;
                    Btn_Anterior.Enabled = false;
                    Btn_Siguiente.Enabled = false;                    
                    Btn_FlechaFin.Enabled = false;
                    MessageBox.Show("Tabla Vacía! Debe ingresar un registro!");
                    try
                    {
                        sentencia sent = new sentencia();
                        if (sent.consultarPermisos(idUsuario, idAplicacion, 1) == true)
                        {
                            MessageBox.Show("Tabla Vacía! SI puede INGRESAR");
                            Btn_Ingresar.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Tabla Vacía! NO puede INGRESAR");
                        }
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("Estamos en Tabla Vacía! Determinanos si el usuario Activo puede ingresar! ERROR: " + exx);
                    }
                }
                else
                {
                    //validamos con TRY CATCH por si llegará a existir un problema 
                    try
                    {
                        sentencia sen = new sentencia();
                        string[] permisosText = { "INGRESAR", "CONSULTAR", "MODIFICAR", "ELIMINAR", "IMPRIMIR" };
                        for (int i = 1; i < 6; i++)
                        {
                            if (sen.consultarPermisos(idUsuario, idAplicacion, i) == true)
                            {
                                //mostramos un mensaje para indicar que si tiene permiso
                                //MessageBox.Show("Tiene permiso para " + permisosText[i - 1]);
                                //bloqueamos botones
                                switch (permisosText[i - 1])
                                {
                                    case "INGRESAR":
                                        Btn_Ingresar.Enabled = true; break;
                                    case "CONSULTAR":
                                        Btn_Consultar.Enabled = true; break;
                                    case "MODIFICAR":
                                        Btn_Modificar.Enabled = true; break;
                                    case "ELIMINAR":
                                        Btn_Eliminar.Enabled = true; break;
                                    case "IMPRIMIR":
                                        Btn_Imprimir.Enabled = true; break;
                                    default:
                                        MessageBox.Show("Entro al case default! TIENE PERMISO! Por favor hablar con Administrador!"); break;
                                }
                            }
                            else
                            {
                                //MessageBox.Show("No tiene permiso para " + permisosText[i - 1]);
                                switch (permisosText[i - 1])
                                {
                                    case "INGRESAR":
                                        Btn_Ingresar.Enabled = false; break;
                                    case "CONSULTAR":
                                        Btn_Consultar.Enabled = false; break;
                                    case "MODIFICAR":
                                        Btn_Modificar.Enabled = false; break;
                                    case "ELIMINAR":
                                        Btn_Eliminar.Enabled = false; break;
                                    case "IMPRIMIR":
                                        Btn_Imprimir.Enabled = false; break;
                                    default:
                                        MessageBox.Show("Entro al case default! NO TIENE PERMISO! Por favor hablar con Administrador!"); break;
                                }
                            }
                            /* 1 ingresar - 2 consultar - 3 modificar - 4 eliminar - 5 imprimir */
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Surgió el siguiente problema: " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error General en Botones y Permisos. ERROR: " + ex);
            }
        }

        public void botonesYPermisosInicial(string userActivo, string appActivo)
        {
            try
            {
                if (logic.TestRegistros(enca) <= 0)
                {
                    Btn_Ingresar.Enabled = false;
                    Btn_Modificar.Enabled = false;
                    Btn_Guardar.Enabled = false;
                    Btn_Cancelar.Enabled = false;
                    Btn_Eliminar.Enabled = false;
                    Btn_Consultar.Enabled = false;
                    Btn_Imprimir.Enabled = false;
                    Btn_Refrescar.Enabled = false;
                    Btn_FlechaInicio.Enabled = false;
                    Btn_Anterior.Enabled = false;
                    Btn_Siguiente.Enabled = false;
                    Btn_FlechaFin.Enabled = false;
                    MessageBox.Show("Tabla Vacía! Debe ingresar un registro!");
                    try
                    {
                        sentencia sent = new sentencia();
                        if (sent.consultarPermisos(userActivo, appActivo, 1) == true)
                        {
                            MessageBox.Show("Tabla Vacía! SI puede INGRESAR");
                            Btn_Ingresar.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Tabla Vacía! NO puede INGRESAR");
                        }
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("Estamos en Tabla Vacía! Determinanos si el usuario Activo puede ingresar! ERROR: " + exx);
                    }
                }
                else
                {
                    //validamos con TRY CATCH por si llegará a existir un problema 
                    try
                    {
                        sentencia sen = new sentencia();
                        string[] permisosText = { "INGRESAR", "CONSULTAR", "MODIFICAR", "ELIMINAR", "IMPRIMIR" };
                        for (int i = 1; i < 6; i++)
                        {
                            if (sen.consultarPermisos(userActivo, appActivo, i) == true)
                            {
                                //mostramos un mensaje para indicar que si tiene permiso
                                //MessageBox.Show("Tiene permiso para " + permisosText[i - 1]);
                                //bloqueamos botones
                                switch (permisosText[i - 1])
                                {
                                    case "INGRESAR":
                                        Btn_Ingresar.Enabled = true; break;
                                    case "CONSULTAR":
                                        Btn_Consultar.Enabled = true; break;
                                    case "MODIFICAR":
                                        Btn_Modificar.Enabled = true; break;
                                    case "ELIMINAR":
                                        Btn_Eliminar.Enabled = true; break;
                                    case "IMPRIMIR":
                                        Btn_Imprimir.Enabled = true; break;
                                    default:
                                        MessageBox.Show("Entro al case default! TIENE PERMISO! Por favor hablar con Administrador!"); break;
                                }
                            }
                            else
                            {
                                //MessageBox.Show("No tiene permiso para " + permisosText[i - 1]);
                                switch (permisosText[i - 1])
                                {
                                    case "INGRESAR":
                                        Btn_Ingresar.Enabled = false; break;
                                    case "CONSULTAR":
                                        Btn_Consultar.Enabled = false; break;
                                    case "MODIFICAR":
                                        Btn_Modificar.Enabled = false; break;
                                    case "ELIMINAR":
                                        Btn_Eliminar.Enabled = false; break;
                                    case "IMPRIMIR":
                                        Btn_Imprimir.Enabled = false; break;
                                    default:
                                        MessageBox.Show("Entro al case default! NO TIENE PERMISO! Por favor hablar con Administrador!"); break;
                                }
                            }
                            /* 1 ingresar - 2 consultar - 3 modificar - 4 eliminar - 5 imprimir */
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Surgió el siguiente problema: " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error General en Botones y Permisos Inicial. ERROR: " + ex);
            }

        }

        private void Btn_Ayuda_Click_1(object sender, EventArgs e)
        {
            Help.ShowHelp(this, AsRuta, AsIndice);//Abre el menu de ayuda HTML     
        }

        private void Btn_MasAyuda_Click(object sender, EventArgs e)
        {
            string AyudaOK = logic.TestTabla("ayuda");
            if (AyudaOK == "")
            {
                Ayudas nuevo = new Ayudas();
                nuevo.Show();
            }
            else
            {
                DialogResult validacion = MessageBox.Show(AyudaOK +" \n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
                if (validacion == DialogResult.OK)
                {
                    
                }
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LblTabla_Click(object sender, EventArgs e)
        {

        }

		private void DataGridView1_SelectionChanged(object sender, EventArgs e)
		{

		}

		private void DataGridView1_CurrentCellChanged(object sender, EventArgs e)
		{

		}

		private void DataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void DataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void DataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void DataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
		{

		}
	}
}
