namespace DZ_Security_DataBase
{
    internal static class cMain
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            cDataBase.createDatabase();
            cDataBase.editDatabase();
            ApplicationConfiguration.Initialize();
            Application.Run(new cMenu());
        }
    }
}