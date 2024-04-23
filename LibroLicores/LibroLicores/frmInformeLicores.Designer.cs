namespace LibroLicores
{
    partial class frmInformeLicores
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.TableFacturaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataFactura = new LibroLicores.DataFactura();
            ((System.ComponentModel.ISupportInitialize)(this.TableFacturaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataFactura)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataFactura";
            reportDataSource1.Value = this.TableFacturaBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "LibroLicores.ReportLibro.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1163, 510);
            this.reportViewer1.TabIndex = 0;
            // 
            // TableFacturaBindingSource
            // 
            this.TableFacturaBindingSource.DataMember = "TableFactura";
            this.TableFacturaBindingSource.DataSource = this.DataFactura;
            // 
            // DataFactura
            // 
            this.DataFactura.DataSetName = "DataFactura";
            this.DataFactura.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // frmInformeLicores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 510);
            this.Controls.Add(this.reportViewer1);
            this.Name = "frmInformeLicores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Libro de Licores";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInformeLicores_FormClosing);
            this.Load += new System.EventHandler(this.frmInformeLicores_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TableFacturaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataFactura)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource TableFacturaBindingSource;
        private DataFactura DataFactura;
    }
}