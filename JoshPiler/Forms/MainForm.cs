using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO; // Directory
using System.Diagnostics; // stopwatch

namespace JoshPiler
{
    public partial class MainForm : Form
    {
        // OpenFileDialogue class object
        OpenFileDialog m_ofdBox;

        // Singleton instances
        private FileManager m_FM = FileManager.Instance;
        private Facade m_Facade = Facade.Instance;

        // recent file list
        List<string> m_lsRecentFiles = new List<string>();
        
        /// <summary>
        /// Form constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //Debugging();

            // Prep m_ofdBox and set filter
            m_ofdBox = new OpenFileDialog();
            m_ofdBox.Filter = "Modula-2 files (*.mod)|*.mod";
            m_ofdBox.Title = "Open Modula-2 source file";

            // check if MASM dir exists, ask user to set directory
            if (!Directory.Exists(m_FM.MASMdir))
                setMASMDirectoryToolStripMenuItem.PerformClick();

            // check if default file exists
            if (!File.Exists(m_FM.LastFilePath + "\\" + m_FM.LastFileName + ".mod"))
                openModFileToolStripMenuItem.PerformClick();
            else
                OpenDefaultFile(); // open default file

            UpdateGUI();
        } // MainForm

        /// <summary>
        /// Get Char button
        /// Displays messagebox with a char from the active file. Each click increments
        /// the position in the file the char is retrieved from.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetChar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                string.Format("The next char is: {0}\r\nLine number is: {1}", m_Facade.getChar(), GetChar.Instance.LineCount));
        } // btnGetChar_Click

        /// <summary>
        /// Open Modula-2 File menu item
        /// Diplays an OpenFileDialogue box for user to select a file to open.
        /// File is then opened and loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openModFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // For file open prompt
            if (m_ofdBox.ShowDialog() == DialogResult.OK)
            {
                // Close file if open
                if (m_FM.isOpen())
                    m_FM.CloseFile();

                // Load file
                m_FM.OpenFile(m_ofdBox.FileName);

                // Update last file opened
                m_FM.LastFilePath = Path.GetDirectoryName(m_ofdBox.FileName);
                m_FM.LastFileName = Path.GetFileName(m_ofdBox.FileName);

                // Create clean directory
                m_FM.CreateCleanDirectory(m_FM.LastFilePath + "\\" + m_FM.getFolderName());

                // add to recent files
                m_lsRecentFiles.Insert(0, m_ofdBox.FileName);

                UpdateGUI();

                // Reset GetChar pointer
                m_Facade.resetChar();
            } // if m_ofdBox.ShowDialog()
            // else do nothing
        } // openModFile

        /// <summary>
        /// Reset Button
        /// Reset's position of the GetChar function to beginning of the active file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetChar_Click(object sender, EventArgs e)
        { m_Facade.resetChar(); }

        /// <summary>
        /// Quit menu item
        /// Exits the program 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close file if open
            if (m_FM.isOpen()) m_FM.CloseFile();

            // Quit program
            Application.Exit();
        } // quit

        /// <summary>
        /// Set MASM Directory menu item
        /// Displays a FolderBrowserDialog to select MASM directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setMASMDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdTemp = new FolderBrowserDialog();
            fbdTemp.Description = "Select MASM directory";

            if (fbdTemp.ShowDialog() == DialogResult.OK)
            {

                string sTemp = fbdTemp.SelectedPath;
                FileManager.Instance.MASMdir = sTemp;
                lbMASMdirpath.Text = sTemp;
            } // if OK
        } // setMasmDirectory

        /// <summary>
        /// About menu item
        /// Displays program information and author
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(
            //    string.Format("{0} is a Modula-2 compiler written by Josh Harmon for the\r\nPrincipia CSCI 380 Compilers course taught by John Broere",
            //    FileManager.Instance.CompilerName),
            //    FileManager.Instance.CompilerName);
            AboutBox abx = new AboutBox();
            abx.Show();
        } // about

        /// <summary>
        /// Loads default files for easier debugging
        /// </summary>
        private void OpenDefaultFile()
        {
            // Idea by John Broere

            // Load file
            m_FM.OpenFile(m_FM.LastFilePath + "\\" + m_FM.LastFileName + ".mod");

            // Create clean directory
            m_FM.CreateCleanDirectory(m_FM.LastFilePath + "\\" + m_FM.getFolderName());

            m_lsRecentFiles.Insert(0, m_FM.LastFilePath + "\\" + m_FM.LastFileName + ".mod");      // add to recent files

            UpdateGUI();
        } // Debugging

        /// <summary>
        /// Update all textboxes, tabs, and other form objects. Synonymous with "refreshing"
        /// the form.
        /// </summary>
        private void UpdateGUI()
        {
            // Idea by John Broere

            // Form Title
            this.Text = string.Format("{0} - {1}", m_FM.CompilerName, m_FM.LastFileName);

            // Source console code
            richSourceBox.Text = m_FM.ToString();// Display file in source tab

            // Token console code
            richToken.Text = m_Facade.GetTokenList();

            // Symbol console code
            richSymTable.Text = m_Facade.GetSymbolTable();

            // inc tabs
            richStringInc.Text = m_Facade.GetStringInc();
            richProcListInc.Text = m_Facade.GetProclistInc();

            // MASM directory
            lbMASMdirpath.Text = m_FM.MASMdir;

            // Current Folder directory
            lbCurFolderDir.Text = m_FM.LastFilePath + "\\" + m_FM.getFolderName();

            // Recent opened files
            if (m_lsRecentFiles.Count > 0)
            {
                int i = 0;      // recent file counter
                foreach (string file in m_lsRecentFiles)
                {
					//if (i > 2) i = 0; // write over if more then 3 recent files
                    // set recent item text and visibility
                    switch (i)
                    {
                        case 0:
                            recentFile1ToolStripMenuItem.Text = file;
                            recentFile1ToolStripMenuItem.Visible = true;
                            break;
                        case 1:
                            recentFile2ToolStripMenuItem.Text = file;
                            recentFile2ToolStripMenuItem.Visible = true;
                            break;
                        case 2:
                            recentFile3ToolStripMenuItem.Text = file;
                            recentFile3ToolStripMenuItem.Visible = true;
                            break;
                        default:
                            break;
                    } // switch
                    ++i;
                } // foreach recent file
            } // recent file
        } // UpdateGUI

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        { }

        /// <summary>
        /// ShowTokenList menu item
        /// Tokenizes the active file and displays the token list in the Token List tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showTokenListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Facade.TokenizeFile();
            // Write output file
            m_FM.CreateFile(m_Facade.GetTokenList(), m_FM.LastFilePath + "\\" +
                m_FM.getFolderName() + "\\", // specify active folder
                m_FM.LastFileName + "_Tokenlist.txt");

            UpdateGUI();

            // Set TokenList tab active
            this.tabsMain.SelectedTab = tabTokens;
        } // showTokenList

        /// <summary>
        /// TestSymbolTable menu item
        /// Performs a test of the symbol and displays all symbols in the Symbol Table tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testSymbolTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Facade.TestSymbolTable();

            // Write output file
            m_FM.CreateFile(m_Facade.GetSymbolTable(), m_FM.LastFilePath + "\\" +
                m_FM.getFolderName() + "\\", // specify active folder
                m_FM.LastFileName + "_SymbolTable.txt");

            UpdateGUI();

            // Set SymbolTable tab active
            this.tabsMain.SelectedTab = tabSymTabl;
        }

		/// <summary>
		/// Sets the default folder tool strip menu item_ click.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void setDefaultFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdTemp = new FolderBrowserDialog();
            if (fbdTemp.ShowDialog() == DialogResult.OK)
            {
                m_FM.LastFilePath = fbdTemp.SelectedPath;
                m_FM.LastFileName = "Temp";
                m_FM.CreateDir(m_FM.LastFilePath);
                UpdateGUI();
            } // if OK

        }

		/// <summary>
		/// Compiles and runs active file
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void compileFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_FM.isOpen())
            {
                m_Facade.CompileFile();

                // display compile time
                lbCompileTime.Text = "Compile Time: " + m_Facade.CompileTime.TotalMilliseconds
                    + " milliseconds";
                // for Tokens tab
                m_Facade.TokenizeFile();

            }
            else
                ErrorHandler.Error(ERROR_CODE.FILE_NOT_OPEN, -1,
                    "No file is open to compile.\r\nPlease open a file and try again.");
            UpdateGUI();

        } // testSymbolTable 

		/// <summary>
		/// Saves the error log to default directory
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void saveErrorLogToolStripMenuItem_Click(object sender, EventArgs e)
        { ErrorHandler.SaveErrorLog(); }

		/// <summary>
		/// Displays the error console
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void displayErrorConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErrorConsoleForm errConsForm = new ErrorConsoleForm();
            errConsForm.Show();
        } // DisplayerrorConsole

		/// <summary>
		/// Displays compile flag menu
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void compileFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlagsForm flagForm = new FlagsForm();
            flagForm.Show();
        } // CompileFlagMenu

		/// <summary>
		/// Clears the recent files open
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void clearRecentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_lsRecentFiles.Clear();
            recentFile1ToolStripMenuItem.Visible = false;
            recentFile2ToolStripMenuItem.Visible = false;
            recentFile3ToolStripMenuItem.Visible = false;
            UpdateGUI();
        } // ClearRecentFile

		/// <summary>
		/// Opens the file at path, helper function
		/// </summary>
		/// <param name="path">Path.</param>
        private void openFilePath(string path)
        {
            // Close file if open
            if (m_FM.isOpen())
                m_FM.CloseFile();

            // Load file
            m_FM.OpenFile(path);

            // Update last file opened
            m_FM.LastFilePath = Path.GetDirectoryName(path);
            m_FM.LastFileName = Path.GetFileName(path);

            // Create clean directory
            m_FM.CreateCleanDirectory(m_FM.LastFilePath + "\\" + m_FM.getFolderName());

            UpdateGUI();

            // Reset GetChar pointer
            m_Facade.resetChar();
        } // OpenFilePath

		/// <summary>
		/// Recent File 1
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void recentFile1ToolStripMenuItem_Click(object sender, EventArgs e)
        { openFilePath(m_lsRecentFiles[0]); }

		/// <summary>
		/// Recent File 2
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void recentFile2ToolStripMenuItem_Click(object sender, EventArgs e)
        { openFilePath(m_lsRecentFiles[1]); }

		/// <summary>
		/// Recent File 3
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        private void recentFile3ToolStripMenuItem_Click(object sender, EventArgs e)
        { openFilePath(m_lsRecentFiles[2]); }

    } // MainForm partial class
} // Namespace JoshPiler
