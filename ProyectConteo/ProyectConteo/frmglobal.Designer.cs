namespace ProyectConteo {
    partial class frmglobal {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmglobal));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.button3 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.datefin = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.labelinicio = new System.Windows.Forms.Label();
            this.dateInicio = new System.Windows.Forms.DateTimePicker();
            this.reportViewer2 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Dataconteo = new ProyectConteo.Dataconteo();
            this.Sp_ProcesoConteoPorLineaCategoriaGeneralTableAdapter = new ProyectConteo.DataconteoTableAdapters.Sp_ProcesoConteoPorLineaCategoriaGeneralTableAdapter();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dataconteo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.datefin);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.labelinicio);
            this.groupBox2.Controls.Add(this.dateInicio);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(996, 51);
            this.groupBox2.TabIndex = 103;
            this.groupBox2.TabStop = false;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(370, 15);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(73, 17);
            this.radioButton3.TabIndex = 109;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Facturado";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(462, 17);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(83, 17);
            this.radioButton2.TabIndex = 108;
            this.radioButton2.Text = "Por Facturar";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.Location = new System.Drawing.Point(945, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 36);
            this.button3.TabIndex = 106;
            this.button3.Text = "Salir";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(218, -16);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(73, 17);
            this.radioButton1.TabIndex = 107;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Facturado";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Fin";
            // 
            // datefin
            // 
            this.datefin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datefin.Location = new System.Drawing.Point(189, 19);
            this.datefin.Name = "datefin";
            this.datefin.Size = new System.Drawing.Size(83, 20);
            this.datefin.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.GreenYellow;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(278, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 25);
            this.button1.TabIndex = 6;
            this.button1.Text = "Genera";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelinicio
            // 
            this.labelinicio.AutoSize = true;
            this.labelinicio.Location = new System.Drawing.Point(8, 25);
            this.labelinicio.Name = "labelinicio";
            this.labelinicio.Size = new System.Drawing.Size(32, 13);
            this.labelinicio.TabIndex = 6;
            this.labelinicio.Text = "Inicio";
            // 
            // dateInicio
            // 
            this.dateInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateInicio.Location = new System.Drawing.Point(46, 19);
            this.dateInicio.Name = "dateInicio";
            this.dateInicio.Size = new System.Drawing.Size(83, 20);
            this.dateInicio.TabIndex = 5;
            // 
            // reportViewer2
            // 
            reportDataSource1.Name = "DataSet";
            reportDataSource1.Value = this.Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource;
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer2.LocalReport.ReportEmbeddedResource = "ProyectConteo.rptGlobal.rdlc";
            this.reportViewer2.Location = new System.Drawing.Point(12, 69);
            this.reportViewer2.Name = "reportViewer2";
            this.reportViewer2.Size = new System.Drawing.Size(996, 556);
            this.reportViewer2.TabIndex = 104;
            // 
            // Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource
            // 
            this.Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource.DataMember = "Sp_ProcesoConteoPorLineaCategoriaGeneral";
            this.Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource.DataSource = this.Dataconteo;
            // 
            // Dataconteo
            // 
            this.Dataconteo.DataSetName = "Dataconteo";
            this.Dataconteo.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Sp_ProcesoConteoPorLineaCategoriaGeneralTableAdapter
            // 
            this.Sp_ProcesoConteoPorLineaCategoriaGeneralTableAdapter.ClearBeforeFill = true;
            // 
            // frmglobal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 637);
            this.Controls.Add(this.reportViewer2);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmglobal";
            this.Text = "Situación Empresa";
            this.Load += new System.EventHandler(this.frmglobal_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dataconteo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker datefin;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelinicio;
        private System.Windows.Forms.DateTimePicker dateInicio;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton3;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer2;
        private System.Windows.Forms.BindingSource Sp_ProcesoConteoPorLineaCategoriaGeneralBindingSource;
        private Dataconteo Dataconteo;
        private DataconteoTableAdapters.Sp_ProcesoConteoPorLineaCategoriaGeneralTableAdapter Sp_ProcesoConteoPorLineaCategoriaGeneralTableAdapter;
    }
}