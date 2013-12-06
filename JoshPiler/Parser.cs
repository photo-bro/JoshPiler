using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshPiler
{
    class Parser
    {
        // static class instance
        private static Parser c_parInstance;

        // lock object
        private static object c_parLock = new object();

        // SymbolTable instance
        private SymbolTable m_SymTable = SymbolTable.Instance;

        // Lexer instance
        private Tokenizer m_Tknzr = Tokenizer.GetTokenizer();

        // Emitter instance
        private Emitter m_Em = Emitter.Instance;

        // current token
        private Token m_tok;

        // memory offset - default 32bit offset 
        private int m_iOff = 4; // start at 4, 0 is BP

        // label level
        private int m_iLabelLevel = 0;
        private int m_iArithmeticLevel = 0;
        private const string m_LABEL_BASE = "LABEL_";
        private const string m_ARITH_BASE = "COND_";


        // ASM Debug compiler Flag
        public static bool c_bParserASMDebug = true;

        /// <summary>
        /// Default constructor
        /// </summary>
        private Parser() { }

        /// <summary>
        /// Return static instance of class
        /// </summary>
        public static Parser Instance
        {
            get
            {
                lock (c_parLock)
                {
                    if (c_parInstance == null)
                        c_parInstance = new Parser();
                    return c_parInstance;
                } // lock
            } // get
        } // Instance

        /// <summary>
        /// Parses a modula-2 file. File must be open.
        /// </summary>
        public void ParseFile()
        {
            // make sure tokenizer is reset
            m_Tknzr.Reset();

            // reset emitter
            m_Em.Reset();

            // reset symbol table
            m_SymTable.Reset();

            // reset offset and label count
            m_iOff = 4;
            m_iLabelLevel = 0;
            m_iArithmeticLevel = 0;

            m_tok = m_Tknzr.NextToken();

            // Force jump to main when first run
            m_Em.Jump("jmp", "__MAIN");

            // Loop through file
            while (m_tok.m_tokType != Token.TOKENTYPE.EOF || m_tok == null)
            {
                Module();
                while (m_tok.m_tokType != Token.TOKENTYPE.BEGIN)
                {
                    switch (m_tok.m_tokType)
                    {
                        case Token.TOKENTYPE.CONST:
                            Const();
                            break;
                        case Token.TOKENTYPE.TYPE:
                            Type();
                            break;
                        case Token.TOKENTYPE.VAR:
                            Vars();
                            break;
                        case Token.TOKENTYPE.PROCEDURE:
                            Procedure();
                            break;
                        default:
                            //error
                            break;
                    }
                }
                // print symbol table
                Function();
            } // while
        } // ParseFile

        /// <summary>
        /// Parses the header of a modula-2 file
        /// </summary>
        private void Module()
        {
            // MODULE ID;
            Match(Token.TOKENTYPE.MODULE);
            Match(Token.TOKENTYPE.ID);
            Match(Token.TOKENTYPE.SEMI_COLON);
        } // Module()

        /// <summary>
        /// Parses the PROCEDURE header of a modula-2 file
        /// EX: 
        /// PROCEDURE ID ( ID : TYPE ) ;
        /// </summary>
        private void Procedure()
        {
            // PROCEDURE little
            //( i : INTEGER ) ;
            // store calling scope
            int iCallScope = m_SymTable.ActiveScope;

            // consume PROCEDURE token
            m_tok = m_Tknzr.NextToken(); 

            // look up proc ID, check for redeclaration
            Symbol procSym = m_SymTable.FindSymbol(m_tok.m_strName);
            if (procSym != null)
                ErrorHandler.Error(ERROR_CODE.PROC_REDECLARATION, m_tok.m_iLineNum, "PROCEDURE already declared");

            Token tkProcID = m_tok; // save proc ID

            // Consume ID token
            m_tok = m_Tknzr.NextToken();

            Match(Token.TOKENTYPE.LEFT_PAREN);
            // check for passed by reference
            //  if VAR token exists then parameters are reference types
            if (m_tok.m_tokType == Token.TOKENTYPE.VAR)
            {
                // Add PROC to symbol table to calling scope
                m_SymTable.AddSymbol(new Symbol(tkProcID.m_strName, iCallScope, Symbol.SYMBOL_TYPE.TYPE_REFPROC,
                    Symbol.STORAGE_TYPE.STORE_NONE, Symbol.PARAMETER_TYPE.LOCAL_VAR, 0));

                // Create new scope for procedure
                m_SymTable.AddScope();

                m_tok = m_Tknzr.NextToken(); // consume VAR token
                while (m_tok.m_tokType != Token.TOKENTYPE.RIGHT_PAREN)
                    VarDef(m_SymTable.ActiveScope, 4, Symbol.PARAMETER_TYPE.REF_PARM);

                // Add Proc sym with TYPE_REFPROC so that we know the parameters are of reference type
                //  in the ID() function. Do so by overwriting the symbol
            }
            // Pass by value
            else 
            {
                // Add PROC to symbol table to calling scope
                m_SymTable.AddSymbol(new Symbol(tkProcID.m_strName, iCallScope, Symbol.SYMBOL_TYPE.TYPE_PROC,
                    Symbol.STORAGE_TYPE.STORE_NONE, Symbol.PARAMETER_TYPE.LOCAL_VAR, 0));

                // Create new scope for procedure
                m_SymTable.AddScope();

                // Parse procedure paramaters (arguments)
                while (m_tok.m_tokType != Token.TOKENTYPE.RIGHT_PAREN)          // while not )
                    VarDef(m_SymTable.ActiveScope, 4, Symbol.PARAMETER_TYPE.VAL_PARM);   // baseoffset for scope starts at 4
            }
            m_tok = m_Tknzr.NextToken();     // Consume )

            Match(Token.TOKENTYPE.SEMI_COLON);

            // CONST
            if (m_tok.m_tokType == Token.TOKENTYPE.CONST) Const(m_SymTable.ActiveScope);

            // VAR  Parse PROCEDURE's local vars
            if (m_tok.m_tokType == Token.TOKENTYPE.VAR) Vars(m_SymTable.ActiveScope, 4);

            // Parse PROCEDURE's contents
            m_Em.asm(string.Format("\r\n\r\n;// START Procedure: {0}", tkProcID.m_strName));
            ProcFunction(iCallScope, tkProcID.m_strName);
            m_Em.asm(string.Format(";// END   Procedure: {0}\r\n\r\n", tkProcID.m_strName));
        } // Procedure()

        /// <summary>
        /// Parses the TYPE section
        /// </summary>
        private void Type()
        {
            Match(Token.TOKENTYPE.TYPE);
            List<int> intList = new List<int>();        // type values
            List<string> lsIDs = new List<string>();    // type IDs

            // ID, Equal, ARRAY, LEFT_BRACK, INT, .., INT, RIGHT_BRACK, OF, INTEGER, ;
            //     prListType = ARRAY [11 .. 30] OF INTEGER ;

            while (m_tok.m_tokType != Token.TOKENTYPE.CONST && m_tok.m_tokType != Token.TOKENTYPE.VAR)
            {

                // ID = ARRAY
                if (m_tok.m_tokType == Token.TOKENTYPE.ID)
                {
                    // check if it exists already
                    if (lsIDs.Contains(m_tok.m_strName))
                    {
                        ErrorHandler.Error(ERROR_CODE.TYPE_REDECLARATION, m_tok.m_iLineNum, "TYPE ID already exists");
                        break;
                    }
                    // store ID in temp string
                    lsIDs.Add(m_tok.m_strName);

                    Match(Token.TOKENTYPE.ID);
                    Match(Token.TOKENTYPE.EQUAL);

                    // Determine which type being defined (Eventually refactor cases into separate functions)
                    switch (m_tok.m_tokType)
                    {
                        case Token.TOKENTYPE.ARRAY:
                            m_tok = m_Tknzr.NextToken();                // eat ARRAY
                            if (m_tok.m_tokType == Token.TOKENTYPE.LEFT_BRACK)
                            {
                                m_tok = m_Tknzr.NextToken();            // eat '['
                                // Add ARRAY bounds to intlist ->  [INT .. INT]
                                intList.Add(int.Parse(m_tok.m_strName));
                                m_tok = m_Tknzr.NextToken();
                                Match(Token.TOKENTYPE.DOT_DOT);         // ..
                                intList.Add(int.Parse(m_tok.m_strName));
                                m_tok = m_Tknzr.NextToken();

                                Match(Token.TOKENTYPE.RIGHT_BRACK);     // ]

                                Match(Token.TOKENTYPE.OF);
                                // Match type
                                switch (m_tok.m_tokType)
                                {
                                    case Token.TOKENTYPE.INTEGER:
                                        m_tok = m_Tknzr.NextToken(); // eat token
                                        break;
                                    default:
                                        ErrorHandler.Error(ERROR_CODE.INVALID_TYPE, m_tok.m_iLineNum,
                                            string.Format("Invalid type:{0}, should be INTEGER", m_tok.m_tokType));
                                        break;

                                } // ARRAY data type
                                Match(Token.TOKENTYPE.SEMI_COLON);
                            } // If LEFT_BRACK
                            // Add "template" to symbol table
                            foreach (string id in lsIDs)
                                m_SymTable.AddSymbol(new Symbol(id, 0, Symbol.SYMBOL_TYPE.TYPE_ARRAY, Symbol.STORAGE_TYPE.TYPE_INT,
                                    Symbol.PARAMETER_TYPE.LOCAL_VAR, 0, 0, intList[intList.Count - 2], intList[intList.Count - 1]));
                            break;
                        default:
                            ErrorHandler.Error(ERROR_CODE.INVALID_TYPE, m_tok.m_iLineNum, string.Format("Invalid type:{0}, should be ARRAY",
                                m_tok.m_tokType));
                            break;
                    } // switch TYPE
                } // ID = 
            } // != CONST
        } // Type()

        /// <summary>
        /// Parses the CONST section of a modula-2 file
        /// </summary>
        private void Const(int scope = 0)
        {
            Match(Token.TOKENTYPE.CONST);
            List<int> intList = new List<int>();        // const values
            List<string> lsIDs = new List<string>();    // const IDs

            // loop through until end of CONST section
            while (m_tok.m_tokType != Token.TOKENTYPE.VAR && m_tok.m_tokType != Token.TOKENTYPE.TYPE)
            {
                // ID = INT_NUM ;
                if (m_tok.m_tokType == Token.TOKENTYPE.ID)
                {
                    // check if it exists already
                    if (lsIDs.Contains(m_tok.m_strName))
                    {
                        ErrorHandler.Error(ERROR_CODE.CONST_REDECLARATION, m_tok.m_iLineNum, "CONST ID already exists");
                        break;
                    }
                    // store ID in temp string
                    lsIDs.Add(m_tok.m_strName);
                }
                Match(Token.TOKENTYPE.ID);
                Match(Token.TOKENTYPE.EQUAL);

                // CONST value
                if (m_tok.m_tokType == Token.TOKENTYPE.INT_NUM)
                    intList.Add(int.Parse(m_tok.m_strName));

                Match(Token.TOKENTYPE.INT_NUM);
                Match(Token.TOKENTYPE.SEMI_COLON);
            }

            // add static symbols to table
            for (int i = 0; i < lsIDs.Count; ++i)
            {
                m_SymTable.AddSymbol(new Symbol(lsIDs[i], scope, Symbol.SYMBOL_TYPE.TYPE_CONST, Symbol.STORAGE_TYPE.TYPE_INT,
                    Symbol.PARAMETER_TYPE.LOCAL_VAR, 0, intList[i]));
            }
        } // Const()

        /// <summary>
        /// Parses:
        /// ID : TYPE
        /// and adds the appropriate symbols to the table at scope.
        /// NOTE: m_tok is now the token after TYPE
        /// </summary>
        /// <param name="scope"></param>
        private void VarDef(int scope = 0, int baseoffset = -1, Symbol.PARAMETER_TYPE Paramater_Type = Symbol.PARAMETER_TYPE.LOCAL_VAR)
        {
            List<string> lsIDs = new List<string>();
            
            // stack offset to access variable
            int iOffset;
            // check if using local or global offset
            iOffset = (baseoffset < 0) ? m_iOff : baseoffset;

            // loop through all var definitions (ex. i, k, j : INTEGER)
            while (m_tok.m_tokType != Token.TOKENTYPE.COLON)
            {
                // check for identifier
                if (m_tok.m_tokType == Token.TOKENTYPE.ID)
                {
                    // check if it exists
                    if (lsIDs.Contains(m_tok.m_strName))
                    {
                        ErrorHandler.Error(ERROR_CODE.VAR_REDECLARATION, m_tok.m_iLineNum, "VAR ID already exists");
                        break;
                    }
                    // store ID in temp string
                    lsIDs.Add(m_tok.m_strName);
                } // If ID

                // consume ID token
                Match(Token.TOKENTYPE.ID);

                // check if last VAR to be declared
                if (m_tok.m_tokType == Token.TOKENTYPE.COMMA)
                    // consume token
                    m_tok = m_Tknzr.NextToken();
            } // while not COLON

            Match(Token.TOKENTYPE.COLON);
            // add symbol for each var to symbol table
            // - probably not the best way to implement the loop and switch
            foreach (string s in lsIDs)
            {
                switch (m_tok.m_tokType)
                {
                    case Token.TOKENTYPE.INTEGER:
                        // Add symbol
                        m_SymTable.AddSymbol(new Symbol(s, scope, Symbol.SYMBOL_TYPE.TYPE_SIMPLE, Symbol.STORAGE_TYPE.TYPE_INT,
                      Paramater_Type, iOffset, 0));
                        iOffset += 4; // inc offset by 32bits
                        break;
                    case Token.TOKENTYPE.CARDINAL:
                        // not implemented
                        break;
                    case Token.TOKENTYPE.REAL:
                        // not implemented
                        break;
                    default:
                        // Look for user defined type in symbol type
                        Symbol sym = m_SymTable.FindSymbol(m_tok.m_strName);
                        if (sym == null)
                            ErrorHandler.Error(ERROR_CODE.TOKEN_INVALID, m_tok.m_iLineNum,
                                "Expecting INTEGER, CARDINAL, or REAL token");
                        else
						// ARRAY
                            m_SymTable.AddSymbol(new Symbol(s, scope, Symbol.SYMBOL_TYPE.TYPE_ARRAY, Symbol.STORAGE_TYPE.TYPE_INT,
                                Symbol.PARAMETER_TYPE.LOCAL_VAR, iOffset, 0, sym.BaseOffset, sym.ArrayEnd));
                        iOffset += (sym.ArrayEnd - sym.BaseOffset) * 4; // memory offset of array-> sizeof(int)*(EndIndex - BeginIndex)
                        break;
                } // switch
            } // foreach

            // update m_iOff if using global scope
            if (baseoffset < 0) m_iOff = iOffset;

            // consume token
            m_tok = m_Tknzr.NextToken();
        } // VarDef

        /// <summary>
        /// Parses the var section of a modula-2 file
        /// </summary>
        private void Vars(int scope = 0, int baseoffset = -1)
        {
            // VAR i : INTEGER;
            Match(Token.TOKENTYPE.VAR);
            // Master loop to look for multiple line definitions
            while (m_tok.m_tokType != Token.TOKENTYPE.BEGIN && m_tok.m_tokType != Token.TOKENTYPE.VAR
                && m_tok.m_tokType != Token.TOKENTYPE.PROCEDURE)
            {
                VarDef(scope, baseoffset);
                Match(Token.TOKENTYPE.SEMI_COLON);
            } // While != BEGIN
        } // Var()

        private void ProcFunction(int callingscope, string procname)
        {
            // *********************
            // ***** PROCEDURE ***** 
            // skip BEGIN
            m_tok = m_Tknzr.NextToken();

            // Find PROCEDURE symbol in one below top scope
            Scope scpActive = m_SymTable.GetCurrentScope();
            Scope scpCalling = m_SymTable.GetScope(callingscope);
            Symbol symProc = null;
            foreach (Symbol sym in scpCalling.Symbols)
                if (sym.Name == procname)
                {
                    symProc = sym;
                    break;
                } // get procedure symbol
            // error
            if (symProc == null)
                ErrorHandler.Error(ERROR_CODE.SYMBOL_UNDEFINED, m_tok.m_iLineNum,
                    "Expecting PROCEDURE name");

            // procedure header
            if (c_bParserASMDebug) m_Em.asm(string.Format(";======= PROCEDURE: {0} =======", symProc.Name));

            // Print jump label
            m_Em.Label(symProc.Name);

            // Prep procedure stack, save old stack info
            m_Em.pushReg("BP");         // save old stack base
            m_Em.movReg("BP", "SP");    // stack pointer now points to "bottom" of procedure stack

            // Make room for local variables
            // Get number of local variables
            if (c_bParserASMDebug) m_Em.asm("; Make room for local variables ;;;;;;;;");
            int iArg = 0;
            foreach (Symbol sym in scpActive.Symbols)
                if (sym.ParamType == Symbol.PARAMETER_TYPE.LOCAL_VAR ||
                    sym.ParamType == Symbol.PARAMETER_TYPE.REF_PARM) ++iArg;

            // space for local variables
            m_Em.Sub("SP", (4 + (4 * (iArg))).ToString());

            if (c_bParserASMDebug) m_Em.asm("; Function Body ;;;;;;;;");
            // Parse/Emit until END
            ParseLoop();

            if (c_bParserASMDebug) m_Em.asm("; Epilogue  ;;;;;;;;");
            // Deallocate variables (readjust stack)
            m_Em.movReg("SP", "BP");

            // Restore old basepointer
            m_Em.popReg("BP");

            // Print return instruction
            m_Em.asm("ret     ; Return to calling code");

            // procedure footer
            if (c_bParserASMDebug) m_Em.asm(string.Format(";======= END PROCEDURE: {0} =======", symProc.Name));

            // match end
            Match(Token.TOKENTYPE.END);
            Match(Token.TOKENTYPE.ID);
            Match(Token.TOKENTYPE.SEMI_COLON);

            // remove active scope, set next active
            m_SymTable.RemoveScope();

        } // ProcFunction()

        /// <summary>
        /// Parses the the function body of a modula-2 file, including PROCEDURE's
        /// </summary>
        private void Function()
        {
            // Check for BEGIN token
            if (m_tok.m_tokType == Token.TOKENTYPE.BEGIN)
            {
                Match(Token.TOKENTYPE.BEGIN);

                // Label to jump to upon first run
                m_Em.Label("__MAIN");

                // Enter main procedure
                m_Em.EnterProc();

                // Make room for local variables
                if (m_iOff > 4) m_Em.Sub("SP", (m_iOff-4).ToString());

                // parse content
                ParseLoop();

                // Restore stack
                if (m_iOff > 4) m_Em.Add("SP", (m_iOff-4).ToString());

                // match end
                Match(Token.TOKENTYPE.END);
                Match(Token.TOKENTYPE.ID);
                Match(Token.TOKENTYPE.DOT);

                // Exit main procedure
                m_Em.ExitProc();
            } // if BEGIN
        } // Function

        /// <summary>
        /// Loops and parses keywords of functions
        /// </summary>
        private void ParseLoop(Token.TOKENTYPE endType = Token.TOKENTYPE.END)
        {
            bool runloop = true;
            // While not endType and no error
            while (m_tok.m_tokType != endType && ErrorHandler.LastError == (ERROR_CODE)0
                && runloop)
            {
                switch (m_tok.m_tokType)
                {
                    case Token.TOKENTYPE.WRSTR:
                        WRSTR();
                        break;
                    case Token.TOKENTYPE.WRLN:
                        WRLN();
                        break;
                    case Token.TOKENTYPE.WRINT:
                        WRINT();
                        break;
                    case Token.TOKENTYPE.ID:
                        ID();
                        break;
                    case Token.TOKENTYPE.IF:
                        IF();
                        break;
                    case Token.TOKENTYPE.LOOP:
                        LOOP();
                        break;
                    case Token.TOKENTYPE.EXIT:
                        m_tok = m_Tknzr.NextToken(); // consume token
                        goto case Token.TOKENTYPE.END;
                    case Token.TOKENTYPE.ELSE:
                    case Token.TOKENTYPE.END:
                        runloop = false;
                        break;   // exit
                    default:
                        // Invalid token
                        ErrorHandler.Error(ERROR_CODE.TOKEN_INVALID, m_tok.m_iLineNum,
                            string.Format("Token {0} invalid\r\nExpected a proper keyword",
                            m_tok.m_tokType));
                        break;
                } // switch
            } // while
        } // Function()

        /// <summary>
        /// True has been returned if tokType matches the current token, false if not.
        /// One token has been consumed.
        /// </summary>
        /// <param name="tokType"></param>
        /// <returns></returns>
        private bool Match(Token.TOKENTYPE tokType)
        {
            bool b = (m_tok.m_tokType == tokType);
            // Throw error if not expected token
            if (!b) ErrorHandler.Error(ERROR_CODE.TOKEN_INVALID, m_tok.m_iLineNum,
                string.Format("Expected token: {0}", tokType.ToString()));
            m_tok = m_Tknzr.NextToken();
            return b;
        } // Match

        // *********************************************************************************
        // *********************************************************************************
        //              K E Y W O R D     H E L P E R    F U N C T I O N S
        // *********************************************************************************

        /// <summary>
        /// Parse and emit WRSTR modula keyword
        /// ex. WRSTR("string contents");
        /// </summary>
        private void WRSTR()
        {
            // consume token
            m_tok = m_Tknzr.NextToken();
            Match(Token.TOKENTYPE.LEFT_PAREN);
            // Check for parse error
            if (m_tok.m_tokType != Token.TOKENTYPE.STRING) ErrorHandler.Error(ERROR_CODE.TOKEN_INVALID,
                m_tok.m_iLineNum, "Expected STRING token");
            m_Em.WRSTR(m_tok.m_strName);
            Match(Token.TOKENTYPE.STRING);
            Match(Token.TOKENTYPE.RIGHT_PAREN);
            Match(Token.TOKENTYPE.SEMI_COLON);
        } // WRSTR()

        /// <summary>
        /// Parse and emit WRLN modula keyword
        /// ex. WRLN;
        /// </summary>
        private void WRLN()
        {
            // consume token
            m_tok = m_Tknzr.NextToken();
            m_Em.WRLN();
            Match(Token.TOKENTYPE.SEMI_COLON);
        } // WRLN()

        /// <summary>
        /// Parse and emit WRINT module keyword
        /// ex. WRINT(42);
        /// </summary>
        private void WRINT()
        {
            // consume token
            m_tok = m_Tknzr.NextToken();
            // eat '('
            Match(Token.TOKENTYPE.LEFT_PAREN);

            // evaluate expression
            //  consumes RIGHT_PAREN and SEMICOLON tokens
            List<Token> exp = new List<Token>();
            for (; m_tok.m_tokType != Token.TOKENTYPE.SEMI_COLON; m_tok = m_Tknzr.NextToken())
                exp.Add(m_tok);
            exp.RemoveAt(exp.Count - 1); // remove ')' in WRINT(exp)

            // Get value of INT
            EvalPostFix(InFixToPostFix(exp));

            // write to console
            // - int to print should be in EAX
            m_Em.WRINT(); // prints what is in EAX

            Match(Token.TOKENTYPE.SEMI_COLON);

            // consume token
            //m_tok = m_Tknzr.NextToken();
        } // WRINT()

        /// <summary>
        /// Parse Assignment function
        /// ex. ID := Expression;
        /// </summary>
        private void ID()
        {
            // temporary Symbol
            Symbol sym;

            // lookup in symbol table
            // check all scopes first
            sym = FindSymbol(m_tok.m_strName);

            switch (sym.SymType)
            {
                case Symbol.SYMBOL_TYPE.TYPE_CONST:
                    ErrorHandler.Error(ERROR_CODE.CONST_REDEFINITION, m_tok.m_iLineNum, "Cannot redefine a CONST value");
                    return; // get out of function
                case Symbol.SYMBOL_TYPE.TYPE_REFPROC:
                case Symbol.SYMBOL_TYPE.TYPE_PROC:
                    // ***** PROCEDURE ***** //
                    // Parse argument list, pushing each value onto the stack
                    // little ( k ) ;
                    int iArgCount = 0;
                    m_tok = m_Tknzr.NextToken();
                    Match(Token.TOKENTYPE.LEFT_PAREN);

                    // Parse arguments and prep for procedure call
                    List<List<Token>> lslsArgs = new List<List<Token>>(); // hold the arguments, list of token (expression) lists
                    for (; m_tok.m_tokType != Token.TOKENTYPE.SEMI_COLON; m_tok = m_Tknzr.NextToken())
                    {
                        // parse argument into EAX
                        List<Token> lsArg = new List<Token>();
                        for (; m_tok.m_tokType != Token.TOKENTYPE.COMMA &&
                            m_tok.m_tokType != Token.TOKENTYPE.RIGHT_PAREN; m_tok = m_Tknzr.NextToken())
                            lsArg.Add(m_tok);
                        lslsArgs.Add(lsArg); // add argument expression to list of arg expressions
                        ++iArgCount; // used when restoring the stack
                    } // Get arguments
                    // Must push arguments in reverse order
                    for (int i = iArgCount; i > 0; --i)
                    {
                        // if proc is reference then push just BP-Off not [BP-Off]
                        if (sym.SymType == Symbol.SYMBOL_TYPE.TYPE_REFPROC)
                        {
                            Symbol syRef = FindSymbol(lslsArgs[i-1][0].m_strName);
                            m_Em.asm("  movzx EAX, BP     ; mov zero extend");
                            m_Em.Sub("EAX", syRef.Offset.ToString());
                            m_Em.pushReg("EAX"); 		   // final address of variable
                            continue;
                        }
                        // Get value into EAX then push onto stack
                        EvalPostFix(InFixToPostFix(lslsArgs[i-1]));
                        m_Em.pushReg("EAX");
                    } // Push parameters for function

                    Match(Token.TOKENTYPE.SEMI_COLON);

                    // Call function
                    m_Em.asm(string.Format("call     {0}     ; PROCEDURE {0}", sym.Name));

                    // restore stack
                    //for (int i = 0; i < iArgCount; ++i) m_Em.popInt();
                    m_Em.Add("SP", (4 * iArgCount).ToString());
                    return; // no need to go through rest of function, exit
                default:
                    break;
            } // SymType 

            // consume token
            m_tok = m_Tknzr.NextToken(); // ID
            //m_Em.asm(";;;; ID -- ARRAY");
            // Check if ARRAY, get proper offset
            if (m_tok.m_tokType == Token.TOKENTYPE.LEFT_BRACK)
            {
                // ***** ARRAY  ID() ***** //
                if (c_bParserASMDebug) m_Em.asm(";;;; ARRAY Assignment ;;;;");
                m_tok = m_Tknzr.NextToken();
                List<Token> lsIndex = new List<Token>();
                
                // get index expression (contents between [ ] )
                for (; m_tok.m_tokType != Token.TOKENTYPE.RIGHT_BRACK; m_tok = m_Tknzr.NextToken())
                    lsIndex.Add(m_tok);

                // Get value for the proper index
                // Offset for BP:
                // BaseOffset + (ArrayEnd - Index) * IntSize
                
                //m_Em.GetVar(symNdx.Offset);                     // get index value
                EvalPostFix(InFixToPostFix(lsIndex));
                m_Em.asm(";//// Array Assign. Index ^ Offset Calculation below: "); // trace
                m_Em.movReg("EBX", "EAX"); 	                      // put index in ebx
                m_Em.movReg("EAX", sym.ArrayEnd.ToString());      // Array end in eax
                m_Em.Sub("EAX", "EBX"); 				          // get proper offset-> ArrEnd - Index
                m_Em.iMul("EAX", "4");					          // multiply by int size
                m_Em.movReg("EBX", sym.Offset.ToString());        // EBX = base
                m_Em.Add("EAX", "EBX");			                  // compute final offset

                // calcuate address from calculated offset
                m_Em.asm(";; Store address of var in EAX"); // trace
                m_Em.asm("  movzx ECX, BP     ; mov zero extend");
                m_Em.Sub("ECX", "EAX");
                m_Em.pushReg("ECX");                              // push offset 

                m_Em.asm(";//// value assigned to Array");

                // Now push BP-EAX
                //m_Em.movReg("DI", "AX");                          // offset into Destination Index register
                //m_Em.pushReg("[BP+DI]");                          // put value at index onto stack

               // m_tok = m_Tknzr.NextToken();
                Match(Token.TOKENTYPE.RIGHT_BRACK);
            } // ARRAY
            else // type INT
            {
                // PUSH STACK ADDRESS OF VARIABLE
                // check the paramater type
                switch (sym.ParamType)
                {
                    case Symbol.PARAMETER_TYPE.LOCAL_VAR:
                        // push address of variable on the stack -> BP-Offset
                        m_Em.asm("  movzx EAX, BP     ; mov zero extend");
                        m_Em.Sub("EAX", sym.Offset.ToString());
                        m_Em.pushReg("EAX");  // stack address of variable
                        break;
                    case Symbol.PARAMETER_TYPE.REF_PARM:
                        // Address for REF_PARM is at BP+offset or [BP+offset]
                        //m_Em.movReg("EBX", string.Format("[BP+{0}]", sym.Offset)); // get address passed into function
                        m_Em.asm(";** Address of REF_PARM - ID()"); // trace
                        m_Em.asm("  movzx EBX, BP     ; mov zero extend");
                        m_Em.Add("EBX", sym.Offset.ToString()); // 
                        m_Em.movReg("EAX", "[EBX]");   // 
                        m_Em.pushReg("EAX"); 		   // final address of variable
                        break;
                    case Symbol.PARAMETER_TYPE.VAL_PARM:
                    // not implemented
                    default:
                        break;
                }
            }

            Match(Token.TOKENTYPE.ASSIGN); // :=
            if (m_tok.m_tokType == Token.TOKENTYPE.RDINT)
            {
                m_Em.RDINT();                  // get int from user
                m_tok = m_Tknzr.NextToken();
                Match(Token.TOKENTYPE.LEFT_PAREN);
                Match(Token.TOKENTYPE.RIGHT_PAREN);
            } // RDINT
            else
                EvalPostFix(InFixToPostFix()); // evaluate expression, emit asm in process

            // Value to be stored should be in EAX
            //  store the value at proper place:
            //m_Em.pushReg("EAX"); // value is in EAX push onto stack
            //m_Em.movReg("EBX", "EAX");       // value to be stored into EBX   
            m_Em.popReg("ECX");                   // get address from stack
            //m_Em.movReg("DI", "AX");         // mov offset to Destination Index register
            if (m_SymTable.ActiveScope > 0)
                switch (sym.ParamType)
                {
                    case Symbol.PARAMETER_TYPE.LOCAL_VAR:
                        m_Em.movReg(string.Format("[BP-{0}]", sym.Offset + 4), "EAX");   // assign value to BP+OFFSET
                        break;
                    case Symbol.PARAMETER_TYPE.VAL_PARM:
                        m_Em.movReg("[ECX]", "EAX");   // assign value to BP+OFFSET
                        break;
                    case Symbol.PARAMETER_TYPE.REF_PARM:
                        m_Em.asm(";** Store value in REF_PARM - ID()"); // trace
                        // mov EBX, [BP+offset]
                        // mov EAX, [EBX]
                        //  
                       //m_Em.movReg("ECX", "[BP+DI]"); // load absolute address of var into ECX
                        m_Em.movReg("[ECX]", "EAX");    // store address saved at [ECX]
                        //m_Em.movReg("EAX", "[EBX]");    // store value at address pointed to by [ECX]
                        break;
                    default:
                        break;
                }
            else
            {
                //m_Em.asm(";; Store value at location pointed to by ECX"); // trace
                m_Em.movReg("[ECX]", "EAX");
                //m_Em.movReg("[BP+DI]", "EBX");   // assign value to BP+OFFSET
            }
            //if (c_bParserASMDebug) m_Em.asm(";;;; END ID -- ARRAY");
            Match(Token.TOKENTYPE.SEMI_COLON);
        } // ID()

        /// <summary>
        /// Parse IF statement
        /// </summary>
        private void IF()
        {
            if (c_bParserASMDebug) m_Em.asm(";;;;;; IF statement begin ;;;;;;");
            // labels
            string sIfLabel = m_LABEL_BASE + m_iLabelLevel++;
            string sElseLabel = m_LABEL_BASE + m_iLabelLevel++;
            string sEndLabel = m_LABEL_BASE + m_iLabelLevel++;

            // consume token (IF)
            m_tok = m_Tknzr.NextToken();

            List<Token> inFix = new List<Token>();
            for (; m_tok.m_tokType != Token.TOKENTYPE.THEN; m_tok = m_Tknzr.NextToken())
                inFix.Add(m_tok);

            // eval conditional expression
            EvalPostFix(InFixToPostFix(inFix));

            // if EAX is one then statement is true and jump to IF
            m_Em.Compare("EAX", "1");
            m_Em.Jump("je", sIfLabel);

            // Force jump to else
            m_Em.Jump("jmp", sElseLabel);

            // consume token (THEN)
            m_tok = m_Tknzr.NextToken();

            ///// run code in IF section
            if (c_bParserASMDebug) m_Em.asm(";;; IF");
            m_Em.Label(sIfLabel);            // IF Label
            ParseLoop(); // Parse inside of IF until ELSE or END is reached  
            m_Em.Jump("jmp", sEndLabel);     // jump around else code

            // Else label, put even if no ELSE token
            m_Em.Label(sElseLabel);          // ELSE Label
            if (m_tok.m_tokType == Token.TOKENTYPE.ELSE) // if END then there is no ELSE section
            {
                // consume token (ELSE)
                m_tok = m_Tknzr.NextToken();

                //// else code
                if (c_bParserASMDebug) m_Em.asm(";;; ELSE");
                ParseLoop(Token.TOKENTYPE.ELSE); // Parse inside of ELSE until END token
            } // ELSE
            m_Em.Label(sEndLabel);          // END Label
            // consume remaining "END;"
            Match(Token.TOKENTYPE.END);
            Match(Token.TOKENTYPE.SEMI_COLON);

            if (c_bParserASMDebug) m_Em.asm(";;;;;; end IF statement ;;;;;;");
        } // IF()

        /// <summary>
        /// Parse Loop statement
        /// </summary>
        private void LOOP()
        {
            if (c_bParserASMDebug) m_Em.asm(";;;;;; LOOP statement begin ;;;;;;");
            // Labels
            string sBeginLabel = m_LABEL_BASE + m_iLabelLevel++;
            string sEndLabel = m_LABEL_BASE + m_iLabelLevel++;

            // consume LOOP token
            m_tok = m_Tknzr.NextToken();

            // Beginning of loop
            m_Em.Label(sBeginLabel);

            // Parse until IF
            ParseLoop(Token.TOKENTYPE.IF);

            // IF
            Match(Token.TOKENTYPE.IF);

            List<Token> inFix = new List<Token>();
            for (; m_tok.m_tokType != Token.TOKENTYPE.THEN; m_tok = m_Tknzr.NextToken())
                inFix.Add(m_tok);

            //// LOOP instantiation
            // Match tokens
            Match(Token.TOKENTYPE.THEN);

            // parse after condition
            ParseLoop();

            Match(Token.TOKENTYPE.SEMI_COLON);
            Match(Token.TOKENTYPE.END);
            Match(Token.TOKENTYPE.SEMI_COLON);


            // eval conditional expression
            EvalPostFix(InFixToPostFix(inFix));
            // if EAX is one then statement is true and jump to IF
            m_Em.Compare("EAX", "1");
            m_Em.Jump("je", sEndLabel);

            //// evaluate inside loop
            ParseLoop();

            //m_Em.RDINT(); // trace

            // jump to top of loop
            m_Em.Jump("jmp", sBeginLabel);

            // label if condition fails jump over
            m_Em.Label(sEndLabel);

            // consume remaining "END;"
            Match(Token.TOKENTYPE.END);
            Match(Token.TOKENTYPE.SEMI_COLON);

            if (c_bParserASMDebug) m_Em.asm(";;;;;; end LOOP statement ;;;;;;");
        } // LOOP()

        /// <summary>
        /// Parse tokens from infix to postfix, including conditionals and logical operators
        /// </summary>
        private List<Token> InFixToPostFix(List<Token> tokList = null)
        {
            // InFix to PostFix
            Stack<Token> stkInToPost = new Stack<Token>(); // infix to postfix stack
            List<Token> lsExpression = new List<Token>();  // get the whole expression as seperate list
            List<Token> lsPostFix = new List<Token>();     // postfix statement
            Token tok; // temp

            // get expression into list
            // Use passed in list if possible
            if (tokList == null)
                for (; m_tok.m_tokType != Token.TOKENTYPE.SEMI_COLON; m_tok = m_Tknzr.NextToken())
                    lsExpression.Add(m_tok);
            else
                lsExpression = tokList;

            // loop through expression
            for (int j = 0; j < lsExpression.Count; ++j)
            {
                switch (lsExpression[j].m_tokType)
                {
                    case Token.TOKENTYPE.RIGHT_PAREN:
                        // Pop elements until '(' is found
                        if (stkInToPost.Count > 0)
                        {
                            tok = stkInToPost.Pop();
                            while (stkInToPost.Count > 0 && tok.m_tokType != Token.TOKENTYPE.LEFT_PAREN)
                            {
                                lsPostFix.Add(tok);
                                tok = stkInToPost.Pop();
                            } // while not '('
                        } // if stack no empty
                        break;
                    //** HIGHEST PRECEDENCE
                    case Token.TOKENTYPE.MOD:
                    case Token.TOKENTYPE.MULT:
                    case Token.TOKENTYPE.DIV:
                    case Token.TOKENTYPE.LEFT_PAREN:
                        // Highest precedence, push onto stack
                        if (!checkNeg(ref lsPostFix, ref lsExpression, ref j))
                            stkInToPost.Push(lsExpression[j]); // push onto stack
                        break;
                    //** MIDDLE PRECEDENCE
                    case Token.TOKENTYPE.AND:
                    case Token.TOKENTYPE.OR:
                    case Token.TOKENTYPE.NOT:
                    case Token.TOKENTYPE.EQUAL:
                    case Token.TOKENTYPE.NOT_EQ:
                    case Token.TOKENTYPE.GRTR_THAN:
                    case Token.TOKENTYPE.GRTR_THAN_EQ:
                    case Token.TOKENTYPE.LESS_THAN:
                    case Token.TOKENTYPE.LESS_THAN_EQ:
                        // if previous token was operator then check if number should be negative
                        if (!checkNeg(ref lsPostFix, ref lsExpression, ref j))
                        {
                            if (stkInToPost.Count > 0)
                            {
                                tok = stkInToPost.Peek();
                                // check top of stack and compare precedence
                                while (true)
                                {
                                    // left paren on top is like having an empty stack
                                    if (tok.m_tokType == Token.TOKENTYPE.LEFT_PAREN)
                                    {
                                        stkInToPost.Push(lsExpression[j]); // push onto stack
                                        break;
                                    } // if '('
                                    // Current lower / top higher
                                    if (tok.m_tokType == Token.TOKENTYPE.MULT || tok.m_tokType == Token.TOKENTYPE.DIV
                                        || tok.m_tokType == Token.TOKENTYPE.MOD)
                                    {
                                        tok = stkInToPost.Pop();
                                        lsPostFix.Add(tok);
                                        if (stkInToPost.Count > 0)
                                            tok = stkInToPost.Peek();
                                        else // stack empty push
                                        {
                                            stkInToPost.Push(lsExpression[j]); // push onto stack
                                            break;
                                        } // else
                                        continue; // not needed, but kept for readability
                                    } // Higher
                                    // equal
                                    if (InfixPlusMinusCondition(tok.m_tokType))
                                    {
                                        tok = stkInToPost.Pop();
                                        lsPostFix.Add(tok);
                                        stkInToPost.Push(lsExpression[j]); // push onto stack
                                        break;
                                    } // Equal
                                    // Current higher /  top lower
                                    if (tok.m_tokType == Token.TOKENTYPE.PLUS || tok.m_tokType == Token.TOKENTYPE.MINUS)
                                    {
                                        stkInToPost.Push(lsExpression[j]); // push onto stack
                                        break;
                                    } // Lower
                                } // while
                            } // if stack not empty
                            else // stack empty
                                stkInToPost.Push(lsExpression[j]); // push onto stack
                        } // if not neg
                        break;
                    //** LOWEST PRECEDENCE
                    case Token.TOKENTYPE.PLUS:
                    case Token.TOKENTYPE.MINUS:
                        // if previous token was operator then check if number should be negative
                        if (!checkNeg(ref lsPostFix, ref lsExpression, ref j))
                        {
                            if (stkInToPost.Count > 0)
                            {
                                tok = stkInToPost.Peek();
                                // check top of stack and compare precedence
                                while (true)
                                {
                                    // left paren on top is like having an empty stack
                                    if (tok.m_tokType == Token.TOKENTYPE.LEFT_PAREN)
                                    {
                                        stkInToPost.Push(lsExpression[j]); // push onto stack
                                        break;
                                    }
                                    // Current lower / top higher
                                    if (InfixPlusMinusCondition(tok.m_tokType) || tok.m_tokType == Token.TOKENTYPE.MULT
                                        || tok.m_tokType == Token.TOKENTYPE.DIV || tok.m_tokType == Token.TOKENTYPE.MOD)
                                    {
                                        tok = stkInToPost.Pop();
                                        lsPostFix.Add(tok);
                                        // check stack
                                        if (stkInToPost.Count > 0)
                                            tok = stkInToPost.Peek();
                                        else // stack empty push
                                        {
                                            stkInToPost.Push(lsExpression[j]); // push onto stack
                                            break;
                                        } // else
                                        continue; // not needed, but kept for readability
                                    } // Top higher
                                    // equal
                                    if (tok.m_tokType == Token.TOKENTYPE.PLUS || tok.m_tokType == Token.TOKENTYPE.MINUS)
                                    {
                                        tok = stkInToPost.Pop();
                                        lsPostFix.Add(tok);
                                        stkInToPost.Push(lsExpression[j]); // push onto stack
                                        break;
                                    } // Equal / Lower
                                } // while 
                            } // if stack not empty
                            else // stack empty
                                stkInToPost.Push(lsExpression[j]); // push onto stack
                        } // if not neg
                        break;
                    case Token.TOKENTYPE.ID:
                        
                        // Check if ARRAY
                        if (j + 1 < lsExpression.Count && lsExpression[j + 1].m_tokType /*m_Tknzr.PeekToken().m_tokType*/
                            == Token.TOKENTYPE.LEFT_BRACK)
                        {
                            lsPostFix.Add(lsExpression[j]);          // add ID
                            ++j;                                     // only skip over ID, [ is current token
                            lsPostFix.Add(lsExpression[j]);          // add [
                            ++j;                                     // move to next

                            // Add array as ID [ postfix index exp ]
                            // ex:          list[23*]
                            List<Token> ltTemp = new List<Token>();
                            // Add everything in brackets and convert to postfix
                            for (; lsExpression[j].m_tokType != Token.TOKENTYPE.RIGHT_BRACK; ++j)
                                ltTemp.Add(lsExpression[j]);
                            lsPostFix.AddRange(InFixToPostFix(ltTemp)); // add post fix expression
                            lsPostFix.Add(lsExpression[j]);             // add ] 
                            break;
                        }
                        goto case Token.TOKENTYPE.INT_NUM;
                    case Token.TOKENTYPE.INT_NUM:
                        lsPostFix.Add(lsExpression[j]);
                        break;
                    default:
                        ErrorHandler.Error(ERROR_CODE.TOKEN_INVALID, lsExpression[j].m_iLineNum,
                            "Expected (,),+,-,*,/,MOD,INT_NUM, or ID token");
                        break;
                } // switch
            } // for all tokens in lsExpression
            while (stkInToPost.Count > 0)
                lsPostFix.Add(stkInToPost.Pop()); // get last element out
            return lsPostFix;
        }

        /// <summary>
        /// Returns true if type is one of: (, AND, OR, =, !=, >, >=, <, <=
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool InfixPlusMinusCondition(Token.TOKENTYPE type)
        {
            return (type == Token.TOKENTYPE.AND || type == Token.TOKENTYPE.OR
                || type == Token.TOKENTYPE.EQUAL || type == Token.TOKENTYPE.NOT_EQ
                || type == Token.TOKENTYPE.GRTR_THAN || type == Token.TOKENTYPE.GRTR_THAN_EQ
                || type == Token.TOKENTYPE.LESS_THAN || type == Token.TOKENTYPE.LESS_THAN_EQ);
        }

        /// <summary>
        /// Determine is the next ID or INT_NUM is negative. If determined negative, the ID or INT_NUM
        /// is added to the PostFix list followed by a NEGATE token. True has then been returned. If 
        /// determined positive, false has been returned. All arguments are passed by reference.
        /// </summary>
        /// <param name="PostFix"></param>
        /// <param name="Expression"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool checkNeg(ref List<Token> PostFix, ref List<Token> Expression, ref int pos)
        {
            // check if previous token is an operator
            if ((pos > 0))  // previous token exists         
                // previous token is not an operator                
                if (!(Expression[pos - 1].m_tokType == Token.TOKENTYPE.MINUS ||      //NOT(prev token MINUS OR
                            Expression[pos - 1].m_tokType == Token.TOKENTYPE.PLUS || // prev token PLUS  OR
                            Expression[pos - 1].m_tokType == Token.TOKENTYPE.MOD ||  // prev token MOD   OR
                            Expression[pos - 1].m_tokType == Token.TOKENTYPE.MULT || // prev token MULT  OR
                            Expression[pos - 1].m_tokType == Token.TOKENTYPE.DIV)    // prev token DIV   OR ) 
                            )
                    return false; // not negative

            if (pos == 0 && pos + 1 < Expression.Count)                 //  First AND not only
                // next token is not an integer or var
                if (!(Expression[pos + 1].m_tokType == Token.TOKENTYPE.ID ||         // NOT(next token ID       OR
                 Expression[pos + 1].m_tokType == Token.TOKENTYPE.INT_NUM))          // next token INT_Num  OR )
                    return false; // not negative

            // parenthesis
            if (Expression[pos].m_tokType == Token.TOKENTYPE.LEFT_PAREN)
                return false;

            int i = 0;
            for (; Expression[pos].m_tokType == Token.TOKENTYPE.MINUS; ++pos) ++i;
            if (i % 2 == 0)
                // even, positive
                PostFix.Add(Expression[pos]);
            else
            // odd, negative
            {
                // value first then negative token
                PostFix.Add(Expression[pos]);
                PostFix.Add(new Token(Token.TOKENTYPE.NEGATE, "neg", 01)); // add negative token
            } // else
            return true;
        } // checkNeg

        /// <summary>
        /// Evaluates postfix list of arithmetic tokens. Result is in EAX. Works with conditional and
        /// logical operators.
        /// </summary>
        /// <param name="tokens"></param>
        private void EvalPostFix(List<Token> tokens)
        {
            //m_Em.asm(";;;;;;;; EvalPostFix ;;;;;;;;");
            //foreach (Token t in tokens)
            for (int i = 0; i < tokens.Count; ++i)
            {
                switch (tokens[i].m_tokType)
                {
                    case Token.TOKENTYPE.AND:
                        m_Em.popInt();              // pop into EAX
                        m_Em.movReg("EBX", "EAX");  //
                        m_Em.popInt();
                        m_Em.And("EAX", "EBX");
                        m_Em.pushReg("EAX");
                        break;
                    case Token.TOKENTYPE.OR:
                        m_Em.popInt();              // pop into EAX
                        m_Em.movReg("EBX", "EAX");  //
                        m_Em.popInt();
                        m_Em.Or("EAX", "EBX");
                        m_Em.pushReg("EAX");
                        break;
                    case Token.TOKENTYPE.NOT:
                        // Compare last condition value to 0 and invert
                        m_Em.pushInt(0);
                        m_Em.StackCompare("je", m_ARITH_BASE, ref m_iArithmeticLevel);
                        break;
                    case Token.TOKENTYPE.EQUAL:
                        m_Em.StackCompare("je", m_ARITH_BASE, ref m_iArithmeticLevel);
                        break;
                    case Token.TOKENTYPE.NOT_EQ:
                        m_Em.StackCompare("jne", m_ARITH_BASE, ref m_iArithmeticLevel);
                        break;
                    case Token.TOKENTYPE.GRTR_THAN:
                        m_Em.StackCompare("jg", m_ARITH_BASE, ref m_iArithmeticLevel);
                        break;
                    case Token.TOKENTYPE.GRTR_THAN_EQ:
                        m_Em.StackCompare("jge", m_ARITH_BASE, ref m_iArithmeticLevel);
                        break;
                    case Token.TOKENTYPE.LESS_THAN:
                        m_Em.StackCompare("jl", m_ARITH_BASE, ref m_iArithmeticLevel);
                        break;
                    case Token.TOKENTYPE.LESS_THAN_EQ:
                        m_Em.StackCompare("jle", m_ARITH_BASE, ref m_iArithmeticLevel);
                        break;
                    case Token.TOKENTYPE.NEGATE:
                        // negate whatever is in EAX
                        m_Em.popInt();              // get last int from stack into EAX
                        m_Em.asm("neg    EAX   ; -1 * EAX"); // negate EAX
                        m_Em.pushReg("EAX");        // push back onto stack
                        break;
                    case Token.TOKENTYPE.DIV:
                        m_Em.popInt();              // EAX = op1 divisor
                        m_Em.movReg("EBX", "EAX");  // EBX = EAX, EBX = op1
                        m_Em.popInt();              // EAX = op2 dividend
                        // sign extend
                        m_Em.asm("CDQ    ; sign extend EAX to EDX:EAX, prep for idiv");
                        m_Em.iDiv("EBX");           // EDX:EAX / EBX -> EAX = op2 / op1
                        m_Em.pushReg("EAX");        // push back on stack
                        break;
                    case Token.TOKENTYPE.MULT:
                        m_Em.popInt();              // EAX = op1 
                        m_Em.movReg("EBX", "EAX");  // EBX = EAX
                        m_Em.popInt();              // EAX = op2 
                        m_Em.iMul("EAX", "EBX");    // op1 * op2 -> EAX = op2 * op1
                        m_Em.pushReg("EAX");        // push back on stack
                        break;
                    case Token.TOKENTYPE.MOD:
                        m_Em.popInt();              // EAX = op1 
                        m_Em.movReg("EBX", "EAX");  // EBX = EAX
                        m_Em.popInt();              // EAX = op2 
                        // sign extend
                        m_Em.asm("CDQ    ; sign extend EAX to EDX:EAX, prep for idiv");
                        m_Em.iDiv("EBX");           // EDX:EAX / EBX -> EAX = op2 / op1
                        m_Em.pushReg("EDX");        // push remainder on stack
                        break;
                    case Token.TOKENTYPE.PLUS:
                        m_Em.popInt();              // EAX = op1 
                        m_Em.movReg("EBX", "EAX");  // EBX = EAX
                        m_Em.popInt();              // EAX = op2 
                        m_Em.Add("EAX", "EBX");     // EAX = EAX + EBX
                        m_Em.pushReg("EAX");        // push EAX back on stack
                        break;
                    case Token.TOKENTYPE.MINUS:
                        m_Em.popInt();              // EAX = op1 
                        m_Em.movReg("EBX", "EAX");  // EBX = EAX, EBX = op2
                        m_Em.popInt();              // EAX = op2 
                        m_Em.Sub("EAX", "EBX");     // EBX = EAX - EBX --> (op2 - op1)
                        m_Em.pushReg("EAX");        // push EAX back on stack
                        break;
                    case Token.TOKENTYPE.ID:
                        // Find symbol
                        Symbol sym = m_SymTable.FindSymbol(tokens[i].m_strName);
                        // check if symbol undefined
                        if (sym == null)
                        {
                            ErrorHandler.Error(ERROR_CODE.SYMBOL_UNDEFINED, tokens[i].m_iLineNum,
                                               string.Format("Symbol: {0} undefined", sym.Name));
                            break;
                        } // if symbol is undefined
                        switch (sym.SymType)
                        {
                            case Symbol.SYMBOL_TYPE.TYPE_SIMPLE:
                                if (c_bParserASMDebug) m_Em.asm(string.Format(";;; Retrieving Variable: {0}", sym.Name));
                                // Check if inside a procedure or not
                                if (m_SymTable.ActiveScope > 0)
                                {
                                    switch (sym.ParamType)
                                    {
                                        case Symbol.PARAMETER_TYPE.LOCAL_VAR:
                                            m_Em.movReg("EAX", string.Format("[BP-{0}]", 4 + sym.Offset)); // start at 4
                                            //m_Em.GetVar(sym.Offset);
                                            m_Em.pushReg("EAX"); // push EAX
                                            break;
                                        case Symbol.PARAMETER_TYPE.VAL_PARM:
                                            //m_Em.GetVar(sym.RelativeOffset);
                                            m_Em.movReg("EAX", string.Format("[BP+{0}]", sym.Offset));
                                            m_Em.pushReg("EAX"); // push EAX
                                            break;
										case Symbol.PARAMETER_TYPE.REF_PARM:
                                            m_Em.asm(";** Get value of REF_PARM - EvalPostFix()"); // trace
                                            // address of ref var based upon current stack implementation:
											// little ( j ) ; Implementation:					  
											// mov EAX, BP+j.offset
											// push EAX ; abs address of j
											//
											// j := 12; Implementation:
											// mov EAX, [BP+refPar.Offset] ; EAX = absAddress of j
											// mov [EAX], 12
											//
                                            // 00 
                                            // 04 
                                            // 08 NEW BP //
                                            // 0C param c
											// 10 offset j / param b (4)
											// 14 param a
											// 18 
											// 1C j
                                            // 20 OLD BP // i = [OLD BP+8]

                                            // Address of reference var is stored like a local var
                                            //  where instead of the value of the variable being stored
                                            //  at BP+off it is the address
                                            m_Em.movReg("EBX", string.Format("[BP+{0}]", sym.Offset));
                                            m_Em.movReg("EAX", "[EBX]");  // store value at absolute address in EAX
                                            m_Em.pushReg("EAX"); 		  // push value 
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else // not in a procedure, base scope
                                {
                                    switch (sym.ParamType)
                                    {
                                        case Symbol.PARAMETER_TYPE.LOCAL_VAR:
                                            m_Em.movReg("EAX", string.Format("[BP-{0}]", sym.Offset));
                                            m_Em.pushReg("EAX"); // push EAX
                                            break;
                                        case Symbol.PARAMETER_TYPE.REF_PARM:
                                            m_Em.asm(";*** Address for REF_PARM - EvalPostFix()");
                                            m_Em.asm("  movzx EBX, BP     ; mov zero extend");
                                            m_Em.Add("EBX", sym.Offset.ToString()); 
                                            m_Em.movReg("EAX", "[EBX]");    
                                            m_Em.pushReg("EAX"); 		   // final address of variable
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Symbol.SYMBOL_TYPE.TYPE_CONST:
                                m_Em.pushInt(sym.Value);
                                break;
                            case Symbol.SYMBOL_TYPE.TYPE_ARRAY:
                                // Postfix should be such as ARRAY[postfix exp] so 1+list[2*3] => 1list[23*]+
                                Symbol symNdx = m_SymTable.FindSymbol(tokens[i+2].m_strName); // get index symbol
                                // Get value from array at proper index
                                // Offset for BP:
                                // BaseOffset + (ArrayEnd - Index) * sizeof(int)
                                if (c_bParserASMDebug) m_Em.asm(string.Format("; Retrieving ARRAY value: {0}[{1}]", sym.Name, symNdx.Name));
                                //m_Em.movReg("EBX", string.Format("[BP-{0}]", symNdx.Offset));

                                // Get index referenced from postfix tokens
                                //  evaluate expression 
                                i+=2;        // move over [
                                List<Token> ltIndex = new List<Token>();
                                for (; tokens[i].m_tokType != Token.TOKENTYPE.RIGHT_BRACK; ++i)
                                    ltIndex.Add(tokens[i]);
                                if (ltIndex.Count == 1)
                                    m_Em.movReg("EBX", string.Format("[BP-{0}]", symNdx.Offset));
                                else
                                {
                                    EvalPostFix(ltIndex);
                                    m_Em.movReg("EBX", "EAX");
                                }

                                m_Em.movReg("EAX", sym.ArrayEnd.ToString());      // Array end in eax
                                m_Em.Sub("EAX", "EBX"); 				          // get proper offset-> ArrEnd - Index
                                m_Em.iMul("EAX", "4");					          // multiply by int size
                                m_Em.movReg("EBX", sym.Offset.ToString());        // EBX = base	
                                m_Em.Add("EAX", "EBX");			                  // compute final offset  

                       
                                // calcuate address from calculated offset
                                m_Em.asm("  movzx ECX, BP     ; mov zero extend");
                                m_Em.Sub("ECX", "EAX");                           // address of array element
                                m_Em.movReg("EAX", "[ECX]");                      // get value
                                m_Em.pushReg("EAX");      
                                //++i;						                      // increment counter (count over index)
                                if (c_bParserASMDebug) m_Em.asm(";;;; END EVAL ARRAY");
                                break;
                            default:
                                break;
                        } // switch
                        break;
                    case Token.TOKENTYPE.INT_NUM:
                        m_Em.pushInt(int.Parse(tokens[i].m_strName)); // push INT_NUM value
                        break;
                    default:
                        ErrorHandler.Error(ERROR_CODE.TOKEN_INVALID, tokens[i].m_iLineNum,
                                        "Expected (,),+,-,*,/,MOD,INT_NUM, or ID token");
                        return; // get out of here!
                } // switch (t.type)
            } // foreach Token t
            // Pop off last element into EAX
            m_Em.popInt();
            //m_Em.asm(";;;;;;;; END EvalPostFix ;;;;;;;;");
        } // EvalPostFix

        /// <summary>
        /// Returns Symbol associated with string name. If not found null has been returned.
        /// Performs error handling.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Symbol FindSymbol(string name)
        {
            Symbol sym = m_SymTable.FindSymbol(name);
            if (sym == null) // not in any scope
            {
                // error, undefined variable
                ErrorHandler.Error(ERROR_CODE.SYMBOL_UNDEFINED, m_tok.m_iLineNum,
                    string.Format("Symbol: {0} is undefined in all scopes", m_tok.ToString()));
            } // if sym == null
            // check in current scope
            if (m_SymTable.FindInScope(name) == null)
            {
                // error, undefined variable
                // ErrorHandler.Error(ERROR_CODE.SYMBOL_NOT_IN_SCOPE, m_tok.m_iLineNum,
                // string.Format("Symbol: {0} is undefined in current scope", m_tok.ToString()));
            } // if FindInScope == null
            else
                // symbol is in current scope
                sym = m_SymTable.FindInScope(name);

            return sym;
        } // FindSymbol

        /// <summary>
        /// Return the memory offset
        /// </summary>
        public int MemoryOffset
        { get { return m_iOff; } }

        /// <summary>
        /// Returns the contents of the symbol table as a formatted string
        /// </summary>
        /// <returns></returns>
        public string GetSymbolTable()
        { return m_SymTable.ToString(); }

        /// <summary>
        /// Tests symbol table by arbitrarily adding and removing scopes as well as
        /// adding and finding symbols.
        /// </summary>
        public void TestSymbolTable()
        {
            // Make sure the table is clear
            m_SymTable.Reset();

            // add to scope 0
            // scope 0 is implicitly created if AddScope() has not been called before
            m_SymTable.AddSymbol(new Symbol("A", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));
            m_SymTable.AddSymbol(new Symbol("B", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));

            // go into scope 1
            m_SymTable.AddScope(); // inc scope
            // add symbols
            m_SymTable.AddSymbol(new Symbol("C", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));

            // go into scope 2
            m_SymTable.AddScope(); // inc scope
            // add symbols
            m_SymTable.AddSymbol(new Symbol("D", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));

            // go back to scope 1
            m_SymTable.RemoveScope();
            // add symbols
            m_SymTable.AddSymbol(new Symbol("E", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));

            // go into scope 3
            m_SymTable.AddScope(); // inc scope
            // add symbols
            m_SymTable.AddSymbol(new Symbol("F", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));
            m_SymTable.AddSymbol(new Symbol("G", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));

            // go back to scope 0
            m_SymTable.RemoveScope();
            m_SymTable.AddSymbol(new Symbol("H", -1, Symbol.SYMBOL_TYPE.TYPE_SIMPLE,
                Symbol.STORAGE_TYPE.TYPE_INT, Symbol.PARAMETER_TYPE.LOCAL_VAR, 8, 0));

            // test FindSymbol 
            Symbol symA = m_SymTable.FindSymbol("A");
            string s = (symA == null) ? "Symbol not in table" : symA.ToString();
            System.Windows.Forms.MessageBox.Show(s, "");

            // test FindInScope      
            symA = m_SymTable.FindInScope("A");
            s = (symA == null) ? "Symbol not in scope" : symA.ToString();
            System.Windows.Forms.MessageBox.Show(s, "");

        } // TestSymbolTable
    } // Parser
} // Namespace JoshPiler
