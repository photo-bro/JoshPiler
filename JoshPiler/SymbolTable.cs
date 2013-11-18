using System;
using System.Collections; // ArrayList, HashTable, Stack
using System.Collections.Generic; // LinkedList
using System.Linq;
using System.Text;

namespace JoshPiler
{
    /// <summary>
    /// Defines each object in the symbol table. Stores ID's which can be variables,
    /// constants, or procedures
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Enumerator categorizing type of Symbol
        /// </summary>
        public enum SYMBOL_TYPE
        { TYPE_SIMPLE = 0, TYPE_CONST, TYPE_ARRAY, TYPE_PROC }

        /// <summary>
        /// Enumerator categorizing storage type of Symbol
        /// </summary>
        public enum STORAGE_TYPE
        { STORE_NONE = 0, TYPE_INT }

        /// <summary>
        /// Enumerator categorizing parameter type of Symbol
        /// </summary>
        public enum PARAMETER_TYPE
        { LOCAL_VAR = 0, VAL_PARM, REF_PARM }

        // Class variables
        private string m_sName;                 // Symbol name
        private int m_iScope;                   // scope level
        private SYMBOL_TYPE m_SymType;          // type
        private STORAGE_TYPE m_StorType;        // storage type
        private PARAMETER_TYPE m_ParType;       // parameter type
        private int m_iMemOff;                  // memory offset
        private int m_iVal;                     // int val - for CONST
        private int m_iBaseOff;                 // base offset - for ARRAY
        private int m_iArrEnd;                  // array length - for ARRAY
        private int m_iRelativeOff;             // relative offset for VAL_PARM
        /// <summary>
        /// Constructor
        /// </summary>
        public Symbol(string name, int scope, SYMBOL_TYPE SymType,
            STORAGE_TYPE StorType, PARAMETER_TYPE ParType, int memoffset, 
            int value = 0, int baseoff = 0, int arrEnd = 0, int relOff = 0)
        {
            m_sName = name;
            m_iScope = scope;
            m_SymType = SymType;
            m_StorType = StorType;
            m_ParType = ParType;
            m_iMemOff = memoffset;
            m_iVal = value;
            m_iBaseOff = baseoff;
            m_iArrEnd = arrEnd;
            m_iRelativeOff = relOff;
        } // Symbol

        /// <summary>
        /// Blanket method for retrieving Symbol information. All returns are passed through
        /// their respective arguments.
        /// </summary>
        public void GetSymbolInfo(out string name, out int scope, out SYMBOL_TYPE SymType,
            out STORAGE_TYPE StorType, out PARAMETER_TYPE ParType, out int memoffset, out int value,
            out int baseoff, out int arrEnd, out int relOff)
        {
            name = m_sName;
            scope = m_iScope;
            SymType = m_SymType;
            StorType = m_StorType;
            ParType = m_ParType;
            memoffset = m_iMemOff;
            value = m_iVal;
            baseoff = m_iBaseOff;
            arrEnd = m_iArrEnd;
            relOff = m_iRelativeOff;
        } // GetSymbolInfo

        /// <summary>
        /// Returns Symbol name
        /// </summary>
        public string Name
        { get { return m_sName; } }

        /// <summary>
        /// Returns scope level. Sets scope level.
        /// </summary>
        public int Scope
        {
            get { return m_iScope; }
            set { m_iScope = value; }
        } // Scope

        /// <summary>
        /// Returns the SYMBOL_TYPE.
        /// </summary>
        public SYMBOL_TYPE SymType
        { get { return m_SymType; } }

        /// <summary>
        /// Returns the PARAMATER_TYPE
        /// </summary>
        public PARAMETER_TYPE ParamType
        { get { return m_ParType; } }

        /// <summary>
        /// Returns the memory offset.
        /// </summary>
        public int Offset
        { get { return m_iMemOff; } }

