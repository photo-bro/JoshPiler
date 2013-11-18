using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshPiler
{
    class Emitter
    {
        // static class instance
        private static Emitter c_Em;

        // class lock object
        private static Object c_EmLock = new Object();

        // Filemanager object
        private static FileManager c_FM = FileManager.Instance;

        // Optimizer object
        private Optimizer m_Opt = Optimizer.Instance;

        // memory use
        private static int c_iMainMemoryUse = 4;

        // proc inc
        private static string c_sProcInc = "";

        // string inc
        private static string c_sStrInc = "";
        private static int c_iStr = 1;

        // compiler flags
        public bool m_bMoreComments = false;      // print extra comments in proclist.inc
        public bool m_bOptimize = true;           // optimize asm
        public static bool c_bEmitterASMDebug = true;
        /// <summary>
        /// Default constructor
        /// </summary>
        private Emitter() { }

        /// <summary>
        /// Returns static instance of class
        /// </summary>
        public static Emitter Instance
        {
            get
            {
                lock (c_EmLock)
                {
                    if (c_Em == null) c_Em = new Emitter();
                    return c_Em;
                } // lock
            } // get
        } // Instance

        // #########################################################################################
        // ASSEMBLER METHODS   ASSEMBLER METHODS   ASSEMBLER METHODS   ASSEMBLER METHODS     
        // #########################################################################################

        /// <summary>
        /// Emit asm to relating to WRSTR to write to console. 
        /// </summary>
        /// <param name="s"></param>
        public void WRSTR(string s)
        {
            // add string to str.inc
            AddToStrInc(s);
            if (m_bMoreComments) AddToProcInc(string.Format(";===== WRSTR(\"{0}\") =====", s)); // header
            // call PutStr with last string
            AddToProcInc(/*          */"  PutStr " + getLastStr + "     ;display string");
        } // WRSTR(string)

        /// <summary>
        /// Emit asm relating to WRLN to write newline to console
        /// </summary>
        public void WRLN()
        {
            if (m_bMoreComments) AddToProcInc(";===== WRLN() ====="); // header
            // call nwln
            AddToProcInc(/*          */"  nwln                         ;new line");
        } // WRLN() 

        /// <summary>
        /// Write int value in EAX to console
        /// </summary>
        public void WRINT()
        {
            if (m_bMoreComments) AddToProcInc(";===== WRINT() ====="); // header
            AddToProcInc(/*          */"  PutLInt     EAX              ; print value in EAX");
        } // WRINT()

        /// <summary>
        /// Retrieves user inputed 32bit integer into EAX
        /// </summary>
        public void RDINT()
        {
            if (m_bMoreComments) AddToProcInc(";===== RDINT() ====="); // header
            AddToProcInc(/*          */"  GetLInt     EAX              ; print value in EAX");
            WRLN(); // create new line
        }

        // #########################################################################################
        //        M I S C    A S M    M E T H O D S           M I S C    A S M    M E T H O D S  
        // #########################################################################################

        /// <summary>
        /// Insert label at current point of code. A colon ":" is NOT needed in string label.
        /// </summary>
        /// <param name="label"></param>
        public void Label(string label)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== Label({0}) =====", label)); // header
            AddToProcInc(string.Format("{0}:                           ;", label));
        } // Label()

        /// <summary>
        /// Jump to specified label. jcond is any legal x86 jump instruction.
        /// </summary>
        /// <param name="jcond"></param>
        /// <param name="label"></param>
        public void Jump(string jcond, string label)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== Jump({0}, {1}) =====", jcond, label)); // header
            AddToProcInc(string.Format("  {0}   {1}                   ;", jcond, label));
        } // Jump(jcond, label)

        /// <summary>
        /// Set compare flag by comparing valA and valB against each other. valA and valB can be registers or memory locations. 
        /// Both cannot be memory locations however.
        /// </summary>
        /// <param name="valA"></param>
        /// <param name="valB"></param>
        public void Compare(string valA, string valB)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== Compare({0}, {1}) =====", valA, valB)); // header
            AddToProcInc(string.Format("  cmp        {0}, {1}         ;", valA, valB));
        } // Compare(valA, valB)

        /// <summary>
        /// AND valA and valB, result in valA.valA and valB can be registers or memory locations. 
        /// Both cannot be memory locations however.
        /// </summary>
        /// <param name="valA"></param>
        /// <param name="valB"></param>
        public void And(string valA, string valB)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== AND({0}, {1}) =====", valA, valB)); // header
            AddToProcInc(string.Format("  and        {0}, {1}         ;", valA, valB));
        } // AND

        /// <summary>
        /// OR valA and valB, result in valA. valA and valB can be registers or memory locations. 
        /// Both cannot be memory locations however.
        /// </summary>
        /// <param name="valA"></param>
        /// <param name="valB"></param>
        public void Or(string valA, string valB)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== Or({0}, {1}) =====", valA, valB)); // header
            AddToProcInc(string.Format("  or         {0}, {1}         ;", valA, valB));
        } // OR

        /// <summary>
        /// NOT valA result in valA. alA can be a register or memory location. 
        /// </summary>
        /// <param name="valA"></param>
        public void Not(string valA)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== Not({0}) =====", valA)); // header
            AddToProcInc(string.Format("  not         {0}             ;", valA));

        } // NOT

        /// <summary>
        /// Compare the last two values in the stack. Result is pushed back on stack. 1 = true,
        /// 0 = false;
        /// </summary>
        /// <param name="jumpcond"></param>
        /// <param name="label"></param>
        /// <param name="labelcount"></param>
        public void StackCompare(string jumpcond, string label, ref int labelcount)
        {
            string sIf = label + labelcount++;
            string sEnd = label + labelcount++;
            //asm(";;;; Stack Compare ;;;;");
            popInt();              // pop into EAX
            movReg("EBX", "EAX");  //
            popInt();
            Compare("EAX", "EBX");
            Jump(jumpcond, sIf);
            pushInt(0);            // condition false
            Jump("jmp", sEnd);
            Label(sIf);
            pushInt(1);            // condition true
            Label(sEnd);
            //asm(";;;;  END Stack Compare ;;;;");
        } // StackCompare

        /// <summary>
        /// Set value at regB int regA. regA = regB. Assumes regA and regB are valid registers
        /// </summary>
        /// <param name="regA"></param>
        /// <param name="regB"></param>
        public void movReg(string regA, string regB)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== movReg({0}, {1}) =====", regA, regB)); // header
            AddToProcInc(string.Format("  mov        {0}, {1}         ; {0} = {1} ", regA, regB));
        } // movReg(regA, regB)

        /// <summary>
        /// Divide EDX:EAX by divisor. Divisor can be a literal or register. Recommend sign extending EDX for 32 bit division.
        /// </summary>
        /// <param name="divisor"></param>
        public void iDiv(string divisor)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== iDiv({0}) =====", divisor)); // header
            AddToProcInc(string.Format("  idiv       {0}              ; EDX:EAX / divisor ", divisor));
        } // iDiv(divisor)

        /// <summary>
        /// Multiply regA by valB, result is stored in regA. regA must be a register, valB can be a register or memory location. 
        /// </summary>
        /// <param name="regA"></param>
        /// <param name="valB"></param>
        public void iMul(string regA, string valB)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== iMul({0}, {1}) =====", regA, valB)); // header
            AddToProcInc(string.Format("  imul       {0}, {1}         ; {0} = {0} * {1} ", regA, valB));
        } // iMul(regA, valB)

        /// <summary>
        /// Subtract valB from valA (valA = valA - valB), result is stored at valA. valA and valB can be registers or memory locations. 
        /// Both cannot be memory locations however.
        /// </summary>
        /// <param name="valA"></param>
        /// <param name="valB"></param>
        public void Add(string valA, string valB)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== Add({0}, {1}) =====", valA, valB)); // header
            AddToProcInc(string.Format("  add        {0}, {1}         ; {0} = {0} + {1} ", valA, valB));
        } // Add(valA, valB)

        /// <summary>
        /// Add valA to valB, result is stored at valA. valA and valB can be registers or memory locations. 
        /// Both cannot be memory locations however
        /// </summary>
        /// <param name="valA"></param>
        /// <param name="valB"></param>
        public void Sub(string valA, string valB)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== Sub({0}, {1}) =====", valA, valB)); // header
            AddToProcInc(string.Format("  sub        {0}, {1}         ; {0} = {0} - {1} ", valA, valB));
            // sub EAX, EBX ; EAX - EBX result in EAX
        } // Sub(valA, valB)

        /// <summary>
        /// Add raw asm to file. Use only for non repeating instances of asm. (Rarely please)
        /// </summary>
        /// <param name="asm"></param>
        public void asm(string asm)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== asm({0}) =====", asm)); // header
            AddToProcInc(asm);
        } // asm(asm)

        // #########################################################################################
        //            STACK METHODS   STACK METHODS   STACK METHODS   STACK METHODS   
        // #########################################################################################

        /// <summary>
        /// Push i onto the runtime stack
        /// </summary>
        /// <param name="i"></param>
        public void pushInt(int i)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== pushInt({0}) =====", i)); // header
            AddToProcInc(string.Format("  mov        EAX, {0}         ; EAX = {0}", i));
            AddToProcInc(/*          */"  push       EAX              ; push EAX onto stack");
        } // pushInt(i)

        /// <summary>
        /// Push value in register reg onto the runtime stack, reg must be a valid register
        /// </summary>
        public void pushReg(string reg)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== pushReg({0}) =====", reg)); // header
            AddToProcInc(string.Format("  push       {0}              ; push {0} onto stack", reg));
        } // pushReg(reg)

        /// <summary>
        /// Pop top element of runtime stack into EAX
        /// </summary>
        public void popInt()
        {
            if (m_bMoreComments) AddToProcInc(";===== popInt() ====="); // header
            AddToProcInc(/*          */"  pop        EAX              ; pop into EAX");
        } // popInt()

        /// <summary>
        /// Pop top element of runtime stack into reg
        /// </summary>
        public void popReg(string reg)
        {
            if (m_bMoreComments) AddToProcInc(";===== popInt() ====="); // header
            AddToProcInc(string.Format("  pop        {0}              ; pop into {0}", reg));
        } // popReg()

        /// <summary>
        /// Pop top element of runtime stack and push into varstack at BP+offset
        /// </summary>
        /// <param name="offset"></param>
        public void AssignVar(int offset)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== AssignVar(offset: {0}) =====", offset)); // header
            AddToProcInc(/*          */"  pop        EAX               ; pop EAX off stack");
            AddToProcInc(string.Format("  mov        [BP+{0}], EAX     ; store EAX at memory offset in varstack", offset));
        } // AssignVar(offset)

        /// <summary>
        /// Set EAX to value at BP+offset. EAX = BP+offset
        /// </summary>
        /// <param name="offset"></param>
        public void GetVar(int offset)
        {
            if (m_bMoreComments) AddToProcInc(string.Format(";===== GetVar(offset: {0}) =====", offset)); // header
            // EAX = value at BP+offset
            AddToProcInc(string.Format("  mov        EAX, [BP+{0}]     ; put var at memory offset into EAX ", offset));
        } // GetVar(offset)

        // #########################################################################################
        // FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   
        // #########################################################################################

        /// <summary>
        /// Reset class and static members
        /// </summary>
        public void Reset()
        {
            c_sProcInc = "";
            c_sStrInc = "";
            c_iStr = 1;
        } // Reset()

        /// <summary>
        /// Emit asm to enter to procedure
        /// </summary>
        public void EnterProc()
        {
            AddToProcInc(";============== BEGIN PROCEDURE ============");
            AddToProcInc(string.Format("{0} PROC ; Procedure definition", c_FM.MainProcName));
            AddToProcInc("  push    BP    ; save BP since we use it");
            AddToProcInc("  mov     BP,SP ; ");
        } // EnterProc()

        /// <summary>
        /// Emit asm to exit procedure
        /// </summary>
        public void ExitProc()
        {
            AddToProcInc("  pop     BP    ;");
            AddToProcInc("  ret     0     ;");
            AddToProcInc(string.Format("{0} ENDP", c_FM.MainProcName));
            AddToProcInc(";============== END PROCEDURE ==============");
        } // ExitProc()

        /// <summary>
        /// Add assembly to temp procedure.inc internal structure
        /// </summary>
        /// <param name="s"></param>
        public void AddToProcInc(string s)
        { c_sProcInc += string.Format("{0}\r\n", s); }

        /// <summary>
        /// Add assembly to temp Strings.inc internal structure
        /// </summary>
        /// <param name="s"></param>
        public void AddToStrInc(string s)
        {
            // str#     DB 'string s',0
            c_sStrInc += string.Format("str{0}       DB '{1}',0\r\n", c_iStr++, s);
        } // AddToStrInc(string)

        /// <summary>
        /// Return last string identifier in temp Strings.inc internal structure
        /// </summary>
        public string getLastStr
        { get { return "str" + (c_iStr - 1); } }

        /// <summary>
        /// PRE:  The parse is complete.
        ///    c_iMainMemoryUse stores the amount of memory needed by the main procedure.
        /// POST: A string is created and the assembler "shell" is written to it.
        /// </summary>
        public string MainAsmFile() // 
        {
            string strHead = "COMMENT |\r\nTITLE " + c_FM.CompilerName + " output: " + c_FM.LastFileName + ".mod" + "\r\n| Created: ";

            // create a time stamp
            DateTime dt = DateTime.Now;
            strHead += dt.ToString("F") + "\r\n";
            strHead += ".MODEL SMALL\r\n" // use the small (16-bit) memory model
                + ".486\r\n"          // this allows 32-bit arithmetic (EAX, EDX registers, etc.)
                + ".STACK 1000H\r\n"  // plenty of stack space: 4096 bytes
                + ".DATA\r\n"         // begin DATA section

                //definition of a char for "Press the any key to continue:" (John Broere 2002)
                + "end_ch  DB  ?          ; John Broere 2002 idea to 'pause' at end.\r\n\r\n"

                // The following file must be created later in the same directory
                //    to contain string constants of the form:
                + ";===== string constants inserted here: ======\r\n"
                + "INCLUDE strings.inc\r\n\r\n"     // strings.inc

                + ".CODE\r\n"
                + "INCLUDE io.mac\r\n"
                + "main PROC\r\n"
                + ".STARTUP\r\n"
                + "push    BP            ; save BP since we use it\r\n"
                + "sub     SP, " + Parser.Instance.MemoryOffset
                + "         ; Room for main proc local vars\r\n"
                + "mov     BP,SP          ; set the stack pointer as the base pointer\r\n"
                + "call " + c_FM.MainProcName + "\r\n"

                // adds a "pause" to the end of the program - thanks to John Broere 2002 !

                // note that Kevin added str0 to the string collection,
                //    and he added a character (end_ch) in the data segment above.
                + "nwln\r\n"
                + "PutStr  str0\r\n"
                + "GetCh   end_ch\r\n"

                // end the program
                + "pop     BP            ; restore BP\r\n"
                + ".EXIT\r\n"
                + "main    ENDP           ; end of assembly outermost function\r\n\r\n"
                + "; The following procedures must be included.\r\n"
                + "INCLUDE proclist.inc   ; lines like 'INCLUDE V000000main.inc'\r\n" // proclist.inc
                + "END\r\n";

            return strHead;
        }  // MainAsmFile()

        /// <summary>
        /// Returns string containing the contents for the proclist.inc file
        /// </summary>
        /// <returns></returns>
        public string ProcListInc()
        {
            StringBuilder sb = new StringBuilder();

            // add header
            sb.Append("; These are all the procedures for Test01.mod. Main is first.\r\n");

            // add program proc
            sb.Append(c_sProcInc);

            if (m_bOptimize) return m_Opt.OptimizeASM(sb.ToString());
            else return sb.ToString();
        } // ProcListInc

        /// <summary>
        /// Returns string containing contents for the strings.inc file
        /// </summary>
        /// <returns></returns>
        public string StringsInc()
        {
            StringBuilder sb = new StringBuilder();
            // add header
            sb.Append(";===== string constants for the program: ======\r\n");
            sb.Append("str0       DB  'Press any key to continue . . . .',0\r\n");

            // add program strings
            sb.Append(c_sStrInc);

            return sb.ToString();
        } // StringsInc

        /// <summary>
        /// PRE:  The assembler files have all been "written" to strings.
        /// POST: The files are written to the disk.
        /// </summary>
        public void WriteAFiles()
        {
            // write the outermost "shell" assembler file
            c_FM.CreateFile(MainAsmFile(), c_FM.LastFilePath + "\\" + c_FM.getFolderName(), c_FM.LastFileName + ".asm");

            // proc inc
            c_FM.CreateFile(ProcListInc(), c_FM.LastFilePath + "\\" + c_FM.getFolderName(), "proclist.inc");

            // string inc
            c_FM.CreateFile(StringsInc(), c_FM.LastFilePath + "\\" + c_FM.getFolderName(), "strings.inc");

            // Create the command file and invoke it to complete the assembly
            BuildCmdFile(); // Undo for full compile
        } // WriteAFiles

        /// <summary>
        /// PRE:  The parse is complete.
        /// POST: The command file is created for remaining steps of the assembly process
        ///    (compilation and linking to create an execcutable).
        ///    This command file is then run to complete the compilation.
        /// </summary>
        void BuildCmdFile()
        {
            // System.Windows.Forms.MessageBox.Show(System.IO.Path.GetPathRoot(c_FM.MASMdir).Remove(2, 1)); // trace
            string strMakeFile =
                "REM ===== " + c_FM.CompilerName + ": auto-created command file ======\r\n"
                //+ "set path=%path%;" + c_FM.MASMdir + "\r\n"

                // set the directory
                + System.IO.Path.GetPathRoot(c_FM.LastFilePath).Remove(2, 1) + "\r\n"
                + "cd " + c_FM.LastFilePath + "\\" + c_FM.getFolderName() + "\r\n"

                // copy files needed for the compiling and linking (respectively)
                + "copy " + c_FM.MASMdir + "\\io.mac \r\n"
                + "copy " + c_FM.MASMdir + "\\io.obj \r\n"

                // assemble to create the object file
                + c_FM.MASMdir + "\\ml /c " + c_FM.LastFileName + ".asm\r\n"

                // link the files to create the executable
                + c_FM.MASMdir + "\\link16 "
                + c_FM.LastFileName + ".obj io.obj, "
                + c_FM.LastFileName + ".exe, "
                + c_FM.LastFileName + ".map, , , \r\n\r\n" // Yes, the three commas are necessary!


                // set the directory
                + "cd " + c_FM.LastFilePath + "\\" + c_FM.getFolderName() + "\r\n"
                + "del *.MAC *.obj *.map\r\n"

                // add a pause, so we can see the results of the assembly and linking
                // Thanks to John Broere 2002 !
                + "@ PAUSE\r\n\r\n"
                + "cls\r\n"
                + c_FM.LastFileName + ".exe";       // Open compiled program


            //System.Windows.Forms.MessageBox.Show(strMakeFile); // trace
            // Write the command string to the proper file.
            c_FM.CreateFile(strMakeFile, c_FM.LastFilePath + "\\" + c_FM.getFolderName(), c_FM.LastFileName + ".cmd");

            // Invoke the file just created. This uses the static method in our SystemCommand class.
            //    If an error occurs it will throw the appropriate exception.
            SystemCommand.SysCommand(c_FM.LastFilePath + "\\" + c_FM.getFolderName() + "\\" + c_FM.LastFileName + ".cmd");
        } // BuildCmdFile()
    } // Emitter
} // Namespace JoshPiler
