using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;        // StringBuilder
using System.Collections; // Hashtable, List<T>

namespace JoshPiler
{

    /// <summary>
    /// Tokenizer returns tokens (lexemes).
    /// This is implemented as a singleton pattern (Design Patterns in C#)
    /// </summary>
    public class Tokenizer
    {
        // The Modula-2 source file
        FileManager m_FM;

        // The hashtable of Modula-2 keywords
        Hashtable m_htKeywords;

        // The instance token list
        List<Token> m_tokList = new List<Token>();

        // Active token pointer
        int m_iActiveToken = 0;

        // The single object instance for this class.
        private static Tokenizer c_tokenizer;

        // To prevent access by more than one thread. This is the specific lock 
        //    belonging to the Tokenizer Class object.
        private static Object c_tokenizerLock = typeof(Tokenizer);

        // Instead of a constructor, we offer a static method to return the only
        //    instance.
        private Tokenizer() { } // private constructor so no one else can create one.

        static public Tokenizer GetTokenizer()
        {
            lock (c_tokenizerLock)
            {
                // if this is the first request, initialize the one instance
                if (c_tokenizer == null)
                {
                    // create the single object instance
                    c_tokenizer = new Tokenizer();

                    // Load the keywords
                    c_tokenizer.LoadKeywords();
                } // if (c_tokenizer == null)

                // return a reference to the only instance
                return c_tokenizer;
            } // lock
        } // GetTokenizer()

        /// <summary>
        /// Set the name of the Modula-2 source code file (including path).
        /// NOTE: NOT USED AT THIS TIME
        /// PRE:  A source file is available and has the given name.
        /// POST: The SourceFile object is created with the given name. 
        ///    The source file is not yet opened nor is its existence verified.
        /// </summary>
        public void SetSourceFileName(string inName)
        {
            m_FM = FileManager.Instance;
            m_FM.OpenFile(inName);
            Reset();
        } // SetSourceFileName

        /// <summary>
        /// Reset the lexing process.
        /// PRE:  A source file is known.
        /// POST: The SourceFile object is reset.
        /// </summary>
        public void Reset()
        {
            // set the file position back to the starting point
            m_tokList = new List<Token>();
            m_iActiveToken = 0;
            FileManager.Instance.Reset();
            GetChar.Instance.Reset();
        } // Reset

        /// <summary>
        /// Load M2 keywords.
        /// PRE:  An array of keywords is available in Token.
        /// POST: The hashtable is loaded and the correct token type is
        ///    associated with each entry.
        /// </summary>
        private void LoadKeywords()
        {
            m_htKeywords = new Hashtable();

            // load the keyword hashtable
            for (int i = 0; i < Token.c_iKeywordCount; i++)
                // For each entry, we add the string as the *key*,
                //    and the loop control variable i (cast as a TOKENTYPE) as the *value*.
                m_htKeywords.Add(((Token.TOKENTYPE)i).ToString(), (Token.TOKENTYPE)i);
        } // LoadKeywords

        /// <summary>
        /// Returns next token from active file. Increments token position.
        /// </summary>
        /// <returns></returns>
        public Token NextToken()
        {
            if (m_tokList.Count < 1)
                TokenizeFile();
            if (m_iActiveToken + 1 > m_tokList.Count)
            {
                ErrorHandler.Error(ERROR_CODE.OUT_OF_RANGE, -1, "Next token is does not exist");
                return null;
            }
            return m_tokList[m_iActiveToken++];
        } // NextToken()

        /// <summary>
        /// Returns the next token from active file. Does not increment token position.
        /// </summary>
        /// <returns></returns>
        public Token PeekToken()
        {
            if (m_tokList.Count < 1)
                TokenizeFile();
            if (m_iActiveToken + 1 > m_tokList.Count)
            {
                ErrorHandler.Error(ERROR_CODE.OUT_OF_RANGE, -1, "Next token is does not exist");
                return null;
            }
            return m_tokList[m_iActiveToken];
        } // PeekToken()

