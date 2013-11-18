using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;   // StreamReader

namespace JoshPiler
{
    /// <summary>
    /// Singleton class that controls file opening, reading, and closing.
    /// </summary>
    class SourceReader
    {
        // static class instance
        private static SourceReader c_srInstance;
        // lock object
        private static object c_srLock = new Object();

        // StreamReader object to read from file
        private StreamReader m_StrRdr;

        private string m_sFile;  // Active file in string form
        private string m_sFilePath; // Path of opened file

        /// <summary>
        /// Default constructor
        /// </summary>
        private SourceReader() { }

        /// <summary>
        /// Return static instance of class
        /// </summary>
        public static SourceReader Instance
        {
            get
            {
                lock (c_srLock)
                {
                    if (c_srInstance == null) c_srInstance = new SourceReader();
                    return c_srInstance;
                } // lock
            } // get
        } // Instance

        /// <summary>
        /// Load file from path into class member. If successful true has been returned, else 
        /// false has been returned.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool OpenFile(string path)
        {
            // Attempt to load file
            try
            { m_StrRdr = new StreamReader(path); }
            catch (Exception e)     // File open failure
            {
                ErrorHandler.Error(ERROR_CODE.FILE_OPEN_ERROR, -1,
                    e.ToString());
                return false; 
            }

            m_sFilePath = path;

            m_sFile = m_StrRdr.ReadToEnd();
            // Reset file pointer to beginning
            // Credit: stackoverflow.com/questions/6467853/return-streamreader-to-beginning-when-his-basestream-has-bom
            m_StrRdr.BaseStream.Position = 0;
            m_StrRdr = new StreamReader(m_StrRdr.BaseStream, m_StrRdr.CurrentEncoding, false);

            return true;
        } // OpenFile

        /// <summary>
        /// Returns true if there is a file open, false if a file is not
        /// </summary>
        /// <returns></returns>
        public bool isOpen()
        { return (m_StrRdr != null); }

        /// <summary>
        /// Class member file has been closed. If successful true has been returned, else 
        /// false has been returned.
        /// </summary>
        /// <returns></returns>
        public bool CloseFile()
        {
            try
            { m_StrRdr.Close(); }
            catch (Exception e)
            {
                ErrorHandler.Error(ERROR_CODE.FILE_CLOSE_ERROR, -1, e.ToString());
                return false; 
            }

            // Null file pointer
            c_srInstance = null;
            return true;
        } // CloseFile

        /// <summary>
        /// Return class StreamReader member
        /// </summary>
        public StreamReader File
        { get { return m_StrRdr; } }

        public string FileName
        { get { return m_sFilePath; } }

        /// <summary>
        /// Reset read position in file
        /// </summary>
        public void Reset()
        { m_StrRdr.BaseStream.Position = 0; }

        /// <summary>
        /// Contents in class member file have been converted to string
        /// and returned. NOTE Position in file has been reset to beginning.
        /// </summary>
        public String FileToString
        {
            get
            {
                if (m_sFile == null) return "";
                else return m_sFile;
            } // get
        } // FileToString
    } // SourceReader
} // namespace JoshPiler
