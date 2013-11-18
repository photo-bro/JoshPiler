using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; // Error window

namespace JoshPiler
{
    /// <summary>
    /// Program level error codes
    /// </summary>
    public enum ERROR_CODE
    {
        UKNOWN_ERROR = -1,
        NONE = 0,
        TOKEN_INVALID = 1,
        FILE_NOT_OPEN = 2,
        FILE_OPEN_ERROR = 3,
        FILE_SAVE_ERROR = 4,
        FILE_CLOSE_ERROR = 5,
        SYMBOL_NOT_IN_SCOPE = 6,
        SYMBOL_UNDEFINED = 7,
        SYMBOL_UNITIALIZED = 8,
        VAR_REDECLARATION = 9,
        CONST_REDECLARATION = 10,
        CONST_REDEFINITION = 11,
        OUT_OF_RANGE = 12,
        INVALID_TYPE = 13,
        TYPE_REDECLARATION = 14,
        TYPE_REDEFINITION = 15,
        PROC_REDECLARATION = 16

    } // ERROR_CODE

    /// <summary>
    /// Basic static class for handling errors
    /// </summary>
    class ErrorHandler
    {
        // current error
        private static ERROR_CODE c_curErr = 0;

        // error log
        private static string c_sErrLog = "";

        // error count
        private static int c_iErrCount = 0;

        // error window flag
        private static bool c_bShowWindow = true;

        /// <summary>
        /// Default constructor, not used
        /// </summary>
        private ErrorHandler() { }

        /// <summary>
        /// "Throw" error, displays MessageBox with error information
        /// </summary>
        /// <param name="err"></param>
        /// <param name="line"></param>
        /// <param name="comment"></param>
        public static void Error(ERROR_CODE err, int line, string comment)
        {
            c_curErr = err;
            // error string
            string s = string.Format("Error {0}: {1} at line: {2}\r\n{3}", 
                (int) err, err.ToString(), line, comment);
            // add error to log
            AddToLog(s);
            // display window
            if (c_bShowWindow)  MessageBox.Show(
                s,
                "ERROR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);
        } // Error

        /// <summary>
        /// Add error string to class log
        /// </summary>
        /// <param name="s"></param>
        private static void AddToLog(string s)
        {
            DateTime dt = DateTime.Now;
            c_sErrLog += string.Format("{0}\r\n#{1}: {2}\r\n====================================================\r\n",
            dt.ToString(), c_iErrCount++, s);
        }

        /// <summary>
        /// Enable / Disable popup error box. Popup box will display when true.
        /// </summary>
        public static bool ShowPopups
        { 
            get { return c_bShowWindow; }
            set { c_bShowWindow = value; }
        }

        /// <summary>
        /// Returns class error log
        /// </summary>
        public static string ErrorLog
        { get { return c_sErrLog; } }

        /// <summary>
        /// A file "ErrorLog.txt" has been created at last known directory
        /// </summary>
        public static void SaveErrorLog()
        {
            FileManager fm = FileManager.Instance;
            fm.CreateFile(c_sErrLog, fm.LastFilePath + "\\" + fm.getFolderName(), "ErrorLog.txt");
        }

        /// <summary>
        /// Returns last error set
        /// </summary>
        public static ERROR_CODE LastError
        { get { return c_curErr; } }

    } // ErrorHandler
} // Namespace JoshPiler
