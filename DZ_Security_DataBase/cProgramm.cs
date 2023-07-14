namespace DZ_Security_DataBase
{
    internal static class cProgramm
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new cLoginMenu());
        }
    }
}