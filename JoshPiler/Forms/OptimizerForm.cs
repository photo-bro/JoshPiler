using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JoshPiler.Forms
{
    public partial class OptimizerForm : Form
    {

        public OptimizerForm()
        {
            InitializeComponent();
            chbxDupPushPop.Checked = Optimizer.c_bDupPushPop;
            chbxRedunMov.Checked = Optimizer.c_bRedunMov;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {this.Close();}

        private void chbxDupPushPop_CheckedChanged(object sender, EventArgs e)
        { Optimizer.c_bDupPushPop = chbxDupPushPop.Checked;}

        private void chbxRedunMov_CheckedChanged(object sender, EventArgs e)
        { Optimizer.c_bRedunMov = chbxRedunMov.Checked; }
    }
}