        /// <summary>
        /// Returns the previous token from the active file. Does not decrement token position.
        /// </summary>
        /// <returns></returns>
        public Token PeekPrevToken()
        {
            if (m_tokList.Count < 1)
                TokenizeFile();
            if (m_iActiveToken < 1)
                ErrorHandler.Error(ERROR_CODE.OUT_OF_RANGE, -1, "Previous token does not exist");
            return m_tokList[m_iActiveToken - 2];
        } // PeekPrevToken()

        /// <summary>
        /// This is the principal function of this class.
        ///
        /// PRE:  A source file must be available.
        ///       The source file is open and positioned at the next token (or white space).
        /// POST: The Token object is created, loaded with correct information, and returned.
        /// </summary>
        /// <returns>Token loaded with the correct data.
        /// If tokenizing has failed, the TOKENTYPE is T_ERROR</returns>
        private Token GetNextToken()
        {
            GetChar gc = GetChar.Instance;  // grabs each char from file, increments pos in file
            char cNextChar = gc.nextChar;   // hold char from file
            StringBuilder sb = new StringBuilder();  // concatenating larger tokens

            // Check for space(s), mult 'x', and tabs
            while (cNextChar == ' ' || cNextChar == (char)0xD7 || cNextChar == '	')
                cNextChar = gc.nextChar;

            // Check for smaller tokens first
            switch (cNextChar)
            {
                case ',':
                    return new Token(Token.TOKENTYPE.COMMA, cNextChar.ToString(), gc.LineCount);
                case '.':
                    if (gc.peekChar == '.')
                    {
                        sb.Append(cNextChar);
                        sb.Append(gc.nextChar);
                        return new Token(Token.TOKENTYPE.DOT_DOT, sb.ToString(), gc.LineCount);
                    } // if (gc.peekChar == '.')
                    return new Token(Token.TOKENTYPE.DOT, cNextChar.ToString(), gc.LineCount);
                case '=':
                    return new Token(Token.TOKENTYPE.EQUAL, cNextChar.ToString(), gc.LineCount);
                case '[':
                    return new Token(Token.TOKENTYPE.LEFT_BRACK, cNextChar.ToString(), gc.LineCount);
                case '(':
                    if (gc.peekChar == '*')
                    {
                        // Recursive comment eater - probably not the best implementation
                        RemoveComment();
                        return GetNextToken();
                    } // if (gc.peekChar == '*')
                    return new Token(Token.TOKENTYPE.LEFT_PAREN, cNextChar.ToString(), gc.LineCount);
                case '-':
                    //if (char.IsDigit(gc.peekChar)) break; // negative number
                    return new Token(Token.TOKENTYPE.MINUS, cNextChar.ToString(), gc.LineCount);
                case '*':
                    return new Token(Token.TOKENTYPE.MULT, cNextChar.ToString(), gc.LineCount);
                case '+':
                    return new Token(Token.TOKENTYPE.PLUS, cNextChar.ToString(), gc.LineCount);
                case ']':
                    return new Token(Token.TOKENTYPE.RIGHT_BRACK, cNextChar.ToString(), gc.LineCount);
                case ')':
                    return new Token(Token.TOKENTYPE.RIGHT_PAREN, cNextChar.ToString(), gc.LineCount);
                case ';':
                    return new Token(Token.TOKENTYPE.SEMI_COLON, cNextChar.ToString(), gc.LineCount);
                case '/':
                    return new Token(Token.TOKENTYPE.SLASH, cNextChar.ToString(), gc.LineCount);
                case ':':
                    // Check if ":="
                    if (gc.peekChar == '=')
                    {
                        sb.Append(cNextChar);
                        sb.Append(gc.nextChar);
                        return new Token(Token.TOKENTYPE.ASSIGN, sb.ToString(), gc.LineCount);
                    } // if (gc.peekChar == '=')
                    // Assume just ':'
                    else
                        return new Token(Token.TOKENTYPE.COLON, cNextChar.ToString(), gc.LineCount);
                case '!':
                    if (gc.peekChar == '=')
                    {
                        sb.Append(cNextChar);
                        sb.Append(gc.nextChar);
                        return new Token(Token.TOKENTYPE.NOT_EQ, sb.ToString(), gc.LineCount);
                    } // (gc.peekChar == '=')
                    // Error
                    return new Token(Token.TOKENTYPE.ERROR, cNextChar.ToString(), gc.LineCount);
                case '>':
                    // Check if ">="
                    if (gc.peekChar == '=')
                    {
                        sb.Append(cNextChar);
                        sb.Append(gc.nextChar);
                        return new Token(Token.TOKENTYPE.GRTR_THAN_EQ, sb.ToString(), gc.LineCount);
                    } // (gc.peekChar == '=')
                    // Assume just '>'
                    else
                        return new Token(Token.TOKENTYPE.GRTR_THAN, cNextChar.ToString(), gc.LineCount);
                case '<':
                    // Check if "<="
                    if (gc.peekChar == '=')
                    {
                        sb.Append(cNextChar);
                        sb.Append(gc.nextChar);
                        return new Token(Token.TOKENTYPE.LESS_THAN_EQ, sb.ToString(), gc.LineCount);
                    } // (gc.peekChar == '=')
                    else if (gc.peekChar == '>') // <>
                    {
                        sb.Append(cNextChar);
                        sb.Append(gc.nextChar);
                        return new Token(Token.TOKENTYPE.NOT_EQ, sb.ToString(), gc.LineCount);
                    } // if (gc.peekChar == '>')
                    // Assume just '<'
                    else
                        return new Token(Token.TOKENTYPE.LESS_THAN, cNextChar.ToString(), gc.LineCount);
                case '#':
                    return new Token(Token.TOKENTYPE.NOT_EQ, cNextChar.ToString(), gc.LineCount);
                case '\"': // string
                    cNextChar = gc.nextChar;
                    while (cNextChar != '\"')
                    {
                        sb.Append(cNextChar);
                        cNextChar = gc.nextChar;
                    } // while (cNextChar != '\"')
                    return new Token(Token.TOKENTYPE.STRING, sb.ToString(), gc.LineCount);
                // EOF GetChar should return 0x03 (ETX) when it reaches the EOF, but check alternates in case
                case (char)0x03: // etx
                case (char)0x04: // eot
                case (char)0x05: // enq
                    return new Token(Token.TOKENTYPE.EOF, "End of source file", gc.LineCount);
                default:
                    break;
            } // switch(cNextChar) - small token switch

            // Bigger tokens
            // ID and Keywords
            if (char.IsLetter(cNextChar) || cNextChar == '_')
                return LoadIDToken(cNextChar);

            // INT / REAL
            if (char.IsDigit(cNextChar) || cNextChar == '-')
            { return LoadNumToken(cNextChar); }

            // We only get here by mistake! Throw an exception.
            string strMsg = string.Format("Non-tokenizable character '{0}' at source line {1}.",
               cNextChar, gc.LineCount);

            throw new Exception(strMsg);
        } // GetNextToken

