using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; // StringReader

namespace JoshPiler
{
    class Optimizer
    {
        // Static instance of calss
        private static Optimizer c_opInstance;

        // Optimizer options
        public static bool c_bDupPushPop = true;
        public static bool c_bRedunMov = true;

        // Lock object
        private static object c_opLock = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        private Optimizer() { }

        /// <summary>
        /// Return static instance of class
        /// </summary>
        public static Optimizer Instance
        {
            get
            {
                lock (c_opLock)
                {
                    if (c_opInstance == null)
                        c_opInstance = new Optimizer();
                    return c_opInstance;
                } // lock
            } // get
        } // Instance

        /// <summary>
        /// Optimize string of assembly. Optimize is defined as reducing the amount of commands while
        /// still retaining full functionality
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="passes"></param>
        /// <returns></returns>
        public string OptimizeASM(string asm, int passes = 1)
        {
            StringBuilder sb = new StringBuilder();
            StringReader reader = new StringReader(asm);
            List<string> lsLines = new List<string>();

            // parse asm into list of lines
            string l = reader.ReadLine();
            while (l != null)
            {
                lsLines.Add(l);
                l = reader.ReadLine();
            }

            // skip extra line when bool
            bool b = false;

            // check each line for the dups
            for (int i = 0; i < lsLines.Count; ++i)
            {
                string[] words1 = { "" }, words2 = { "" };

                // get words in each line
                if (i + 1 < lsLines.Count)
                {
                    words1 = lsLines[i].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    words2 = lsLines[i + 1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                }

                // Check for duplicate Push/Pop
                // Ex. Push EAX; Pop EAX
                if (lsLines[i].ToUpper().Contains("PUSH")
                    && c_bDupPushPop)
                {
                    // check for pop
                    if (lsLines[i + 1].ToUpper().Contains("POP"))
                    {
                        // second word should be register
                        if (words1[1].ToUpper() == words2[1].ToUpper())
                        {
                            b = true;   // skip extra line
                            continue;
                        } // check for register
                    }
                } // if push then pop
                // Optimize redundant Mov 
                // Ex. mov EAX, 3; mov EBX, EAX; -> mov EBX, 3;
                else if (lsLines[i].ToUpper().Contains("MOV")
                    && c_bRedunMov)
                {
                    if (lsLines.Count < i + 1) break;
                    // check for next mov
                    else if (lsLines[i + 1].ToUpper().Contains("MOV") && !lsLines[i + 1].Contains("["))
                    {
                        if (words1[1] == words2[2])
                        {
                            sb.Append(string.Format("   mov     {0}, {1};          optimized redundant mov's\r\n",
                                words2[1], words1[2]));
                            b = true;
                            continue;
                        } // compare registers
                    } // if next line contains mov
                } // if line contains mov
                if (!b)
                    sb.Append(lsLines[i] + "\r\n");
                b = false;
            } // for
            if (passes > 0)
                return OptimizeASM(sb.ToString(), --passes);
            else
                return sb.ToString();
        } // OptimizeASM

        public string FormatASM(string asm)
        {
            StringBuilder sb = new StringBuilder();
            StringReader reader = new StringReader(asm);
            List<string> lsLines = new List<string>();

            // parse asm into list of lines
            string l = reader.ReadLine();
            while (l != null)
            {
                lsLines.Add(l);
                l = reader.ReadLine();
            }

            // Parse each line into words and add back with format
            foreach (string line in lsLines)
            {
                List<string> sl = new List<string>(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                bool bComment = false;
                foreach (string word in sl)
                {
                    // Add five spaces between operands and only one space between words in comments
                    if (word.Contains(";")) bComment = true;
                    if (!bComment)
                        sb.Append(word + "     ");
                    else
                        sb.Append(word + " ");
                } // foreach(sl)
                sb.Append("\r\n");
                bComment = false;
            } // foreach(lsLines)
            return sb.ToString();
        } // FormatASM()



    } // Optimizer
} // namespace JoshPiler
