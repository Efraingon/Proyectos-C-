using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace DistribucionCosto
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            
            dtpFechaIni.Format = DateTimePickerFormat.Custom;
            dtpFechaIni.CustomFormat = "dd/MM/yyyy";

            dtpFechaFin.Format = DateTimePickerFormat.Custom;
            dtpFechaFin.CustomFormat = "dd/MM/yyyy";
           
            


            InicializaList();
        }
        private void InicializaList()
        {

            string queryString;
            inicializaColumnas(listEmpresa);
            queryString = "select * from dbo.compania";

            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                                    "AplicationConnectionString")))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListViewItem List;
                        List = listEmpresa.Items.Add(reader["Nombre"].ToString());
                        List.SubItems.Add(reader["consecutivoCompania"].ToString());
                        List.UseItemStyleForSubItems = false;
                    }
                    reader.NextResult();
                }
            }

        }
        private void inicializaColumnas(ListView Lista1)
        {
            try
            {
                Lista1.Clear();
                Lista1.MultiSelect = true;
                Lista1.View = View.Details;
                Lista1.GridLines = true;
                //Lista1.o = true;
                Lista1.Columns.Add("Nombre", 500, HorizontalAlignment.Left);
                Lista1.Columns.Add("Consecutivo", 0, HorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
            }
        }
        private void inicializaColumnasVendedor(ListView Lista1)
        {
            try
            {
                Lista1.Clear();
                Lista1.MultiSelect = true;
                Lista1.View = View.Details;
                Lista1.GridLines = true;
                //Lista1.o = true;
                Lista1.Columns.Add("Nombre", 500, HorizontalAlignment.Left);
                Lista1.Columns.Add("Codigo", 0, HorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
            }
        }
        static string GetConnectionStringByProvider(string providerName, string ConexionName)
        {
            string returnValue = null;

            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                    {
                        if (cs.Name == ConexionName)
                        {
                            returnValue = cs.ToString();
                            break;
                        }

                    }


                }
            }
            return returnValue;
        }

        private void listEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listEmpresa.SelectedItems.Count == 1)
            {

                string ntcustomerId = listEmpresa.SelectedItems[0].Text;
                string CodigoEmpresa = listEmpresa.SelectedItems[0].SubItems[1].Text;
                txtCodigoEmpresa.Text = CodigoEmpresa;
                txtNombreEmpresa.Text = listEmpresa.SelectedItems[0].SubItems[0].Text;
                txtCodigoVendedor.Text = "";
                txtNombreVendedor.Text = "";
                

                string queryString;
                inicializaColumnasVendedor(listVendedor);
                queryString = "SELECT dbo.COMPANIA.Nombre AS NombreCompania, dbo.Vendedor.Nombre AS NombreVendedor, dbo.Vendedor.Codigo";
                queryString = queryString + " FROM  dbo.Vendedor INNER JOIN  ";
                queryString = queryString + " dbo.COMPANIA ON dbo.Vendedor.ConsecutivoCompania = dbo.COMPANIA.ConsecutivoCompania ";
                queryString = queryString + " WHERE dbo.COMPANIA.Nombre ='" + ntcustomerId + "'";
                using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                                    "AplicationConnectionString")))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ListViewItem List;
                            List = listVendedor.Items.Add(reader["NombreVendedor"].ToString());
                            List.SubItems.Add(reader["Codigo"].ToString());
                            List.UseItemStyleForSubItems = false;
                        }
                        reader.NextResult();
                    }
                }


            }
        }

        private void listVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listVendedor.SelectedItems.Count == 1)
            {
                txtNombreVendedor.Text = listVendedor.SelectedItems[0].SubItems[0].Text;
                txtCodigoVendedor.Text = listVendedor.SelectedItems[0].SubItems[1].Text;


            }
        }


       

        private void button1_Click(object sender, EventArgs e)
        {
            int consecutivoCompania = 0;
            foreach (ListViewItem row in listEmpresa.Items) {
                if (row.Selected  == true) {
                    consecutivoCompania =  Convert.ToInt32(row.SubItems[1].Text);
                }
            }

            if (consecutivoCompania == 0)
            {
                MessageBox.Show("Debe seleccionar una Compañia para generar una consulta");
            }
            else
            {

                if (button1.Text == "Costo de Facturación")
                {
                    label5.Text = "";
                    label5.Text = "Espere estamos procesando la información.................................";
                    label5.Refresh();
                    frmInforme newFrm = new frmInforme();


                    newFrm.Fechadesde = dtpFechaIni.Text;
                    newFrm.Fechahasta = dtpFechaFin.Text;
                    newFrm.CodigoEmpresa = txtCodigoEmpresa.Text;
                    newFrm.CodigoVendedor = txtCodigoVendedor.Text;
                    newFrm.NombreEmpresa = txtNombreEmpresa.Text;
                    label5.Refresh();
                    label5.Refresh();
                    label5.Refresh();
                    newFrm.ShowDialog();
                    MessageBox.Show("Proceso Finalizado.....................");
                    label5.Refresh();
                    label5.Text = "Inicio ..........................................................";

                }
            }

       }

        private void ArticuloDiario_Click(object sender, EventArgs e)
        {
            button1.Text = "Costo de Facturación";
        }

        private void CategoriaDiario_Click(object sender, EventArgs e)
        {

            button1.Text = "Comparativo cobranza";

        }
    }
}