        /// <summary>
        /// Method to scan an integer.
        /// PRE: char c is the current char in the file
        /// POST: The full integer or float has been scanned and the token returned.
        /// </summary>
        Token LoadNumToken(char c)
        {
            GetChar gc = GetChar.Instance;
            StringBuilder sb = new StringBuilder();
            char cNextChar = c;

            sb.Append(cNextChar);
            while (char.IsDigit(gc.peekChar))
            {
                cNextChar = gc.nextChar;
                sb.Append(cNextChar);
                // Check for decimal point (real number)
                if (gc.peekChar == '.')
                {
                    cNextChar = gc.nextChar;
                    sb.Append(cNextChar);
                    while (char.IsDigit(gc.peekChar))
                    {
                        cNextChar = gc.nextChar;
                        sb.Append(cNextChar);
                    } // inner while next char is digit
                    return new Token(Token.TOKENTYPE.REAL_NUM, sb.ToString(), gc.LineCount);
                } // if gc.peekChar == '.'
            } // outer while char is digit
            return new Token(Token.TOKENTYPE.INT_NUM, sb.ToString(), gc.LineCount);
        } // LoadNumToken

        /// <summary>
        /// Remove a comment (* comment *).
        /// PRE:  (* has been scanned.
        /// POST: The full comment has been removed.
        /// </summary>
        void RemoveComment()
        {
            GetChar gc = GetChar.Instance;
            char cTemp = gc.nextChar;
            // Eat comment content
            while (true)
            {
                if (cTemp == '*')
                    if (gc.peekChar == ')')
                        break;
                cTemp = gc.nextChar;
            } // while (1)

            cTemp = gc.nextChar; // Eat one more char
        } // RemoveComment

