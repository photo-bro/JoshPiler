using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JoshPiler.Forms; // OptimizerForm

namespace JoshPiler
{
    public partial class FlagsForm : Form
    {
        // facade instance
        private Facade m_fac = Facade.Instance;

        // emitter instance
        private Emitter m_Em = Emitter.Instance;

        /// <summary>
        /// Form constructor
        /// </summary>
        public FlagsForm()
        {
            InitializeComponent();
            chbxMoreComments.Checked = m_fac.FlagMoreComments;
            chbxDisableErrorPop.Checked = ErrorHandler.ShowPopups;
            chbxOptAsm.Checked = m_Em.m_bOptimize;
            chbxASMDebug.Checked = (Parser.c_bParserASMDebug && Emitter.c_bEmitterASMDebug);
        } // FlagsForm()

        /// <summary>
        /// OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        { this.Close(); }

        /// <summary>
        /// MoreComments checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbxMoreComments_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Em.m_bOptimize)
            {
                chbxMoreComments.Checked = false;
                MessageBox.Show("Cannot enable More Comments when Optimize Assembly is enabled");
            }
            else
                m_fac.FlagMoreComments = chbxMoreComments.Checked;
        }

        /// <summary>
        /// Error popup checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbxDisableErrorPop_CheckedChanged(object sender, EventArgs e)
        { ErrorHandler.ShowPopups = chbxDisableErrorPop.Checked; }

        private void chbxOptAsm_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_fac.FlagMoreComments)
            //{
            //    chbxOptAsm.Checked = false;
            //    MessageBox.Show("Cannot optimize assembly when More Comments is enabled");
            //}
            //else
                m_Em.m_bOptimize = chbxOptAsm.Checked;
        }

        private void btnOptOptions_Click(object sender, EventArgs e)
        {
            OptimizerForm of = new OptimizerForm();
            of.Show();
        }

        private void chbxASMDebug_CheckedChanged(object sender, EventArgs e)
        {
            Parser.c_bParserASMDebug = chbxASMDebug.Checked;
            Emitter.c_bEmitterASMDebug = chbxASMDebug.Checked;
        }

    } // FlagsForm
} // namespace JoshPiler
