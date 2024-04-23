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
using Microsoft.Reporting.WinForms;

namespace LibroLicores
{
    public partial class frmInformeLicores : Form
    {
        public string Fechadesde;
        public string Fechahasta;
        public string Mes;
        public string Ano;

        public string CodigoEmpresa;
        public string NombreEmpresa;
        public frmInformeLicores()
        {
            InitializeComponent();
        }

        private void frmInformeLicores_Load(object sender, EventArgs e)
        {

            try
            {
                string vSql = "";
                vSql = SqlReporte(Fechadesde, Fechahasta, Convert.ToInt16(CodigoEmpresa));
                DataTable Articulo = CreateTablaLibro(vSql);


                DataSet dsReport1 = new DataSet();
                dsReport1.Namespace = "DataFactura";

                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataFactura";
                rds.Value = Articulo;
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.LocalReport.Refresh();
                reportViewer1.RefreshReport();
            }

            catch (Exception ex)
            {
                reportViewer1.Dispose();
                MessageBox.Show(ex.Message);
            }

            
        }

        private string SqlReporte(string FechaA, string FechaB, int consecutivo)
        {
            string vSql = "";
            vSql = vSql + " SET DATEFORMAT dmy \n";
            vSql = vSql + " SELECT  dbo.factura.Numero AS NUMERO, \n";
		    vSql = vSql + " convert(NVARCHAR, dbo.factura.fecha, 103) As Fecha,\n";
		    vSql = vSql + " dbo.Cliente.Nombre,\n";
		    vSql = vSql + " dbo.Cliente.Direccion,\n";
		    vSql = vSql + " dbo.Cliente.NumeroRIF As NREGISTRO,\n";
		    vSql = vSql + " dbo.ArticuloInventario.LineaDeProducto AS Clase,\n";
		    vSql = vSql + " dbo.ArticuloInventario.Categoria AS Clase2, \n";
		    vSql = vSql + " dbo.ArticuloInventario.Descripcion AS DENOMINACION, \n";
		    vSql = vSql + " dbo.ArticuloInventario.Marca AS Distribuidor, \n";
		    vSql = vSql + " dbo.ArticuloInventario.CampoDefinible1 AS Grados, \n";
		    vSql = vSql + " (dbo.renglonFactura.Cantidad * dbo.ArticuloInventario.UnidadDeVentaSecundaria)  AS Litros, \n";
            vSql = vSql + " dbo.COMPANIA.Nombre AS NombreCompania, \n";
            vSql = vSql + " dbo.COMPANIA.NumeroDeRif AS NumeroDeRifCompania, \n";
            vSql = vSql + " dbo.COMPANIA.Direccion AS DireccionCompania \n";
		  
            vSql = vSql + " FROM dbo.factura INNER JOIN dbo.renglonFactura \n";
            vSql = vSql + " ON dbo.factura.ConsecutivoCompania = dbo.renglonFactura.ConsecutivoCompania \n"; 
			vSql = vSql + " AND dbo.factura.Numero = dbo.renglonFactura.NumeroFactura \n"; 
            vSql = vSql + " AND dbo.factura.TipoDeDocumento = dbo.renglonFactura.TipoDeDocumento \n"; 
			vSql = vSql + " INNER JOIN dbo.Cliente ON dbo.factura.ConsecutivoCompania = dbo.Cliente.ConsecutivoCompania \n"; 
			vSql = vSql + "	AND dbo.factura.CodigoCliente = dbo.Cliente.Codigo \n"; 
			vSql = vSql + " INNER JOIN dbo.ArticuloInventario \n"; 
            vSql = vSql + " ON dbo.renglonFactura.ConsecutivoCompania = dbo.ArticuloInventario.ConsecutivoCompania\n";
            vSql = vSql + " INNER JOIN dbo.COMPANIA \n";
            vSql = vSql + " ON dbo.COMPANIA.ConsecutivoCompania = dbo.factura.ConsecutivoCompania\n"; 
            vSql = vSql + " AND  dbo.renglonFactura.Articulo  = dbo.ArticuloInventario.Codigo \n"; 
			
            vSql = vSql + " WHERE dbo.ArticuloInventario.Descripcion NOT Like '%Resumen%' \n"; 
            vSql = vSql + " AND dbo.ArticuloInventario.LineaDeProducto <> 'LINEA DE PRODUCTO' \n"; 
			vSql = vSql + " AND dbo.ArticuloInventario.Codigo NOT Like '%RD' \n"; 
			vSql = vSql + " AND factura.StatusFactura = '0' AND factura.TipoDeDocumento <> '3' \n";
            vSql = vSql + " AND factura.TipoDeDocumento <> '8' \n";

            vSql = vSql + " AND factura.Fecha BETWEEN  '" + FechaA + "'  AND '" + FechaB + "' \n";
            vSql = vSql + " AND factura.ConsecutivoCompania = " + consecutivo.ToString() + "\n";
           


            vSql = vSql + " ORDER BY Fecha \n"; 
            return vSql;
        }
        
