﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AEDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //frmFrame frmMain = new frmFrame();
            Application.Run(new frmFrame());
        }
    }
}
