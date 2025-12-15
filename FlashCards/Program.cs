using System;
using System.Windows.Forms;
using OfficeOpenXml;

namespace FlashcardsApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            ApplicationConfiguration.Initialize();
            Database.Init();
            Application.Run(new Form1());
           
        }
    }
}