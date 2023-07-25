namespace Festival_Manager
{
    internal static class cProgramm
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new cLoginMenu());
        }


    }
}