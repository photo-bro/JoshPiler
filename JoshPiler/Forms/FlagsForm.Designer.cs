namespace JoshPiler
{
    partial class FlagsForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.lbMoreComments = new System.Windows.Forms.Label();
            this.gbFlags = new System.Windows.Forms.GroupBox();
            this.chbxOptAsm = new System.Windows.Forms.CheckBox();
            this.lbOptAsm = new System.Windows.Forms.Label();
            this.chbxDisableErrorPop = new System.Windows.Forms.CheckBox();
            this.lbShowErrorForm = new System.Windows.Forms.Label();
            this.chbxMoreComments = new System.Windows.Forms.CheckBox();
            this.btnOptOptions = new System.Windows.Forms.Button();
            this.lbASMDebug = new System.Windows.Forms.Label();
            this.chbxASMDebug = new System.Windows.Forms.CheckBox();
            this.gbFlags.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Location = new System.Drawing.Point(197, 227);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbMoreComments
            // 
            this.lbMoreComments.AutoSize = true;
            this.lbMoreComments.Location = new System.Drawing.Point(16, 57);
            this.lbMoreComments.Name = "lbMoreComments";
            this.lbMoreComments.Size = new System.Drawing.Size(89, 13);
            this.lbMoreComments.TabIndex = 1;
            this.lbMoreComments.Text = "More Comments: ";
            // 
            // gbFlags
            // 
            this.gbFlags.Controls.Add(this.chbxASMDebug);
            this.gbFlags.Controls.Add(this.lbASMDebug);
            this.gbFlags.Controls.Add(this.chbxOptAsm);
            this.gbFlags.Controls.Add(this.lbOptAsm);
            this.gbFlags.Controls.Add(this.chbxDisableErrorPop);
            this.gbFlags.Controls.Add(this.lbShowErrorForm);
            this.gbFlags.Controls.Add(this.chbxMoreComments);
            this.gbFlags.Controls.Add(this.lbMoreComments);
            this.gbFlags.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbFlags.Location = new System.Drawing.Point(12, 12);
            this.gbFlags.Name = "gbFlags";
            this.gbFlags.Size = new System.Drawing.Size(260, 196);
            this.gbFlags.TabIndex = 2;
            this.gbFlags.TabStop = false;
            this.gbFlags.Text = "Compiler Flags";
            // 
            // chbxOptAsm
            // 
            this.chbxOptAsm.AutoSize = true;
            this.chbxOptAsm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chbxOptAsm.Location = new System.Drawing.Point(174, 99);
            this.chbxOptAsm.Name = "chbxOptAsm";
            this.chbxOptAsm.Size = new System.Drawing.Size(57, 17);
            this.chbxOptAsm.TabIndex = 7;
            this.chbxOptAsm.Text = "Enable";
            this.chbxOptAsm.UseVisualStyleBackColor = true;
            this.chbxOptAsm.CheckedChanged += new System.EventHandler(this.chbxOptAsm_CheckedChanged);
            // 
            // lbOptAsm
            // 
            this.lbOptAsm.AutoSize = true;
            this.lbOptAsm.Location = new System.Drawing.Point(16, 100);
            this.lbOptAsm.Name = "lbOptAsm";
            this.lbOptAsm.Size = new System.Drawing.Size(94, 13);
            this.lbOptAsm.TabIndex = 6;
            this.lbOptAsm.Text = "Optimize Assembly";
            // 
            // chbxDisableErrorPop
            // 
            this.chbxDisableErrorPop.AutoSize = true;
            this.chbxDisableErrorPop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chbxDisableErrorPop.Location = new System.Drawing.Point(174, 78);
            this.chbxDisableErrorPop.Name = "chbxDisableErrorPop";
            this.chbxDisableErrorPop.Size = new System.Drawing.Size(57, 17);
            this.chbxDisableErrorPop.TabIndex = 5;
            this.chbxDisableErrorPop.Text = "Enable";
            this.chbxDisableErrorPop.UseVisualStyleBackColor = true;
            this.chbxDisableErrorPop.CheckedChanged += new System.EventHandler(this.chbxDisableErrorPop_CheckedChanged);
            // 
            // lbShowErrorForm
            // 
            this.lbShowErrorForm.AutoSize = true;
            this.lbShowErrorForm.Location = new System.Drawing.Point(16, 78);
            this.lbShowErrorForm.Name = "lbShowErrorForm";
            this.lbShowErrorForm.Size = new System.Drawing.Size(106, 13);
            this.lbShowErrorForm.TabIndex = 4;
            this.lbShowErrorForm.Text = "Disable Error Popups";
            // 
            // chbxMoreComments
            // 
            this.chbxMoreComments.AutoSize = true;
            this.chbxMoreComments.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chbxMoreComments.Location = new System.Drawing.Point(174, 57);
            this.chbxMoreComments.Name = "chbxMoreComments";
            this.chbxMoreComments.Size = new System.Drawing.Size(57, 17);
            this.chbxMoreComments.TabIndex = 3;
            this.chbxMoreComments.Text = "Enable";
            this.chbxMoreComments.UseVisualStyleBackColor = true;
            this.chbxMoreComments.CheckedChanged += new System.EventHandler(this.chbxMoreComments_CheckedChanged);
            // 
            // btnOptOptions
            // 
            this.btnOptOptions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOptOptions.Location = new System.Drawing.Point(12, 227);
            this.btnOptOptions.Name = "btnOptOptions";
            this.btnOptOptions.Size = new System.Drawing.Size(121, 23);
            this.btnOptOptions.TabIndex = 3;
            this.btnOptOptions.Text = "Optimize Options";
            this.btnOptOptions.UseVisualStyleBackColor = true;
            this.btnOptOptions.Click += new System.EventHandler(this.btnOptOptions_Click);
            // 
            // lbASMDebug
            // 
            this.lbASMDebug.AutoSize = true;
            this.lbASMDebug.Location = new System.Drawing.Point(16, 124);
            this.lbASMDebug.Name = "lbASMDebug";
            this.lbASMDebug.Size = new System.Drawing.Size(65, 13);
            this.lbASMDebug.TabIndex = 8;
            this.lbASMDebug.Text = "ASM Debug";
            // 
            // chbxASMDebug
            // 
            this.chbxASMDebug.AutoSize = true;
            this.chbxASMDebug.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chbxASMDebug.Location = new System.Drawing.Point(174, 120);
            this.chbxASMDebug.Name = "chbxASMDebug";
            this.chbxASMDebug.Size = new System.Drawing.Size(57, 17);
            this.chbxASMDebug.TabIndex = 9;
            this.chbxASMDebug.Text = "Enable";
            this.chbxASMDebug.UseVisualStyleBackColor = true;
            this.chbxASMDebug.CheckedChanged += new System.EventHandler(this.chbxASMDebug_CheckedChanged);
            // 
            // FlagsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnOptOptions);
            this.Controls.Add(this.gbFlags);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FlagsForm";
            this.Text = "Compiler Flags";
            this.gbFlags.ResumeLayout(false);
            this.gbFlags.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbMoreComments;
        private System.Windows.Forms.GroupBox gbFlags;
        private System.Windows.Forms.CheckBox chbxMoreComments;
        private System.Windows.Forms.CheckBox chbxDisableErrorPop;
        private System.Windows.Forms.Label lbShowErrorForm;
        private System.Windows.Forms.CheckBox chbxOptAsm;
        private System.Windows.Forms.Label lbOptAsm;
        private System.Windows.Forms.Button btnOptOptions;
        private System.Windows.Forms.CheckBox chbxASMDebug;
        private System.Windows.Forms.Label lbASMDebug;
    }
}