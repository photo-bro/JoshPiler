using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshPiler
{
    /// <summary>
    /// Singleton class to access a single char of a given string.
    /// Uses the FileManager class to access active file
    /// </summary>
    class GetChar
    {   
        // static class instance
        private static GetChar c_GC;    
        // lock object
        private static object c_gcLock = new Object();

        private int m_iCP = 0;               // char pointer
        private int m_iLC = 1;               // line counters, starts at 1
        private char m_cBLANK = (char)0xD7;  // Default empty char (0xD7 is an multiplication x), make space (0x20)?
        private char m_cEOF = (char)0x03;    // ETX - end of text

        // singleton instances
        private FileManager m_FileManager = FileManager.Instance;

        /// <summary>
        /// Default constructor
        /// </summary>
        private GetChar() { }

        /// <summary>
        /// Return static instance of class
        /// </summary>
        public static GetChar Instance
        {
            get
            {
                lock (c_gcLock)
                {
                    if (c_GC == null) c_GC = new GetChar();
                    return c_GC;
                } // lock
            } // get
        } // Instance

        /// <summary>
        /// Reset pointer to char in class source string
        /// </summary>
        public void Reset()
        {
            m_iCP = 0;
            m_iLC = 1;
        } // Reset

        /// <summary>
        /// Get next char in active file, increments position
        /// </summary>
        public char nextChar
        {
            get
            {
                // Check if new line and carriage return char
                if (m_iCP + 1 < m_FileManager.ToString().Length)
                    if (m_FileManager.ToString()[m_iCP] == '\r'
                    && m_FileManager.ToString()[m_iCP + 1] == '\n')
                    {
                        m_iLC++;    // increment line count
                        m_iCP += 2; // consume 2 chars
                        return m_cBLANK; // return empty char
                    }
                return (m_iCP >= m_FileManager.ToString().Length) ? m_cEOF : m_FileManager.ToString()[m_iCP++];
            } // get
        } // nextChar

        /// <summary>
        /// Peek at next char in active file, does not increment position
        /// </summary>
        public char peekChar
        {
            get
            { return (m_iCP + 1 >= m_FileManager.ToString().Length) ? m_cEOF : m_FileManager.ToString()[m_iCP]; }
        } // peekChar

        /// <summary>
        /// Get previous char in active file, decrements position
        /// </summary>
        public char prevChar
        {
            get
            {
                // Check if new line and carriage return char
                if (m_iCP > 2)
                    if (m_FileManager.ToString()[m_iCP - 1] == '\n'
                    && m_FileManager.ToString()[m_iCP - 2] == '\r')
                    {
                        m_iLC--;    // increment line count
                        m_iCP -= 3; // consume 3 chars, point to third
                        return m_cBLANK; // return empty char
                    }
                return (m_iCP < 1) ? m_cBLANK : m_FileManager.ToString()[m_iCP--];
            } // get
        } // prevChar

        /// <summary>
        /// Return the current line count
        /// </summary>
        public int LineCount
        { get { return m_iLC; } }

        /// <summary>
        /// Return true if class string is loaded, false if not.
        /// </summary>
        /// <returns></returns>
        public bool Loaded()
        { return (m_FileManager.ToString().Length > 0); }

    } // class GetChar
} // Namespace JoshPiler
