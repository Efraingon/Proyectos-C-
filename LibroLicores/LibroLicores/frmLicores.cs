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

namespace LibroLicores
{
    public partial class frmLicores : Form
    {
        string vNombreEmpresa = "";
        public frmLicores()
        {
            InitializeComponent();
            InicializaList(false);
        }
        private void InicializaList(bool vMostrar)
        {

            string queryString;

            inicializaColumnas(listEmpresa);
            queryString = "select * from dbo.compania ";
            try
            {

                using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                                    "IntegrarLicores")))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ListViewItem List;
                            List = listEmpresa.Items.Add(reader["Nombre"].ToString() + "-" + reader["NumeroDeRif"].ToString());
                            List.SubItems.Add(reader["consecutivoCompania"].ToString());
                            List.UseItemStyleForSubItems = false;
                        }
                        reader.NextResult();
                    }
                }

            }
            catch (SqlException ex)
            {

                foreach (SqlError sError in ex.Errors)
                {

                    switch (sError.Number)
                    {
                        case 17:
                            MessageBox.Show("El servidor Gala no existe, por favor verifique el nombre");
                            break;
                        case 18452:    //Login failed for user '%ls'. Reason: Not associated with a trusted SQL Server connection.
                            MessageBox.Show("Especifique un usuario y password");
                            break;
                        case 18456: //Login failed for user '%ls'.
                            MessageBox.Show("El usuario o password es incorrecto");
                            break;
                        case 4060:    //Cannot open database requested in login '%.*ls'. Login fails.
                            MessageBox.Show("El usuario no tiene permisos para acceder a la base de datos SAW");
                            break;
                        case 208:    //Invalid object name '%ls'."
                            MessageBox.Show("El nombre del objeto es incorrecto, para mayor información consulte la lnea :" + sError.LineNumber);
                            break;
                        default:
                            MessageBox.Show(sError.Message);
                            break;
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
            }
        }

        private void listEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listEmpresa.SelectedItems.Count == 1)
            {

                string ntcustomerId = listEmpresa.SelectedItems[0].Text;
                string CodigoEmpresa = listEmpresa.SelectedItems[0].SubItems[1].Text;
                txtNombreEmpresa.Text = listEmpresa.SelectedItems[0].SubItems[0].Text;
                txtCodigoEmpresa.Text = CodigoEmpresa;

                vNombreEmpresa = listEmpresa.SelectedItems[0].SubItems[0].Text;


            }
        }
  

        private void CompMenFacLinea_Click_1(object sender, EventArgs e)
        {
            button1.Text = "Libro de Licores";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DateTime date = dtpFechaIni.Value;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);


            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            string fechainicio = oPrimerDiaDelMes.ToString("dd/MM/yyyy");
            string ferchafin = oUltimoDiaDelMes.ToString("dd/MM/yyyy");
            string vMes = date.Month.ToString("D2");
            string vYear = date.Year.ToString();


            int consecutivocompania = 0;
            string NombreCompania = "";
            foreach (ListViewItem row in listEmpresa.Items)
            {
                if (row.Selected == true)
                {
                    consecutivocompania = Convert.ToInt32(row.SubItems[1].Text);
                    NombreCompania = row.SubItems[0].Text;
                }
            }

            if (consecutivocompania == 0)
            {
                MessageBox.Show("Debe seleccionar una Compañia para generar una consulta");
            }
            else
            {
                if (button1.Text == "Libro de Licores")
                {

                    label5.Text = "";
                    label5.Text = "Espere estamos procesando la información.................................";
                    label5.Refresh();
                    frmInformeLicores newFrm = new frmInformeLicores();


                    newFrm.Fechadesde = fechainicio;
                    newFrm.Fechahasta = ferchafin;
                    newFrm.Mes = vMes;
                    newFrm.Ano = vYear;

                    newFrm.CodigoEmpresa = txtCodigoEmpresa.Text;
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
    }
}
