using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace ProyectConteo {
    public partial class frmFacturaruta : Form {
        int vConsecutivoCompania = 0;
        private System.Windows.Forms.ListViewItem[] itemsTodos;
        private System.Windows.Forms.ListViewItem[] itemsTodosLinea;

        public frmFacturaruta() {
            InitializeComponent();
        }

        private void InicializaList() {

            string queryString;
            inicializaColumnas(listEmpresa,true );

            queryString = " SELECT dbo.Vendedor.Codigo, dbo.Vendedor.Nombre, dbo.Vendedor.ConsecutivoCompania ";
            queryString = queryString + " FROM dbo.Vendedor INNER JOIN ";
            queryString = queryString + " dbo.COMPANIA ON dbo.Vendedor.ConsecutivoCompania = dbo.COMPANIA.ConsecutivoCompania ";
            queryString = queryString + " WHERE dbo.COMPANIA.Nombre ='DISTRIBUIDORA PASARES FBR, C.A.' ";

            using (SqlConnection conexion = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient"))) {
                SqlCommand command = new SqlCommand(queryString, conexion);
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows) {
                    while (reader.Read()) {
                        ListViewItem List;
                        List = listEmpresa.Items.Add(reader["Nombre"].ToString());

                        List.SubItems.Add(reader["consecutivoCompania"].ToString());
                        List.UseItemStyleForSubItems = false;
                    }
                    reader.NextResult();
                }
            }
            txtRuta.TextChanged += new EventHandler(txtRuta_TextChanged);
            itemsTodos = new ListViewItem[listEmpresa.Items.Count];
            this.listEmpresa.Items.CopyTo(itemsTodos, 0);
           


        }
        private void txtRuta_TextChanged(object sender, EventArgs e) {
           

            listEmpresa.Items.Clear();  //Borra el ListView
            List<ListViewItem> itemsAUX = new List<ListViewItem>();  //Lista Auxiliar para el filtrado
            //Recorre todos los items
            foreach (ListViewItem lvi in itemsTodos) {
                //Filtra los items que comienzan con el valor de textBox1.Text
                if (lvi.Text.StartsWith(txtRuta.Text))
                    itemsAUX.Add(lvi); //Agregar el Item encontrado.
            }
            listEmpresa.Items.AddRange(itemsAUX.ToArray()); //Recargar el ListView

        }
        
        
        private void InicializaListLinea(int Consecutivo) {

            string queryString;
            inicializaColumnas(listLinea,false );
            
            queryString = " SELECT [ConsecutivoCompania] ,[Nombre] ";
            queryString = queryString + " FROM [SAWDB].[dbo].[LineaDeProducto] ";
            queryString = queryString + " WHERE ConsecutivoCompania = " + Consecutivo.ToString();
            using (SqlConnection conexion = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient"))) {
                SqlCommand command = new SqlCommand(queryString, conexion);
                conexion.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows) {
                    while (reader.Read()) {
                        ListViewItem List;
                        List = listLinea.Items.Add(reader["Nombre"].ToString());

                        List.SubItems.Add(reader["consecutivoCompania"].ToString());
                        List.UseItemStyleForSubItems = false;
                    }
                    reader.NextResult();
                }
            }
            // BuscarArticulo(2);
            //BuscarClientes(2);

            txtLinea.TextChanged += new EventHandler(txtLinea_TextChanged);
            itemsTodosLinea = new ListViewItem[listLinea.Items.Count];
            this.listLinea.Items.CopyTo(itemsTodosLinea, 0);
           

           

        }

        private void txtLinea_TextChanged(object sender, EventArgs e) {


            listLinea.Items.Clear();  //Borra el ListView
            List<ListViewItem> itemsAUX = new List<ListViewItem>();  //Lista Auxiliar para el filtrado
            //Recorre todos los items
            if (itemsTodosLinea != null) {
                foreach (ListViewItem lvi in itemsTodosLinea) {
                    //Filtra los items que comienzan con el valor de textBox1.Text
                    if (lvi.Text.StartsWith(txtLinea.Text))
                        itemsAUX.Add(lvi); //Agregar el Item encontrado.
                }
                listLinea.Items.AddRange(itemsAUX.ToArray()); //Recargar el ListView
            }

        }
        

        static string GetConnectionStringByProvider(string providerName) {
            string returnValue = null;

            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null) {
                foreach (ConnectionStringSettings cs in settings) {
                    if (cs.ProviderName == providerName)
                        returnValue = cs.ToString();
                    //break;
                }
            }
            return returnValue;
        }

        private void inicializaColumnas(ListView Lista1, bool  ruta) {

            Lista1.Clear();
            Lista1.MultiSelect = true;
            Lista1.View = View.Details;
            Lista1.GridLines = true;
             if (ruta) {
                Lista1.Columns.Add("Ruta de Venta", 500, HorizontalAlignment.Left);
            
            } else {
                Lista1.Columns.Add("Línea del producto", 500, HorizontalAlignment.Left);
             
            }
            
            Lista1.Columns.Add("Consecutivo", 0, HorizontalAlignment.Left);

        }
        private void listEmpresa_SelectedIndexChanged(object sender, EventArgs e) {
            int consecutivo = 0;
            if (listEmpresa.SelectedItems.Count > 0) {
                ListViewItem listItem = listEmpresa.SelectedItems[0];

                consecutivo = Convert.ToInt16(listItem.SubItems[1].Text);
                vConsecutivoCompania = Convert.ToInt16(listItem.SubItems[1].Text);
                InicializaListLinea(vConsecutivoCompania);
                txtRuta.Text = listItem.SubItems[0].Text;

            }
        }
        
        //listLinea
        private void listLinea_SelectedIndexChanged(object sender, EventArgs e) {
            if (listLinea.SelectedItems.Count > 0) {
                ListViewItem listItem = listLinea.SelectedItems[0];
                txtLinea.Text = listItem.SubItems[0].Text;
            }
        }


        private void frmFacturaruta_Load(object sender, EventArgs e) {
            InicializaList();
            this.Sp_ProcesoConteoPorLineaCategoriaTableAdapter.Fill(this.Dataconteo.Sp_ProcesoConteoPorLineaCategoria, 8, txtRuta.Text, txtLinea.Text, dateInicio.Text, datefin.Text);
           

            this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e) {
            
            //this.Sp_ProcesoConteoPorLineaCategoriaTableAdapter.Fill(this.Dataconteo.Sp_ProcesoConteoPorLineaCategoria, 8, txtRuta.Text, txtLinea.Text, dateInicio.Text, datefin.Text);
            this.Sp_ProcesoConteoPorLineaCategoriaTableAdapter.Fill(this.Dataconteo.Sp_ProcesoConteoPorLineaCategoria, 8, txtRuta.Text, txtLinea.Text, dateInicio.Text, datefin.Text);
           
            this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();

        }

        private void button2_Click(object sender, EventArgs e) {
            InicializaList();
        }

        private void reportViewer1_Load(object sender, EventArgs e) {

        }

        private void button3_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            if (radioButton1.Checked == true) {
                reportViewer1.Visible = true;
                reportViewer2.Visible = false;


            } else {
                reportViewer1.Visible = false;
                reportViewer2.Visible = true;

            
            }  
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            if (radioButton2.Checked == true) {
                reportViewer2.Visible = true;
                reportViewer1.Visible = false;


            } else {
                reportViewer2.Visible = false;
                reportViewer1.Visible = true;

            
            }  
        }


    }
}
