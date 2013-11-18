using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; // error box

namespace JoshPiler
{
    /// <summary>
    /// Singleton class controlling the main functionality of JoshPiler
    /// </summary>
    class Facade
    {
        // static class instance
        private static Facade c_Facade;
        // lock object
        private static object c_fLock = new Object();

        // singleton instances
        private FileManager m_FileManager = FileManager.Instance;
        private GetChar m_GetChar = GetChar.Instance;
        private Parser m_Parser = Parser.Instance;
        private Emitter m_Emitter = Emitter.Instance;

        // Token list in string form
        private string m_sTokList = "";

        // symbol table in string form
        private string m_sSymTable = "";

        // inc strings
        private string m_sProcInc = "";
        private string m_sStringInc = "";

        // compile time
        private TimeSpan m_tsCompTime;

        /// <summary>
        /// Default Constructor - Not used
        /// </summary>
        private Facade() { }

        /// <summary>
        /// Return static instance of class
        /// </summary>
        public static Facade Instance
        {
            get
            {
                lock (c_fLock)
                {
                    if (c_Facade == null) c_Facade = new Facade();
                    return c_Facade;
                } // lock
            } // get
        } // Instance

        /// <summary>
        /// Load a text file to be used. Error box displayed if error opening file.
        /// </summary>
        /// <param name="path"></param>
        public void LoadFile(string path)
        {
            if (!m_FileManager.OpenFile(path))
            {
                MessageBox.Show("File open error", "ERROR");
                return;
            }
        } // LoadFile

        /// <summary>
        /// Return a char from active file, increments position everytime function is called
        /// </summary>
        /// <returns></returns>
        public char getChar()
        { return m_GetChar.nextChar; }

        /// <summary>
        /// Reset char position to beginning of active file
        /// </summary>
        public void resetChar()
        { m_GetChar.Reset(); }

        /// <summary>
        /// Tokenizes the active file
        /// </summary>
        public void TokenizeFile()
        { m_sTokList = Tokenizer.GetTokenizer().ListTokens(); }

        /// <summary>
        /// Returns the token list of the active file in string form
        /// </summary>
        /// <returns></returns>
        public string GetTokenList()
        { return m_sTokList; }

        /// <summary>
        /// Adds test symbols to the symbol table.
        /// </summary>
        /// <returns></returns>
        public void TestSymbolTable()
        {
            m_Parser.TestSymbolTable();
            m_sSymTable = m_Parser.GetSymbolTable();
        }
        
        /// <summary>
        /// Return the symbol table of the active file in string form
        /// </summary>
        /// <returns></returns>
        public string GetSymbolTable()
        { return m_sSymTable; }

        public string GetStringInc()
        { return m_sStringInc; }

        public string GetProclistInc()
        { return m_sProcInc; }

        public TimeSpan CompileTime
        { get { return m_tsCompTime; } }

        /// <summary>
        /// Compile file. All output files have been created.
        /// </summary>
        public void CompileFile()
        {
            var sw = new System.Diagnostics.Stopwatch(); // to get compile time
            sw.Start();  // start stopwatch 

            // Parse / Emit open file
            m_Parser.ParseFile();

            // set inc strings
            m_sProcInc = m_Emitter.ProcListInc();
            m_sStringInc = m_Emitter.StringsInc();

            // get symbol table
            m_sSymTable = m_Parser.GetSymbolTable();

            sw.Stop();  // stop stopwatch
            m_tsCompTime = sw.Elapsed;

            // Save to file
            m_Emitter.WriteAFiles();
        }

        // Compiler flags
        /// <summary>
        /// Set/get MoreComments compiler flag
        /// </summary>
        public bool FlagMoreComments
        {
            get { return m_Emitter.m_bMoreComments; }
            set { m_Emitter.m_bMoreComments = value; }
        } // FlagMoreComments

    } // class Facade
} // namespace JoshPiler