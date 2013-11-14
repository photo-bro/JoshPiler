namespace JoshPiler.Forms
{
    partial class OptimizerForm
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
            this.gbxOptimize = new System.Windows.Forms.GroupBox();
            this.chbxRedunMov = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbxDupPushPop = new System.Windows.Forms.CheckBox();
            this.lbOptAsm = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbxOptimize.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxOptimize
            // 
            this.gbxOptimize.Controls.Add(this.chbxRedunMov);
            this.gbxOptimize.Controls.Add(this.label1);
            this.gbxOptimize.Controls.Add(this.chbxDupPushPop);
            this.gbxOptimize.Controls.Add(this.lbOptAsm);
            this.gbxOptimize.Location = new System.Drawing.Point(12, 12);
            this.gbxOptimize.Name = "gbxOptimize";
            this.gbxOptimize.Size = new System.Drawing.Size(260, 196);
            this.gbxOptimize.TabIndex = 0;
            this.gbxOptimize.TabStop = false;
            this.gbxOptimize.Text = "Optimize Assembly Options";
            // 
            // chbxRedunMov
            // 
            this.chbxRedunMov.AutoSize = true;
            this.chbxRedunMov.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chbxRedunMov.Location = new System.Drawing.Point(9, 95);
            this.chbxRedunMov.Name = "chbxRedunMov";
            this.chbxRedunMov.Size = new System.Drawing.Size(57, 17);
            this.chbxRedunMov.TabIndex = 11;
            this.chbxRedunMov.Text = "Enable";
            this.chbxRedunMov.UseVisualStyleBackColor = true;
            this.chbxRedunMov.CheckedChanged += new System.EventHandler(this.chbxRedunMov_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Optimize Redundant Mov and Push Instructions";
            // 
            // chbxDupPushPop
            // 
            this.chbxDupPushPop.AutoSize = true;
            this.chbxDupPushPop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chbxDupPushPop.Location = new System.Drawing.Point(9, 45);
            this.chbxDupPushPop.Name = "chbxDupPushPop";
            this.chbxDupPushPop.Size = new System.Drawing.Size(57, 17);
            this.chbxDupPushPop.TabIndex = 9;
            this.chbxDupPushPop.Text = "Enable";
            this.chbxDupPushPop.UseVisualStyleBackColor = true;
            this.chbxDupPushPop.CheckedChanged += new System.EventHandler(this.chbxDupPushPop_CheckedChanged);
            // 
            // lbOptAsm
            // 
            this.lbOptAsm.AutoSize = true;
            this.lbOptAsm.Location = new System.Drawing.Point(6, 29);
            this.lbOptAsm.Name = "lbOptAsm";
            this.lbOptAsm.Size = new System.Drawing.Size(209, 13);
            this.lbOptAsm.TabIndex = 8;
            this.lbOptAsm.Text = "Remove Duplicate Push / Pop Instructions";
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Location = new System.Drawing.Point(197, 227);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // OptimizerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbxOptimize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptimizerForm";
            this.Text = "Optimize Assembly";
            this.gbxOptimize.ResumeLayout(false);
            this.gbxOptimize.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxOptimize;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chbxRedunMov;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbxDupPushPop;
        private System.Windows.Forms.Label lbOptAsm;

    }
}