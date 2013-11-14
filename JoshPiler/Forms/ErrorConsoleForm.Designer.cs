namespace JoshPiler
{
    partial class ErrorConsoleForm
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
            this.tbErrorConsole = new System.Windows.Forms.TextBox();
            this.btnSaveErrCons = new System.Windows.Forms.Button();
            this.btnErrClose = new System.Windows.Forms.Button();
            this.btnErrRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbErrorConsole
            // 
            this.tbErrorConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbErrorConsole.BackColor = System.Drawing.Color.White;
            this.tbErrorConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbErrorConsole.Location = new System.Drawing.Point(12, 61);
            this.tbErrorConsole.Multiline = true;
            this.tbErrorConsole.Name = "tbErrorConsole";
            this.tbErrorConsole.ReadOnly = true;
            this.tbErrorConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbErrorConsole.Size = new System.Drawing.Size(600, 373);
            this.tbErrorConsole.TabIndex = 0;
            // 
            // btnSaveErrCons
            // 
            this.btnSaveErrCons.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveErrCons.Location = new System.Drawing.Point(12, 21);
            this.btnSaveErrCons.Name = "btnSaveErrCons";
            this.btnSaveErrCons.Size = new System.Drawing.Size(75, 23);
            this.btnSaveErrCons.TabIndex = 1;
            this.btnSaveErrCons.Text = "Save";
            this.btnSaveErrCons.UseVisualStyleBackColor = true;
            this.btnSaveErrCons.Click += new System.EventHandler(this.btnSaveErrCons_Click);
            // 
            // btnErrClose
            // 
            this.btnErrClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnErrClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnErrClose.Location = new System.Drawing.Point(537, 21);
            this.btnErrClose.Name = "btnErrClose";
            this.btnErrClose.Size = new System.Drawing.Size(75, 23);
            this.btnErrClose.TabIndex = 2;
            this.btnErrClose.Text = "Close";
            this.btnErrClose.UseVisualStyleBackColor = true;
            this.btnErrClose.Click += new System.EventHandler(this.btnErrClose_Click);
            // 
            // btnErrRefresh
            // 
            this.btnErrRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnErrRefresh.Location = new System.Drawing.Point(93, 21);
            this.btnErrRefresh.Name = "btnErrRefresh";
            this.btnErrRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnErrRefresh.TabIndex = 3;
            this.btnErrRefresh.Text = "Refresh";
            this.btnErrRefresh.UseVisualStyleBackColor = true;
            this.btnErrRefresh.Click += new System.EventHandler(this.btnErrRefresh_Click);
            // 
            // ErrorConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.btnErrRefresh);
            this.Controls.Add(this.btnErrClose);
            this.Controls.Add(this.btnSaveErrCons);
            this.Controls.Add(this.tbErrorConsole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "ErrorConsoleForm";
            this.Text = "ErrorConsole";
            this.Load += new System.EventHandler(this.ErrorConsoleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbErrorConsole;
        private System.Windows.Forms.Button btnSaveErrCons;
        private System.Windows.Forms.Button btnErrClose;
        private System.Windows.Forms.Button btnErrRefresh;
    }
}