        /// <summary>
        /// Method to scan an identifier.
        /// PRE:  char c is the current char in the file
        /// POST: The full identifier has been scanned and the token returned. This method
        ///    correctly handles keywords by searching for them in the keyword table.
        /// </summary>
        Token LoadIDToken(char c)
        {
            GetChar gc = GetChar.Instance;  // pull chars from file
            StringBuilder sb = new StringBuilder(); // collect string
            char cNextChar = c;

            sb.Append(cNextChar);
            while (char.IsLetterOrDigit(gc.peekChar) || (gc.peekChar == '_'))
            {
                cNextChar = gc.nextChar;
                sb.Append(cNextChar);
            } // while is letter-digit or '_'
            // Check if keyword exists in hashtable
            if (m_htKeywords.Contains(sb.ToString()))
                return new Token((Token.TOKENTYPE)m_htKeywords[sb.ToString()], sb.ToString(), gc.LineCount);
            // Assume ID token
            return new Token(Token.TOKENTYPE.ID, sb.ToString(), gc.LineCount);
        } // LoadIDToken

        /// <summary>
        /// PRE:  The source file has been set.
        /// POST: A string is returned that includes all tokens from the source file
        ///    attractively formatted. Exceptions are wisely handled and 
        ///    informatively shared with the user.
        /// </summary>
        public string ListTokens()
        {
            // Make sure there are tokens in m_tokList
            TokenizeFile();

            StringBuilder sb = new StringBuilder();
            uint uiC = 0;

            // Header info
            sb.Append("Token list:\r\n\r\n");
            sb.Append(string.Format("  {4,1} {3,3} {0,-2} {1,-15} {2,23}\r\n\r\n", "Type",
                "", "Lexeme", "Ln", "TK"));

            // Build string of tokens
            foreach (Token t in m_tokList)
            {
                sb.Append(
                    string.Format("{0,4} {1}", uiC++, t.ToString()));
                sb.Append("\r\n");
            } // foreach
            return sb.ToString();
        } // ListTokens

        /// <summary>
        /// PRE: A file is open
        /// POST: The active file has been tokenized to internal data structure
        /// </summary>
        public void TokenizeFile()
        {
            m_tokList = new List<Token>(); // Reset token list - assume garbage collector eats old one
            GetChar.Instance.Reset(); // Reset GetChar
            Token tok = this.GetNextToken();

            // Build list of tokens
            while (tok.m_tokType != Token.TOKENTYPE.ERROR && tok.m_tokType != Token.TOKENTYPE.EOF)
            {
                m_tokList.Add(tok);
                tok = this.GetNextToken();
                if (tok.m_tokType == Token.TOKENTYPE.EOF) m_tokList.Add(tok);
            } // while
        } // TokenizeFile
    } // class Tokenizer
} // Namespace JoshPiler