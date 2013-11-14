namespace JoshPiler
{
    partial class MainForm
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
            this.lbSRCfile = new System.Windows.Forms.Label();
            this.lbMASMdir = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDefaultFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setMASMDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveErrorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTokenListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testSymbolTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayErrorConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGetChar = new System.Windows.Forms.Button();
            this.btnResetChar = new System.Windows.Forms.Button();
            this.lbMASMdirpath = new System.Windows.Forms.Label();
            this.lbCurFolderDir = new System.Windows.Forms.Label();
            this.lbCurFolder = new System.Windows.Forms.Label();
            this.tabProcLst = new System.Windows.Forms.TabPage();
            this.lineNumbersForRichText4 = new LineNumbersControlForRichTextBox.LineNumbersForRichText();
            this.richProcListInc = new System.Windows.Forms.RichTextBox();
            this.tabStrInc = new System.Windows.Forms.TabPage();
            this.lineNumbersForRichText3 = new LineNumbersControlForRichTextBox.LineNumbersForRichText();
            this.richStringInc = new System.Windows.Forms.RichTextBox();
            this.tabSymTabl = new System.Windows.Forms.TabPage();
            this.richSymTable = new System.Windows.Forms.RichTextBox();
            this.tabTokens = new System.Windows.Forms.TabPage();
            this.lineNumbersForRichText2 = new LineNumbersControlForRichTextBox.LineNumbersForRichText();
            this.richToken = new System.Windows.Forms.RichTextBox();
            this.tabSRCview = new System.Windows.Forms.TabPage();
            this.lineNumbersForRichText1 = new LineNumbersControlForRichTextBox.LineNumbersForRichText();
            this.richSourceBox = new System.Windows.Forms.RichTextBox();
            this.tabsMain = new System.Windows.Forms.TabControl();
            this.lbCompileTime = new System.Windows.Forms.Label();
            this.gbxDir = new System.Windows.Forms.GroupBox();
            this.recentFile1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFile3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabProcLst.SuspendLayout();
            this.tabStrInc.SuspendLayout();
            this.tabSymTabl.SuspendLayout();
            this.tabTokens.SuspendLayout();
            this.tabSRCview.SuspendLayout();
            this.tabsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbSRCfile
            // 
            this.lbSRCfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbSRCfile.AutoSize = true;
            this.lbSRCfile.Location = new System.Drawing.Point(114, 545);
            this.lbSRCfile.Name = "lbSRCfile";
            this.lbSRCfile.Size = new System.Drawing.Size(0, 13);
            this.lbSRCfile.TabIndex = 4;
            // 
            // lbMASMdir
            // 
            this.lbMASMdir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMASMdir.AutoSize = true;
            this.lbMASMdir.Location = new System.Drawing.Point(16, 549);
            this.lbMASMdir.Name = "lbMASMdir";
            this.lbMASMdir.Size = new System.Drawing.Size(90, 13);
            this.lbMASMdir.TabIndex = 6;
            this.lbMASMdir.Text = "MASM Directory: ";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.actionsToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openModFileToolStripMenuItem,
            this.openRecentToolStripMenuItem,
            this.setDefaultFolderToolStripMenuItem,
            this.setMASMDirectoryToolStripMenuItem,
            this.saveErrorLogToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openModFileToolStripMenuItem
            // 
            this.openModFileToolStripMenuItem.Name = "openModFileToolStripMenuItem";
            this.openModFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openModFileToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.openModFileToolStripMenuItem.Text = "Open Mod File";
            this.openModFileToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.openModFileToolStripMenuItem.Click += new System.EventHandler(this.openModFileToolStripMenuItem_Click);
            // 
            // openRecentToolStripMenuItem
            // 
            this.openRecentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recentFile1ToolStripMenuItem,
            this.recentFile2ToolStripMenuItem,
            this.recentFile3ToolStripMenuItem,
            this.clearRecentToolStripMenuItem});
            this.openRecentToolStripMenuItem.Name = "openRecentToolStripMenuItem";
            this.openRecentToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.openRecentToolStripMenuItem.Text = "Open Recent";
            // 
            // clearRecentToolStripMenuItem
            // 
            this.clearRecentToolStripMenuItem.Name = "clearRecentToolStripMenuItem";
            this.clearRecentToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearRecentToolStripMenuItem.Text = "Clear Recent";
            this.clearRecentToolStripMenuItem.Click += new System.EventHandler(this.clearRecentToolStripMenuItem_Click);
            // 
            // setDefaultFolderToolStripMenuItem
            // 
            this.setDefaultFolderToolStripMenuItem.Name = "setDefaultFolderToolStripMenuItem";
            this.setDefaultFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.M)));
            this.setDefaultFolderToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.setDefaultFolderToolStripMenuItem.Text = "Set Default Folder";
            this.setDefaultFolderToolStripMenuItem.Click += new System.EventHandler(this.setDefaultFolderToolStripMenuItem_Click);
            // 
            // setMASMDirectoryToolStripMenuItem
            // 
            this.setMASMDirectoryToolStripMenuItem.Name = "setMASMDirectoryToolStripMenuItem";
            this.setMASMDirectoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.setMASMDirectoryToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.setMASMDirectoryToolStripMenuItem.Text = "Set MASM Directory";
            this.setMASMDirectoryToolStripMenuItem.Click += new System.EventHandler(this.setMASMDirectoryToolStripMenuItem_Click);
            // 
            // saveErrorLogToolStripMenuItem
            // 
            this.saveErrorLogToolStripMenuItem.Name = "saveErrorLogToolStripMenuItem";
            this.saveErrorLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveErrorLogToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.saveErrorLogToolStripMenuItem.Text = "Save Error Log";
            this.saveErrorLogToolStripMenuItem.Click += new System.EventHandler(this.saveErrorLogToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileFileToolStripMenuItem,
            this.showTokenListToolStripMenuItem,
            this.testSymbolTableToolStripMenuItem,
            this.displayErrorConsoleToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.actionsToolStripMenuItem.Text = "Actions";
            // 
            // compileFileToolStripMenuItem
            // 
            this.compileFileToolStripMenuItem.Name = "compileFileToolStripMenuItem";
            this.compileFileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.compileFileToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.compileFileToolStripMenuItem.Text = "Compile File";
            this.compileFileToolStripMenuItem.Click += new System.EventHandler(this.compileFileToolStripMenuItem_Click);
            // 
            // showTokenListToolStripMenuItem
            // 
            this.showTokenListToolStripMenuItem.Name = "showTokenListToolStripMenuItem";
            this.showTokenListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.showTokenListToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.showTokenListToolStripMenuItem.Text = "Show Token List";
            this.showTokenListToolStripMenuItem.Click += new System.EventHandler(this.showTokenListToolStripMenuItem_Click);
            // 
            // testSymbolTableToolStripMenuItem
            // 
            this.testSymbolTableToolStripMenuItem.Name = "testSymbolTableToolStripMenuItem";
            this.testSymbolTableToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.testSymbolTableToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.testSymbolTableToolStripMenuItem.Text = "Test Symbol Table";
            this.testSymbolTableToolStripMenuItem.Click += new System.EventHandler(this.testSymbolTableToolStripMenuItem_Click);
            // 
            // displayErrorConsoleToolStripMenuItem
            // 
            this.displayErrorConsoleToolStripMenuItem.Name = "displayErrorConsoleToolStripMenuItem";
            this.displayErrorConsoleToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.displayErrorConsoleToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.displayErrorConsoleToolStripMenuItem.Text = "Display Error Console";
            this.displayErrorConsoleToolStripMenuItem.Click += new System.EventHandler(this.displayErrorConsoleToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileFlagsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // compileFlagsToolStripMenuItem
            // 
            this.compileFlagsToolStripMenuItem.Name = "compileFlagsToolStripMenuItem";
            this.compileFlagsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.compileFlagsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.compileFlagsToolStripMenuItem.Text = "Compile Flags";
            this.compileFlagsToolStripMenuItem.Click += new System.EventHandler(this.compileFlagsToolStripMenuItem_Click);
            // 
            // btnGetChar
            // 
            this.btnGetChar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetChar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGetChar.Location = new System.Drawing.Point(634, 568);
            this.btnGetChar.Name = "btnGetChar";
            this.btnGetChar.Size = new System.Drawing.Size(74, 21);
            this.btnGetChar.TabIndex = 10;
            this.btnGetChar.Text = "Get Char";
            this.btnGetChar.UseVisualStyleBackColor = true;
            this.btnGetChar.Click += new System.EventHandler(this.btnGetChar_Click);
            // 
            // btnResetChar
            // 
            this.btnResetChar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetChar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResetChar.Location = new System.Drawing.Point(714, 567);
            this.btnResetChar.Name = "btnResetChar";
            this.btnResetChar.Size = new System.Drawing.Size(74, 21);
            this.btnResetChar.TabIndex = 11;
            this.btnResetChar.Text = "Reset Char";
            this.btnResetChar.UseVisualStyleBackColor = true;
            this.btnResetChar.Click += new System.EventHandler(this.btnResetChar_Click);
            // 
            // lbMASMdirpath
            // 
            this.lbMASMdirpath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMASMdirpath.AutoSize = true;
            this.lbMASMdirpath.Location = new System.Drawing.Point(114, 549);
            this.lbMASMdirpath.Name = "lbMASMdirpath";
            this.lbMASMdirpath.Size = new System.Drawing.Size(0, 13);
            this.lbMASMdirpath.TabIndex = 13;
            // 
            // lbCurFolderDir
            // 
            this.lbCurFolderDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbCurFolderDir.AutoSize = true;
            this.lbCurFolderDir.Location = new System.Drawing.Point(114, 568);
            this.lbCurFolderDir.Name = "lbCurFolderDir";
            this.lbCurFolderDir.Size = new System.Drawing.Size(0, 13);
            this.lbCurFolderDir.TabIndex = 15;
            // 
            // lbCurFolder
            // 
            this.lbCurFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbCurFolder.AutoSize = true;
            this.lbCurFolder.Location = new System.Drawing.Point(16, 568);
            this.lbCurFolder.Name = "lbCurFolder";
            this.lbCurFolder.Size = new System.Drawing.Size(76, 13);
            this.lbCurFolder.TabIndex = 14;
            this.lbCurFolder.Text = "Current Folder:";
            // 
            // tabProcLst
            // 
            this.tabProcLst.Controls.Add(this.lineNumbersForRichText4);
            this.tabProcLst.Controls.Add(this.richProcListInc);
            this.tabProcLst.Location = new System.Drawing.Point(4, 22);
            this.tabProcLst.Name = "tabProcLst";
            this.tabProcLst.Size = new System.Drawing.Size(768, 476);
            this.tabProcLst.TabIndex = 4;
            this.tabProcLst.Text = "proclist.inc";
            this.tabProcLst.UseVisualStyleBackColor = true;
            // 
            // lineNumbersForRichText4
            // 
            this.lineNumbersForRichText4.AutoSizing = true;
            this.lineNumbersForRichText4.BackgroundGradientAlphaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText4.BackgroundGradientBetaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText4.BackgroundGradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lineNumbersForRichText4.BorderLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText4.BorderLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText4.BorderLinesThickness = 1F;
            this.lineNumbersForRichText4.DockSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Left;
            this.lineNumbersForRichText4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lineNumbersForRichText4.GridLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText4.GridLinesStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lineNumbersForRichText4.GridLinesThickness = 1F;
            this.lineNumbersForRichText4.LineNumbersAlignment = System.Drawing.ContentAlignment.TopRight;
            this.lineNumbersForRichText4.LineNumbersAntiAlias = true;
            this.lineNumbersForRichText4.LineNumbersAsHexadecimal = false;
            this.lineNumbersForRichText4.LineNumbersClippedByItemRectangle = true;
            this.lineNumbersForRichText4.LineNumbersLeadingZeroes = true;
            this.lineNumbersForRichText4.LineNumbersOffset = new System.Drawing.Size(0, 0);
            this.lineNumbersForRichText4.Location = new System.Drawing.Point(23, 3);
            this.lineNumbersForRichText4.Margin = new System.Windows.Forms.Padding(0);
            this.lineNumbersForRichText4.MarginLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText4.MarginLinesSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Right;
            this.lineNumbersForRichText4.MarginLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText4.MarginLinesThickness = 1F;
            this.lineNumbersForRichText4.Name = "lineNumbersForRichText4";
            this.lineNumbersForRichText4.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lineNumbersForRichText4.ParentRichTextBox = this.richProcListInc;
            this.lineNumbersForRichText4.SeeThroughMode = false;
            this.lineNumbersForRichText4.ShowBackgroundGradient = false;
            this.lineNumbersForRichText4.ShowBorderLines = false;
            this.lineNumbersForRichText4.ShowGridLines = false;
            this.lineNumbersForRichText4.ShowLineNumbers = true;
            this.lineNumbersForRichText4.ShowMarginLines = true;
            this.lineNumbersForRichText4.Size = new System.Drawing.Size(20, 470);
            this.lineNumbersForRichText4.TabIndex = 4;
            // 
            // richProcListInc
            // 
            this.richProcListInc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richProcListInc.BackColor = System.Drawing.Color.White;
            this.richProcListInc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richProcListInc.Location = new System.Drawing.Point(44, 3);
            this.richProcListInc.Name = "richProcListInc";
            this.richProcListInc.ReadOnly = true;
            this.richProcListInc.Size = new System.Drawing.Size(721, 470);
            this.richProcListInc.TabIndex = 3;
            this.richProcListInc.Text = "";
            // 
            // tabStrInc
            // 
            this.tabStrInc.Controls.Add(this.lineNumbersForRichText3);
            this.tabStrInc.Controls.Add(this.richStringInc);
            this.tabStrInc.Location = new System.Drawing.Point(4, 22);
            this.tabStrInc.Name = "tabStrInc";
            this.tabStrInc.Size = new System.Drawing.Size(768, 476);
            this.tabStrInc.TabIndex = 3;
            this.tabStrInc.Text = "string.inc";
            this.tabStrInc.UseVisualStyleBackColor = true;
            // 
            // lineNumbersForRichText3
            // 
            this.lineNumbersForRichText3.AutoSizing = true;
            this.lineNumbersForRichText3.BackgroundGradientAlphaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText3.BackgroundGradientBetaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText3.BackgroundGradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lineNumbersForRichText3.BorderLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText3.BorderLinesStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lineNumbersForRichText3.BorderLinesThickness = 1F;
            this.lineNumbersForRichText3.DockSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Left;
            this.lineNumbersForRichText3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lineNumbersForRichText3.GridLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText3.GridLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText3.GridLinesThickness = 1F;
            this.lineNumbersForRichText3.LineNumbersAlignment = System.Drawing.ContentAlignment.TopRight;
            this.lineNumbersForRichText3.LineNumbersAntiAlias = true;
            this.lineNumbersForRichText3.LineNumbersAsHexadecimal = false;
            this.lineNumbersForRichText3.LineNumbersClippedByItemRectangle = true;
            this.lineNumbersForRichText3.LineNumbersLeadingZeroes = true;
            this.lineNumbersForRichText3.LineNumbersOffset = new System.Drawing.Size(0, 0);
            this.lineNumbersForRichText3.Location = new System.Drawing.Point(14, 3);
            this.lineNumbersForRichText3.Margin = new System.Windows.Forms.Padding(0);
            this.lineNumbersForRichText3.MarginLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText3.MarginLinesSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Right;
            this.lineNumbersForRichText3.MarginLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText3.MarginLinesThickness = 1F;
            this.lineNumbersForRichText3.Name = "lineNumbersForRichText3";
            this.lineNumbersForRichText3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lineNumbersForRichText3.ParentRichTextBox = this.richStringInc;
            this.lineNumbersForRichText3.SeeThroughMode = false;
            this.lineNumbersForRichText3.ShowBackgroundGradient = false;
            this.lineNumbersForRichText3.ShowBorderLines = false;
            this.lineNumbersForRichText3.ShowGridLines = false;
            this.lineNumbersForRichText3.ShowLineNumbers = true;
            this.lineNumbersForRichText3.ShowMarginLines = true;
            this.lineNumbersForRichText3.Size = new System.Drawing.Size(20, 470);
            this.lineNumbersForRichText3.TabIndex = 4;
            // 
            // richStringInc
            // 
            this.richStringInc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richStringInc.BackColor = System.Drawing.Color.White;
            this.richStringInc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richStringInc.Location = new System.Drawing.Point(35, 3);
            this.richStringInc.Name = "richStringInc";
            this.richStringInc.ReadOnly = true;
            this.richStringInc.Size = new System.Drawing.Size(730, 470);
            this.richStringInc.TabIndex = 3;
            this.richStringInc.Text = "";
            // 
            // tabSymTabl
            // 
            this.tabSymTabl.Controls.Add(this.richSymTable);
            this.tabSymTabl.Location = new System.Drawing.Point(4, 22);
            this.tabSymTabl.Name = "tabSymTabl";
            this.tabSymTabl.Size = new System.Drawing.Size(768, 476);
            this.tabSymTabl.TabIndex = 2;
            this.tabSymTabl.Text = "Symbol Table";
            this.tabSymTabl.UseVisualStyleBackColor = true;
            // 
            // richSymTable
            // 
            this.richSymTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richSymTable.BackColor = System.Drawing.Color.White;
            this.richSymTable.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richSymTable.Location = new System.Drawing.Point(3, 3);
            this.richSymTable.Name = "richSymTable";
            this.richSymTable.ReadOnly = true;
            this.richSymTable.Size = new System.Drawing.Size(761, 470);
            this.richSymTable.TabIndex = 4;
            this.richSymTable.Text = "";
            // 
            // tabTokens
            // 
            this.tabTokens.Controls.Add(this.lineNumbersForRichText2);
            this.tabTokens.Controls.Add(this.richToken);
            this.tabTokens.Location = new System.Drawing.Point(4, 22);
            this.tabTokens.Name = "tabTokens";
            this.tabTokens.Padding = new System.Windows.Forms.Padding(3);
            this.tabTokens.Size = new System.Drawing.Size(768, 476);
            this.tabTokens.TabIndex = 1;
            this.tabTokens.Text = "Token List";
            this.tabTokens.UseVisualStyleBackColor = true;
            // 
            // lineNumbersForRichText2
            // 
            this.lineNumbersForRichText2.AutoSizing = true;
            this.lineNumbersForRichText2.BackgroundGradientAlphaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText2.BackgroundGradientBetaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText2.BackgroundGradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lineNumbersForRichText2.BorderLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText2.BorderLinesStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lineNumbersForRichText2.BorderLinesThickness = 1F;
            this.lineNumbersForRichText2.DockSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Left;
            this.lineNumbersForRichText2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lineNumbersForRichText2.GridLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText2.GridLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText2.GridLinesThickness = 1F;
            this.lineNumbersForRichText2.LineNumbersAlignment = System.Drawing.ContentAlignment.TopRight;
            this.lineNumbersForRichText2.LineNumbersAntiAlias = true;
            this.lineNumbersForRichText2.LineNumbersAsHexadecimal = false;
            this.lineNumbersForRichText2.LineNumbersClippedByItemRectangle = true;
            this.lineNumbersForRichText2.LineNumbersLeadingZeroes = true;
            this.lineNumbersForRichText2.LineNumbersOffset = new System.Drawing.Size(0, 0);
            this.lineNumbersForRichText2.Location = new System.Drawing.Point(13, 3);
            this.lineNumbersForRichText2.Margin = new System.Windows.Forms.Padding(0);
            this.lineNumbersForRichText2.MarginLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText2.MarginLinesSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Right;
            this.lineNumbersForRichText2.MarginLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText2.MarginLinesThickness = 1F;
            this.lineNumbersForRichText2.Name = "lineNumbersForRichText2";
            this.lineNumbersForRichText2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lineNumbersForRichText2.ParentRichTextBox = this.richToken;
            this.lineNumbersForRichText2.SeeThroughMode = false;
            this.lineNumbersForRichText2.ShowBackgroundGradient = false;
            this.lineNumbersForRichText2.ShowBorderLines = false;
            this.lineNumbersForRichText2.ShowGridLines = false;
            this.lineNumbersForRichText2.ShowLineNumbers = true;
            this.lineNumbersForRichText2.ShowMarginLines = true;
            this.lineNumbersForRichText2.Size = new System.Drawing.Size(20, 470);
            this.lineNumbersForRichText2.TabIndex = 4;
            // 
            // richToken
            // 
            this.richToken.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richToken.BackColor = System.Drawing.Color.White;
            this.richToken.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richToken.Location = new System.Drawing.Point(34, 3);
            this.richToken.Name = "richToken";
            this.richToken.ReadOnly = true;
            this.richToken.Size = new System.Drawing.Size(730, 470);
            this.richToken.TabIndex = 3;
            this.richToken.Text = "";
            // 
            // tabSRCview
            // 
            this.tabSRCview.Controls.Add(this.lineNumbersForRichText1);
            this.tabSRCview.Controls.Add(this.richSourceBox);
            this.tabSRCview.Location = new System.Drawing.Point(4, 22);
            this.tabSRCview.Name = "tabSRCview";
            this.tabSRCview.Padding = new System.Windows.Forms.Padding(3);
            this.tabSRCview.Size = new System.Drawing.Size(768, 476);
            this.tabSRCview.TabIndex = 0;
            this.tabSRCview.Text = "Source";
            this.tabSRCview.UseVisualStyleBackColor = true;
            // 
            // lineNumbersForRichText1
            // 
            this.lineNumbersForRichText1.AutoSizing = true;
            this.lineNumbersForRichText1.BackgroundGradientAlphaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText1.BackgroundGradientBetaColor = System.Drawing.Color.Transparent;
            this.lineNumbersForRichText1.BackgroundGradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lineNumbersForRichText1.BorderLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText1.BorderLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText1.BorderLinesThickness = 1F;
            this.lineNumbersForRichText1.DockSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Left;
            this.lineNumbersForRichText1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lineNumbersForRichText1.GridLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText1.GridLinesStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lineNumbersForRichText1.GridLinesThickness = 1F;
            this.lineNumbersForRichText1.LineNumbersAlignment = System.Drawing.ContentAlignment.TopRight;
            this.lineNumbersForRichText1.LineNumbersAntiAlias = true;
            this.lineNumbersForRichText1.LineNumbersAsHexadecimal = false;
            this.lineNumbersForRichText1.LineNumbersClippedByItemRectangle = true;
            this.lineNumbersForRichText1.LineNumbersLeadingZeroes = true;
            this.lineNumbersForRichText1.LineNumbersOffset = new System.Drawing.Size(0, 0);
            this.lineNumbersForRichText1.Location = new System.Drawing.Point(13, 3);
            this.lineNumbersForRichText1.Margin = new System.Windows.Forms.Padding(0);
            this.lineNumbersForRichText1.MarginLinesColor = System.Drawing.Color.Black;
            this.lineNumbersForRichText1.MarginLinesSide = LineNumbersControlForRichTextBox.LineNumbersForRichText.LineNumberDockSide.Right;
            this.lineNumbersForRichText1.MarginLinesStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lineNumbersForRichText1.MarginLinesThickness = 1F;
            this.lineNumbersForRichText1.Name = "lineNumbersForRichText1";
            this.lineNumbersForRichText1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lineNumbersForRichText1.ParentRichTextBox = this.richSourceBox;
            this.lineNumbersForRichText1.SeeThroughMode = false;
            this.lineNumbersForRichText1.ShowBackgroundGradient = false;
            this.lineNumbersForRichText1.ShowBorderLines = false;
            this.lineNumbersForRichText1.ShowGridLines = false;
            this.lineNumbersForRichText1.ShowLineNumbers = true;
            this.lineNumbersForRichText1.ShowMarginLines = true;
            this.lineNumbersForRichText1.Size = new System.Drawing.Size(20, 470);
            this.lineNumbersForRichText1.TabIndex = 2;
            // 
            // richSourceBox
            // 
            this.richSourceBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richSourceBox.BackColor = System.Drawing.Color.White;
            this.richSourceBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richSourceBox.Location = new System.Drawing.Point(34, 3);
            this.richSourceBox.Name = "richSourceBox";
            this.richSourceBox.ReadOnly = true;
            this.richSourceBox.Size = new System.Drawing.Size(730, 470);
            this.richSourceBox.TabIndex = 1;
            this.richSourceBox.Text = "";
            // 
            // tabsMain
            // 
            this.tabsMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsMain.Controls.Add(this.tabSRCview);
            this.tabsMain.Controls.Add(this.tabTokens);
            this.tabsMain.Controls.Add(this.tabSymTabl);
            this.tabsMain.Controls.Add(this.tabStrInc);
            this.tabsMain.Controls.Add(this.tabProcLst);
            this.tabsMain.Location = new System.Drawing.Point(12, 35);
            this.tabsMain.Name = "tabsMain";
            this.tabsMain.SelectedIndex = 0;
            this.tabsMain.Size = new System.Drawing.Size(776, 502);
            this.tabsMain.TabIndex = 0;
            // 
            // lbCompileTime
            // 
            this.lbCompileTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCompileTime.AutoSize = true;
            this.lbCompileTime.Location = new System.Drawing.Point(491, 545);
            this.lbCompileTime.Name = "lbCompileTime";
            this.lbCompileTime.Size = new System.Drawing.Size(76, 13);
            this.lbCompileTime.TabIndex = 16;
            this.lbCompileTime.Text = "Compile Time: ";
            // 
            // gbxDir
            // 
            this.gbxDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbxDir.Location = new System.Drawing.Point(12, 536);
            this.gbxDir.Name = "gbxDir";
            this.gbxDir.Size = new System.Drawing.Size(457, 53);
            this.gbxDir.TabIndex = 17;
            this.gbxDir.TabStop = false;
            // 
            // recentFile1ToolStripMenuItem
            // 
            this.recentFile1ToolStripMenuItem.Name = "recentFile1ToolStripMenuItem";
            this.recentFile1ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recentFile1ToolStripMenuItem.Text = "Recent_1";
            this.recentFile1ToolStripMenuItem.Visible = false;
            this.recentFile1ToolStripMenuItem.Click += new System.EventHandler(this.recentFile1ToolStripMenuItem_Click);
            // 
            // recentFile2ToolStripMenuItem
            // 
            this.recentFile2ToolStripMenuItem.Name = "recentFile2ToolStripMenuItem";
            this.recentFile2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recentFile2ToolStripMenuItem.Text = "Recent_2";
            this.recentFile2ToolStripMenuItem.Visible = false;
            this.recentFile2ToolStripMenuItem.Click += new System.EventHandler(this.recentFile2ToolStripMenuItem_Click);
            // 
            // recentFile3ToolStripMenuItem
            // 
            this.recentFile3ToolStripMenuItem.Name = "recentFile3ToolStripMenuItem";
            this.recentFile3ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recentFile3ToolStripMenuItem.Text = "Recent_3";
            this.recentFile3ToolStripMenuItem.Visible = false;
            this.recentFile3ToolStripMenuItem.Click += new System.EventHandler(this.recentFile3ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.lbCompileTime);
            this.Controls.Add(this.lbCurFolderDir);
            this.Controls.Add(this.lbCurFolder);
            this.Controls.Add(this.lbMASMdirpath);
            this.Controls.Add(this.btnResetChar);
            this.Controls.Add(this.btnGetChar);
            this.Controls.Add(this.lbMASMdir);
            this.Controls.Add(this.lbSRCfile);
            this.Controls.Add(this.tabsMain);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.gbxDir);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "JoshPiler";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabProcLst.ResumeLayout(false);
            this.tabStrInc.ResumeLayout(false);
            this.tabSymTabl.ResumeLayout(false);
            this.tabTokens.ResumeLayout(false);
            this.tabSRCview.ResumeLayout(false);
            this.tabsMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSRCfile;
        private System.Windows.Forms.Label lbMASMdir;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openModFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Button btnGetChar;
        private System.Windows.Forms.Button btnResetChar;
        private System.Windows.Forms.ToolStripMenuItem setMASMDirectoryToolStripMenuItem;
        private System.Windows.Forms.Label lbMASMdirpath;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTokenListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testSymbolTableToolStripMenuItem;
        private System.Windows.Forms.Label lbCurFolderDir;
        private System.Windows.Forms.Label lbCurFolder;
        private System.Windows.Forms.ToolStripMenuItem setDefaultFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveErrorLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayErrorConsoleToolStripMenuItem;
        private System.Windows.Forms.TabPage tabProcLst;
        private System.Windows.Forms.TabPage tabStrInc;
        private System.Windows.Forms.TabPage tabSymTabl;
        private System.Windows.Forms.TabPage tabTokens;
        private System.Windows.Forms.TabPage tabSRCview;
        private System.Windows.Forms.TabControl tabsMain;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileFlagsToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richSourceBox;
        private LineNumbersControlForRichTextBox.LineNumbersForRichText lineNumbersForRichText1;
        private LineNumbersControlForRichTextBox.LineNumbersForRichText lineNumbersForRichText4;
        private System.Windows.Forms.RichTextBox richProcListInc;
        private LineNumbersControlForRichTextBox.LineNumbersForRichText lineNumbersForRichText3;
        private System.Windows.Forms.RichTextBox richStringInc;
        private LineNumbersControlForRichTextBox.LineNumbersForRichText lineNumbersForRichText2;
        private System.Windows.Forms.RichTextBox richToken;
        private System.Windows.Forms.RichTextBox richSymTable;
        private System.Windows.Forms.Label lbCompileTime;
        private System.Windows.Forms.GroupBox gbxDir;
        private System.Windows.Forms.ToolStripMenuItem openRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentFile1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentFile2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentFile3ToolStripMenuItem;
    }
}