        private DataTable CreateTabla()
        {
            DataTable FacturaTable = new DataTable("Factura");
            DataColumn dtColumn;
        
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NUMERO";
            dtColumn.Caption = "NUMERO";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FECHA";
            dtColumn.Caption = "FECHA";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MES";
            dtColumn.Caption = "MES";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ANO";
            dtColumn.Caption = "ANO";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NOMBRE";
            dtColumn.Caption = "NOMBRE";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NOMBRECompania";
            dtColumn.Caption = "NOMBRECompania";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "DIRECCION";
            dtColumn.Caption = "DIRECCION";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "DIRECCIONCompania";
            dtColumn.Caption = "DIRECCIONCompania";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "RIFCompania";
            dtColumn.Caption = "RIFCompania";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NREGISTRO";
            dtColumn.Caption = "N° REGISTRO";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CLASE";
            dtColumn.Caption = "CLASE";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "DENOMINACION";
            dtColumn.Caption = "DENOMINACION COMERCIAL";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FABRICANTE";
            dtColumn.Caption = "FABRICANTE  Y/O DISTRIBUIDOR";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "LITROS";
            dtColumn.Caption = "LITROS";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "GRADOS";
            dtColumn.Caption = "GRADOS";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);
            return FacturaTable;
        }
        private DataTable CreateTablaLibro(string vSql)
        {
            DataTable FacturaTable = new DataTable();
            DataRow myDataRow;
            FacturaTable = CreateTabla();
        
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarLicores")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        string vNUMERO = reader["NUMERO"].ToString();
                        string vFECHA = reader["FECHA"].ToString();
                        string vNOMBRE = reader["NOMBRE"].ToString();
                        string vNOMBRECompania = reader["NOMBRECompania"].ToString();
                        string vRIFCompania = reader["NumeroDeRifCompania"].ToString();

                        string vDIRECCION = reader["DIRECCION"].ToString();
                        string vDIRECCIONCompania = reader["DireccionCompania"].ToString();


                        string vNREGISTRO = reader["NREGISTRO"].ToString();
                        string vCLASE = reader["CLASE2"].ToString();
                        string vDENOMINACION = reader["DENOMINACION"].ToString();
                        string vFABRICANTE = reader["Distribuidor"].ToString();
                        string vLITROS = reader["LITROS"].ToString();

                        string vGRADOS = reader["GRADOS"].ToString();
                        

                        myDataRow = FacturaTable.NewRow();
                        myDataRow["NUMERO"] = vNUMERO;
                        myDataRow["FECHA"] = vFECHA;
                        myDataRow["NOMBRE"] = vNOMBRE;
                        myDataRow["NOMBRECompania"] = vNOMBRECompania;
                        myDataRow["RIFCompania"] = vRIFCompania;
                        myDataRow["DIRECCION"] = vDIRECCION;
                        myDataRow["DIRECCIONCompania"] = vDIRECCIONCompania;

                        myDataRow["NREGISTRO"] = vNREGISTRO;
                        myDataRow["CLASE"] = vCLASE;
                        myDataRow["DENOMINACION"] = vDENOMINACION;
                        myDataRow["FABRICANTE"] = vFABRICANTE;
                        myDataRow["LITROS"] =Truncate(Convert.ToDouble(vLITROS),2);
                        myDataRow["GRADOS"] = vGRADOS;
                        myDataRow["MES"] = Mes;
                        myDataRow["ANO"] = Ano;

                        FacturaTable.Rows.Add(myDataRow);
                    }
                    reader.NextResult();
                }


                connection.Close();
            }
            return FacturaTable;
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

        private void frmInformeLicores_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        public static double Truncate(double value, int decimales)
        {
            double aux_value = Math.Pow(10, decimales);
            return (Math.Truncate(value * aux_value) / aux_value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            reportViewer1.Dispose();
        }
    }
}
