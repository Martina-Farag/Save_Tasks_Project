using DevExpress.Utils.TouchHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TasksApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Create a new object, representing the German culture.
            CultureInfo culture = CultureInfo.CreateSpecificCulture("ar-EG");

            // The following line provides localization for the application's user interface.
            Thread.CurrentThread.CurrentUICulture = culture;

            // The following line provides localization for data formats.
            Thread.CurrentThread.CurrentCulture = culture;

            // Set this culture as the default culture for all threads in this application.
            // Note: The following properties are supported in the .NET Framework 4.5+
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Note that the above code should be added BEFORE calling the Run() method.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //EnableTouchKeyboard
            TouchKeyboardSupport.EnableTouchKeyboard = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
