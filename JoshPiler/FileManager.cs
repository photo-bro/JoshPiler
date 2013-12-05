using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshPiler
{
    /// <summary>
    /// Singleton Class wrapping the functionality to load and save files
    /// </summary>
    class FileManager
    {
        // Static instance of class
        private static FileManager c_fmInstance;

        // Lock object
        private static object c_fmLock = new Object();

        // Compiler name 
        private string m_sCompName = "JoshPiler";
        // Version
        private string m_sVer = "1.02a";

        private Filer m_filer = new Filer();

        // Default paths and filenames
        private string m_sMASMdir = "F:\\Compilers\\MASM32";
        private string m_sLastFilePath = "F:\\Compilers\\MODS";
        private string m_sLastFileName = "18_Bubl.mod";

        // Main Proc name
        private string m_sMainProc = "PJoshPiler_Main";

        // SourceReader object
        private SourceReader m_SrcRdr = SourceReader.Instance;

        /// <summary>
        /// Default constructor 
        /// </summary>
        private FileManager() { }

        /// <summary>
        /// Return static instance of class
        /// </summary>
        public static FileManager Instance
        {
            get
            {
                lock (c_fmLock)
                {
                    if (c_fmInstance == null)
                        c_fmInstance = new FileManager();
                    return c_fmInstance;
                } // lock
            } // get
        } // Instance

        /// <summary>
        /// Returns the compiler's name
        /// </summary>
        public string CompilerName
        { get { return m_sCompName; } }

        /// <summary>
        /// Returns the compiler's current version
        /// </summary>
        public string CompilerVersion
        { get { return m_sVer; } }

        /// <summary>
        /// Access to default MASM directory
        /// </summary>
        public string MASMdir
        {
            get { return m_sMASMdir; }
            set { m_sMASMdir = value; }
        } // MASMdir

        /// <summary>
        /// Get/set last path of file opened
        /// </summary>
        public string LastFilePath
        {
            get { return m_sLastFilePath; }
            set { m_sLastFilePath = value; }
        } // LastFilePath

        /// <summary>
        /// Get/set name of last opened file without extension
        /// </summary>
        public string LastFileName
        {
            // Remove .mod
            get { return m_sLastFileName.Remove(m_sLastFileName.Length - 4, 4); }
            set { m_sLastFileName = value; }
        } // LastFileName

        /// <summary>
        /// Gets the name for the _procs.inc file
        /// </summary>
        public string MainProcName
        { get { return m_sMainProc; } }


        /// <summary>
        /// Returns a new folder name based on the last opened file
        /// </summary>
        /// <returns></returns>
        public string getFolderName()
        {
            if (m_sLastFileName.Contains(".mod"))
            {
                StringBuilder sb = new StringBuilder(m_sLastFileName);
                return string.Format("{0}_{1}", m_sCompName,
                    sb.ToString(0, sb.Length - 4)); // Cut off .mod
            }
            else
                return m_sCompName + "_" + m_sLastFileName;
        } // getFolderName

        /// <summary>
        /// Opens the file specified at path. True is returned if successful, else
        /// false has been returned
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool OpenFile(string path)
        { return m_SrcRdr.OpenFile(path); }

        /// <summary>
        /// Deletes any directory located at path. True has been returned if 
        /// successful, false if not.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CreateCleanDirectory(string path)
        { return m_filer.CreateCleanDir(path); }

        /// <summary>
        /// Creates a text file containing text at directory path. True has been
        /// returned if successful, false if not.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CreateFile(string text, string path, string name)
        { return m_filer.CreateFile(text, path, name); }

        /// <summary>
        /// Creates a new directory at path. True has been returned if successful,
        /// false if not
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CreateDir(string path)
        { return m_filer.CreateDir(path); }

        /// <summary>
        /// Returns true if there is a file open, false if not
        /// </summary>
        /// <returns></returns>
        public bool isOpen()
        { return m_SrcRdr.isOpen(); }

        /// <summary>
        /// Closes the active file. True is returned if successful, else
        /// false has been returned
        /// </summary>
        /// <returns></returns>
        public bool CloseFile()
        { return m_SrcRdr.CloseFile(); }

        /// <summary>
        /// Resets current char and line count in source file
        /// </summary>
        public void Reset()
        { m_SrcRdr.Reset(); }

        /// <summary>
        /// Returns the contents of the active file in string form
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        { return m_SrcRdr.FileToString; }

        /// <summary>
        /// Returns the active files full name and path
        /// </summary>
        public string CurrentFileName
        { get { return m_SrcRdr.FileName; } }

    } // Class FileManager
} // Namespace JoshPiler