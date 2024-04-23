namespace ProyectConteo {
    partial class frmFacturaruta {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFacturaruta));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.Sp_ProcesoConteoPorLineaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Dataconteo = new ProyectConteo.Dataconteo();
            this.Sp_ProcesoConteoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.listLinea = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLinea = new System.Windows.Forms.TextBox();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.datefin = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.labelinicio = new System.Windows.Forms.Label();
            this.dateInicio = new System.Windows.Forms.DateTimePicker();
            this.listEmpresa = new System.Windows.Forms.ListView();
            this.Sp_ProcesoConteoTableAdapter = new ProyectConteo.DataconteoTableAdapters.Sp_ProcesoConteoTableAdapter();
            this.Sp_ProcesoConteoPorLineaTableAdapter = new ProyectConteo.DataconteoTableAdapters.Sp_ProcesoConteoPorLineaTableAdapter();
            this.button3 = new System.Windows.Forms.Button();
            this.Sp_ProcesoConteoPorLineaCategoriaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Sp_ProcesoConteoPorLineaCategoriaTableAdapter = new ProyectConteo.DataconteoTableAdapters.Sp_ProcesoConteoPorLineaCategoriaTableAdapter();
            this.reportViewer2 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoPorLineaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dataconteo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoPorLineaCategoriaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Sp_ProcesoConteoPorLineaBindingSource
            // 
            this.Sp_ProcesoConteoPorLineaBindingSource.DataMember = "Sp_ProcesoConteoPorLinea";
            this.Sp_ProcesoConteoPorLineaBindingSource.DataSource = this.Dataconteo;
            // 
            // Dataconteo
            // 
            this.Dataconteo.DataSetName = "Dataconteo";
            this.Dataconteo.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Sp_ProcesoConteoBindingSource
            // 
            this.Sp_ProcesoConteoBindingSource.DataMember = "Sp_ProcesoConteo";
            this.Sp_ProcesoConteoBindingSource.DataSource = this.Dataconteo;
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSetCateroria";
            reportDataSource1.Value = this.Sp_ProcesoConteoPorLineaCategoriaBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "ProyectConteo.rptReporteFactura.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(2, 158);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1017, 602);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.listLinea);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.listEmpresa);
            this.groupBox1.Location = new System.Drawing.Point(2, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1017, 152);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(548, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Listar";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listLinea
            // 
            this.listLinea.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listLinea.AllowColumnReorder = true;
            this.listLinea.BackColor = System.Drawing.Color.Aquamarine;
            this.listLinea.BackgroundImageTiled = true;
            this.listLinea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listLinea.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listLinea.Location = new System.Drawing.Point(630, 19);
            this.listLinea.Name = "listLinea";
            this.listLinea.Size = new System.Drawing.Size(284, 119);
            this.listLinea.TabIndex = 103;
            this.listLinea.UseCompatibleStateImageBehavior = false;
            this.listLinea.View = System.Windows.Forms.View.Details;
            this.listLinea.SelectedIndexChanged += new System.EventHandler(this.listLinea_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtLinea);
            this.groupBox2.Controls.Add(this.txtRuta);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.datefin);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.labelinicio);
            this.groupBox2.Controls.Add(this.dateInicio);
            this.groupBox2.Location = new System.Drawing.Point(0, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(334, 126);
            this.groupBox2.TabIndex = 102;
            this.groupBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Linea:";
            // 
            // txtLinea
            // 
            this.txtLinea.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLinea.Location = new System.Drawing.Point(57, 42);
            this.txtLinea.Name = "txtLinea";
            this.txtLinea.Size = new System.Drawing.Size(196, 20);
            this.txtLinea.TabIndex = 11;
            this.txtLinea.TextChanged += new System.EventHandler(this.txtLinea_TextChanged);
            // 
            // txtRuta
            // 
            this.txtRuta.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRuta.Location = new System.Drawing.Point(57, 16);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(196, 20);
            this.txtRuta.TabIndex = 10;
            this.txtRuta.TextChanged += new System.EventHandler(this.txtRuta_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ruta:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Fin";
            // 
            // datefin
            // 
            this.datefin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datefin.Location = new System.Drawing.Point(170, 68);
            this.datefin.Name = "datefin";
            this.datefin.Size = new System.Drawing.Size(83, 20);
            this.datefin.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.GreenYellow;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(259, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 43);
            this.button1.TabIndex = 6;
            this.button1.Text = "Genera";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelinicio
            // 
            this.labelinicio.AutoSize = true;
            this.labelinicio.Location = new System.Drawing.Point(18, 74);
            this.labelinicio.Name = "labelinicio";
            this.labelinicio.Size = new System.Drawing.Size(32, 13);
            this.labelinicio.TabIndex = 6;
            this.labelinicio.Text = "Inicio";
            // 
            // dateInicio
            // 
            this.dateInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateInicio.Location = new System.Drawing.Point(56, 68);
            this.dateInicio.Name = "dateInicio";
            this.dateInicio.Size = new System.Drawing.Size(83, 20);
            this.dateInicio.TabIndex = 5;
            // 
            // listEmpresa
            // 
            this.listEmpresa.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listEmpresa.AllowColumnReorder = true;
            this.listEmpresa.BackColor = System.Drawing.Color.Aquamarine;
            this.listEmpresa.BackgroundImageTiled = true;
            this.listEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listEmpresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listEmpresa.Location = new System.Drawing.Point(340, 19);
            this.listEmpresa.Name = "listEmpresa";
            this.listEmpresa.Size = new System.Drawing.Size(284, 119);
            this.listEmpresa.TabIndex = 101;
            this.listEmpresa.UseCompatibleStateImageBehavior = false;
            this.listEmpresa.View = System.Windows.Forms.View.Details;
            this.listEmpresa.SelectedIndexChanged += new System.EventHandler(this.listEmpresa_SelectedIndexChanged);
            // 
            // Sp_ProcesoConteoTableAdapter
            // 
            this.Sp_ProcesoConteoTableAdapter.ClearBeforeFill = true;
            // 
            // Sp_ProcesoConteoPorLineaTableAdapter
            // 
            this.Sp_ProcesoConteoPorLineaTableAdapter.ClearBeforeFill = true;
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.Location = new System.Drawing.Point(948, 102);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 36);
            this.button3.TabIndex = 3;
            this.button3.Text = "Salir";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Sp_ProcesoConteoPorLineaCategoriaBindingSource
            // 
            this.Sp_ProcesoConteoPorLineaCategoriaBindingSource.DataMember = "Sp_ProcesoConteoPorLineaCategoria";
            this.Sp_ProcesoConteoPorLineaCategoriaBindingSource.DataSource = this.Dataconteo;
            // 
            // Sp_ProcesoConteoPorLineaCategoriaTableAdapter
            // 
            this.Sp_ProcesoConteoPorLineaCategoriaTableAdapter.ClearBeforeFill = true;
            // 
            // reportViewer2
            // 
            reportDataSource2.Name = "DataSetCateroria";
            reportDataSource2.Value = this.Sp_ProcesoConteoPorLineaCategoriaBindingSource;
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer2.LocalReport.ReportEmbeddedResource = "ProyectConteo.rptReporteFacturaNo.rdlc";
            this.reportViewer2.Location = new System.Drawing.Point(2, 158);
            this.reportViewer2.Name = "reportViewer2";
            this.reportViewer2.Size = new System.Drawing.Size(1017, 602);
            this.reportViewer2.TabIndex = 4;
            this.reportViewer2.Visible = false;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(920, 24);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(73, 17);
            this.radioButton1.TabIndex = 104;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Facturado";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(920, 57);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(83, 17);
            this.radioButton2.TabIndex = 105;
            this.radioButton2.Text = "Por Facturar";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // frmFacturaruta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 772);
            this.Controls.Add(this.reportViewer2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmFacturaruta";
            this.Load += new System.EventHandler(this.frmFacturaruta_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoPorLineaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dataconteo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoPorLineaCategoriaBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource Sp_ProcesoConteoBindingSource;
        private Dataconteo Dataconteo;
        private DataconteoTableAdapters.Sp_ProcesoConteoTableAdapter Sp_ProcesoConteoTableAdapter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listEmpresa;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker datefin;
        private System.Windows.Forms.Label labelinicio;
        private System.Windows.Forms.DateTimePicker dateInicio;
        private System.Windows.Forms.ListView listLinea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLinea;
        private System.Windows.Forms.BindingSource Sp_ProcesoConteoPorLineaBindingSource;
        private DataconteoTableAdapters.Sp_ProcesoConteoPorLineaTableAdapter Sp_ProcesoConteoPorLineaTableAdapter;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.BindingSource Sp_ProcesoConteoPorLineaCategoriaBindingSource;
        private DataconteoTableAdapters.Sp_ProcesoConteoPorLineaCategoriaTableAdapter Sp_ProcesoConteoPorLineaCategoriaTableAdapter;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

