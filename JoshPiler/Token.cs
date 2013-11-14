using System;               // Exception 

/// <summary>
/// A token is a category of lexemes.
/// Some tokens are keywords like "MODULE". Some tokens are symbols; the token of type
///    PLUS is the symbol "+". Some tokens are a large set; some tokens of type
///    INTEGER are "73" and "255".
/// The enumerations below include all the tokens that we need to recognize.
/// 
/// Note that the Modula-2 User's Manual offers both # and <> to mean NOT EQUAL 
///    (pp. 96, 97, and 110). Our code does not use #. We only use <>.
/// 
/// Many errors are reported by TOKENTYPE which is just a number (like 34 for ID).
/// Therefore, I added enumeration numbers for tokens in the left column as a
///    convenience during debugging.
///    
/// Author: Tom Fuller
/// Date:   January 6, 2007
/// </summary>
/// 
namespace JoshPiler
{
    public class Token
    {
        public enum TOKENTYPE
        {
            /* 0*/
            ERROR = 0, AND, ARRAY, BEGIN,
            /* 4*/
            CARDINAL, CONST, DIV, DO,
            /* 8*/
            ELSE, END, EXIT, FOR, IF,
            /*13*/
            INTEGER, LOOP, MOD, MODULE,
            /*17*/
            NOT, OF, OR, PROCEDURE,
            /*21*/
            REAL, THEN, TYPE, VAR,
            /*25*/
            WHILE, WRCARD, WRINT, WRLN,
            /*29*/
            WRREAL, RDCARD, RDINT, RDREAL, WRSTR,

            /*34*/
            ID, CARD_NUM, REAL_NUM, INT_NUM,

            /*38*/
            STRING,

            /*39*/
            ASSIGN, COLON, COMMA, DOT,
            /*43*/
            DOT_DOT, EQUAL, LEFT_BRACK, LEFT_PAREN,
            /*47*/
            MINUS, MULT, NOT_EQ, PLUS,
            /*51*/
            RIGHT_BRACK, RIGHT_PAREN, SEMI_COLON, SLASH,
            /*55*/
            GRTR_THAN, GRTR_THAN_EQ, LESS_THAN, LESS_THAN_EQ,

            /* The two comment tokens are not presently used */
            /*59*/
            COMMENT_BEG, COMMENT_END,

            /*61*/
            EOF,
            /*62*/
            NEGATE
        }; // enum TOKENTYPE

        /*********************************************************************************
         There are 34 reserved keywords we need to recognize in Modula-2 source code files.
            Note that enumerated types 0 - 33 above correspond precisely to these keywords. 
            Some number types are not used except by very ambitious students: CARDINAL, REAL.
            Some statements are also reserved for the fearless: WHILE, DO, FOR.
        *********************************************************************************/

        public static int c_iKeywordCount = 35;

        /*********************************************************************************
          The Token class itself stores information about a single token.
         *********************************************************************************/
        public TOKENTYPE m_tokType;    /* what token (i.e. class of lexemes)          */
        public string m_strName;    /* lexeme name: reserved word, identifier      */
        public int m_iLineNum;   /* the (source file)line containing the token  */

        /// <summary>
        /// default constructor, not used
        /// </summary>
        public Token()
        {
            m_tokType = Token.TOKENTYPE.ERROR;
            m_strName = "blank token";
            m_iLineNum = 0;
        } // default constructor

        /// <summary>
        /// lexing error, gives lexeme and line number
        /// </summary>
        /// <param name="inName"></param>
        /// <param name="inLine"></param>
        public Token(string inName, int inLine)
        {
            m_tokType = Token.TOKENTYPE.ERROR;
            m_strName = inName;
            m_iLineNum = inLine;
        } // Token(string inName, int inLine)

        /// <summary>
        /// normal constructor with tokentype, lexeme and line number
        /// </summary>
        /// <param name="tok"></param>
        /// <param name="inName"></param>
        /// <param name="inLine"></param>
        public Token(Token.TOKENTYPE tok, string inName, int inLine)
        {
            m_tokType = tok;
            m_strName = inName;
            m_iLineNum = inLine;
        } // Token(Token.TOKENTYPE tok, string inName, int inLine)

        /// <summary>
        /// Returns the token in string form below:
        /// Linenum:Token (int)tokType: tokType Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{3,3}:Token {0,-2}: {1,-15} {2,18}", (int)m_tokType,
                m_tokType.ToString(), m_strName, m_iLineNum);
        } // ToString()

    } // class Token
} // Namespace JoshPiler