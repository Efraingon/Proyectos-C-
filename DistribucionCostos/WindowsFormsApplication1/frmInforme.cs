using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace DistribucionCosto
{
    public partial class frmInforme : Form
    {
        public string Fechadesde;
        public string Fechahasta;
        public string CodigoEmpresa;
        public string CodigoVendedor;
        public string NombreEmpresa;

        public frmInforme()
        {
            InitializeComponent();
        }

        private void frmInforme_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable Articulo = CreateTablaArticulo();


                DataSet dsReport1 = new DataSet();
                dsReport1.Namespace = "DataRentalidad";

                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataSet1";
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
        private DataTable CreateTablaArticulo()
        {

            string queryString;


            DataTable ArticuloTable = new DataTable("TablaRentalidad");
            DataColumn dtColumn;
            DataRow myDataRow;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ConsecutivoCompania";
            dtColumn.Caption = "ConsecutivoCompania";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Fecha";
            dtColumn.Caption = "Fecha";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Numero";
            dtColumn.Caption = "Numero";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Cliente";
            dtColumn.Caption = "Cliente";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Vendedor";
            dtColumn.Caption = "Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Articulo";
            dtColumn.Caption = "Articulo";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Serial";
            dtColumn.Caption = "Serial";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Rollo";
            dtColumn.Caption = "Rollo";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Descripcion";
            dtColumn.Caption = "Descripcion";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Cantidad";
            dtColumn.Caption = "Cantidad";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "PrecioVenta";
            dtColumn.Caption = "PrecioVenta";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "PrecioCosto";
            dtColumn.Caption = "PrecioCosto";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "VentaTotal";
            dtColumn.Caption = "VentaTotal";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CostoTotal";
            dtColumn.Caption = "CostoTotal";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Diferencia";
            dtColumn.Caption = "Diferencia";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Moneda";
            dtColumn.Caption = "Moneda";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "DescripcionCompania";
            dtColumn.Caption = "DescripcionCompania";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FechaVentas";
            dtColumn.Caption = "FechaVentas";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Porcentaje";
            dtColumn.Caption = "Porcentaje";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Correspondiente";
            dtColumn.Caption = "Correspondiente";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);
            
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Comision";
            dtColumn.Caption = "Comision";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            ArticuloTable.Columns.Add(dtColumn);

            queryString = squery(Fechadesde, Fechahasta);

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

                        string vConsecutivoCompania = reader["ConsecutivoCompania"].ToString();
                        string vFecha = reader["Fecha"].ToString();
                        string vNumero = reader["Numero"].ToString();
                        string vCliente = reader["Cliente"].ToString();
                        string vVendedor = reader["Vendedor"].ToString();

                        string vArticulo = reader["Articulo"].ToString();
                        string vSerial = reader["Serial"].ToString();
                        string vRollo = reader["Rollo"].ToString();
                        string vDescripcion = reader["Descripcion"].ToString();
                        string vCantidad = reader["Cantidad"].ToString();
                        

                        string vPrecioVenta = reader["PrecioVenta"].ToString();
                        string vPrecioCosto = reader["PrecioCosto"].ToString();
                        string vVentaTotal = reader["VentaTotal"].ToString();
                        string vCostoTotal = reader["CostoTotal"].ToString();
                        string vDiferencia = reader["Diferencia"].ToString();
                        string vDescripcionCompania = reader["DescripcionCompania"].ToString();
                        string vFechaVentas = reader["FechaVentas"].ToString();

                        string vMoneda = reader["Moneda"].ToString();


                        decimal montoCambio = 0;
                        decimal TotalVenta = 0;
                        decimal TotalCosto = 0;
                        decimal Porcentaje = 0;
                        decimal Correspondiente = 0;
                        decimal Comision = 0;
                        montoCambio = sBucarCambioMonto(vFecha);
                        if (montoCambio == 0)
                        {
                            montoCambio = sBucarCambioMontoFactura(vNumero, vConsecutivoCompania);
                        }
                        
                        if (montoCambio == 0)
                        {
                            montoCambio = 1;
                        } 
                       
                        
                        myDataRow = ArticuloTable.NewRow();
                        myDataRow["ConsecutivoCompania"] = vConsecutivoCompania;
                        myDataRow["Fecha"] = vFecha;
                        myDataRow["Numero"] = vNumero;
                        myDataRow["Cliente"] = vCliente;
                        myDataRow["Vendedor"] = vVendedor;

                        myDataRow["Articulo"] = vArticulo;
                        myDataRow["Serial"] = vSerial;
                        myDataRow["Rollo"] = vRollo;
                        myDataRow["Descripcion"] = vDescripcion;
                        decimal Valor1 = 0; 
                        if (vCantidad != "")
                        {
                            myDataRow["Cantidad"] = vCantidad;
                        }
                        if (vPrecioVenta != "")
                        {
                            if (vMoneda == "Dólar")
                            {
                                myDataRow["PrecioVenta"] = vPrecioVenta;
                                Valor1 = Convert.ToDecimal(vPrecioVenta);
                         
               
                            }
                            else
                            {

                               // myDataRow["PrecioVenta"] = (Convert.ToDecimal(vPrecioVenta) / montoCambio);
                               // Valor1 = (Convert.ToDecimal(vPrecioVenta) / montoCambio);

                                myDataRow["PrecioVenta"] = Convert.ToDecimal(vPrecioVenta);
                                Valor1 = Convert.ToDecimal(vPrecioVenta);
 
                            }
                        }
                        Valor1 = decimal.Round(Valor1, 2);
                        decimal Valor2 = 0; 
                        if (vPrecioCosto != "")
                        {
                            if (vMoneda == "Dólar")
                            {
                                myDataRow["PrecioCosto"] = vPrecioCosto;
                            
                            }
                            else {
                                myDataRow["PrecioCosto"] = (Convert.ToDecimal(vPrecioCosto) * montoCambio);
                                Valor2 = (Convert.ToDecimal(vPrecioCosto) * montoCambio);
                            }
                        }
                        
                        if (vVentaTotal != "")
                        {
                            myDataRow["VentaTotal"] = Valor1 * Convert.ToDecimal(vCantidad);
                            TotalVenta = Valor1 * Convert.ToDecimal(vCantidad);

                        }


                        if (vCostoTotal != "")
                        {
                            if (vMoneda == "Dólar")
                            {
                                myDataRow["CostoTotal"] = Convert.ToDecimal(vPrecioCosto) * Convert.ToDecimal(vCantidad);
                                TotalCosto = Convert.ToDecimal(vPrecioCosto) * Convert.ToDecimal(vCantidad);

                            }
                            else {
                                myDataRow["CostoTotal"] = Valor2 * Convert.ToDecimal(vCantidad);
                                TotalCosto = Valor2 * Convert.ToDecimal(vCantidad);

                            }
                        }
                        
                        
                        
                        
                        if (vDiferencia != "")
                        {
                           // myDataRow["Diferencia"] = (Convert.ToDecimal(vDiferencia));
                            myDataRow["Diferencia"] =TotalVenta- TotalCosto;

                        }
                        if (TotalCosto != 0)
                        {
                            Porcentaje = (TotalVenta/ TotalCosto) * 100;
                            Porcentaje =  Porcentaje-100;
                        }
                        else {
                            if (TotalVenta < 0)
                            {
                                Porcentaje = 0;
                            }
                            else
                            {
                                Porcentaje = 100;
                            }
                        }

                        //if (Porcentaje < 0) {
                        //    Porcentaje = 0;
                        //}

                       // Porcentaje = Convert.ToDecimal(69.9999998);

                        myDataRow["Porcentaje"] = Math.Truncate(Porcentaje* 100) / 100 ;
                        
                        if (Porcentaje >= 1 && Porcentaje < 10) {
                            Correspondiente = Convert.ToDecimal(0.5);
                        }
                        else if (Porcentaje >= 10 && Porcentaje < 20)
                        {
                            Correspondiente = 1;

                        }
                        else if (Porcentaje >= 20 && Porcentaje <30)
                        {
                            Correspondiente = 2;

                        }
                        else if (Porcentaje >= 30 && Porcentaje <40)
                        {
                            Correspondiente = 3;

                        }
                        else if (Porcentaje >= 40 && Porcentaje <50)
                        {
                            Correspondiente = 4;

                        }
                        else if (Porcentaje >= 50 && Porcentaje <60)
                        {
                            Correspondiente = 5;

                        }
                        else if (Porcentaje >= 60 && Porcentaje <70)
                        {
                            Correspondiente = 6;

                        }
                        else if (Porcentaje >= 70)
                        {
                            Correspondiente = 7;
                        }
                        myDataRow["Correspondiente"] = Correspondiente;
                        Comision = (TotalVenta * Correspondiente) / 100;
                        myDataRow["Comision"] = Comision;
                        
                        


                        myDataRow["DescripcionCompania"] = vDescripcionCompania;
                        myDataRow["FechaVentas"] = vFechaVentas;
                        myDataRow["Moneda"] = vMoneda;

                        ArticuloTable.Rows.Add(myDataRow);
                    }
                    reader.NextResult();
                }
                reader.Close();
                connection.Close();
            }

            return ArticuloTable;

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
        private string squery(string vFechadesde, string vFechahasta)
        {
            string queryString = "";

            queryString = queryString + " SET DATEFORMAT dmy   SELECT  dbo.IGV_VentasConDescripcionArticulos_B1.ConsecutivoCompania, ";
            //queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Fecha, ";
            queryString = queryString + " convert(NVARCHAR, dbo.IGV_VentasConDescripcionArticulos_B1.Fecha, 103) AS Fecha, ";

            

            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Numero, ";
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.CodigoCliente +' ' + dbo.IGV_VentasConDescripcionArticulos_B1.Nombre AS Cliente, ";
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.CodigoVendedor +' ' + dbo.IGV_VentasConDescripcionArticulos_B1.NombreVendedor AS Vendedor, ";
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Articulo, ";
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Serial, ";
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Rollo, ";
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Descripcion, ";
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad, ";
            queryString = queryString + " (CASE WHEN  dbo.IGV_VentasConDescripcionArticulos_B1.PorcentajeDescuento = 0 ";
            queryString = queryString + " THEN dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual ";
            queryString = queryString + " ELSE dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual - ";
            queryString = queryString + " (dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual * ";
            queryString = queryString + " (dbo.IGV_VentasConDescripcionArticulos_B1.PorcentajeDescuento/100)) END )  AS PrecioVenta, ";
            
           // queryString = queryString + " dbo.Gf_CompraUnaFecha(dbo.IGV_VentasConDescripcionArticulos_B1.ConsecutivoCompania, ";
            //queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Fecha,dbo.IGV_VentasConDescripcionArticulos_B1.Codigo)  AS PrecioCosto ,";
            queryString = queryString + " ArticuloInventario.MeCostoUnitario AS PrecioCosto,";

            queryString = queryString + " (CASE WHEN  dbo.IGV_VentasConDescripcionArticulos_B1.PorcentajeDescuento = 0 ";
            queryString = queryString + " THEN dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual ";
            queryString = queryString + "  ELSE dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual - ";
            queryString = queryString + "(dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual * (dbo.IGV_VentasConDescripcionArticulos_B1.PorcentajeDescuento/100)) ";
            queryString = queryString + " END )  * dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad  AS VentaTotal, ";


           // queryString = queryString + " dbo.Gf_CompraUnaFecha(dbo.IGV_VentasConDescripcionArticulos_B1.ConsecutivoCompania,  ";
            
            //queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Fecha,dbo.IGV_VentasConDescripcionArticulos_B1.Codigo) ";
            //queryString = queryString + " * dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad   AS CostoTotal, ";
            
            queryString = queryString + " (ArticuloInventario.MeCostoUnitario * dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad ) AS CostoTotal,";

            queryString = queryString + "( (CASE WHEN  dbo.IGV_VentasConDescripcionArticulos_B1.PorcentajeDescuento = 0 ";
            queryString = queryString + " THEN dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual ";
            queryString = queryString + "  ELSE dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual - ";
            queryString = queryString + "(dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual * (dbo.IGV_VentasConDescripcionArticulos_B1.PorcentajeDescuento/100)) ";
            queryString = queryString + " END )  * dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad)  - ";
            queryString = queryString + " (ArticuloInventario.MeCostoUnitario * dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad ) AS Diferencia,";


         
            //queryString = queryString + " - (dbo.IGV_VentasConDescripcionArticulos_B1.PrecioConDescuentoIndividual  ";
            //queryString = queryString + " * (dbo.IGV_VentasConDescripcionArticulos_B1.PorcentajeDescuento/100)) END ) ";
            //queryString = queryString + " * dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad ) ";
            //queryString = queryString + " -(dbo.Gf_CompraUnaFecha(dbo.IGV_VentasConDescripcionArticulos_B1.ConsecutivoCompania, ";
            //queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Fecha,dbo.IGV_VentasConDescripcionArticulos_B1.Codigo) ";
            //queryString = queryString + " * dbo.IGV_VentasConDescripcionArticulos_B1.Cantidad) AS Diferencia, ";
            
            
            
            queryString = queryString + " dbo.IGV_VentasConDescripcionArticulos_B1.Moneda, ";
            queryString = queryString + "'" + NombreEmpresa.ToString() + "' AS DescripcionCompania, ";
            queryString = queryString + "'Ventas desde " + vFechadesde.ToString() + " Hasta " + vFechahasta.ToString() + "' AS FechaVentas";



            queryString = queryString + " FROM dbo.IGV_VentasConDescripcionArticulos_B1 JOIN ArticuloInventario ";
            queryString = queryString + " ON ArticuloInventario.Codigo = dbo.IGV_VentasConDescripcionArticulos_B1.Codigo ";
            queryString = queryString + " AND dbo.IGV_VentasConDescripcionArticulos_B1.ConsecutivoCompania = ArticuloInventario.ConsecutivoCompania ";
            queryString = queryString + " JOIN dbo.COMPANIA ";
            queryString = queryString + " ON dbo.IGV_VentasConDescripcionArticulos_B1.ConsecutivoCompania = Compania.ConsecutivoCompania ";
            queryString = queryString + " WHERE dbo.IGV_VentasConDescripcionArticulos_B1.Fecha BETWEEN '" + vFechadesde.ToString() + "' AND '" + vFechahasta.ToString() + "'";
            //queryString = queryString + " AND  dbo.ArticuloInventario.TipoDeArticulo = '0' ";
            queryString = queryString + " AND  dbo.ArticuloInventario.Codigo Not like '%@%'";

            queryString = queryString + " AND  dbo.IGV_VentasConDescripcionArticulos_B1.StatusFactura ='0' ";
            queryString = queryString + " AND  dbo.IGV_VentasConDescripcionArticulos_B1.TipoDeDocumento <>8 ";
            
            if (CodigoVendedor != "") {
                queryString = queryString + " AND  dbo.IGV_VentasConDescripcionArticulos_B1.CodigoVendedor = '" + CodigoVendedor.ToString() + "'";
            }
            queryString = queryString + " AND dbo.COMPANIA.ConsecutivoCompania =" + CodigoEmpresa.ToString();
         

            queryString = queryString + " ORDER BY  dbo.IGV_VentasConDescripcionArticulos_B1.Moneda, dbo.IGV_VentasConDescripcionArticulos_B1.CodigoVendedor, dbo.IGV_VentasConDescripcionArticulos_B1.Numero ";

            return queryString;

        }
        private decimal sBucarCambioMonto(string FechaA)
        {
            decimal MontoCambio = 0;
            string vSql = "";
            vSql = SqlReporteCambio(FechaA);
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                        "AplicationConnectionString")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MontoCambio = Convert.ToDecimal(reader["CambioAMonedaLocal"].ToString());
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return MontoCambio;
        }
        private decimal sBucarCambioMontoFactura(string NumeroFactura, string Compania)
        {
            decimal MontoCambio = 0;
            string vSql = "";
            vSql = SqlReporteCambioFactura(NumeroFactura, Compania);
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                        "AplicationConnectionString")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MontoCambio = Convert.ToDecimal(reader["CambioMostrartotalEnDivisas"].ToString());
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return MontoCambio;
        }
        private decimal sBucarCambioMontoFacturaObserva(string NumeroFactura, string Compania)
        {
            decimal MontoCambio = 0;
            string vSql = "";
            vSql = SqlReporteCambioFacturaObser(NumeroFactura, Compania);
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                        "AplicationConnectionString")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MontoCambio = Convert.ToDecimal(reader["TasaCambio"].ToString());
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return MontoCambio;
        }
        private string SqlReporteCambio(string FechaA)
        {
            string vSql = "";
            vSql = vSql + " SET DATEFORMAT dmy  SELECT ";
            vSql = vSql + " CambioAMonedaLocal ";
            vSql = vSql + " FROM Comun.Cambio ";
            vSql = vSql + " WHERE CodigoMoneda = 'USD' ";
            vSql = vSql + "  AND FechaDeVigencia= '" + FechaA + "'";

            return vSql;
        }
        private string SqlReporteCambioFactura(string Numero, string vconsecutivoCompania)
        {
            string vSql = "";
            vSql = vSql + " SET DATEFORMAT dmy  SELECT ";
            vSql = vSql + " CambioMostrartotalEnDivisas ";
            vSql = vSql + " FROM dbo.factura ";
            vSql = vSql + " WHERE Numero = '" + Numero + "' AND ConsecutivoCompania =" + vconsecutivoCompania.ToString();
            return vSql;
        }
        private string SqlReporteCambioFacturaObser(string Numero, string vconsecutivoCompania)
        {
            string vSql = "";
            vSql = vSql + " SET DATEFORMAT dmy  SELECT ";
            vSql = vSql + " CASE ";
            vSql = vSql + " WHEN (Observaciones = '' OR Observaciones IS NULL)  THEN CAST(0 AS DECIMAL(10, 2)) ";
            vSql = vSql + " WHEN Observaciones <> '' THEN CAST(Observaciones AS DECIMAL(10, 2)) ";
            vSql = vSql + " END AS TasaCambio ";
            
            vSql = vSql + " FROM dbo.factura ";
            vSql = vSql + " WHERE Numero = '" + Numero + "' AND ConsecutivoCompania =" + vconsecutivoCompania.ToString();
            return vSql;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            reportViewer1.Dispose();
        }

    }
}
