﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JoshPiler
{
    static class Program
    {
        public static MainForm MainForm
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new JoshPiler.Forms.SplashScreen());
            Application.Run(new MainForm());
        }
    }
}