        /// <summary>
        /// Returns the value of the symbol - for CONST symbols
        /// </summary>
        public int Value
        { get { return m_iVal; } }

        /// <summary>
        /// Returns the base offset of the symbol - for ARRAY symbols
        /// </summary>
        public int BaseOffset
        { get { return m_iBaseOff; } }

        /// <summary>
        /// Returns the array length of the symbol - for ARRAY symbols
        /// </summary>
        public int ArrayEnd
        { get { return m_iArrEnd; } }

        /// <summary>
        /// Get/Set the relative offset of the symbol - for VAL_PARM
        /// Offset from BP of active procedure
        /// </summary>
        public int RelativeOffset
        { 
            get { return m_iRelativeOff; }
            set { m_iRelativeOff = value; }
        }

        /// <summary>
        /// Returns formated string form of Symbol.
        /// Ex:
        /// Name:       Scope:   Type:          Storage:     Parameter:    Offset:    Value:
        /// iNum        0       TYPE_SIMPLE     TYPE_INT     LOCAL_VAR     8          0
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0,-20}{1,5}{2,15}{3,15}{4,15}{5,10}{6,18}",
            m_sName,
            m_iScope,
            m_SymType.ToString(),
            m_StorType.ToString(),
            m_ParType.ToString(),
            m_iMemOff,
            m_iVal);
        } // ToString()
    } // Symbol

    /// <summary>
    /// Scope contains the data and methods for storing and accessing Symbol objects. 
    /// The uses a linkedlist implementation.
    /// </summary>
    public class Scope
    {
        // scope level, 0 being the lowest
        private int m_iScope;

        // linked list of stored symbols
        private LinkedList<Symbol> m_llSymbols;

        // node accessing current position in linked list
        private LinkedListNode<Symbol> m_llCurNode;

        /// <summary>
        /// Basic constructor. iScope is the scope level, 0 being the lowest.
        /// </summary>
        /// <param name="iScope"></param>
        public Scope(int iScope)
        {
            m_iScope = iScope;
            m_llSymbols = new LinkedList<Symbol>();
            m_llCurNode = m_llSymbols.First;
        } // Scope

        /// <summary>
        /// Constructor. iScope is the scope level, 0 being the lowest. symFirst is the first symbol
        /// in the Scope.
        /// </summary>
        /// <param name="iScope"></param>
        /// <param name="symFirst"></param>
        public Scope(int iScope, Symbol symFirst)
        {
            m_iScope = iScope;
            m_llSymbols = new LinkedList<Symbol>();
            m_llCurNode = new LinkedListNode<Symbol>(symFirst);

            m_llSymbols.AddFirst(m_llCurNode);
        } // Scope

        /// <summary>
        /// symbol has been added to the internal list. 
        /// </summary>
        /// <param name="symbol"></param>
        public void AddSymbol(Symbol symbol)
        {
            // Check if first in list
            if (m_llSymbols.Count == 0)
                m_llSymbols.AddFirst(symbol);
            else
                m_llSymbols.AddAfter(m_llCurNode, symbol);
            // Update active node
            m_llCurNode = m_llSymbols.Last;
        } // AddSymbol

        /// <summary>
        /// symbol has been removed from the internal list if it exists in the list.
        /// </summary>
        /// <param name="symbol"></param>
        public void RemoveSymbol(Symbol symbol)
        {
            if (m_llSymbols.Contains(symbol))
                m_llSymbols.Remove(symbol);
        } // RemoveSymbol

        /// <summary>
        /// true has been returned if symbol exists in class list, false if not.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool Contains(Symbol symbol)
        { return m_llSymbols.Contains(symbol); }

        /// <summary>
        /// true has been returned if a Symbol with name sName exists in class list, false if not.
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public bool Contains(string sName)
        {
            foreach (Symbol sym in m_llSymbols)
            {
                if (string.Compare(sym.Name, sName) == 0)
                    return true;
            } // foreach
            return false;
        } // Contains

        /// <summary>
        /// Symbol with sName has been returned. If there is no Symbol with name sName null
        /// has been returned.
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public Symbol FindSymbol(string sName)
        {
            foreach (Symbol sym in m_llSymbols)
            {
                if (string.Compare(sym.Name, sName) == 0)
                    return sym;
            } // foreach
            return null;
        } // FindSymbol

        /// <summary>
        /// Scope level of object has been returned
        /// </summary>
        public int CurrentScope
        { get { return m_iScope; } }

        /// <summary>
        /// Return linked list of symbols contained in scope
        /// </summary>
        public LinkedList<Symbol> Symbols
        { get { return m_llSymbols; } }

        /// <summary>
        /// Return a string containing the string form of all Symbols in class data structure
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Symbol sym in m_llSymbols)
            {
                sb.Append(sym.ToString());
                sb.Append("\r\n"); // add new line
            } // foreach
            return sb.ToString();
        } // ToString()
    } // Scope

    /// <summary>
    /// SymbolTable contains the methods to store and access Scope objects and their respective
    /// Symbol objects. This class uses a stack implementation.
    /// </summary>
    public class SymbolTable
    {
        // static class instance
        private static SymbolTable c_SymbolTable;

        // lock object
        private static object c_stLock = new Object();

        // store scopes in stack
        private static Stack<Scope> c_stkScope = new Stack<Scope>();

        private static int c_iTopScope = -1;
        // current scope level
        private static int c_iCurScope = -1;

        // hold popped scope string forms
        private StringBuilder m_sbSymStr = new StringBuilder();

        /// <summary>
        /// Default constructor
        /// </summary>
        private SymbolTable() 
        { 
            // create default scope upon construction
            AddScope();
        }

        /// <summary>
        /// Return static instance of class
        /// </summary>
        public static SymbolTable Instance
        {
            get
            {
                lock (c_stLock)
                {
                    if (c_SymbolTable == null) c_SymbolTable = new SymbolTable();
                    return c_SymbolTable;
                } // lock
            } // get
        } // Instance

        /// <summary>
        /// Add Symbol to active scope
        /// </summary>
        /// <param name="symbol"></param>
        public void AddSymbol(Symbol symbol)
        {
            // check if any scope exists
            if (c_stkScope.Count == 0)
                AddScope(); // add first scope

            // get last scope
            Scope scope = c_stkScope.Pop();

            // check if symbol is in scope
            if (scope.Contains(symbol.Name)) // remove symbol
                scope.RemoveSymbol(symbol);

            // update symbol scope level
            symbol.Scope = c_iCurScope;
            // add symbol
            scope.AddSymbol(symbol);
            // push back
            c_stkScope.Push(scope);
        }

        /// <summary>
        /// Add symbol to given scope
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="scope"></param>
        public void AddSymbol(Symbol symbol, int scope)
        {
            if (scope < 0) return;
            if (c_iCurScope == scope)
            {
                AddSymbol(symbol);
                return;
            }
            // stack to hold popped scopes
            Stack<Scope> stkTemp = new Stack<Scope>();

            // get scope requested
            Scope scp = c_stkScope.Pop();
            while (scp.CurrentScope != scope)
            {
                stkTemp.Push(scp);
                scp = c_stkScope.Pop();
            }

            // push proper scope back on
            c_stkScope.Push(scp);

            // Add symbol to now current scope
            AddSymbol(symbol);

            // restore stack
            while (stkTemp.Count > 0)
                c_stkScope.Push(stkTemp.Pop());

        } // AddSymbol

        /// <summary>
        /// Add Scope object to internal data structure.
        /// <param name="iScopeID"></param>
        /// <param name="scope"></param>
        /// </summary>
        public void AddScope()
        {
            Scope scope = new Scope(++c_iTopScope);
            c_stkScope.Push(scope);
            c_iCurScope = c_iTopScope; // current scope now is the top scope
        } // AddScope

        /// <summary>
        /// Remove most recent Scope object from internal data structure. If stack is empty
        /// nothing is done.
        /// </summary>
        public void RemoveScope()
        {
            if (c_stkScope.Count > 0)
            {
                Scope scope = c_stkScope.Pop();
                m_sbSymStr.Append(scope.ToString());
                m_sbSymStr.Append(string.Format("Scope {0} now removed\r\n", c_iCurScope));
                c_iCurScope = c_stkScope.Peek().CurrentScope; // get current scope
            }
        } // RemoveScope

        /// <summary>
        /// Returns the top scope (active scope)
        /// </summary>
        /// <returns></returns>
        public Scope GetCurrentScope()
        { return c_stkScope.Peek(); }

        public Scope GetScope(int scope)
        {
            if (scope < 0) return null;
            if (c_iCurScope == scope) return c_stkScope.Peek();

            // stack to hold popped scopes
            Stack<Scope> stkTemp = new Stack<Scope>();

            // get scope requested
            Scope scp = c_stkScope.Pop();
            while (scp.CurrentScope != scope)
            {
                stkTemp.Push(scp);
                scp = c_stkScope.Pop();
            }

            // push proper scope back on
            c_stkScope.Push(scp);

            // restore stack
            while (stkTemp.Count > 0)
                c_stkScope.Push(stkTemp.Pop());

            return scp;
        }

        /// <summary>
        /// Returns the active scope level
        /// </summary>
        public int ActiveScope
        { get { return c_iCurScope; } }

        /// <summary>
        /// Returns Symbol referenced in current Scope by sSym. null has been returned if not found.
        /// </summary>
        /// <param name="iScopeID"></param>
        /// <param name="sSym"></param>
        /// <returns></returns>
        public Symbol FindInScope(string sSym)
        {
            Scope scope = c_stkScope.Peek();
            if (scope.Contains(sSym))
                return scope.FindSymbol(sSym);
            else return null;
        } // FindInScope

        /// <summary>
        /// Returns Symbol referenced in any Scope by sSym. null has been returned if not found.
        /// </summary>
        /// <param name="sSym"></param>
        /// <returns></returns>
        public Symbol FindSymbol(string sSym)
        {
            // create array from stack
            Scope[] scpArr = c_stkScope.ToArray();
            // look through array
            for (int i = 0; i < scpArr.Length; ++i)
            {
                // look for sSym
                if (scpArr[i].Contains(sSym))
                    return scpArr[i].FindSymbol(sSym);
            }
            return null;
        } // FindSymbol

        /// <summary>
        /// Reset all internal data structures to default states
        /// </summary>
        public void Reset()
        {
            // reset scope
            c_iCurScope = c_iTopScope = -1;
            // clear stack
            c_stkScope.Clear();
            // add default scope
            AddScope();
            // get new symstr
            m_sbSymStr = new StringBuilder();
        }


        /// <summary>
        /// Returns a formatted string of all symbols in all scopes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Scope[] scpArr = c_stkScope.ToArray();
            Scope scope;

            // header
            sb.Append(String.Format("{0,35}{1,19}{2,16}{3,15}\r\n", "Symbol", "Storage", "Parameter", "Memory"));
            sb.Append(String.Format("{0,-20}{1,5}{2,8}{3,18}{4,14}{5,20}{6,15}\r\n", "Name:",
                "Scope:", "Type:", "Type:", "Type:", "Offset:", "Value:"));
            sb.Append("_____________________________________________________________________________________________________\r\n");

            // add class symbols string
            sb.Append(m_sbSymStr.ToString());

            // for (int i = scpArr.Length - 1; i > -1; --i)
            for (int i = 0; i < scpArr.Length; ++i)
            {
                scope = scpArr[i];
                sb.Append(scope.ToString());
            }

            return sb.ToString();
        } // ToString()
    } // SymbolTable
} // namespace JoshPiler