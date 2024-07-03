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
using System.Globalization;
using System.Diagnostics;
namespace CafeProtectora
{
    public partial class frmComision : Form
    {
        string vNombreEmpresa = "";
        int consecutivo = 0;
        public frmComision()
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
                                                                    "IntegrarProtectora")))
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
        
        private string SqlReporteComision(string FechaA, string FechaB, int consecutivo)
        {
            string vSql = "";
            vSql = vSql + " SET DATEFORMAT dmy  SELECT ";
            vSql = vSql + " dbo.factura.Numero,";
			vSql = vSql + " CONVERT(varchar,dbo.Cobranza.Fecha,103) as Fecha, ";
			vSql = vSql + " Adm.Vendedor.Nombre, ";
			vSql = vSql + " dbo.renglonFactura.Cantidad As Cantidad ";
            vSql = vSql + " ,renglonFactura.PrecioConIVA ";
			vSql = vSql + " ,(dbo.renglonFactura.Cantidad * dbo.renglonFactura.PrecioConIVA) As Factura, ";
            vSql = vSql + " (dbo.DocumentoCobrado.MontoTotalDeCxC) CXCobrada, ";
            vSql = vSql + " (SELECT  SUM(renglonFactura.PrecioConIVA) FROM renglonFactura   ";
            
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + consecutivo.ToString()  + " and NumeroFactura =dbo.factura.Numero) AS TotalPrecio,";



            vSql = vSql + " (SELECT  SUM(FacRen.Cantidad) FROM renglonFactura AS FacRen ";
            vSql = vSql + " WHERE FacRen.ConsecutivoCompania = " + consecutivo.ToString() + " and FacRen.NumeroFactura =dbo.factura.Numero) AS TotalCantidad, ";


			vSql = vSql + " dbo.factura.CodigoMoneda, ";
            vSql = vSql + " dbo.DocumentoCobrado.CodigoMonedaDeCxC, ";
			vSql = vSql + " (SELECT CambioAMonedaLocal ";
	        vSql = vSql + "			FROM Comun.Cambio ";
			vSql = vSql + "			WHERE CONVERT(varchar,FechaDeVigencia,103) =CONVERT(varchar,dbo.Cobranza.Fecha,103) ";
			vSql = vSql + "			AND CodigoMoneda ='USD') As Cambio ";
            vSql = vSql + "       ,dbo.renglonFactura.Descripcion, ";
            vSql = vSql + "       dbo.factura.TotalFactura ";
            vSql = vSql + "      ,dbo.renglonFactura.TotalRenglon ";
           
            

            vSql = vSql + " FROM dbo.Cobranza INNER JOIN ";
            vSql = vSql + " dbo.DocumentoCobrado ON dbo.Cobranza.ConsecutivoCompania = dbo.DocumentoCobrado.ConsecutivoCompania"; 
			vSql = vSql + " AND dbo.Cobranza.Numero = dbo.DocumentoCobrado.NumeroCobranza "; 
			vSql = vSql + "	INNER JOIN dbo.CxC ON dbo.Cobranza.ConsecutivoCompania = dbo.CxC.ConsecutivoCompania ";
			//vSql = vSql + "	AND dbo.CxC.NumeroDocumentoOrigen =dbo.DocumentoCobrado.NumeroDelDocumentoCobrado ";

            vSql = vSql + " AND dbo.DocumentoCobrado.NumeroDelDocumentoCobrado  like   dbo.CxC.NumeroDocumentoOrigen + '%' ";

			vSql = vSql + " AND dbo.CxC.CodigoCliente = dbo.Cobranza.CodigoCliente "; 
			vSql = vSql + " INNER JOIN dbo.factura ON dbo.Cobranza.ConsecutivoCompania = dbo.factura.ConsecutivoCompania ";
			vSql = vSql + "	AND dbo.factura.Numero =dbo.CxC.NumeroDocumentoOrigen ";
			vSql = vSql + "	AND dbo.factura.CodigoCliente = dbo.CxC.CodigoCliente "; 
			vSql = vSql + "	AND dbo.factura.CodigoVendedor = dbo.CxC.CodigoVendedor "; 
				
			vSql = vSql + "	INNER JOIN dbo.renglonFactura ON dbo.renglonFactura.ConsecutivoCompania = dbo.factura.ConsecutivoCompania ";
			vSql = vSql + "	AND dbo.factura.Numero =dbo.renglonFactura.NumeroFactura ";
			vSql = vSql + "	INNER JOIN Adm.Vendedor ON Adm.Vendedor.ConsecutivoCompania = dbo.factura.ConsecutivoCompania ";
			vSql = vSql + "	AND Adm.Vendedor.Codigo =dbo.factura.CodigoVendedor ";

            vSql = vSql + " WHERE factura.StatusFactura = '0' AND factura.TipoDeDocumento <> '3' ";
            vSql = vSql + " AND (dbo.Cobranza.Fecha BETWEEN  '" + FechaA + "'  AND '" + FechaB + "' )";
            vSql = vSql + " AND factura.ConsecutivoCompania = " + consecutivo.ToString();
            vSql = vSql + " AND dbo.CxC.Origen =0" ;

            vSql = vSql + " AND Cobranza.Numero =(SELECT TOP 1 NumeroCobranza FROM ";
            vSql = vSql + " dbo.DocumentoCobrado WHERE NumeroDelDocumentoCobrado like dbo.CxC.NumeroDocumentoOrigen + '%' ";
            vSql = vSql + " AND ConsecutivoCompania =" + consecutivo.ToString() + " ) ";

            //vSql = vSql + " AND Adm.Vendedor.Nombre ='CARLOS PINEDA' ";
            //vSql = vSql + " AND  factura.Numero ='0000002522'";




			vSql = vSql + "	GROUP BY dbo.factura.Numero,";
			vSql = vSql + "			  dbo.Cobranza.Fecha,";	
			vSql = vSql + "			 Adm.Vendedor.Nombre,";
            vSql = vSql + "			renglonFactura.Cantidad,";
			vSql = vSql + "			dbo.factura.CodigoMoneda,";
			vSql = vSql + "			dbo.DocumentoCobrado.CodigoMonedaDeCxC,";
            vSql = vSql + "			dbo.renglonFactura.PrecioConIVA,";
            vSql = vSql + "         dbo.factura.TotalFactura,";
            vSql = vSql + "			dbo.renglonFactura.Descripcion, ";
            vSql = vSql + "         dbo.renglonFactura.ConsecutivoCompania, ";
            vSql = vSql + "         dbo.DocumentoCobrado.MontoTotalDeCxC,cobranza.Numero, ";
            vSql = vSql + "         dbo.renglonFactura.TotalRenglon ";



            vSql = vSql + "	ORDER BY Adm.Vendedor.Nombre, dbo.factura.Numero, cobranza.Numero Asc ";

            return vSql;
        }

        private string SqlReporteComisionResumen(string FechaA, string FechaB, int consecutivo)
        {
            string vSql = "";

            vSql = vSql + " SET DATEFORMAT dmy \n";
            vSql = vSql + " SELECT cobranza.CodigoCobrador,\n ";
            vSql = vSql + " vendedor.Nombre AS NombreVendedor, \n";
            vSql = vSql + " Cobranza.moneda AS MonedaCobro,\n";
            vSql = vSql + " factura.moneda AS MonedaFactura,\n";

            vSql = vSql + " cobranza.Numero, \n";
            vSql = vSql + " CONVERT(varchar,dbo.Cobranza.Fecha,103) as Fecha, \n";
            vSql = vSql + " COUNT(dbo.factura.Numero) AS Total,\n";
            vSql = vSql + " dbo.factura.Numero AS NumeroFactura, \n";
            vSql = vSql + " cliente.Nombre AS NombreCliente, \n";
            //vSql = vSql + " cobranza.TotalCobrado AS MtoTotalCobrado, \n";
            vSql = vSql + " dbo.DocumentoCobrado.MontoAbonado AS MtoTotalCobrado, \n";

            vSql = vSql + " cobranza.CambioABolivares AS CambioABolivares, \n";
            vSql = vSql + " (SELECT CambioAMonedaLocal \n";
            vSql = vSql + " FROM Comun.Cambio \n";
            vSql = vSql + " WHERE CONVERT(varchar,FechaDeVigencia,103) =CONVERT(varchar,dbo.Cobranza.Fecha,103) \n";
            vSql = vSql + " AND CodigoMoneda ='USD') As Cambio, \n";

            vSql = vSql + " (SELECT  TOP 1 renglonFactura.PrecioConIVA FROM renglonFactura \n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + consecutivo.ToString() + " and NumeroFactura =dbo.factura.Numero) AS Precio, \n";

            vSql = vSql + " (SELECT  TOP 1 renglonFactura.PorcentajeDescuento FROM renglonFactura \n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + consecutivo.ToString() + " and NumeroFactura =dbo.factura.Numero) AS Descuento, \n";

            vSql = vSql + " (SELECT  TOP 1 renglonFactura.PrecioConIVA FROM renglonFactura \n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + consecutivo.ToString() + " and NumeroFactura =dbo.factura.Numero) - \n";

            vSql = vSql + " (((SELECT  TOP 1 renglonFactura.PrecioConIVA FROM renglonFactura \n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + consecutivo.ToString() + " and NumeroFactura =dbo.factura.Numero)  * \n";

            vSql = vSql + " (SELECT  TOP 1 renglonFactura.PorcentajeDescuento FROM renglonFactura \n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + consecutivo.ToString() + " and NumeroFactura =dbo.factura.Numero))/100) AS PrecioConDescuento, \n";

            vSql = vSql + " (SELECT  SUM(FacRen.Cantidad) FROM renglonFactura AS FacRen \n";
            vSql = vSql + " WHERE FacRen.ConsecutivoCompania = " + consecutivo.ToString() + " and FacRen.NumeroFactura =dbo.factura.Numero) AS TotalCantidad,\n";
            vSql = vSql + " dbo.factura.TotalFactura\n";


            vSql = vSql + " FROM dbo.Cobranza INNER JOIN  dbo.DocumentoCobrado \n";
            vSql = vSql + " ON dbo.Cobranza.ConsecutivoCompania = dbo.DocumentoCobrado.ConsecutivoCompania \n";
            vSql = vSql + " AND dbo.Cobranza.Numero = dbo.DocumentoCobrado.NumeroCobranza \n";

            vSql = vSql + " INNER JOIN dbo.CxC ON dbo.Cobranza.ConsecutivoCompania = dbo.CxC.ConsecutivoCompania \n";
            vSql = vSql + " AND dbo.DocumentoCobrado.NumeroDelDocumentoCobrado  like   dbo.CxC.NumeroDocumentoOrigen + '%' \n";
            vSql = vSql + " AND dbo.CxC.CodigoCliente = dbo.Cobranza.CodigoCliente \n";

            vSql = vSql + " INNER JOIN dbo.factura ON dbo.Cobranza.ConsecutivoCompania = dbo.factura.ConsecutivoCompania \n";
            vSql = vSql + " AND dbo.factura.Numero =dbo.CxC.NumeroDocumentoOrigen \n";
            vSql = vSql + " AND dbo.factura.CodigoCliente = dbo.CxC.CodigoCliente \n";
            vSql = vSql + " AND dbo.factura.CodigoVendedor = dbo.CxC.CodigoVendedor \n";

            vSql = vSql + " INNER JOIN dbo.renglonFactura ON dbo.renglonFactura.ConsecutivoCompania = dbo.factura.ConsecutivoCompania \n";
            vSql = vSql + " AND dbo.factura.Numero =dbo.renglonFactura.NumeroFactura \n";

            vSql = vSql + " INNER JOIN cliente ON cliente.Codigo = cobranza.CodigoCliente \n";
            vSql = vSql + " AND cliente.ConsecutivoCompania = cobranza.ConsecutivoCompania\n";

            vSql = vSql + " INNER JOIN Adm.Vendedor \n";
            vSql = vSql + " ON Adm.Vendedor.ConsecutivoCompania = dbo.factura.ConsecutivoCompania \n";
            vSql = vSql + " AND Adm.Vendedor.Codigo =dbo.factura.CodigoVendedor \n";
            vSql = vSql + "WHERE cobranza.Fecha BETWEEN  '" + FechaA + "'  AND '" + FechaB + "'\n";

            vSql = vSql + " AND cobranza.StatusCobranza = '0' \n";
            vSql = vSql + " AND vendedor.ConsecutivoCompania = " + consecutivo.ToString();
            vSql = vSql + " AND factura.StatusFactura = '0' AND factura.TipoDeDocumento <> '3' \n";

            //Cambio
            //vSql = vSql + " AND factura.Numero = 'NE110000697'\n";


            vSql = vSql + " GROUP BY \n";
            vSql = vSql + " cobranza.CodigoCobrador,\n";
            vSql = vSql + " vendedor.Nombre,\n";
            vSql = vSql + " Cobranza.moneda,\n";
            vSql = vSql + " factura.moneda,\n";
            vSql = vSql + " cobranza.Numero,\n";
            vSql = vSql + " dbo.Cobranza.Fecha,\n";
            vSql = vSql + " dbo.factura.Numero,\n";
            vSql = vSql + " cliente.Nombre,\n";
            vSql = vSql + " cobranza.TotalCobrado,\n";
            vSql = vSql + " cobranza.CambioABolivares,\n";
            vSql = vSql + " dbo.renglonFactura.ConsecutivoCompania,\n ";
            vSql = vSql + " dbo.factura.TotalFactura,\n";
            vSql = vSql + " dbo.DocumentoCobrado.MontoAbonado\n";

            vSql = vSql + " ORDER BY vendedor.Nombre, cobranza.CodigoCobrador, MonedaCobro, cobranza.Fecha, cobranza.Numero \n";

            return vSql;
        }


        private string SqlReporteCxC(int consecutivo)
        {
            string vSql = "";
             DateTime fechaNow = DateTime.Now;
             fechaNow.ToString("dd/MM/yyyy");

            vSql = vSql + " SET DATEFORMAT dmy \n";
            vSql = vSql + " SELECT Cxc.CodigoVendedor,\n";
            vSql = vSql + " LTRIM(RTRIM(CAST( Cxc.CodigoVendedor AS varchar))) + ' - ' + LTRIM(RTRIM(CAST(Vendedor.Nombre AS varchar))) AS NombreVendedor, \n"; 
            vSql = vSql + " Cxc.Moneda,  Cliente.Codigo AS CodigoCliente, \n";
            vSql = vSql + " LTRIM(RTRIM(CAST( Cliente.Codigo AS varchar))) + ' - ' + LTRIM(RTRIM(CAST( Cliente.Nombre AS varchar))) AS NombreCliente, \n";
            vSql = vSql + " ( CASE  WHEN CxC.Origen = '0' THEN (CxC.Numero \n";
            vSql = vSql + " + ' -' + CAST(Descripcion AS VARCHAR(5)))  ELSE CxC.Numero + ''  END ) AS Numero,\n";
            vSql = vSql + " cxc.NumeroDocumentoOrigen As NumeroFactura,\n";
            vSql = vSql + " (SELECT SUM(cantidad) from renglonFactura WHERE NumeroFactura = cxc.NumeroDocumentoOrigen) AS Cantidad,\n";

            vSql = vSql + "  CONVERT(varchar,Cxc.Fecha,103)  AS FechaDoc,\n";
            vSql = vSql + " CONVERT(varchar,Cxc.FechaVencimiento,103)  AS FechaVencimiento ,  DATEDIFF(d, Cxc.FechaVencimiento, '" + fechaNow.ToString("dd/MM/yyyy") + "')  AS DiasVencidos, \n"; 
            vSql = vSql + " (MontoExento + MontoGravado  + MontoIva ) AS MontoTotal, \n";
            vSql = vSql + " Cxc.MontoAbonado AS MontoAbonado, \n";
            vSql = vSql + " ((MontoExento + MontoGravado  + MontoIva ) - Cxc.MontoAbonado) \n"; 
            vSql = vSql + " AS MontoRestante \n"; 
            vSql = vSql + " FROM Cliente INNER JOIN \n"; 
            vSql = vSql + " ( Vendedor INNER JOIN  Cxc ON ( Vendedor.Codigo =  Cxc.CodigoVendedor) \n"; 
            vSql = vSql + " AND ( Vendedor.ConsecutivoCompania = Cxc.ConsecutivoCompania)) \n";
            vSql = vSql + " ON ( Cliente.Codigo = Cxc.CodigoCliente) \n";  
            vSql = vSql + " AND ( Cliente.ConsecutivoCompania =  Cxc.ConsecutivoCompania) \n"; 
            vSql = vSql + " WHERE  Cxc.ConsecutivoCompania =" + consecutivo.ToString() + " AND  Cxc.Status IN ('0', '3') \n";

            //vSql = vSql + " AND  cxc.NumeroDocumentoOrigen ='NE110000794' \n";
 
            vSql = vSql + " ORDER BY  CxC.CodigoVendedor,  CxC.Moneda, \n";
            vSql = vSql + " CxC.FechaVencimiento \n"; 


            return vSql;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string vSql = "";
            string fechainicio = dtpFechaIni.Text;
            string ferchafin = dtpFechaFin.Text;
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
                if (button1.Text == "Comisiones Detallada")
                {

                    vSql = SqlReporteComision(fechainicio, ferchafin, consecutivocompania);

                    CreateCSVFile(CreateTablaFactura(vSql));
                }
                if (button1.Text == "Comisiones Resumidas")
                {

                    string FechaReporte = "del " + fechainicio + " al " + ferchafin;
                    vSql = SqlReporteComisionResumen(fechainicio, ferchafin, consecutivocompania);
                    consecutivo = consecutivocompania;
                    
                    DataTable FacturaTableTotal;
                    DataTable FacturaTableProtector;

                    FacturaTableTotal = CreateTablaFacturaResumen(vSql);

                    FacturaTableProtector = CreateTablaFacturaResumenCategoriaGene(FacturaTableTotal);



                    CreateCSVFileResumen(FacturaTableProtector,FacturaTableTotal, NombreCompania, FechaReporte);
                }

                if (button1.Text == "Cuenta por Cobrar Vendedor")
                {

                    string FechaReporte = "Emitodo el " + fechainicio;
                    vSql = SqlReporteCxC(consecutivocompania);
                    consecutivo = consecutivocompania;

                    DataTable FacturaTableProtector;
                    DataTable FacturaTableSERRA;

                    FacturaTableProtector = CreateTablaCxCProtectora(vSql);
                    FacturaTableSERRA = CreateTablaCxCSERRA(vSql);

                    CreateCSVFileCxC(FacturaTableProtector, FacturaTableSERRA, NombreCompania, FechaReporte);
                }


            }
        }
        
        private DataTable CreateTablaFactura(string vSql)
        {
            DataTable FacturaTable = new DataTable("Factura");
            DataColumn dtColumn;
            DataRow myDataRow;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreVendedor";
            dtColumn.Caption = "Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Cantidad";
            dtColumn.Caption = "Cantidad";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Articulo";
            dtColumn.Caption = "Descripción";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "PrecioConIVA";
            dtColumn.Caption = "Precio x kg";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Total";
            dtColumn.Caption = "Total";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoTotalDeCxC";
            dtColumn.Caption = "Abono $";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "kgAbonados";
            dtColumn.Caption = "kg abonados";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Resta";
            dtColumn.Caption = "Resta $";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "TotalkgPendiente";
            dtColumn.Caption = "Total kg pendientes";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                string vNumero = "";
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        string vNombreVendedor = reader["Nombre"].ToString();
                        string vCantidad = reader["Cantidad"].ToString();
                        string vPrecioConIVA = reader["PrecioConIVA"].ToString();

                        string vArticulo = reader["Descripcion"].ToString();
                        string vFactura = reader["Factura"].ToString();
                        string vFecha = reader["Fecha"].ToString();
                        string vCXCobrada = reader["CXCobrada"].ToString();
                        string vCodigoMoneda = reader["CodigoMoneda"].ToString();
                        string vCodigoMonedaDeCxC = reader["CodigoMonedaDeCxC"].ToString();
                        string vCambio = reader["Cambio"].ToString();
                        string vTotalRenglon = reader["TotalFactura"].ToString();
                        string vTotalPrecio = reader["TotalPrecio"].ToString();
                        string vTotalCantidad = reader["TotalCantidad"].ToString();
                        string vTotal = reader["TotalRenglon"].ToString();

                        

                        decimal Totalprecio = 0;

                        myDataRow = FacturaTable.NewRow();
                        myDataRow["NombreVendedor"] = vNombreVendedor;
                        myDataRow["Cantidad"] = vCantidad;
                        myDataRow["Articulo"] = vArticulo;
                        //myDataRow["PrecioConIVA"] = vPrecioConIVA;
                        if (reader["CodigoMoneda"].ToString() == "VED")
                        {
                            myDataRow["PrecioConIVA"] = Convert.ToDecimal(vPrecioConIVA) / Convert.ToDecimal(vCambio);
                        }
                        else
                        {
                            myDataRow["PrecioConIVA"] = Convert.ToDecimal(vPrecioConIVA);
                        }


                        //if (reader["CodigoMoneda"].ToString() == "VED")
                        //{
                        //    myDataRow["Total"] = (Convert.ToDecimal(vCantidad) * (Convert.ToDecimal(vPrecioConIVA) / Convert.ToDecimal(vCambio)));
                        //}
                        //else
                        //{
                        //    myDataRow["Total"] = Convert.ToDecimal(vCantidad) * Convert.ToDecimal(vPrecioConIVA);
                        //}

                        if (reader["CodigoMoneda"].ToString() == "VED")
                        {
                            myDataRow["Total"] = (Convert.ToDecimal(vTotal)/ Convert.ToDecimal(vCambio));
                        }
                        else
                        {
                            myDataRow["Total"] = Convert.ToDecimal(vTotal);
                        }
                        

                        if (vNumero != reader["Numero"].ToString())
                        {

                            Totalprecio = Convert.ToDecimal(vTotalPrecio);
                            decimal montoCxC = Convert.ToDecimal(vCXCobrada);
                            decimal cantidadfactura = Convert.ToDecimal(vTotalCantidad);
                            if (reader["CodigoMonedaDeCxC"].ToString() == "VED")
                            {
                                montoCxC = montoCxC / Convert.ToDecimal(vCambio);
                            }

                            if (reader["CodigoMoneda"].ToString() == "VED")
                            {
                                Totalprecio = Totalprecio / Convert.ToDecimal(vCambio);
                            }
                            decimal TotalFactura = Convert.ToDecimal(vTotalRenglon);
                            if (reader["CodigoMoneda"].ToString() == "VED")
                            {
                                TotalFactura = TotalFactura / Convert.ToDecimal(vCambio);
                            }

                            myDataRow["MontoTotalDeCxC"] = montoCxC;
                            myDataRow["kgAbonados"] = (montoCxC * cantidadfactura) / (TotalFactura);
                            myDataRow["Resta"] = TotalFactura - montoCxC;


                            decimal Pendiente = cantidadfactura - (montoCxC * cantidadfactura) / (TotalFactura);

                            myDataRow["TotalkgPendiente"] = Pendiente;

                        }
                        else {
                            Totalprecio = Totalprecio + Convert.ToDecimal(vPrecioConIVA);
                            myDataRow["MontoTotalDeCxC"] = Convert.ToDecimal("0");
                            myDataRow["kgAbonados"] = Convert.ToDecimal("0");
                            myDataRow["Resta"] = Convert.ToDecimal("0");
                            myDataRow["TotalkgPendiente"] = Convert.ToDecimal("0");
                        }
                        vNumero = reader["Numero"].ToString();

                        FacturaTable.Rows.Add(myDataRow);
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return FacturaTable;
        }
        
        private DataTable CreateTablaFacturaResumen(string vSql)
        {
            DataTable FacturaTable = new DataTable("Factura");
            DataColumn dtColumn;
            DataRow myDataRow;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CodigoVendedor";
            dtColumn.Caption = "Código Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreVendedor";
            dtColumn.Caption = "Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NumeroCobranza";
            dtColumn.Caption = "N° Cobranza";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NumeroFactura";
            dtColumn.Caption = "N° Factura";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Fecha";
            dtColumn.Caption = "Fecha";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Cliente";
            dtColumn.Caption = "Cliente";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Categoria";
            dtColumn.Caption = "Categoria";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoCobranza";
            dtColumn.Caption = "Monto Cobranza";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "TASA";
            dtColumn.Caption = "TASA";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "COSTO";
            dtColumn.Caption = "COSTO";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "PAGO";
            dtColumn.Caption = "PAGO $";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "KILOS";
            dtColumn.Caption = "KILOS";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "COMISION";
            dtColumn.Caption = "COMISION";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MonedaCobro";
            dtColumn.Caption = "MonedaCobro";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTable.Columns.Add(dtColumn);

            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
               
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        string vCodigoVendedor = reader["CodigoCobrador"].ToString();
                        string vNombreVendedor = reader["NombreVendedor"].ToString();
                        string vMonedaCobro = reader["MonedaCobro"].ToString();
                        string vNumeroCobranza = reader["Numero"].ToString();
                      
                        string vFecha = reader["Fecha"].ToString();
                        string vTotalFactura = reader["Total"].ToString();
                        string vNumeroFactura = reader["NumeroFactura"].ToString();
                        string vNombreCliente = reader["NombreCliente"].ToString();
                        string vMtoTotalCobrado = reader["MtoTotalCobrado"].ToString();
                        decimal vMtoTotalCobrado2 = Convert.ToDecimal(reader["MtoTotalCobrado"].ToString());

                        string vCambioABolivares = reader["Cambio"].ToString();
                        string vTotalPrecio = reader["Precio"].ToString();
                        string vDescuento = reader["Descuento"].ToString();
                        string vPrecioConDescuento = reader["PrecioConDescuento"].ToString();
                        string vTotalCantidad = reader["TotalCantidad"].ToString();
                        decimal vTotalCantidad2 = 0;

                        decimal vMtoTotalFactura = Convert.ToDecimal(reader["TotalFactura"].ToString());
                        

                        myDataRow = FacturaTable.NewRow();
                        myDataRow["CodigoVendedor"] = vCodigoVendedor;
                        myDataRow["NombreVendedor"] = vNombreVendedor;
                        myDataRow["NumeroCobranza"] = vNumeroCobranza;
                        myDataRow["NumeroFactura"] = vNumeroFactura;

                        myDataRow["Fecha"] = vFecha;
                        myDataRow["Cliente"] = vNombreCliente;
                        myDataRow["MontoCobranza"] = vMtoTotalCobrado;
                        myDataRow["TASA"] = vCambioABolivares;
                        myDataRow["MonedaCobro"] = vMonedaCobro;
 
                        if (reader["MonedaCobro"].ToString() == "Bolívar")
                        {
                            myDataRow["PAGO"] = Convert.ToDecimal(vMtoTotalCobrado) / Convert.ToDecimal(vCambioABolivares);
                            vMtoTotalCobrado2 = Convert.ToDecimal(vMtoTotalCobrado) / Convert.ToDecimal(vCambioABolivares);
                        
                        }
                        else {
                            myDataRow["PAGO"] = vMtoTotalCobrado;
                        }


                       
                        if (reader["MonedaFactura"].ToString() == "Bolívar")
                        {
                            myDataRow["COSTO"] = Convert.ToDecimal(vPrecioConDescuento) / Convert.ToDecimal(vCambioABolivares);
                        }
                        else
                        {
                            myDataRow["COSTO"] = vPrecioConDescuento;
                        }
                        string CategoriaInicial = sBucarCategoriaInicial(vNumeroFactura,consecutivo.ToString());
                        string MonedaFactura = reader["MonedaFactura"].ToString();

                        myDataRow["Categoria"] = CategoriaInicial;


                        if (reader["MonedaFactura"].ToString() == "Bolívar")
                        {
                            vMtoTotalFactura = vMtoTotalFactura / Convert.ToDecimal(vCambioABolivares);
                        }

                        vTotalCantidad2 = (vMtoTotalCobrado2 * Convert.ToDecimal(vTotalCantidad)) / vMtoTotalFactura;
                        decimal TotalCantidadTotal = 0;
                        TotalCantidadTotal = vTotalCantidad2;
                        //Cambios por categoria
                        vTotalCantidad2 = Convert.ToDecimal(sBucarCantidad(vNumeroFactura, consecutivo.ToString(), CategoriaInicial));
                        vTotalCantidad2 = (vMtoTotalCobrado2 * Convert.ToDecimal(vTotalCantidad2)) / vMtoTotalFactura;


                        //myDataRow["KILOS"] = vTotalCantidad;
                        myDataRow["KILOS"] = vTotalCantidad2;

                        myDataRow["COMISION"] = 0;
                        FacturaTable.Rows.Add(myDataRow);

                        myDataRow["MontoCobranza"] = (Convert.ToDecimal(vMtoTotalCobrado) * vTotalCantidad2) / TotalCantidadTotal;

                        myDataRow["PAGO"] = (vMtoTotalCobrado2 * vTotalCantidad2) / TotalCantidadTotal;
 

                        ////Agregar los otros Tipos Categoria
                        if (CategoriaInicial.CompareTo("CATEGORIA")==-1)
                        {
                            string TIPOCategoria = sBucarCategoria(vNumeroFactura, consecutivo.ToString(), "CATEGORIA");
                            if (TIPOCategoria != "") {
                                myDataRow = FacturaTable.NewRow();
                                   vPrecioConDescuento="";
                                   vPrecioConDescuento = sBucarCosto(vNumeroFactura, consecutivo.ToString(), "CATEGORIA");
                                   myDataRow["Categoria"] = "CATEGORIA";


                                   myDataRow["MontoCobranza"] = 0;
                                   myDataRow["TASA"] = vCambioABolivares;
                                   myDataRow["KILOS"] = 0;
                                 
                                   myDataRow["COMISION"] = 0;
                                   myDataRow["PAGO"] = 0;

                                  
                                   myDataRow["CodigoVendedor"] = vCodigoVendedor;
                                   myDataRow["NombreVendedor"] = vNombreVendedor;
                                   myDataRow["NumeroCobranza"] = vNumeroCobranza;
                                   myDataRow["NumeroFactura"] = vNumeroFactura;

                                   myDataRow["MonedaCobro"] = vMonedaCobro;


                                   if (MonedaFactura == "Bolívar")
                                   {
                                       myDataRow["COSTO"] = Convert.ToDecimal(vPrecioConDescuento) / Convert.ToDecimal(vCambioABolivares);
                                   }
                                   else
                                   {
                                       myDataRow["COSTO"] = vPrecioConDescuento;
                                   }
                                   vTotalCantidad2 = 0;
                                   vTotalCantidad2 = Convert.ToDecimal(sBucarCantidad(vNumeroFactura, consecutivo.ToString(),"CATEGORIA"));
                                   vTotalCantidad2 = (vMtoTotalCobrado2 * Convert.ToDecimal(vTotalCantidad2)) / vMtoTotalFactura;

                                   myDataRow["KILOS"] = vTotalCantidad2;
                                   myDataRow["MontoCobranza"] = (Convert.ToDecimal(vMtoTotalCobrado) * vTotalCantidad2) / TotalCantidadTotal;
                                   myDataRow["PAGO"] = (vMtoTotalCobrado2 * vTotalCantidad2) / TotalCantidadTotal;
 
                       

                                FacturaTable.Rows.Add(myDataRow);
                            }
                        }

                        if (CategoriaInicial.CompareTo("50Y100") == -1)
                        {
                            string TIPOCategoria = sBucarCategoria(vNumeroFactura, consecutivo.ToString(), "50Y100");
                            if (TIPOCategoria != "")
                            {
                                myDataRow = FacturaTable.NewRow();
                                vPrecioConDescuento = "";
                                vPrecioConDescuento = sBucarCosto(vNumeroFactura, consecutivo.ToString(), "50Y100");
                                myDataRow["Categoria"] = "50Y100";

                                myDataRow["MontoCobranza"] = 0;
                                myDataRow["TASA"] = vCambioABolivares;
                                myDataRow["KILOS"] = 0;

                                myDataRow["COMISION"] = 0;
                                myDataRow["PAGO"] = 0;

                                myDataRow["CodigoVendedor"] = vCodigoVendedor;
                                myDataRow["NombreVendedor"] = vNombreVendedor;
                                myDataRow["NumeroCobranza"] = vNumeroCobranza;
                                myDataRow["NumeroFactura"] = vNumeroFactura;

                                myDataRow["MonedaCobro"] = vMonedaCobro;

                                if (MonedaFactura == "Bolívar")
                                {
                                    myDataRow["COSTO"] = Convert.ToDecimal(vPrecioConDescuento) / Convert.ToDecimal(vCambioABolivares);
                                }
                                else
                                {
                                    myDataRow["COSTO"] = vPrecioConDescuento;
                                }
                                vTotalCantidad2 = 0;
                                vTotalCantidad2 = Convert.ToDecimal(sBucarCantidad(vNumeroFactura, consecutivo.ToString(), "50Y100"));
                                vTotalCantidad2 = (vMtoTotalCobrado2 * Convert.ToDecimal(vTotalCantidad2)) / vMtoTotalFactura;

                                myDataRow["KILOS"] = vTotalCantidad2;
                                myDataRow["MontoCobranza"] = (Convert.ToDecimal(vMtoTotalCobrado) * vTotalCantidad2) / TotalCantidadTotal;
                                myDataRow["PAGO"] = (vMtoTotalCobrado2 * vTotalCantidad2) / TotalCantidadTotal;
 

                                FacturaTable.Rows.Add(myDataRow);
                            }
                        }

                        if (CategoriaInicial.CompareTo("GRANO") == -1)
                        {
                            string TIPOCategoria = sBucarCategoria(vNumeroFactura, consecutivo.ToString(), "GRANO");
                            if (TIPOCategoria != "")
                            {
                                myDataRow = FacturaTable.NewRow();
                                vPrecioConDescuento = "";
                                vPrecioConDescuento = sBucarCosto(vNumeroFactura, consecutivo.ToString(), "GRANO");

                                myDataRow["Categoria"] = "GRANO";

                                myDataRow["MontoCobranza"] = 0;
                                myDataRow["TASA"] = vCambioABolivares;
                                myDataRow["KILOS"] = 0;

                                myDataRow["COMISION"] = 0;
                                myDataRow["PAGO"] = 0;
                                myDataRow["CodigoVendedor"] = vCodigoVendedor;
                                myDataRow["NombreVendedor"] = vNombreVendedor;
                                myDataRow["NumeroCobranza"] = vNumeroCobranza;
                                myDataRow["NumeroFactura"] = vNumeroFactura;

                                myDataRow["MonedaCobro"] = vMonedaCobro;

                                if (MonedaFactura == "Bolívar")
                                {
                                    myDataRow["COSTO"] = Convert.ToDecimal(vPrecioConDescuento) / Convert.ToDecimal(vCambioABolivares);
                                }
                                else
                                {
                                    myDataRow["COSTO"] = vPrecioConDescuento;
                                }
                                vTotalCantidad2 = 0;
                                vTotalCantidad2 = Convert.ToDecimal(sBucarCantidad(vNumeroFactura, consecutivo.ToString(), "GRANO"));
                                vTotalCantidad2 = (vMtoTotalCobrado2 * Convert.ToDecimal(vTotalCantidad2)) / vMtoTotalFactura;

                                myDataRow["KILOS"] = vTotalCantidad2;
                                myDataRow["MontoCobranza"] = (Convert.ToDecimal(vMtoTotalCobrado) * vTotalCantidad2) / TotalCantidadTotal;
                                myDataRow["PAGO"] = (vMtoTotalCobrado2 * vTotalCantidad2) / TotalCantidadTotal;
 

                                FacturaTable.Rows.Add(myDataRow);
                            }
                        }

                        if (CategoriaInicial.CompareTo("SERRA") == -1)
                        {
                            string TIPOCategoria = sBucarCategoria(vNumeroFactura, consecutivo.ToString(), "SERRA");
                            if (TIPOCategoria != "")
                            {
                                myDataRow = FacturaTable.NewRow();
                                vPrecioConDescuento = "";
                                vPrecioConDescuento = sBucarCosto(vNumeroFactura, consecutivo.ToString(), "SERRA");
                                myDataRow["Categoria"] = "SERRA";
                                myDataRow["MontoCobranza"] = 0;
                                myDataRow["TASA"] = vCambioABolivares;
                                myDataRow["KILOS"] = 0;

                                myDataRow["COMISION"] = 0;
                                myDataRow["PAGO"] = 0;
                                myDataRow["CodigoVendedor"] = vCodigoVendedor;
                                myDataRow["NombreVendedor"] = vNombreVendedor;
                                myDataRow["NumeroCobranza"] = vNumeroCobranza;
                                myDataRow["NumeroFactura"] = vNumeroFactura;

                                myDataRow["MonedaCobro"] = vMonedaCobro;

                                if (MonedaFactura == "Bolívar")
                                {
                                    myDataRow["COSTO"] = Convert.ToDecimal(vPrecioConDescuento) / Convert.ToDecimal(vCambioABolivares);
                                }
                                else
                                {
                                    myDataRow["COSTO"] = vPrecioConDescuento;
                                }
                                vTotalCantidad2 = 0;
                                vTotalCantidad2 = Convert.ToDecimal(sBucarCantidad(vNumeroFactura, consecutivo.ToString(), "SERRA"));
                                vTotalCantidad2 = (vMtoTotalCobrado2 * Convert.ToDecimal(vTotalCantidad2)) / vMtoTotalFactura;

                                myDataRow["KILOS"] = vTotalCantidad2;
                                myDataRow["MontoCobranza"] = (Convert.ToDecimal(vMtoTotalCobrado) * vTotalCantidad2) / TotalCantidadTotal;
                                myDataRow["PAGO"] = (vMtoTotalCobrado2 * vTotalCantidad2) / TotalCantidadTotal;
 
                                FacturaTable.Rows.Add(myDataRow);
                            }
                        }


                    }
                    reader.NextResult();
                }


                connection.Close();
            }
            return FacturaTable;
        }
        
        public void CreateCSVFile(DataTable dataTable)
        {

            var lines = new List<string>();

            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                                 Select(column => column.ColumnName).
                                                 ToArray();

            var header = string.Join(",", columnNames);
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable()
                                  .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);


            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();

                hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                hoja_trabajo.Cells[3, 4] = "Comisión por Vendedor DISTRIBUIDORA GERMOCA 2022, C.A.";
                hoja_trabajo.Cells[3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[3, 4].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[3, 4].Font.Bold = true;
              
                int FilaInicio = 5;
                int CampoInicio = 2;
                hoja_trabajo.Cells[FilaInicio, CampoInicio] = "Vendedor";
                hoja_trabajo.Cells[FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].ColumnWidth = 20;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1] = "Cantidad kg";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].ColumnWidth = 15;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2] = "Descripcion";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].ColumnWidth = 75;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3] = "Precio x kg";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].ColumnWidth = 10;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4] = "Total ";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].ColumnWidth = 10;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5] = "Abono $";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].ColumnWidth = 10;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6] = "kg Abonados";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].ColumnWidth = 15;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7] = "Resta $";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].ColumnWidth = 10;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8] = "Kg Pendientes";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, 8].ColumnWidth = 25;


                labelProgreso.Text = "100%";
                labelProgreso.Refresh();
                progressBar1.Minimum = 1;
                int total = dataTable.Rows.Count;
                progressBar1.Maximum = total;

                progressBar1.Step = 1;

                FilaInicio = FilaInicio + 1;
                string vVendedor = "";
                decimal TotalCantidad = 0;
                decimal TotalRenglon = 0;
                decimal TotalCobrado = 0;
                decimal TotalPendiente = 0;

                int registros = 0;
                // Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < dataTable.Rows.Count; i++)  // fila
                {
                    progressBar1.Value = i + 1;
                    labelProgreso.Text = Convert.ToString(((i + 1) * 100) / total) + "%";
                    labelProgreso.Refresh();
                    registros++;
                    for (int j = 0; j < dataTable.Columns.Count; j++) //columna
                    {
                        if (j == 0 )
                        {

                            if (vVendedor != dataTable.Rows[i][j].ToString())
                            {
                                
                                if (i != 0) {
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j] = "Total : ";
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j].Font.Bold = true;
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;


                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].NumberFormat = "@";
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Font.Bold = true;
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = TotalCantidad.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                                    TotalCantidad = 0;

                                    //hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].NumberFormat = "@";
                                    //hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Font.Bold = true;
                                    //hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4] = TotalRenglon.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                    //hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                    //hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                                    TotalRenglon = 0;

                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].NumberFormat = "@";
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Font.Bold = true;
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5] = TotalCobrado.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                                    TotalCobrado = 0;

                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].NumberFormat = "@";
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Font.Bold = true;
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6] = TotalPendiente.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                    hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                                    TotalPendiente = 0;

                                    FilaInicio = FilaInicio + 2;
                                }
                                vVendedor = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;
                            }
                        }
                        else if (j == 2)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j] =  dataTable.Rows[i][j].ToString();

                 
                        }
                        else
                        {
                            decimal numero = Convert.ToDecimal(dataTable.Rows[i][j].ToString());
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + j].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                 
                        }
                        ///Calculo de los Totales
                        if (j == 1) // Cantidad
                        {
                            if (vVendedor == dataTable.Rows[i][0].ToString())
                            {
                                TotalCantidad = TotalCantidad + Convert.ToDecimal(dataTable.Rows[i][1].ToString());
                            }
                         }

                        ///Calculo de los Totales
                        if (j == 4) // TotalFactura
                        {
                            if (vVendedor == dataTable.Rows[i][0].ToString())
                            {
                                TotalRenglon = TotalRenglon + Convert.ToDecimal(dataTable.Rows[i][4].ToString());
                            }
                        }
                        ///Calculo de los Totales
                        if (j == 5) // Totalcobrado
                        {
                            if (vVendedor == dataTable.Rows[i][0].ToString())
                            {
                                TotalCobrado = TotalCobrado + Convert.ToDecimal(dataTable.Rows[i][5].ToString());
                            }
                        }
                        ///Calculo de los Totales
                        if (j == 6) // TotalPendiente
                        {
                            if (vVendedor == dataTable.Rows[i][0].ToString())
                            {
                                TotalPendiente = TotalPendiente + Convert.ToDecimal(dataTable.Rows[i][6].ToString());
                            }
                        }

                    }
                }
                
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio ] = "Total : ";
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio ].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio ].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;


                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 1].NumberFormat = "@";
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 1].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 1] = TotalCantidad.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 1].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                TotalCantidad = 0;

                //hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 4].NumberFormat = "@";
                //hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 4].Font.Bold = true;
                //hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 4] = TotalRenglon.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                //hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                //hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 4].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                TotalRenglon = 0;

                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 5].NumberFormat = "@";
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 5].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 5] = TotalCobrado.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 5].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                TotalCobrado = 0;

                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 6].NumberFormat = "@";
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 6].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 6] = TotalPendiente.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbYellow;

                TotalPendiente = 0;




                progressBar1.Minimum = 1;
                progressBar1.Value = 1;
                labelProgreso.Text = "0%";
                labelProgreso.Refresh();

                libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
                // abrimos el excel
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = fichero.FileName;
                Process.Start(startInfo);

            }
        }

        private DataTable CreateTablaCxCProtectora(string vSql)
        {
            DataTable CxCTable = new DataTable("CxC");
            DataColumn dtColumn;
            DataRow myDataRow;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CodigoVendedor";
            dtColumn.Caption = "CodigoVendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreVendedor";
            dtColumn.Caption = "Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Moneda";
            dtColumn.Caption = "Moneda";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreCliente";
            dtColumn.Caption = "NombreCliente";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NroCxC";
            dtColumn.Caption = "NroCxC";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FechaDoc";
            dtColumn.Caption = "FechaDoc";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FechaVenc";
            dtColumn.Caption = "Fecha Vto";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Dias";
            dtColumn.Caption = "Dias";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "TotalCxC";
            dtColumn.Caption = "Total CxC";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoAbonado";
            dtColumn.Caption = "MontoAbonado";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoRestante";
            dtColumn.Caption = "MontoRestante";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);


            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {

                       
                        string categoria="50Y100','CATEGORIA','GRANO";
                        string vNumeroFactura = reader["NumeroFactura"].ToString();
                        decimal vTotalCantidad2 = Convert.ToDecimal(sBucarCantidadCategoria(vNumeroFactura, consecutivo.ToString(),categoria));
                        if (vTotalCantidad2 > 0) {
                            string vCodigoVendedor = reader["CodigoVendedor"].ToString();
                            string vNombreVendedor = reader["NombreVendedor"].ToString();
                            string vMoneda = reader["Moneda"].ToString();
                            string vMonedaFactura = sBucarMonedaFactura(vNumeroFactura, consecutivo.ToString()); 
                            string vCambioCxC = "";

                            string vCodigoCliente = reader["CodigoCliente"].ToString();
                            string vNombreCliente = reader["NombreCliente"].ToString();
                            string vNumero = reader["Numero"].ToString();

                            string vCantidad = reader["Cantidad"].ToString();

                            string vFechaDoc = reader["FechaDoc"].ToString();
                            string vFechaVencimiento = reader["FechaVencimiento"].ToString();
                            string vDiasVencidos = reader["DiasVencidos"].ToString();
                            string vMontoTotal = reader["MontoTotal"].ToString();
                            decimal vMontoTotal2 = Convert.ToDecimal(sBucarMontoFacturadoCategoria(vNumeroFactura, consecutivo.ToString(), categoria));

                            string vMontoAbonado = reader["MontoAbonado"].ToString();
                            string vMontoRestante = reader["MontoRestante"].ToString();

                            myDataRow = CxCTable.NewRow();

                            myDataRow["CodigoVendedor"] = vCodigoVendedor;
                            myDataRow["NombreVendedor"] = vNombreVendedor;
                            myDataRow["Moneda"] = vMoneda;

                            myDataRow["NombreCliente"] = vNombreCliente;
                            myDataRow["NroCxC"] = vNumero;

                            myDataRow["FechaDoc"] = vFechaDoc;
                            myDataRow["FechaVenc"] = vFechaVencimiento;


                            myDataRow["Dias"] = vDiasVencidos;
                            myDataRow["TotalCxC"] = vMontoTotal;
                            myDataRow["TotalCxC"] = (Convert.ToDecimal(vMontoTotal) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);
                            //cambio factura
                            if (vMoneda !=vMonedaFactura){
                                vCambioCxC = sBucarCambioFactura(vNumeroFactura, consecutivo.ToString());
                                if (vMonedaFactura == "Bolívar")
                                {
                                    vMontoTotal2 = vMontoTotal2 / Convert.ToDecimal(vCambioCxC);

                                }
                                else {
                                    vMontoTotal2 = vMontoTotal2 * Convert.ToDecimal(vCambioCxC);
                                }
 
                            }
                            myDataRow["TotalCxC"] = vMontoTotal2;

                            
                            myDataRow["MontoAbonado"] = vMontoAbonado;
                            myDataRow["MontoAbonado"] = (Convert.ToDecimal(vMontoAbonado) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);
                            
                            myDataRow["MontoRestante"] = vMontoRestante;
                            myDataRow["MontoRestante"] = (Convert.ToDecimal(vMontoRestante) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);
                            //cambio factura
                            myDataRow["MontoRestante"] = vMontoTotal2 - Convert.ToDecimal(vMontoAbonado);
                            myDataRow["MontoRestante"] = vMontoTotal2 - (Convert.ToDecimal(vMontoAbonado) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);



                            

                            CxCTable.Rows.Add(myDataRow);
                        
                        }

                        
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return CxCTable;
        }

        private DataTable CreateTablaCxCSERRA(string vSql)
        {
            DataTable CxCTable = new DataTable("CxC");
            DataColumn dtColumn;
            DataRow myDataRow;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CodigoVendedor";
            dtColumn.Caption = "CodigoVendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreVendedor";
            dtColumn.Caption = "Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Moneda";
            dtColumn.Caption = "Moneda";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreCliente";
            dtColumn.Caption = "NombreCliente";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NroCxC";
            dtColumn.Caption = "NroCxC";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FechaDoc";
            dtColumn.Caption = "FechaDoc";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FechaVenc";
            dtColumn.Caption = "Fecha Vto";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Dias";
            dtColumn.Caption = "Dias";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "TotalCxC";
            dtColumn.Caption = "Total CxC";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoAbonado";
            dtColumn.Caption = "MontoAbonado";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoRestante";
            dtColumn.Caption = "MontoRestante";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            CxCTable.Columns.Add(dtColumn);


            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {


                        string vNumeroFactura = reader["NumeroFactura"].ToString();
                        decimal vTotalCantidad2 = Convert.ToDecimal(sBucarCantidadCategoria(vNumeroFactura, consecutivo.ToString(), "SERRA"));
                        if (vTotalCantidad2 > 0)
                        {
                            string vCodigoVendedor = reader["CodigoVendedor"].ToString();
                            string vNombreVendedor = reader["NombreVendedor"].ToString();
                            string vMoneda = reader["Moneda"].ToString();
                            string vMonedaFactura = sBucarMonedaFactura(vNumeroFactura, consecutivo.ToString());
                            string vCambioCxC = "";

                            string vCodigoCliente = reader["CodigoCliente"].ToString();
                            string vNombreCliente = reader["NombreCliente"].ToString();
                            string vNumero = reader["Numero"].ToString();

                            string vCantidad = reader["Cantidad"].ToString();

                            string vFechaDoc = reader["FechaDoc"].ToString();
                            string vFechaVencimiento = reader["FechaVencimiento"].ToString();
                            string vDiasVencidos = reader["DiasVencidos"].ToString();
                            string vMontoTotal = reader["MontoTotal"].ToString();
                            decimal vMontoTotal2 = Convert.ToDecimal(sBucarMontoFacturadoCategoria(vNumeroFactura, consecutivo.ToString(), "SERRA"));

                            string vMontoAbonado = reader["MontoAbonado"].ToString();
                            string vMontoRestante = reader["MontoRestante"].ToString();

                            myDataRow = CxCTable.NewRow();

                            myDataRow["CodigoVendedor"] = vCodigoVendedor;
                            myDataRow["NombreVendedor"] = vNombreVendedor;
                            myDataRow["Moneda"] = vMoneda;

                            myDataRow["NombreCliente"] = vNombreCliente;
                            myDataRow["NroCxC"] = vNumero;
                            myDataRow["FechaDoc"] = vFechaDoc;
                            myDataRow["FechaVenc"] = vFechaVencimiento;


                            myDataRow["Dias"] = vDiasVencidos;
                            myDataRow["TotalCxC"] = vMontoTotal;
                            myDataRow["TotalCxC"] = (Convert.ToDecimal(vMontoTotal) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);
                            //Cambio factura
                            if (vMoneda != vMonedaFactura)
                            {
                                vCambioCxC = sBucarCambioFactura(vNumeroFactura, consecutivo.ToString());
                                if (vMonedaFactura == "Bolívar")
                                {
                                    vMontoTotal2 = vMontoTotal2 / Convert.ToDecimal(vCambioCxC);

                                }
                                else
                                {
                                    vMontoTotal2 = vMontoTotal2 * Convert.ToDecimal(vCambioCxC);
                                }

                            }
                            myDataRow["TotalCxC"] = vMontoTotal2;
                            

                            myDataRow["MontoAbonado"] = vMontoAbonado;
                            myDataRow["MontoAbonado"] = (Convert.ToDecimal(vMontoAbonado) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);

                            myDataRow["MontoRestante"] = vMontoRestante;
                            myDataRow["MontoRestante"] = (Convert.ToDecimal(vMontoRestante) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);
                            //cambio factura
                            myDataRow["MontoRestante"] = vMontoTotal2 - Convert.ToDecimal(vMontoAbonado);
                            myDataRow["MontoRestante"] = vMontoTotal2 - (Convert.ToDecimal(vMontoAbonado) * vTotalCantidad2) / Convert.ToDecimal(vCantidad);


                            CxCTable.Rows.Add(myDataRow);

                        }


                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return CxCTable;
        }
        

        public void CreateCSVFileResumen(DataTable dataTable, DataTable dataTableTotal, string NombreCompania, string FechaNombre)
        {

            var lines = new List<string>();

            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                                 Select(column => column.ColumnName).
                                                 ToArray();

            var header = string.Join(",", columnNames);
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable()
                                  .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);


            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();

                hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                hoja_trabajo.Name = "Protectora";

                hoja_trabajo.Cells[3, 4] = NombreCompania;
                hoja_trabajo.Cells[3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[3, 4].Font.Bold = true;


                hoja_trabajo.Cells[4, 4] = "Cobranzas por Vendedor";
                hoja_trabajo.Cells[4, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[4, 4].Font.Bold = true;

                hoja_trabajo.Cells[5, 4] = FechaNombre;
                hoja_trabajo.Cells[5, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[5, 4].Font.Bold = true;

                hoja_trabajo.Cells[7, 2] = "Vendedor";
                hoja_trabajo.Cells[7, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[7, 2].Font.Bold = true;

                hoja_trabajo.Cells[8, 2] = "Moneda";
                hoja_trabajo.Cells[8, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[8, 2].Font.Bold = true;



                int FilaInicio = 9;
                int CampoInicio = 2;

                hoja_trabajo.Cells[FilaInicio, CampoInicio] = "N° Cobranza";
                hoja_trabajo.Cells[FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].ColumnWidth = 12;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1] = "N° Factura";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].ColumnWidth = 12;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2] = "Fecha";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3] = "Cliente";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].ColumnWidth = 50;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4] = "Categoria";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].ColumnWidth = 15;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5] = "Monto Cobranza";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].ColumnWidth = 20;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6] = "TASA ";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7] = "COSTO";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8] = "PAGO $";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].ColumnWidth = 15;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9] = "KILOS";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10] = "COMISION";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
              


                labelProgreso.Text = "100%";
                labelProgreso.Refresh();
                progressBar1.Minimum = 1;
                int total = dataTable.Rows.Count;
                progressBar1.Maximum = total;

                progressBar1.Step = 1;

                FilaInicio = FilaInicio + 1;
                double TotalCantidad = 0;
                double TotalCobrado = 0;
                double TotalPago = 0;
                string vMoneda = "";
                string vVendedor = "";

                int registros = 0;
                // Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < dataTable.Rows.Count; i++)  // fila
                {
                    progressBar1.Value = i + 1;
                    labelProgreso.Text = Convert.ToString(((i + 1) * 100) / total) + "%";
                    labelProgreso.Refresh();
                    registros++;
                    for (int j = 0; j < dataTable.Columns.Count; j++) //columna
                    {



                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                                vVendedor = dataTable.Rows[i][j].ToString();

                            }

                            if (j == 1)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 3, 5] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 3, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                            }

                            if (j == 13)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 2, 3] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 2, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                                vMoneda = dataTable.Rows[i][j].ToString();

                            }
                            // Total
                            if (j == 7)
                            {
                                TotalCobrado = TotalCobrado + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }
                            if (j == 10)
                            {
                                TotalPago = TotalPago + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }
                            if (j == 11)
                            {
                                TotalCantidad = TotalCantidad + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }
                         
                               
                        } else {

                            if (vMoneda != dataTable.Rows[i][13].ToString() || vVendedor != dataTable.Rows[i][0].ToString())
                            {
                                hoja_trabajo.Cells[i + FilaInicio, 6] = "Total Moneda " + vMoneda;
                                hoja_trabajo.Cells[i + FilaInicio, 6].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                

                               
                                hoja_trabajo.Cells[i + FilaInicio, 7].Font.Bold = true;
                                //hoja_trabajo.Cells[i + FilaInicio, 6] = TotalCobrado.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";
                               
                                hoja_trabajo.Cells[i + FilaInicio, 7] = Truncate(TotalCobrado, 2);
                                
                                hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                
                                TotalCobrado = 0;



                                
                                hoja_trabajo.Cells[i + FilaInicio, 10].Font.Bold = true;
                                //hoja_trabajo.Cells[i + FilaInicio, 9] = TotalPago.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                hoja_trabajo.Cells[i + FilaInicio, 10].NumberFormat = "#.##0,00";
                              
                                hoja_trabajo.Cells[i + FilaInicio, 10] = Truncate(TotalPago, 2);
                                hoja_trabajo.Cells[i + FilaInicio, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                TotalPago = 0;

                                hoja_trabajo.Cells[i + FilaInicio, 11].Font.Bold = true;
                                //hoja_trabajo.Cells[i + FilaInicio, 10] = TotalCantidad.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                hoja_trabajo.Cells[i + FilaInicio, 11].NumberFormat = "#.##0,00";
                              
                                hoja_trabajo.Cells[i + FilaInicio, 11] = Truncate(TotalCantidad,2);
                                

                                hoja_trabajo.Cells[i + FilaInicio, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 11].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                TotalCantidad = 0;
                                

                                
                                FilaInicio = FilaInicio + 3;
                                
                                vMoneda = dataTable.Rows[i][13].ToString();


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Vendedor";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = dataTable.Rows[i][0].ToString();

                                vVendedor = dataTable.Rows[i][0].ToString();

                                hoja_trabajo.Cells[i + FilaInicio, 6].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio, 6] = dataTable.Rows[i][1].ToString();
                                

                                FilaInicio = FilaInicio + 1;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Moneda";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 3] = vMoneda;
                                hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;



                                FilaInicio = FilaInicio + 1;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "N° Cobranza";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].ColumnWidth = 12;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = "N° Factura";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].ColumnWidth = 12;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2] = "Fecha";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3] = "Cliente";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].ColumnWidth = 50;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4] = "Categoria";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].ColumnWidth = 15;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;



                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5] = "Monto Cobranza";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].ColumnWidth = 20;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6] = "TASA ";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7] = "COSTO";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8] = "PAGO $";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].ColumnWidth = 15;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9] = "KILOS";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10] = "COMISION";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                FilaInicio = FilaInicio + 1;
                            } 
                        }
                       



                       if (j == 2)
                       {
                            hoja_trabajo.Cells[i + FilaInicio, 2].NumberFormat = "@"; 
                            hoja_trabajo.Cells[i + FilaInicio, 2] = dataTable.Rows[i][j].ToString();
                            hoja_trabajo.Cells[i + FilaInicio, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        }
                        else if ( j == 3)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 3].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 3] = dataTable.Rows[i][j].ToString();
                        }
                       else if (j == 4)
                       {
                           hoja_trabajo.Cells[i + FilaInicio, 4].NumberFormat = "@";
                           hoja_trabajo.Cells[i + FilaInicio, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                           hoja_trabajo.Cells[i + FilaInicio, 4] = dataTable.Rows[i][j].ToString();
                       }


                        else if (j == 5)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 5] = dataTable.Rows[i][j].ToString();


                        }
                        else if (j == 6)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                            hoja_trabajo.Cells[i + FilaInicio, 6] = dataTable.Rows[i][j].ToString();


                        } 
                        else if (j == 7)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);
                           
                            //hoja_trabajo.Cells[i + FilaInicio, 6] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 7] = numero;
                            
                            hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                            if (vMoneda == dataTable.Rows[i][13].ToString())
                            {
                                TotalCobrado = TotalCobrado + numero;
                            }


                        }

                        else if (j == 8)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            //hoja_trabajo.Cells[i + FilaInicio, 7] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            numero = Truncate(numero, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 8].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 8] = numero;
                            hoja_trabajo.Cells[i + FilaInicio, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        }
                        else if (j == 9)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            //hoja_trabajo.Cells[i + FilaInicio, 8] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            numero = Truncate(numero, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 9].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 9] = numero;
                            
                            hoja_trabajo.Cells[i + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        }
                        else if (j == 10 )
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);
                            //hoja_trabajo.Cells[i + FilaInicio, 9] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 10].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 10] = numero;

                            hoja_trabajo.Cells[i + FilaInicio, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            
                            if (vMoneda == dataTable.Rows[i][13].ToString())
                            {
                                TotalPago = TotalPago + numero;
                            }
                        
                        }

                        else if (j == 11)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);
                            //hoja_trabajo.Cells[i + FilaInicio, 10] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 11].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 11] = numero;
                            hoja_trabajo.Cells[i + FilaInicio, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            if (vMoneda == dataTable.Rows[i][13].ToString())
                            {
                                TotalCantidad = TotalCantidad + numero;
                            }
                         

                        }
                        else if (j == 12)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            //hoja_trabajo.Cells[i + FilaInicio, 11] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            numero = Truncate(numero, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 12].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 12] = numero;
                            hoja_trabajo.Cells[i + FilaInicio, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        }





                    }
                }

                hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total Moneda " + vMoneda;
                hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


                hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";
                              
                hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
               // hoja_trabajo.Cells[registros + FilaInicio, 6] = TotalCobrado.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobrado;

                hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[registros + FilaInicio, 10].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 10].Font.Bold = true;
                //hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPago.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, 10] = TotalPago;
                
                hoja_trabajo.Cells[registros + FilaInicio, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[registros + FilaInicio, 11].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 11].Font.Bold = true;
                //hoja_trabajo.Cells[registros + FilaInicio, 10] = TotalCantidad.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, 11] = TotalCantidad;
                
                hoja_trabajo.Cells[registros + FilaInicio, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 11].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
               // hoja_trabajo.Cells[registros + FilaInicio, 10].NumberFormat = "@";

                progressBar1.Minimum = 1;
                progressBar1.Value = 1;
                labelProgreso.Text = "0%";
                labelProgreso.Refresh();

                DataTable FacturaTableCategoria = CreateTablaFacturaResumenCategoria(dataTableTotal, "SERRA");
                if (FacturaTableCategoria.Rows.Count > 0) {
                    CreateCSVFileResumenCategoria(FacturaTableCategoria, NombreCompania, FechaNombre, ref aplicacion, ref libros_trabajo, ref hoja_trabajo);  
                } 
                  
                libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
                // abrimos el excel
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = fichero.FileName;
                Process.Start(startInfo);

            }
        }

        public void CreateCSVFileCxC(DataTable dataTable, DataTable dataTableSERRA, string NombreCompania, string FechaNombre)
        {

            var lines = new List<string>();

            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                                 Select(column => column.ColumnName).
                                                 ToArray();

            var header = string.Join(",", columnNames);
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable()
                                  .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);


            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();

                hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                hoja_trabajo.Name = "Protectora";

                hoja_trabajo.Cells[3, 4] = NombreCompania;
                hoja_trabajo.Cells[3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[3, 4].Font.Bold = true;


                hoja_trabajo.Cells[4, 4] = "Cuentas por Cobrar por Vendedor";
                hoja_trabajo.Cells[4, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[4, 4].Font.Bold = true;

                hoja_trabajo.Cells[5, 4] = FechaNombre;
                hoja_trabajo.Cells[5, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[5, 4].Font.Bold = true;

                hoja_trabajo.Cells[7, 2] = "Vendedor";
                hoja_trabajo.Cells[7, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[7, 2].Font.Bold = true;

                hoja_trabajo.Cells[8, 2] = "Moneda";
                hoja_trabajo.Cells[8, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[8, 2].Font.Bold = true;



                int FilaInicio = 9;
                int CampoInicio = 2;

                hoja_trabajo.Cells[FilaInicio, CampoInicio] = "Codigo/Nombre de cliente";
                hoja_trabajo.Cells[FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].ColumnWidth = 50;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1] = "N° CxC";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].ColumnWidth = 20;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2] = "Fecha Doc";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3] = "Fecha Vto";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4] = "Dias Venc.";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5] = "Total CxC";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].ColumnWidth = 20;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6] = "Monto Abonado ";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                //hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].ColumnWidth = 20;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7] = "Monto Restante";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
               // hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].ColumnWidth = 20;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;



                labelProgreso.Text = "100%";
                labelProgreso.Refresh();
                progressBar1.Minimum = 1;
                int total = dataTable.Rows.Count;
                progressBar1.Maximum = total;

                progressBar1.Step = 1;

                FilaInicio = FilaInicio + 1;
                double TotalCobrado = 0;
                double TotalPago = 0;
                double TotalCobradoGeneral = 0;
                double TotalCobradoGeneraldolar = 0;

                double TotalPagoGeneral = 0;
                double TotalPagoGeneraldolar = 0;
                

                string vMoneda = "";
                string vVendedor = "";

                int registros = 0;
                // Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < dataTable.Rows.Count; i++)  // fila
                {
                    progressBar1.Value = i + 1;
                    labelProgreso.Text = Convert.ToString(((i + 1) * 100) / total) + "%";
                    labelProgreso.Refresh();
                    registros++;
                    for (int j = 0; j < dataTable.Columns.Count; j++) //columna
                    {



                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                vVendedor = dataTable.Rows[i][j].ToString();
                            }
                            if (j == 1)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                           
                            }
                            if (j == 2)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 2, 3].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio - 2, 3] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 2, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                                vMoneda = dataTable.Rows[i][j].ToString();
                           }

                            // Total
                            if (j == 8)
                            {
                                TotalCobrado = TotalCobrado + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }
                            
                            if (j == 10)
                            {
                                TotalPago = TotalPago + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }
                            

                        }
                        else
                        {
                            string aux = dataTable.Rows[i][0].ToString();
                            if (aux == "00003") {
                                aux = dataTable.Rows[i][0].ToString();
                            }
                            

                            if (vMoneda != dataTable.Rows[i][2].ToString() || vVendedor != dataTable.Rows[i][0].ToString())
                            {

                              
                                hoja_trabajo.Cells[i + FilaInicio, 6] = "Total Moneda ";
                                hoja_trabajo.Cells[i + FilaInicio, 6].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación



                                hoja_trabajo.Cells[i + FilaInicio, 7].Font.Bold = true;
                                
                                hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";

                                hoja_trabajo.Cells[i + FilaInicio, 7] = Truncate(TotalCobrado, 2);

                                hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                if (vMoneda == "Bolívar")
                                {
                                    TotalCobradoGeneral = TotalCobradoGeneral + TotalCobrado;

                                }
                                else
                                {
                                    TotalCobradoGeneraldolar = TotalCobradoGeneraldolar + TotalCobrado;

                                }
                                TotalCobrado = 0;
                                


                                hoja_trabajo.Cells[i + FilaInicio, 9].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, 9].NumberFormat = "#.##0,00";

                                hoja_trabajo.Cells[i + FilaInicio, 9] = Truncate(TotalPago, 2);
                                hoja_trabajo.Cells[i + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                if (vMoneda == "Bolívar")
                                {
                                    TotalPagoGeneral = TotalPagoGeneral + TotalPago;

                                }
                                else
                                {
                                    TotalPagoGeneraldolar = TotalPagoGeneraldolar + TotalPago;

                                }
                                TotalPago = 0;
                                vMoneda = dataTable.Rows[i][2].ToString();


                                FilaInicio = FilaInicio + 3;



                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Vendedor";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = dataTable.Rows[i][1].ToString();

                                vVendedor = dataTable.Rows[i][0].ToString();
                                
                                FilaInicio = FilaInicio + 1;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Moneda";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 3] = vMoneda;
                                hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;



                                FilaInicio = FilaInicio + 1;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Codigo/Nombre de cliente";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].ColumnWidth = 50;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = "N° CxC";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].ColumnWidth = 20;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2] = "Fecha Doc";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3] = "Fecha Vto";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4] = "Dias Venc.";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].ColumnWidth =10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;



                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5] = "Total CxC";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].ColumnWidth = 20;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6] = "Monto Abonado";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                               // hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].ColumnWidth = 20;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7] = "Monto Restante";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                //hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].ColumnWidth = 20;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                FilaInicio = FilaInicio + 1;
                            }
                        }




                        if (j == 3)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 2].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, 2] = dataTable.Rows[i][j].ToString();
                            hoja_trabajo.Cells[i + FilaInicio, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        }
                        else if (j == 4)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 3].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 3] = dataTable.Rows[i][j].ToString();
                        }
                        else if (j == 5)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 4].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 4] = dataTable.Rows[i][j].ToString();
                        }


                        else if (j == 6)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 5] = dataTable.Rows[i][j].ToString();


                        }
                        else if (j == 7)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                            hoja_trabajo.Cells[i + FilaInicio, 6] = dataTable.Rows[i][j].ToString();


                        }
                        else if (j == 8)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);

                            //hoja_trabajo.Cells[i + FilaInicio, 6] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 7] = numero;

                            hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                            if (vMoneda == dataTable.Rows[i][2].ToString() && i!=0)
                            {
                                TotalCobrado = TotalCobrado + numero;
                            }


                        }

                        else if (j ==9)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            //hoja_trabajo.Cells[i + FilaInicio, 7] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            numero = Truncate(numero, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 8].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 8] = numero;
                            hoja_trabajo.Cells[i + FilaInicio, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        }
                        
                        else if (j == 10)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);
                            //hoja_trabajo.Cells[i + FilaInicio, 9] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 9].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 9] = numero;

                            hoja_trabajo.Cells[i + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                            if (vMoneda == dataTable.Rows[i][2].ToString() && i != 0)
                            {
                                TotalPago = TotalPago + numero;
                            }

                        }

                    }
                }

                hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total Moneda ";
                hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


                hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobrado;

                hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[registros + FilaInicio, 9].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 9].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPago;

                hoja_trabajo.Cells[registros + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                registros = registros + 2;
                if (vMoneda == "Bolívar")
                {
                    TotalCobradoGeneral = TotalCobradoGeneral + TotalCobrado;
                    TotalPagoGeneral = TotalPagoGeneral + TotalPago;
                }

                hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total Bs ";
                hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


                hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobradoGeneral;

                hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[registros + FilaInicio, 9].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 9].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPagoGeneral;

                hoja_trabajo.Cells[registros + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                registros = registros + 2;

                if (vMoneda == "Dólar")
                {
                    TotalCobradoGeneraldolar = TotalCobradoGeneraldolar + TotalCobrado;
                    TotalPagoGeneraldolar = TotalPagoGeneraldolar + TotalPago;
                }


                hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total $ ";
                hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


                hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobradoGeneraldolar;

                hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[registros + FilaInicio, 9].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 9].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPagoGeneraldolar;

                hoja_trabajo.Cells[registros + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

   
                progressBar1.Minimum = 1;
                progressBar1.Value = 1;
                labelProgreso.Text = "0%";
                labelProgreso.Refresh();

                if (dataTableSERRA.Rows.Count > 0)
                {
                    CreateCSVCxCSERRA(dataTableSERRA, NombreCompania, FechaNombre, ref aplicacion, ref libros_trabajo, ref hoja_trabajo);
                }

                libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
                // abrimos el excel
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = fichero.FileName;
                Process.Start(startInfo);

            }
        }


        public void CreateCSVFileResumenCategoria(DataTable dataTable, string NombreCompania, string FechaNombre, 
            ref Microsoft.Office.Interop.Excel.Application aplicacion,ref  Microsoft.Office.Interop.Excel.Workbook libros_trabajo,
                ref Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo                
            )
        {

                hoja_trabajo = aplicacion.Worksheets.Add(After: hoja_trabajo);    
                hoja_trabajo.Select();
             
                hoja_trabajo.Name = "SERRA";

                hoja_trabajo.Cells[3, 4] = NombreCompania;
                hoja_trabajo.Cells[3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[3, 4].Font.Bold = true;


                hoja_trabajo.Cells[4, 4] = "Cobranzas por Vendedor --SERRA";
                hoja_trabajo.Cells[4, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[4, 4].Font.Bold = true;

                hoja_trabajo.Cells[5, 4] = FechaNombre;
                hoja_trabajo.Cells[5, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[5, 4].Font.Bold = true;

                hoja_trabajo.Cells[7, 2] = "Vendedor";
                hoja_trabajo.Cells[7, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[7, 2].Font.Bold = true;

                hoja_trabajo.Cells[8, 2] = "Moneda";
                hoja_trabajo.Cells[8, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[8, 2].Font.Bold = true;



                int FilaInicio = 9;
                int CampoInicio = 2;

                hoja_trabajo.Cells[FilaInicio, CampoInicio] = "N° Cobranza";
                hoja_trabajo.Cells[FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].ColumnWidth = 12;
                hoja_trabajo.Cells[FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1] = "N° Factura";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].ColumnWidth = 12;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2] = "Fecha";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3] = "Cliente";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].ColumnWidth = 50;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4] = "Categoria";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].ColumnWidth = 15;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5] = "Monto Cobranza";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].ColumnWidth = 20;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6] = "TASA ";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7] = "COSTO";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8] = "PAGO $";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].ColumnWidth = 15;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 8].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9] = "KILOS";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10] = "COMISION";
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].Font.Bold = true;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].ColumnWidth = 10;
                hoja_trabajo.Cells[FilaInicio, CampoInicio + 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;



                labelProgreso.Text = "100%";
                labelProgreso.Refresh();
                progressBar1.Minimum = 1;
                int total = dataTable.Rows.Count;
                progressBar1.Maximum = total;

                progressBar1.Step = 1;

                FilaInicio = FilaInicio + 1;
                double TotalCantidad = 0;
                double TotalCobrado = 0;
                double TotalPago = 0;
                string vMoneda = "";
                string vVendedor = "";

                int registros = 0;
                // Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < dataTable.Rows.Count; i++)  // fila
                {
                    progressBar1.Value = i + 1;
                    labelProgreso.Text = Convert.ToString(((i + 1) * 100) / total) + "%";
                    labelProgreso.Refresh();
                    registros++;
                    for (int j = 0; j < dataTable.Columns.Count; j++) //columna
                    {



                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 3, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                                vVendedor = dataTable.Rows[i][j].ToString();

                            }

                            if (j == 1)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 3, 5] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 3, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                            }

                            if (j == 13)
                            {
                                hoja_trabajo.Cells[i + FilaInicio - 2, 3] = dataTable.Rows[i][j].ToString();
                                hoja_trabajo.Cells[i + FilaInicio - 2, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                                vMoneda = dataTable.Rows[i][j].ToString();

                            }
                            // Total
                            if (j == 7)
                            {
                                TotalCobrado = TotalCobrado + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }
                            if (j == 10)
                            {
                                TotalPago = TotalPago + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }
                            if (j == 11)
                            {
                                TotalCantidad = TotalCantidad + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            }


                        }
                        else
                        {

                            if (vMoneda != dataTable.Rows[i][13].ToString() || vVendedor != dataTable.Rows[i][0].ToString())
                            {
                                hoja_trabajo.Cells[i + FilaInicio, 6] = "Total Moneda " + vMoneda;
                                hoja_trabajo.Cells[i + FilaInicio, 6].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación



                                hoja_trabajo.Cells[i + FilaInicio, 7].Font.Bold = true;
                                //hoja_trabajo.Cells[i + FilaInicio, 6] = TotalCobrado.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";

                                hoja_trabajo.Cells[i + FilaInicio, 7] = Truncate(TotalCobrado, 2);

                                hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                TotalCobrado = 0;




                                hoja_trabajo.Cells[i + FilaInicio, 10].Font.Bold = true;
                                //hoja_trabajo.Cells[i + FilaInicio, 9] = TotalPago.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                hoja_trabajo.Cells[i + FilaInicio, 10].NumberFormat = "#.##0,00";

                                hoja_trabajo.Cells[i + FilaInicio, 10] = Truncate(TotalPago, 2);
                                hoja_trabajo.Cells[i + FilaInicio, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                TotalPago = 0;

                                hoja_trabajo.Cells[i + FilaInicio, 11].Font.Bold = true;
                                //hoja_trabajo.Cells[i + FilaInicio, 10] = TotalCantidad.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                                hoja_trabajo.Cells[i + FilaInicio, 11].NumberFormat = "#.##0,00";

                                hoja_trabajo.Cells[i + FilaInicio, 11] = Truncate(TotalCantidad, 2);


                                hoja_trabajo.Cells[i + FilaInicio, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 11].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                TotalCantidad = 0;



                                FilaInicio = FilaInicio + 3;

                                vMoneda = dataTable.Rows[i][13].ToString();


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Vendedor";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = dataTable.Rows[i][0].ToString();

                                vVendedor = dataTable.Rows[i][0].ToString();

                                hoja_trabajo.Cells[i + FilaInicio, 6].NumberFormat = "@";
                                hoja_trabajo.Cells[i + FilaInicio, 6] = dataTable.Rows[i][1].ToString();


                                FilaInicio = FilaInicio + 1;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Moneda";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, 3] = vMoneda;
                                hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;



                                FilaInicio = FilaInicio + 1;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "N° Cobranza";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].ColumnWidth = 12;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = "N° Factura";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].ColumnWidth = 12;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2] = "Fecha";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3] = "Cliente";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].ColumnWidth = 50;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4] = "Categoria";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].ColumnWidth = 15;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;



                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5] = "Monto Cobranza";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].ColumnWidth = 20;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6] = "TASA ";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7] = "COSTO";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8] = "PAGO $";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].ColumnWidth = 15;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 8].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9] = "KILOS";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10] = "COMISION";
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].Font.Bold = true;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].ColumnWidth = 10;
                                hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                FilaInicio = FilaInicio + 1;
                            }
                        }




                        if (j == 2)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 2].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, 2] = dataTable.Rows[i][j].ToString();
                            hoja_trabajo.Cells[i + FilaInicio, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        }
                        else if (j == 3)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 3].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 3] = dataTable.Rows[i][j].ToString();
                        }
                        else if (j == 4)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 4].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 4] = dataTable.Rows[i][j].ToString();
                        }


                        else if (j == 5)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            hoja_trabajo.Cells[i + FilaInicio, 5] = dataTable.Rows[i][j].ToString();


                        }
                        else if (j == 6)
                        {
                            hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                            hoja_trabajo.Cells[i + FilaInicio, 6] = dataTable.Rows[i][j].ToString();


                        }
                        else if (j == 7)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);

                            //hoja_trabajo.Cells[i + FilaInicio, 6] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 7] = numero;

                            hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                            if (vMoneda == dataTable.Rows[i][13].ToString())
                            {
                                TotalCobrado = TotalCobrado + numero;
                            }


                        }

                        else if (j == 8)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            //hoja_trabajo.Cells[i + FilaInicio, 7] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            numero = Truncate(numero, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 8].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 8] = numero;
                            hoja_trabajo.Cells[i + FilaInicio, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        }
                        else if (j == 9)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            //hoja_trabajo.Cells[i + FilaInicio, 8] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            numero = Truncate(numero, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 9].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 9] = numero;

                            hoja_trabajo.Cells[i + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        }
                        else if (j == 10)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);
                            //hoja_trabajo.Cells[i + FilaInicio, 9] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 10].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 10] = numero;

                            hoja_trabajo.Cells[i + FilaInicio, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                            if (vMoneda == dataTable.Rows[i][13].ToString())
                            {
                                TotalPago = TotalPago + numero;
                            }

                        }

                        else if (j == 11)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            numero = Truncate(numero, 2);
                            //hoja_trabajo.Cells[i + FilaInicio, 10] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            hoja_trabajo.Cells[i + FilaInicio, 11].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 11] = numero;
                            hoja_trabajo.Cells[i + FilaInicio, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            if (vMoneda == dataTable.Rows[i][13].ToString())
                            {
                                TotalCantidad = TotalCantidad + numero;
                            }


                        }
                        else if (j == 12)
                        {
                            double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                            //hoja_trabajo.Cells[i + FilaInicio, 11] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                            numero = Truncate(numero, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 12].NumberFormat = "#.##0,00";
                            hoja_trabajo.Cells[i + FilaInicio, 12] = numero;
                            hoja_trabajo.Cells[i + FilaInicio, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        }





                    }
                }

                hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total Moneda " + vMoneda;
                hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
                hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


                hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";

                hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
                // hoja_trabajo.Cells[registros + FilaInicio, 6] = TotalCobrado.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobrado;

                hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                hoja_trabajo.Cells[registros + FilaInicio, 10].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 10].Font.Bold = true;
                //hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPago.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, 10] = TotalPago;

                hoja_trabajo.Cells[registros + FilaInicio, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 10].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                hoja_trabajo.Cells[registros + FilaInicio, 11].NumberFormat = "#.##0,00";
                hoja_trabajo.Cells[registros + FilaInicio, 11].Font.Bold = true;
                //hoja_trabajo.Cells[registros + FilaInicio, 10] = TotalCantidad.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                hoja_trabajo.Cells[registros + FilaInicio, 11] = TotalCantidad;

                hoja_trabajo.Cells[registros + FilaInicio, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                hoja_trabajo.Cells[registros + FilaInicio, 11].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                // hoja_trabajo.Cells[registros + FilaInicio, 10].NumberFormat = "@";

                progressBar1.Minimum = 1;
                progressBar1.Value = 1;
                labelProgreso.Text = "0%";
                labelProgreso.Refresh();
              
            
        }


        public void CreateCSVCxCSERRA(DataTable dataTable, string NombreCompania, string FechaNombre,
           ref Microsoft.Office.Interop.Excel.Application aplicacion, ref  Microsoft.Office.Interop.Excel.Workbook libros_trabajo,
               ref Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo
           )
        {

            hoja_trabajo = aplicacion.Worksheets.Add(After: hoja_trabajo);
            hoja_trabajo.Select();

            hoja_trabajo.Name = "SERRA";


            hoja_trabajo.Cells[3, 4] = NombreCompania;
            hoja_trabajo.Cells[3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
            hoja_trabajo.Cells[3, 4].Font.Bold = true;


            hoja_trabajo.Cells[4, 4] = "Cuentas por Cobrar por Vendedor";
            hoja_trabajo.Cells[4, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
            hoja_trabajo.Cells[4, 4].Font.Bold = true;

            hoja_trabajo.Cells[5, 4] = FechaNombre;
            hoja_trabajo.Cells[5, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
            hoja_trabajo.Cells[5, 4].Font.Bold = true;

            hoja_trabajo.Cells[7, 2] = "Vendedor";
            hoja_trabajo.Cells[7, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
            hoja_trabajo.Cells[7, 2].Font.Bold = true;

            hoja_trabajo.Cells[8, 2] = "Moneda";
            hoja_trabajo.Cells[8, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
            hoja_trabajo.Cells[8, 2].Font.Bold = true;



            int FilaInicio = 9;
            int CampoInicio = 2;

            hoja_trabajo.Cells[FilaInicio, CampoInicio] = "Codigo/Nombre de cliente";
            hoja_trabajo.Cells[FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
            hoja_trabajo.Cells[FilaInicio, CampoInicio].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio].ColumnWidth = 50;
            hoja_trabajo.Cells[FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


            hoja_trabajo.Cells[FilaInicio, CampoInicio + 1] = "N° CxC";
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].ColumnWidth = 20;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


            hoja_trabajo.Cells[FilaInicio, CampoInicio + 2] = "Fecha Doc";
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].ColumnWidth = 10;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


            hoja_trabajo.Cells[FilaInicio, CampoInicio + 3] = "Fecha Vto";
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].ColumnWidth = 10;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            hoja_trabajo.Cells[FilaInicio, CampoInicio + 4] = "Dias Venc.";
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].ColumnWidth = 10;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


            hoja_trabajo.Cells[FilaInicio, CampoInicio + 5] = "Total CxC";
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].ColumnWidth = 20;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


            hoja_trabajo.Cells[FilaInicio, CampoInicio + 6] = "Monto Abonado ";
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            //hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].ColumnWidth = 20;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


            hoja_trabajo.Cells[FilaInicio, CampoInicio + 7] = "Monto Restante";
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            // hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Font.Bold = true;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].ColumnWidth = 20;
            hoja_trabajo.Cells[FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;



            labelProgreso.Text = "100%";
            labelProgreso.Refresh();
            progressBar1.Minimum = 1;
            int total = dataTable.Rows.Count;
            progressBar1.Maximum = total;

            progressBar1.Step = 1;

            FilaInicio = FilaInicio + 1;
            double TotalCobrado = 0;
            double TotalPago = 0;
            double TotalCobradoGeneral = 0;
            double TotalPagoGeneral = 0;
            double TotalCobradoGeneraldolar = 0;
            double TotalPagoGeneraldolar = 0;
            

            string vMoneda = "";
            string vVendedor = "";

            int registros = 0;
            // Recorremos el DataGridView rellenando la hoja de trabajo
            for (int i = 0; i < dataTable.Rows.Count; i++)  // fila
            {
                progressBar1.Value = i + 1;
                labelProgreso.Text = Convert.ToString(((i + 1) * 100) / total) + "%";
                labelProgreso.Refresh();
                registros++;
                for (int j = 0; j < dataTable.Columns.Count; j++) //columna
                {



                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            vVendedor = dataTable.Rows[i][j].ToString();
                        }
                        if (j == 1)
                        {
                            hoja_trabajo.Cells[i + FilaInicio - 3, 3].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio - 3, 3] = dataTable.Rows[i][j].ToString();
                            hoja_trabajo.Cells[i + FilaInicio - 3, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                        }
                        if (j == 2)
                        {
                            hoja_trabajo.Cells[i + FilaInicio - 2, 3].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio - 2, 3] = dataTable.Rows[i][j].ToString();
                            hoja_trabajo.Cells[i + FilaInicio - 2, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                            vMoneda = dataTable.Rows[i][j].ToString();
                        }

                        // Total
                        if (j == 8)
                        {
                            TotalCobrado = TotalCobrado + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                        }

                        if (j == 10)
                        {
                            TotalPago = TotalPago + Convert.ToDouble(dataTable.Rows[i][j].ToString());
                        }


                    }
                    else
                    {

                        if (vMoneda != dataTable.Rows[i][2].ToString() || vVendedor != dataTable.Rows[i][0].ToString())
                        {

                          

                            hoja_trabajo.Cells[i + FilaInicio, 6] = "Total Moneda ";
                            hoja_trabajo.Cells[i + FilaInicio, 6].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación



                            hoja_trabajo.Cells[i + FilaInicio, 7].Font.Bold = true;

                            hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";

                            hoja_trabajo.Cells[i + FilaInicio, 7] = Truncate(TotalCobrado, 2);

                            hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            if (vMoneda == "Bolívar")
                            {
                                TotalCobradoGeneral = TotalCobradoGeneral + TotalCobrado;

                            }else{
                                TotalCobradoGeneraldolar = TotalCobradoGeneraldolar + TotalCobrado;
                            
                            }
                          
                            TotalCobrado = 0;

                           

                            hoja_trabajo.Cells[i + FilaInicio, 9].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, 9].NumberFormat = "#.##0,00";

                            hoja_trabajo.Cells[i + FilaInicio, 9] = Truncate(TotalPago, 2);
                            hoja_trabajo.Cells[i + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                            if (vMoneda == "Bolívar")
                            {
                                TotalPagoGeneral = TotalPagoGeneral + TotalPago;

                            }
                            else
                            {
                                TotalPagoGeneraldolar = TotalPagoGeneraldolar + TotalPago;

                            }
                            TotalPago = 0;
                            
                            FilaInicio = FilaInicio + 3;

                            vMoneda = dataTable.Rows[i][2].ToString();

                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Vendedor";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].NumberFormat = "@";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = dataTable.Rows[i][1].ToString();

                            vVendedor = dataTable.Rows[i][0].ToString();

                            FilaInicio = FilaInicio + 1;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Moneda";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, 3] = vMoneda;
                            hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;



                            FilaInicio = FilaInicio + 1;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio] = "Codigo/Nombre de cliente";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].ColumnWidth = 50;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1] = "N° CxC";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].ColumnWidth = 20;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 1].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2] = "Fecha Doc";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].ColumnWidth = 10;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 2].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3] = "Fecha Vto";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].ColumnWidth = 10;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 3].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4] = "Dias Venc.";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].ColumnWidth = 10;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 4].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;



                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5] = "Total CxC";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].ColumnWidth = 20;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 5].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6] = "Monto Abonado";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            // hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].ColumnWidth = 20;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 6].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7] = "Monto Restante";
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
                            //hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Font.Bold = true;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].ColumnWidth = 20;
                            hoja_trabajo.Cells[i + FilaInicio, CampoInicio + 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                            FilaInicio = FilaInicio + 1;
                        }
                    }

                    if (j == 3)
                    {
                        hoja_trabajo.Cells[i + FilaInicio, 2].NumberFormat = "@";
                        hoja_trabajo.Cells[i + FilaInicio, 2] = dataTable.Rows[i][j].ToString();
                        hoja_trabajo.Cells[i + FilaInicio, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    }
                    else if (j == 4)
                    {
                        hoja_trabajo.Cells[i + FilaInicio, 3].NumberFormat = "@";
                        hoja_trabajo.Cells[i + FilaInicio, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        hoja_trabajo.Cells[i + FilaInicio, 3] = dataTable.Rows[i][j].ToString();
                    }
                    else if (j == 5)
                    {
                        hoja_trabajo.Cells[i + FilaInicio, 4].NumberFormat = "@";
                        hoja_trabajo.Cells[i + FilaInicio, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        hoja_trabajo.Cells[i + FilaInicio, 4] = dataTable.Rows[i][j].ToString();
                    }


                    else if (j == 6)
                    {
                        hoja_trabajo.Cells[i + FilaInicio, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        hoja_trabajo.Cells[i + FilaInicio, 5] = dataTable.Rows[i][j].ToString();


                    }
                    else if (j == 7)
                    {
                        hoja_trabajo.Cells[i + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        hoja_trabajo.Cells[i + FilaInicio, 6] = dataTable.Rows[i][j].ToString();


                    }
                    else if (j == 8)
                    {
                        double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                        numero = Truncate(numero, 2);

                        //hoja_trabajo.Cells[i + FilaInicio, 6] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                        hoja_trabajo.Cells[i + FilaInicio, 7].NumberFormat = "#.##0,00";
                        hoja_trabajo.Cells[i + FilaInicio, 7] = numero;

                        hoja_trabajo.Cells[i + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        if (vMoneda == dataTable.Rows[i][2].ToString() && i != 0)
                        {
                            TotalCobrado = TotalCobrado + numero;
                        }


                    }

                    else if (j == 9)
                    {
                        double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                        //hoja_trabajo.Cells[i + FilaInicio, 7] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                        numero = Truncate(numero, 2);
                        hoja_trabajo.Cells[i + FilaInicio, 8].NumberFormat = "#.##0,00";
                        hoja_trabajo.Cells[i + FilaInicio, 8] = numero;
                        hoja_trabajo.Cells[i + FilaInicio, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                    }

                    else if (j == 10)
                    {
                        double numero = Convert.ToDouble(dataTable.Rows[i][j].ToString());
                        numero = Truncate(numero, 2);
                        //hoja_trabajo.Cells[i + FilaInicio, 9] = numero.ToString("N", new CultureInfo("is-IS")); //= 19.950.000,00
                        hoja_trabajo.Cells[i + FilaInicio, 9].NumberFormat = "#.##0,00";
                        hoja_trabajo.Cells[i + FilaInicio, 9] = numero;

                        hoja_trabajo.Cells[i + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación

                        if (vMoneda == dataTable.Rows[i][2].ToString() && i != 0)
                        {
                            TotalPago = TotalPago + numero;
                        }

                    }

                }
            }

            hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total Moneda ";
            hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


            hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";

            hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobrado;

            hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            hoja_trabajo.Cells[registros + FilaInicio, 9].NumberFormat = "#.##0,00";
            hoja_trabajo.Cells[registros + FilaInicio, 9].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPago;

            hoja_trabajo.Cells[registros + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[registros + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            registros = registros + 2;
            if (vMoneda == "Bolívar")
            {
                TotalCobradoGeneral = TotalCobradoGeneral + TotalCobrado;
                TotalPagoGeneral = TotalPagoGeneral + TotalPago;
            }

            hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total Bs ";
            hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


            hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";
            hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobradoGeneral;

            hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            hoja_trabajo.Cells[registros + FilaInicio, 9].NumberFormat = "#.##0,00";
            hoja_trabajo.Cells[registros + FilaInicio, 9].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPagoGeneral;

            hoja_trabajo.Cells[registros + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[registros + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            registros = registros + 2;
            
            if (vMoneda == "Dólar")
            {
                TotalCobradoGeneraldolar = TotalCobradoGeneraldolar + TotalCobrado;
                TotalPagoGeneraldolar = TotalPagoGeneraldolar + TotalPago;
            }


            hoja_trabajo.Cells[registros + FilaInicio, 6] = "Total $ ";
            hoja_trabajo.Cells[registros + FilaInicio, 6].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación


            hoja_trabajo.Cells[registros + FilaInicio, 7].NumberFormat = "#.##0,00";
            hoja_trabajo.Cells[registros + FilaInicio, 7].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 7] = TotalCobradoGeneraldolar;

            hoja_trabajo.Cells[registros + FilaInicio, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[registros + FilaInicio, 7].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            hoja_trabajo.Cells[registros + FilaInicio, 9].NumberFormat = "#.##0,00";
            hoja_trabajo.Cells[registros + FilaInicio, 9].Font.Bold = true;
            hoja_trabajo.Cells[registros + FilaInicio, 9] = TotalPagoGeneraldolar;

            hoja_trabajo.Cells[registros + FilaInicio, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;//Alineación
            hoja_trabajo.Cells[registros + FilaInicio, 9].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;




            progressBar1.Minimum = 1;
            progressBar1.Value = 1;
            labelProgreso.Text = "0%";
            labelProgreso.Refresh();


        }




        public static double Truncate(double value, int decimales)
        {
            double aux_value = Math.Pow(10, decimales);
            return (Math.Truncate(value * aux_value) / aux_value);
        }


        private void listEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listEmpresa.SelectedItems.Count == 1)
            {

                string ntcustomerId = listEmpresa.SelectedItems[0].Text;
                string CodigoEmpresa = listEmpresa.SelectedItems[0].SubItems[1].Text;
                txtNombreEmpresa.Text = listEmpresa.SelectedItems[0].SubItems[0].Text;
                vNombreEmpresa = listEmpresa.SelectedItems[0].SubItems[0].Text;


            }
        }

        private void ClienteDiario_Click(object sender, EventArgs e)
        {
            button1.Text = "Comisiones Detallada";
            panel1.Enabled = true;


        }

        private void CompMenFacLinea_Click(object sender, EventArgs e)
        {

            button1.Text = "Comisiones Resumidas";
            panel1.Enabled = true;

        }

        private string sBucarCategoriaInicial(string NumeroFactura, string Compania)
        {
            string Categoria = "";
            string vSql = "";
            vSql = vSql + " SELECT TOP 1 dbo.ArticuloInventario.Categoria FROM renglonFactura \n";
            vSql = vSql + " INNER JOIN  dbo.ArticuloInventario ON dbo.renglonFactura.ConsecutivoCompania = dbo.ArticuloInventario.ConsecutivoCompania\n ";
            vSql = vSql + "  AND dbo.renglonFactura.Articulo = dbo.ArticuloInventario.Codigo\n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + Compania + " AND NumeroFactura ='" + NumeroFactura + "'\n";
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Categoria = reader["Categoria"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return Categoria;
        }

        private string sBucarCategoria(string NumeroFactura, string Compania, string Cate)
        {
            string Categoria = "";
            string vSql = "";
            vSql = vSql + " SELECT TOP 1 dbo.ArticuloInventario.Categoria FROM renglonFactura \n";
            vSql = vSql + " INNER JOIN  dbo.ArticuloInventario ON dbo.renglonFactura.ConsecutivoCompania = dbo.ArticuloInventario.ConsecutivoCompania\n ";
            vSql = vSql + "  AND dbo.renglonFactura.Articulo = dbo.ArticuloInventario.Codigo\n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + Compania + " AND NumeroFactura ='" + NumeroFactura + "'\n";
            vSql = vSql + " AND dbo.ArticuloInventario.Categoria = '" + Cate + "'\n";

            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Categoria = reader["Categoria"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return Categoria;
        }

        private string sBucarCosto(string NumeroFactura, string Compania, string Cate)
        {
            string Costo = "";
            string vSql = "";
            vSql = vSql + " (SELECT  TOP 1 renglonFactura.PrecioConIVA - ((renglonFactura.PrecioConIVA * renglonFactura.PorcentajeDescuento)/100)  As Costo FROM renglonFactura \n";
            vSql = vSql + " INNER JOIN  dbo.ArticuloInventario ON dbo.renglonFactura.ConsecutivoCompania = dbo.ArticuloInventario.ConsecutivoCompania \n ";
            vSql = vSql + "  AND dbo.renglonFactura.Articulo = dbo.ArticuloInventario.Codigo\n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + Compania + " AND NumeroFactura ='" + NumeroFactura + "'\n";
            vSql = vSql + "  AND dbo.ArticuloInventario.Categoria ='" + Cate + "')\n";

            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Costo = reader["Costo"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return Costo;
        }

        private string sBucarCantidad(string NumeroFactura, string Compania, string Cate)
        {
            string Cantidad = "";
            string vSql = "";
            vSql = vSql + " (SELECT  SUM(renglonFactura.Cantidad)  As Cantidad FROM renglonFactura \n";
            vSql = vSql + " INNER JOIN  dbo.ArticuloInventario ON dbo.renglonFactura.ConsecutivoCompania = dbo.ArticuloInventario.ConsecutivoCompania \n ";
            vSql = vSql + "  AND dbo.renglonFactura.Articulo = dbo.ArticuloInventario.Codigo\n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + Compania + " AND NumeroFactura ='" + NumeroFactura + "'\n";
            vSql = vSql + "  AND dbo.ArticuloInventario.Categoria ='" + Cate + "')\n";

            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Cantidad = reader["Cantidad"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            return Cantidad;
        }

        private string sBucarCantidadCategoria(string NumeroFactura, string Compania, string Cate)
        {
            string Cantidad = "";
            string vSql = "";


            vSql = vSql + " (SELECT  SUM(renglonFactura.Cantidad)  As Cantidad FROM renglonFactura \n";
            vSql = vSql + " INNER JOIN  dbo.ArticuloInventario ON dbo.renglonFactura.ConsecutivoCompania = dbo.ArticuloInventario.ConsecutivoCompania \n ";
            vSql = vSql + "  AND dbo.renglonFactura.Articulo = dbo.ArticuloInventario.Codigo\n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + Compania + " AND NumeroFactura ='" + NumeroFactura + "'\n";
            vSql = vSql + "  AND dbo.ArticuloInventario.Categoria IN ('" + Cate + "'))\n";
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Cantidad = reader["Cantidad"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            if (Cantidad =="NULL"){
                Cantidad = "0";
            }
            if (Cantidad == "")
            {
                Cantidad = "0";
            }
            
            return Cantidad;
        }

        private string sBucarMontoFacturadoCategoria(string NumeroFactura, string Compania, string Cate)
        {
            string Cantidad = "";
            string vSql = "";


            vSql = vSql + " (SELECT  SUM(renglonFactura.TotalRenglon)  As Cantidad FROM renglonFactura \n";
            vSql = vSql + " INNER JOIN  dbo.ArticuloInventario ON dbo.renglonFactura.ConsecutivoCompania = dbo.ArticuloInventario.ConsecutivoCompania \n ";
            vSql = vSql + "  AND dbo.renglonFactura.Articulo = dbo.ArticuloInventario.Codigo\n";
            vSql = vSql + " WHERE dbo.renglonFactura.ConsecutivoCompania = " + Compania + " AND NumeroFactura ='" + NumeroFactura + "'\n";
            vSql = vSql + "  AND dbo.ArticuloInventario.Categoria IN ('" + Cate + "'))\n";
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Cantidad = reader["Cantidad"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            if (Cantidad == "NULL")
            {
                Cantidad = "0";
            }
            if (Cantidad == "")
            {
                Cantidad = "0";
            }

            return Cantidad;
        }

        private string sBucarCambioFactura(string NumeroFactura, string Compania)
        {
            string Cambio = "";
            string vSql = "";


            vSql = vSql + " SELECT CambioMonedaCXC AS Cantidad from factura \n";
            vSql = vSql + " WHERE ConsecutivoCompania = " + Compania + " AND Numero='" + NumeroFactura + "'\n";
            
            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Cambio = reader["Cantidad"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            if (Cambio == "NULL")
            {
                Cambio = "0";
            }
            if (Cambio == "")
            {
                Cambio = "0";
            }

            return Cambio;
        }

        private string sBucarMonedaFactura(string NumeroFactura, string Compania)
        {
            string Moneda = "";
            string vSql = "";


            vSql = vSql + " SELECT Moneda AS Cantidad from factura \n";
            vSql = vSql + " WHERE ConsecutivoCompania = " + Compania + " AND Numero='" + NumeroFactura + "'\n";

            using (SqlConnection connection = new SqlConnection(GetConnectionStringByProvider("System.Data.SqlClient",
                                                    "IntegrarProtectora")))
            {
                SqlCommand command = new SqlCommand(vSql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Moneda = reader["Cantidad"].ToString();
                    }
                    reader.NextResult();
                }
                connection.Close();
            }
            if (Moneda == "NULL")
            {
                Moneda = "0";
            }
            if (Moneda == "")
            {
                Moneda = "0";
            }

            return Moneda;
        }


        private DataTable CreateTablaFacturaResumenCategoria(DataTable FacturaTable, string vCategoriaBusqueda)
        {
            DataTable FacturaTableTempora = new DataTable("Factura");
            DataColumn dtColumn;
            DataRow myDataRow;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CodigoVendedor";
            dtColumn.Caption = "Código Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreVendedor";
            dtColumn.Caption = "Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NumeroCobranza";
            dtColumn.Caption = "N° Cobranza";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NumeroFactura";
            dtColumn.Caption = "N° Factura";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Fecha";
            dtColumn.Caption = "Fecha";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Cliente";
            dtColumn.Caption = "Cliente";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Categoria";
            dtColumn.Caption = "Categoria";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoCobranza";
            dtColumn.Caption = "Monto Cobranza";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "TASA";
            dtColumn.Caption = "TASA";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "COSTO";
            dtColumn.Caption = "COSTO";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "PAGO";
            dtColumn.Caption = "PAGO $";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "KILOS";
            dtColumn.Caption = "KILOS";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "COMISION";
            dtColumn.Caption = "COMISION";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MonedaCobro";
            dtColumn.Caption = "MonedaCobro";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);
            foreach (DataRow row in FacturaTable.Rows) 

            {
                string vCategoria = Convert.ToString(row["Categoria"]);
                if (vCategoria.CompareTo(vCategoriaBusqueda)== 0)
                 {
 
                        myDataRow = FacturaTableTempora.NewRow();
                
                        myDataRow["CodigoVendedor"] = Convert.ToString(row["CodigoVendedor"]);
                        myDataRow["NombreVendedor"] = Convert.ToString(row["NombreVendedor"]);
                        myDataRow["NumeroCobranza"] = Convert.ToString(row["NumeroCobranza"]);
                        myDataRow["NumeroFactura"] = Convert.ToString(row["NumeroFactura"]);
                        myDataRow["Fecha"] = Convert.ToString(row["Fecha"]);
                        myDataRow["Cliente"] = Convert.ToString(row["Cliente"]);
                        myDataRow["MontoCobranza"] = Convert.ToString(row["MontoCobranza"]);
                        myDataRow["TASA"] = Convert.ToString(row["TASA"]);
                        myDataRow["MonedaCobro"] = Convert.ToString(row["MonedaCobro"]);
                        myDataRow["PAGO"] = Convert.ToString(row["PAGO"]);
                        myDataRow["COSTO"] = Convert.ToString(row["COSTO"]);
                        myDataRow["Categoria"] = Convert.ToString(row["Categoria"]);
                        myDataRow["KILOS"] = Convert.ToString(row["KILOS"]);
                        myDataRow["COMISION"] = 0;
                        FacturaTableTempora.Rows.Add(myDataRow);
               }
            }
            return FacturaTableTempora;
        }

        private DataTable CreateTablaFacturaResumenCategoriaGene(DataTable FacturaTable)
        {
            DataTable FacturaTableTempora = new DataTable("Factura");
            DataColumn dtColumn;
            DataRow myDataRow;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CodigoVendedor";
            dtColumn.Caption = "Código Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NombreVendedor";
            dtColumn.Caption = "Vendedor";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NumeroCobranza";
            dtColumn.Caption = "N° Cobranza";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "NumeroFactura";
            dtColumn.Caption = "N° Factura";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Fecha";
            dtColumn.Caption = "Fecha";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Cliente";
            dtColumn.Caption = "Cliente";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Categoria";
            dtColumn.Caption = "Categoria";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MontoCobranza";
            dtColumn.Caption = "Monto Cobranza";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "TASA";
            dtColumn.Caption = "TASA";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "COSTO";
            dtColumn.Caption = "COSTO";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);



            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "PAGO";
            dtColumn.Caption = "PAGO $";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "KILOS";
            dtColumn.Caption = "KILOS";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "COMISION";
            dtColumn.Caption = "COMISION";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "MonedaCobro";
            dtColumn.Caption = "MonedaCobro";
            dtColumn.AutoIncrement = false;
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            FacturaTableTempora.Columns.Add(dtColumn);
            foreach (DataRow row in FacturaTable.Rows)
            {
                string vCategoria = Convert.ToString(row["Categoria"]);
                if (vCategoria.CompareTo("CATEGORIA") == 0 || vCategoria.CompareTo("50Y100") == 0 || vCategoria.CompareTo("GRANO") == 0)
                {

                    myDataRow = FacturaTableTempora.NewRow();
                 
                    myDataRow["CodigoVendedor"] = Convert.ToString(row["CodigoVendedor"]);
                    myDataRow["NombreVendedor"] = Convert.ToString(row["NombreVendedor"]);
                    myDataRow["NumeroCobranza"] = Convert.ToString(row["NumeroCobranza"]);
                    myDataRow["NumeroFactura"] = Convert.ToString(row["NumeroFactura"]);
                    myDataRow["Fecha"] = Convert.ToString(row["Fecha"]);
                    myDataRow["Cliente"] = Convert.ToString(row["Cliente"]);
                    myDataRow["MontoCobranza"] = Convert.ToString(row["MontoCobranza"]);
                    myDataRow["TASA"] = Convert.ToString(row["TASA"]);
                    myDataRow["MonedaCobro"] = Convert.ToString(row["MonedaCobro"]);
                    myDataRow["PAGO"] = Convert.ToString(row["PAGO"]);
                    myDataRow["COSTO"] = Convert.ToString(row["COSTO"]);
                    myDataRow["Categoria"] = Convert.ToString(row["Categoria"]);
                    myDataRow["KILOS"] = Convert.ToString(row["KILOS"]);
                    myDataRow["COMISION"] = 0;
                    FacturaTableTempora.Rows.Add(myDataRow);
                }
            }
            return FacturaTableTempora;
        }

        private void CxCVendedor_Click(object sender, EventArgs e)
        {
            button1.Text = "Cuenta por Cobrar Vendedor";
            panel1.Enabled = false;

        }
       


    }
}
