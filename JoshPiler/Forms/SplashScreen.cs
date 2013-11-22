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
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashScreen_KeyPress(object sender, KeyPressEventArgs e)
        {this.Close();}

        private void pictureBox1_Click(object sender, EventArgs e)
        { this.Close(); }


    } // SplashScreen
} // namespace JoshPiler.Forms
