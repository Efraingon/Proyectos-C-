namespace LibroLicores
{
    partial class frmLicores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLicores));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelProgreso = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFechaIni = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtNombreEmpresa = new System.Windows.Forms.TextBox();
            this.lblCompania1 = new System.Windows.Forms.Label();
            this.listEmpresa = new System.Windows.Forms.ListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.CompMenFacLinea = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCodigoEmpresa = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelProgreso);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.listEmpresa);
            this.groupBox1.Location = new System.Drawing.Point(280, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(873, 374);
            this.groupBox1.TabIndex = 107;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Compañias";
            // 
            // labelProgreso
            // 
            this.labelProgreso.AutoSize = true;
            this.labelProgreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgreso.Location = new System.Drawing.Point(407, 300);
            this.labelProgreso.Name = "labelProgreso";
            this.labelProgreso.Size = new System.Drawing.Size(23, 13);
            this.labelProgreso.TabIndex = 117;
            this.labelProgreso.Text = "0%";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 316);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(844, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 116;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.Location = new System.Drawing.Point(734, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 56);
            this.button1.TabIndex = 102;
            this.button1.Text = "Libro de Licores";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpFechaIni);
            this.panel1.Location = new System.Drawing.Point(370, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 44);
            this.panel1.TabIndex = 101;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Mes Aplicar:";
            // 
            // dtpFechaIni
            // 
            this.dtpFechaIni.CustomFormat = "MM/yyyy";
            this.dtpFechaIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaIni.Location = new System.Drawing.Point(84, 15);
            this.dtpFechaIni.Name = "dtpFechaIni";
            this.dtpFechaIni.Size = new System.Drawing.Size(82, 20);
            this.dtpFechaIni.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCodigoEmpresa);
            this.groupBox2.Controls.Add(this.txtNombreEmpresa);
            this.groupBox2.Controls.Add(this.lblCompania1);
            this.groupBox2.Location = new System.Drawing.Point(370, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 64);
            this.groupBox2.TabIndex = 93;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Busqueda";
            // 
            // txtNombreEmpresa
            // 
            this.txtNombreEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNombreEmpresa.Location = new System.Drawing.Point(74, 12);
            this.txtNombreEmpresa.Name = "txtNombreEmpresa";
            this.txtNombreEmpresa.Size = new System.Drawing.Size(386, 20);
            this.txtNombreEmpresa.TabIndex = 94;
            // 
            // lblCompania1
            // 
            this.lblCompania1.AutoSize = true;
            this.lblCompania1.Location = new System.Drawing.Point(6, 16);
            this.lblCompania1.Name = "lblCompania1";
            this.lblCompania1.Size = new System.Drawing.Size(54, 13);
            this.lblCompania1.TabIndex = 90;
            this.lblCompania1.Text = "Compañia";
            // 
            // listEmpresa
            // 
            this.listEmpresa.BackColor = System.Drawing.Color.White;
            this.listEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listEmpresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listEmpresa.Location = new System.Drawing.Point(23, 19);
            this.listEmpresa.Name = "listEmpresa";
            this.listEmpresa.Size = new System.Drawing.Size(341, 264);
            this.listEmpresa.TabIndex = 87;
            this.listEmpresa.UseCompatibleStateImageBehavior = false;
            this.listEmpresa.View = System.Windows.Forms.View.Details;
            this.listEmpresa.SelectedIndexChanged += new System.EventHandler(this.listEmpresa_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AllowDrop = true;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CompMenFacLinea});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 18);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(159, 82);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 5;
            // 
            // CompMenFacLinea
            // 
            this.CompMenFacLinea.Image = ((System.Drawing.Image)(resources.GetObject("CompMenFacLinea.Image")));
            this.CompMenFacLinea.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CompMenFacLinea.Name = "CompMenFacLinea";
            this.CompMenFacLinea.Size = new System.Drawing.Size(157, 68);
            this.CompMenFacLinea.Text = "Libro de Licores";
            this.CompMenFacLinea.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.CompMenFacLinea.ToolTipText = "Comisiones Resumidas";
            this.CompMenFacLinea.Click += new System.EventHandler(this.CompMenFacLinea_Click_1);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(219, 642);
            this.panel2.TabIndex = 108;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Purple;
            this.label5.Location = new System.Drawing.Point(383, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(281, 16);
            this.label5.TabIndex = 118;
            this.label5.Text = "Inicio ..........................................................";
            // 
            // txtCodigoEmpresa
            // 
            this.txtCodigoEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtCodigoEmpresa.Location = new System.Drawing.Point(9, 38);
            this.txtCodigoEmpresa.Name = "txtCodigoEmpresa";
            this.txtCodigoEmpresa.Size = new System.Drawing.Size(90, 20);
            this.txtCodigoEmpresa.TabIndex = 124;
            this.txtCodigoEmpresa.Visible = false;
            // 
            // frmLicores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1182, 642);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Name = "frmLicores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Libro de Licores";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelProgreso;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFechaIni;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtNombreEmpresa;
        private System.Windows.Forms.Label lblCompania1;
        private System.Windows.Forms.ListView listEmpresa;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton CompMenFacLinea;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCodigoEmpresa;
    }
}

