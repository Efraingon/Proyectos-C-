namespace DistribucionCosto
{
    partial class FrmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ArticuloDiario = new System.Windows.Forms.ToolStripButton();
            this.CategoriaDiario = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaIni = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCodigoEmpresa = new System.Windows.Forms.TextBox();
            this.txtNombreVendedor = new System.Windows.Forms.TextBox();
            this.txtCodigoVendedor = new System.Windows.Forms.TextBox();
            this.lblVendedor1 = new System.Windows.Forms.Label();
            this.txtNombreEmpresa = new System.Windows.Forms.TextBox();
            this.lblCompania1 = new System.Windows.Forms.Label();
            this.lblVendedor = new System.Windows.Forms.Label();
            this.listVendedor = new System.Windows.Forms.ListView();
            this.listEmpresa = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(198, 503);
            this.panel2.TabIndex = 106;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AllowDrop = true;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ArticuloDiario,
            this.CategoriaDiario});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 18);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(196, 153);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 5;
            // 
            // ArticuloDiario
            // 
            this.ArticuloDiario.Image = ((System.Drawing.Image)(resources.GetObject("ArticuloDiario.Image")));
            this.ArticuloDiario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ArticuloDiario.Name = "ArticuloDiario";
            this.ArticuloDiario.Size = new System.Drawing.Size(194, 68);
            this.ArticuloDiario.Text = "Costo de Facturación";
            this.ArticuloDiario.Click += new System.EventHandler(this.ArticuloDiario_Click);
            // 
            // CategoriaDiario
            // 
            this.CategoriaDiario.Image = ((System.Drawing.Image)(resources.GetObject("CategoriaDiario.Image")));
            this.CategoriaDiario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CategoriaDiario.Name = "CategoriaDiario";
            this.CategoriaDiario.Size = new System.Drawing.Size(194, 68);
            this.CategoriaDiario.Text = "Comparativo cobranza";
            this.CategoriaDiario.ToolTipText = "Comparativo de Fact. por Categoria";
            this.CategoriaDiario.Click += new System.EventHandler(this.CategoriaDiario_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpFechaFin);
            this.panel1.Controls.Add(this.dtpFechaIni);
            this.panel1.Location = new System.Drawing.Point(528, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 44);
            this.panel1.TabIndex = 107;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(179, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Fecha Fin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Fecha Inicio";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(239, 14);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(82, 20);
            this.dtpFechaFin.TabIndex = 13;
            // 
            // dtpFechaIni
            // 
            this.dtpFechaIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaIni.Location = new System.Drawing.Point(92, 14);
            this.dtpFechaIni.Name = "dtpFechaIni";
            this.dtpFechaIni.Size = new System.Drawing.Size(82, 20);
            this.dtpFechaIni.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(435, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 20);
            this.label3.TabIndex = 108;
            this.label3.Text = "Informes Analiticos";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.Location = new System.Drawing.Point(855, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 62);
            this.button1.TabIndex = 109;
            this.button1.Text = "Costo de Facturación";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCodigoEmpresa);
            this.groupBox2.Controls.Add(this.txtNombreVendedor);
            this.groupBox2.Controls.Add(this.txtCodigoVendedor);
            this.groupBox2.Controls.Add(this.lblVendedor1);
            this.groupBox2.Controls.Add(this.txtNombreEmpresa);
            this.groupBox2.Controls.Add(this.lblCompania1);
            this.groupBox2.Location = new System.Drawing.Point(538, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(432, 101);
            this.groupBox2.TabIndex = 111;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Busqueda";
            // 
            // txtCodigoEmpresa
            // 
            this.txtCodigoEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtCodigoEmpresa.Location = new System.Drawing.Point(16, 75);
            this.txtCodigoEmpresa.Name = "txtCodigoEmpresa";
            this.txtCodigoEmpresa.Size = new System.Drawing.Size(90, 20);
            this.txtCodigoEmpresa.TabIndex = 123;
            this.txtCodigoEmpresa.Visible = false;
            // 
            // txtNombreVendedor
            // 
            this.txtNombreVendedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNombreVendedor.Location = new System.Drawing.Point(173, 45);
            this.txtNombreVendedor.Name = "txtNombreVendedor";
            this.txtNombreVendedor.Size = new System.Drawing.Size(255, 20);
            this.txtNombreVendedor.TabIndex = 122;
            // 
            // txtCodigoVendedor
            // 
            this.txtCodigoVendedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtCodigoVendedor.Location = new System.Drawing.Point(74, 45);
            this.txtCodigoVendedor.Name = "txtCodigoVendedor";
            this.txtCodigoVendedor.Size = new System.Drawing.Size(90, 20);
            this.txtCodigoVendedor.TabIndex = 121;
            // 
            // lblVendedor1
            // 
            this.lblVendedor1.AutoSize = true;
            this.lblVendedor1.Location = new System.Drawing.Point(7, 52);
            this.lblVendedor1.Name = "lblVendedor1";
            this.lblVendedor1.Size = new System.Drawing.Size(53, 13);
            this.lblVendedor1.TabIndex = 120;
            this.lblVendedor1.Text = "Vendedor";
            // 
            // txtNombreEmpresa
            // 
            this.txtNombreEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNombreEmpresa.Location = new System.Drawing.Point(74, 19);
            this.txtNombreEmpresa.Name = "txtNombreEmpresa";
            this.txtNombreEmpresa.Size = new System.Drawing.Size(354, 20);
            this.txtNombreEmpresa.TabIndex = 94;
            // 
            // lblCompania1
            // 
            this.lblCompania1.AutoSize = true;
            this.lblCompania1.Location = new System.Drawing.Point(6, 22);
            this.lblCompania1.Name = "lblCompania1";
            this.lblCompania1.Size = new System.Drawing.Size(54, 13);
            this.lblCompania1.TabIndex = 90;
            this.lblCompania1.Text = "Compañia";
            // 
            // lblVendedor
            // 
            this.lblVendedor.AutoSize = true;
            this.lblVendedor.Location = new System.Drawing.Point(204, 174);
            this.lblVendedor.Name = "lblVendedor";
            this.lblVendedor.Size = new System.Drawing.Size(53, 13);
            this.lblVendedor.TabIndex = 114;
            this.lblVendedor.Text = "Vendedor";
            this.lblVendedor.Visible = false;
            // 
            // listVendedor
            // 
            this.listVendedor.BackColor = System.Drawing.Color.White;
            this.listVendedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listVendedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listVendedor.Location = new System.Drawing.Point(204, 190);
            this.listVendedor.Name = "listVendedor";
            this.listVendedor.Size = new System.Drawing.Size(328, 156);
            this.listVendedor.TabIndex = 113;
            this.listVendedor.UseCompatibleStateImageBehavior = false;
            this.listVendedor.View = System.Windows.Forms.View.Details;
            this.listVendedor.SelectedIndexChanged += new System.EventHandler(this.listVendedor_SelectedIndexChanged);
            // 
            // listEmpresa
            // 
            this.listEmpresa.BackColor = System.Drawing.Color.White;
            this.listEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listEmpresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listEmpresa.Location = new System.Drawing.Point(204, 36);
            this.listEmpresa.Name = "listEmpresa";
            this.listEmpresa.Size = new System.Drawing.Size(328, 135);
            this.listEmpresa.TabIndex = 112;
            this.listEmpresa.UseCompatibleStateImageBehavior = false;
            this.listEmpresa.View = System.Windows.Forms.View.Details;
            this.listEmpresa.SelectedIndexChanged += new System.EventHandler(this.listEmpresa_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(204, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 115;
            this.label4.Text = "Empresa";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Purple;
            this.label5.Location = new System.Drawing.Point(204, 364);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(281, 16);
            this.label5.TabIndex = 117;
            this.label5.Text = "Inicio ..........................................................";
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 503);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblVendedor);
            this.Controls.Add(this.listVendedor);
            this.Controls.Add(this.listEmpresa);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.IsMdiContainer = true;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Principal Distribuidora";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ArticuloDiario;
        private System.Windows.Forms.ToolStripButton CategoriaDiario;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaIni;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtNombreVendedor;
        private System.Windows.Forms.TextBox txtCodigoVendedor;
        private System.Windows.Forms.Label lblVendedor1;
        private System.Windows.Forms.TextBox txtNombreEmpresa;
        private System.Windows.Forms.Label lblCompania1;
        private System.Windows.Forms.Label lblVendedor;
        private System.Windows.Forms.ListView listVendedor;
        private System.Windows.Forms.ListView listEmpresa;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCodigoEmpresa;
        private System.Windows.Forms.Label label5;
    }
}

