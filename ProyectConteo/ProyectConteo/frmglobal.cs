using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProyectConteo {
    public partial class frmglobal : Form {
        public frmglobal() {
            InitializeComponent();
        }

        private void frmglobal_Load(object sender, EventArgs e) {
            // TODO: This line of code loads data into the 'Dataconteo.Sp_ProcesoConteoPorLineaCategoriaGeneral' table. You can move, or remove it, as needed.
            

        }

        private void button1_Click(object sender, EventArgs e) {
            this.Sp_ProcesoConteoPorLineaCategoriaGeneralTableAdapter.Fill(this.Dataconteo.Sp_ProcesoConteoPorLineaCategoriaGeneral,8,  dateInicio.Text, datefin.Text);
            this.reportViewer2.RefreshReport();
        }

        private void button3_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
