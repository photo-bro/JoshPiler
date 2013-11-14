using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JoshPiler
{
    public partial class ErrorConsoleForm : Form
    {
        public ErrorConsoleForm()
        {
            InitializeComponent();
        }

        private void ErrorConsoleForm_Load(object sender, EventArgs e)
        {
            tbErrorConsole.Text = ErrorHandler.ErrorLog;
        }

        private void btnSaveErrCons_Click(object sender, EventArgs e)
        {
            ErrorHandler.SaveErrorLog();
        }

        private void btnErrClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnErrRefresh_Click(object sender, EventArgs e)
        {
            tbErrorConsole.Text = ErrorHandler.ErrorLog;
        }
    }
}
